using Gitclient.model;
using GitClient.service;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GitClient.repository
{
    public class CommitsLisLibGit2Repository
    {
        public List<CommitsElements> GetAllCommits()
        {
            List<CommitsElements> listOFCommits = new List<CommitsElements>();
            IntPtr walker = IntPtr.Zero;
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr repo = GetRepo();
            LibGit2Wrapper.GitOid id = new LibGit2Wrapper.GitOid();
            string message = "";

            try
            {
                if (LibGit2Wrapper.git_revwalk_new(out walker, repo) != 0)
                {
                    throw new Exception("Could not create revision walker");
                }

                if (LibGit2Wrapper.git_revwalk_push_head(walker) != 0)
                {
                    throw new Exception("Could not find repository HEAD");
                }

                while (LibGit2Wrapper.git_revwalk_next(out id, walker) == 0)
                {
                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref id) == 0)
                    {
                        string commitMessage = Marshal.PtrToStringAnsi(LibGit2Wrapper.git_commit_message(commitPtr))!;
                        string[] messageParts = commitMessage.Split(new[] { '\n' }, 2);

                        if (messageParts[0].Contains("\n\n"))
                        {
                            int index = messageParts[0].IndexOf('\n');
                            message = messageParts[0].Remove(index);
                        }
                        else
                        {
                            message = messageParts[0].TrimEnd();
                        }

                        string description;

                        if (messageParts.Length > 1)
                        {
                            description = messageParts[1].Trim();
                        }
                        else
                        {
                            description = string.Empty;
                        }

                        listOFCommits.Add(new CommitsElements(GetCommitId(id), GetDateAndTime(commitPtr), GetCommitAuthor(commitPtr), message));
                    }
                    else
                    {
                        throw new Exception("Fail to look up the commit.");
                    }
                }
            }
            finally
            {
                LibGit2Wrapper.git_commit_free(commitPtr);
                LibGit2Wrapper.git_revwalk_free(walker);
            }

            return listOFCommits;
        }


        public IntPtr GetRepo()
        {
            GetProjectPath projectPath = new GetProjectPath();
            var repoPath = projectPath.ProjectPath(Environment.CurrentDirectory);
            IntPtr repo = IntPtr.Zero;

            if (repoPath == null)
            {
                Console.WriteLine("\nCould not find a Git repository in any directory.");
            }

            LibGit2Wrapper.git_libgit2_init();

            if (LibGit2Wrapper.git_repository_open(out repo, repoPath!) != 0)
            {
                throw new Exception("Failed to open the repository.");
            }

            return repo;
        }

        private string GetCommitId(LibGit2Wrapper.GitOid id)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in id.Id)
            {
                sb.Append(b.ToString("x2"));
            }

            var commitIdFullLine = sb.ToString();
            return $"{commitIdFullLine.Remove(7)}";
        }

        private string GetCommitAuthor(IntPtr commit)
        {
            IntPtr signaturePtr = LibGit2Wrapper.git_commit_author(commit);
            if (signaturePtr == IntPtr.Zero)
            {
                throw new Exception("Author information could not be retrieved for the given commit.");
            }

            string authorName = "";
            string commitAuthor;
            try
            {
                authorName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(signaturePtr))!;
            }
            finally
            {
                commitAuthor = $"{authorName}";
            }

            return commitAuthor;
        }

        private string GetDateAndTime(IntPtr commit)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(LibGit2Wrapper.git_commit_time(commit));
            var adjustedDate = date.ToOffset(new TimeSpan(3, 0, 0));

            if (adjustedDate.Date == DateTimeOffset.UtcNow.Date)
            {
                return adjustedDate.ToString("HH:mm:ss");
            }
            else
            {
                return adjustedDate.ToString("yyyy-MM-dd");
            }
        }
    }
}