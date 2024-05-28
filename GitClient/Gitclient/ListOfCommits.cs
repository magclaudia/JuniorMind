using Gitclient;
using System.Runtime.InteropServices;
using System.Text;

namespace GitClient
{
    public class ListOfCommits
    {
        public static void GetAllCommits(IntPtr repo)
        {
            IntPtr walker = IntPtr.Zero;
            IntPtr commitPtr = IntPtr.Zero;
            GitOid id = new GitOid();

            var listOfCommits = new List<string>();
            int index = 0;
            int rightCursor = 1;
            int cursorPosition = 0;
            int cursorPosionBiggerThenHeight = 0;
            int upOrDownOneStep = 0;
            int heightPosition = 1;

            if (LibGit2Wrapper.git_revwalk_new(out walker, repo) == 0)
            {
                if (LibGit2Wrapper.git_revwalk_push_head(walker) == 0)
                {
                    while (LibGit2Wrapper.git_revwalk_next(out id, walker) == 0)
                    {
                        if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref id) == 0)
                        {
                            string commitId = GetCommitId(id);
                            string dateAndTime = GetDateAndTime(commitPtr);
                            string commitAuhor = GetCommitAuthor(commitPtr);
                            string message = Marshal.PtrToStringAnsi(LibGit2Wrapper.git_commit_message(commitPtr))!;
                            var line = $"{commitId} {dateAndTime} {commitAuhor} {message}";
                            listOfCommits.Add(line);
                            LibGit2Wrapper.git_commit_free(commitPtr);
                        }
                        else
                        {
                            throw new Exception("Fail to look up the commit.");
                        }
                    }

                    
                    DrawExternalBorder.DrawBox();
                    bool displayPanel = false;
                    if (index < Console.WindowHeight - 2)
                    {
                        Commits.PrintCommits(displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition, listOfCommits);
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
            return $"{commitIdFullLine.Remove(7)}";
        }

        private static string GetCommitAuthor(IntPtr commit)
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

        private static string GetDateAndTime(IntPtr commit)
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