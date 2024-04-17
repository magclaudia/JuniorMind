using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class ListOfCommits
    {
        public static void GetAllCommits(IntPtr repo)
        {
            IntPtr walker = IntPtr.Zero;
            IntPtr commitPtr = IntPtr.Zero;
            GitOid id = new GitOid();
            
            if (LibGit2Wrapper.git_revwalk_new(out walker, repo) == 0)
            {
                if (LibGit2Wrapper.git_revwalk_push_head(walker) == 0)
                {
                    while (LibGit2Wrapper.git_revwalk_next(out id, walker) == 0)
                    {
                        if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, id) == 0)
                        {
                            string commitId = GetCommitId(id);
                            Console.WriteLine(commitId);

                            string commitAuhor = GetCommitAuthorAndEmail(commitPtr);
                            Console.WriteLine(commitAuhor);

                            string dateAndTime = GetDateAndTime(commitPtr);
                            Console.WriteLine(dateAndTime);

                            string message = Marshal.PtrToStringAnsi(LibGit2Wrapper.git_commit_message(commitPtr));
                            Console.WriteLine($"   {message}");
                            LibGit2Wrapper.git_commit_free(commitPtr);
                        }
                        else
                        {
                            throw new Exception("Fail to look up the commit.");
                        }
                    }
                }
                else
                {
                    throw new Exception("Could not find repository HEAD");
                }
            }
            else
            {
                throw new Exception("Could not create revision walker");
            }

            LibGit2Wrapper.git_revwalk_free(walker);
        }

        private static string GetCommitId(GitOid id)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in id.Id)
            {
                sb.Append(b.ToString("x2"));
            }

            var commitIdFullLine = sb.ToString();
            return $"\ncommit  {commitIdFullLine.Remove(7)}";
        }

        private static string GetCommitAuthorAndEmail(IntPtr commit)
        {
            IntPtr signaturePtr = LibGit2Wrapper.git_commit_author(commit);
            if (signaturePtr == IntPtr.Zero)
            {
                throw new Exception("Author information could not be retrieved for the given commit.");
            }

            string authorName = "";
            string authorEmail = "";
            string commitAuthor = "";
            try
            {
                authorName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(signaturePtr));
                authorEmail = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(signaturePtr + IntPtr.Size));
            }
            finally
            {
                commitAuthor = $"Author: {authorName} <{authorEmail}>";
            }

            return commitAuthor;
        }

        private static string GetDateAndTime(IntPtr commit)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(LibGit2Wrapper.git_commit_time(commit));
            var adjustedDate = date.ToOffset(new TimeSpan(3, 0, 0));
            var commitDate = adjustedDate.ToString("ddd MMM d HH:mm:ss yyyy") + " +0300";
            return $"Date:   {commitDate}";
        }
    }
}
