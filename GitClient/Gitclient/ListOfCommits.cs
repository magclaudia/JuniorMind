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

            var list = new List<string>();
            var completeList = new List<string>();
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            int index = 0;
            int rightCursor = 1;
            int k = 0;
            int start = 0;
            int count = 0;

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
                            count++;
                            var lineList = $"{count} {commitId}";
                            var line = $"{commitId} {dateAndTime} {commitAuhor,-20} {message}";

                            list.Add(lineList);
                            completeList.Add(line);

                            LibGit2Wrapper.git_commit_free(commitPtr);
                        }
                        else
                        {
                            throw new Exception("Fail to look up the commit.");
                        }
                    }

                    int upOrDownOneStep = 0;
                    int commitNumber = 1;
                    int totalNumberOfCommits = list.Count;
                    int cursorPosition = 1;
                    DrawBox(width - 1, height - 1);
                    if (index < Console.WindowHeight - 2)
                    {
                        PrintColumns(cursorPosition, list, commitNumber, totalNumberOfCommits, start, upOrDownOneStep, index, rightCursor, k, completeList);
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
            string commitAuthor = "";
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

        private static void PrintColumns(int cursorPosition, List<string> list, int commitNumber, int totalNumberOfCommits, int start, int upOrDownOneStep, int index, int rightCursor, int k, List<string> completeList)
        {
            ReturnCommitNumber(list, completeList, index, upOrDownOneStep, commitNumber, totalNumberOfCommits);
            
            while (k < Console.WindowHeight - 2 && index < list.Count && index >= 0)
            {
                Console.SetCursorPosition(1, k + 1);
                var commitRow = completeList[index].Split(" ");

                Console.Write($"{commitRow[0]} ", Console.ForegroundColor = ConsoleColor.Magenta);
                Console.Write($"{commitRow[1]} ", Console.ForegroundColor = ConsoleColor.Cyan);
                Console.Write($"{commitRow[2],-20}", Console.ForegroundColor = ConsoleColor.Green);
                Console.ResetColor();
                var message = "";
                var firstThreeColumns = $"{commitRow[0]} ".Length + $"{commitRow[1]} ".Length + $"{commitRow[2],-20}".Length;
                var completeMessage = completeList[index].Length - firstThreeColumns;
                
                if (completeMessage + firstThreeColumns > Console.WindowWidth - 2)
                {
                    message = completeList[index].Substring(firstThreeColumns, Console.WindowWidth - 2 - firstThreeColumns - 1);
                }
                else
                {
                    message = completeList[index].Substring(firstThreeColumns, completeMessage);
                }

                Console.Write($"{message}");

                index++;
                k++;
                commitNumber++;
            }

            rightCursor = index;
            UpdateCursorPosition(cursorPosition, completeList, upOrDownOneStep);
            Console.ResetColor();
            NavigateThroughConsole(cursorPosition, completeList, commitNumber, totalNumberOfCommits, start, upOrDownOneStep, index, rightCursor, k, list);
        }

        private static void NavigateThroughConsole(int cursorPosition, List<string> completeList, int commitNumber, int totalNumberOfCommits, int start, int upOrDownOneStep, int i, int j, int k, List<string> list)
        {
            ConsoleKeyInfo keyInfo;
            totalNumberOfCommits = list.Count;
            do
            {
                keyInfo = Console.ReadKey(true);
                k = 0;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:

                        if (i <= completeList.Count && i > 0)
                        {
                            if (cursorPosition == 1 && upOrDownOneStep == 0)
                            {
                                break;
                            }

                            cursorPosition--;
                            upOrDownOneStep--;

                            if (cursorPosition < 1)
                            {
                                cursorPosition = 1;
                                start--;
                                upOrDownOneStep = start;
                                if (start < 0) start = 0;
                            }

                            Console.Clear();
                            DrawBox(Console.WindowWidth - 1, Console.WindowHeight - 1);
                            i = start;
                            PrintColumns(cursorPosition, list, commitNumber, totalNumberOfCommits, start, upOrDownOneStep, i, j, k, completeList);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (i < completeList.Count && i > 0)
                        {
                            if (i < 0)
                            {
                                i = 0;
                            }

                            upOrDownOneStep++;
                            if (cursorPosition < Console.WindowHeight - 2)
                            {
                                Console.Clear();
                                DrawBox(Console.WindowWidth - 1, Console.WindowHeight - 1);
                                i = 0;
                                cursorPosition++;
                                PrintColumns(cursorPosition, list, commitNumber, totalNumberOfCommits, start, upOrDownOneStep, i, j, k, completeList);
                            }
                            else
                            {
                                Console.Clear();
                                DrawBox(Console.WindowWidth - 1, Console.WindowHeight - 1);
                                start++;
                                i = start;
                                PrintColumns(cursorPosition, list, commitNumber, totalNumberOfCommits, start, upOrDownOneStep, i, j, k, completeList);
                            }
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }

        private static void UpdateCursorPosition(int cursorPosition, List<string> completeList, int index)
        {
            int indicatorPosition = (index * (Console.WindowHeight - 2)) / completeList.Count;
            Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + 1);
            DisplayCustomCursor(cursorPosition, ConsoleColor.DarkBlue, completeList, index);
        }

        private static void DisplayCustomCursor(int cursorPosition, ConsoleColor color, List<string> completeList, int index)
        {
            Console.ForegroundColor = color;
            char horizontal = '█';
            Console.Write(horizontal);
            Console.ResetColor();
            DisplayBlueBox(cursorPosition, index, completeList);
        }

        private static void DisplayBlueBox(int cursorPosition, int index, List<string> completeList)
        {
            Console.SetCursorPosition(1, cursorPosition);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            var commitRow = completeList[index].Split(" ");

            Console.Write($"{commitRow[0]} ");
            Console.Write($"{commitRow[1]} ");
            Console.Write($"{commitRow[2],-20}");
            var message = "";
            var firstThreeColumns = $"{commitRow[0]} ".Length + $"{commitRow[1]} ".Length + $"{commitRow[2],-20}".Length;

            var completeMessage = completeList[index].Length - firstThreeColumns;
            if (completeMessage + firstThreeColumns > Console.WindowWidth - 2)
            {
                message = completeList[index].Substring(firstThreeColumns, Console.WindowWidth - 2 - firstThreeColumns - 1);
            }
            else
            {
                message = completeList[index].Substring(firstThreeColumns, completeMessage);
            }

            Console.Write($"{message}");
            Console.ResetColor();

        }

        private static void ReturnCommitNumber(List<string> list, List<string> completeList, int index,  int upOrDownOneStep, int commitNumber, int totalNumberOfCommits)
        {
            var a = list[upOrDownOneStep].Split(" ");
            var b = completeList[upOrDownOneStep].Split(" ");

            if (a[1] == b[0])
            {
                commitNumber = int.Parse(a[0]);
            }

            var text = $"Commit {commitNumber}/{totalNumberOfCommits} ";
            Console.SetCursorPosition(1, 0);
            Console.Write(text);
        }

        private static void DrawBox(int width, int height)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("┌");
            Console.SetCursorPosition(width, 0);
            Console.Write("┐");
            Console.SetCursorPosition(0, height);
            Console.Write("└");
            Console.SetCursorPosition(width, height);
            Console.Write("┘");

            for (int i = 1; i <  width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("─");
                Console.SetCursorPosition(i, height);
                Console.Write("─");
            }

            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(width, i);
                Console.Write("║");
            }
        }
    }
}