using System.Runtime.InteropServices;
using System.Text;

namespace GitClient
{
    public class ListOfCommits
    {
        public struct CommitElements
        {
            public List<GitOid> IdGitOid;
            public List<string> Id;
            public List<string> DateTime;
            public List<string> Author;
            public List<string> Message;
            public CommitElements()
            {
                IdGitOid = new List<GitOid>();
                Id = new List<string>();
                DateTime = new List<string>();
                Author = new List<string>();
                Message = new List<string>();
            }
        }
        public static void GetAllCommits(IntPtr repo)
        {
            IntPtr walker = IntPtr.Zero;
            IntPtr commitPtr = IntPtr.Zero;
            GitOid id = new GitOid();

            var listOfIds = new List<GitOid>();
            int index = 0;
            int rightCursor = 1;
            int cursorPosition = 0;
            int cursorPosionBiggerThenHeight = 0;
            int upOrDownOneStep = 0;
            int heightPosition = 1;
            var list = new CommitElements();

            if (LibGit2Wrapper.git_revwalk_new(out walker, repo) == 0)
            {
                if (LibGit2Wrapper.git_revwalk_push_head(walker) == 0)
                {
                    while (LibGit2Wrapper.git_revwalk_next(out id, walker) == 0)
                    {
                        if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref id) == 0)
                        {
                            list.IdGitOid.Add(id);
                            list.Id.Add(GetCommitId(id));
                            list.DateTime.Add(GetDateAndTime(commitPtr));
                            list.Author.Add(GetCommitAuthor(commitPtr));
                            list.Message.Add(Marshal.PtrToStringAnsi(LibGit2Wrapper.git_commit_message(commitPtr))!);
                            LibGit2Wrapper.git_commit_free(commitPtr);
                        }
                        else
                        {
                            throw new Exception("Fail to look up the commit.");
                        }
                    }

                    
                    DrawExternalBorder.DrawBox();
                    bool displayPanel = false;
                    bool panelAlreadyDisplayed = false;
                    
                    if (index < Console.WindowHeight - 2)
                    {
                       Commits.PrintCommits(repo, panelAlreadyDisplayed, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition, list);
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