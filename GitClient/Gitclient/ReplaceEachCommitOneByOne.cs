using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class ReplaceEachCommitOneByOne
    {
        public static void PrintNewCommitIfPanel(IntPtr repo, Indexes indexes, CommitElements listOfCommits, List<string> addList, bool reachLimit, int blueFond)
        {
            string message = string.Empty;

            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    indexes.heightPosition++;
                    indexes.currentCommitIndex++;
                }

                Console.SetCursorPosition(1, indexes.heightPosition);
                string list = string.Empty;
                string listWithoutMessage = string.Empty;

                if (i == 0)
                {
                    ClearCommitRow(indexes);
                }

                string id = string.Empty;
                id = $"{listOfCommits.Id[indexes.currentCommitIndex]} ";
                Console.Write($"{listOfCommits.Id[indexes.currentCommitIndex]} ", Console.ForegroundColor = ConsoleColor.Magenta);

                string data = string.Empty;
                if ($"{listOfCommits.DateTime[indexes.currentCommitIndex]} ".Length == 9)
                {
                    data = $"{listOfCommits.DateTime[indexes.currentCommitIndex]}{new string(' ', 2)} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }
                else
                {
                    data = $"{listOfCommits.DateTime[indexes.currentCommitIndex]} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }

                int authorLength = 20 - listOfCommits.Author[indexes.currentCommitIndex].Length;
                string author = $"{listOfCommits.Author[indexes.currentCommitIndex]}{new string(' ', authorLength)}";

                listWithoutMessage = $"{listOfCommits.Id[indexes.currentCommitIndex]}{data}{author}";

                if (listWithoutMessage.Length >= Console.WindowWidth / 2 + 7 - 2)
                {
                    author = $"{author.Substring(0, 2)}..  ";
                    listWithoutMessage = $"{id}{data}{author}";
                    Console.Write($"{author}", Console.ForegroundColor = ConsoleColor.Green);
                }
                else
                {
                    Console.Write($"{author}", Console.ForegroundColor = ConsoleColor.Green);
                }

                Console.ResetColor();


                message = listOfCommits.Message[indexes.currentCommitIndex];
                list = $"{id}{data}{author}{message}";

                var size = new DrawPanel.CommitsPanel();

                if (list.Length > size.width)
                {
                    message = list.Substring(listWithoutMessage.Length, size.width - listWithoutMessage.Length - 2).TrimStart();
                }
                else
                {
                    message = list.Substring(listWithoutMessage.Length, message.Length).TrimStart();
                }

                Console.Write($"{message}");
                list = $"{id}{data}{author}{message}";
                addList.Add(list);
                Console.SetCursorPosition(0, indexes.heightPosition);
                Console.Write("│");
            }


            ClearMessagePanel();
            ClearFilePanel();
            bool clear = false;

            if (indexes.up == true && indexes.heightPosition > 1)
            {
                indexes.currentCommitIndex--;
                indexes.heightPosition--;
            }

            CommitNumber.ReturnCommitNumber(listOfCommits, indexes);
            Navigate.CommitDetail(repo, indexes, listOfCommits, clear);
            Cursor.UpdateCursorPositionList(listOfCommits, addList, indexes, blueFond);

            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughConsole(repo, indexes, listOfCommits, height, width);
        }

        public static void PrintNewCommitIfNoPanel(IntPtr repo, Indexes indexes, CommitElements listOfCommits, List<string> addList, bool reachLimit, int blueFond)
        {
            string message = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    indexes.heightPosition++;
                    indexes.currentCommitIndex++;
                }

                Console.SetCursorPosition(1, indexes.heightPosition);
                string list = string.Empty;
                string listWithoutMessage = string.Empty;

                if (i == 0)
                {
                    ClearCommitRow(indexes);
                }

                string id = string.Empty;
                id = $"{listOfCommits.Id[indexes.currentCommitIndex]} ";
                Console.Write($"{listOfCommits.Id[indexes.currentCommitIndex]} ", Console.ForegroundColor = ConsoleColor.Magenta);

                string data = string.Empty;
                if ($"{listOfCommits.DateTime[indexes.currentCommitIndex]} ".Length == 9)
                {
                    data = $"{listOfCommits.DateTime[indexes.currentCommitIndex]}{new string(' ', 2)} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }
                else
                {
                    data = $"{listOfCommits.DateTime[indexes.currentCommitIndex]} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }

                int authorLength = 20 - listOfCommits.Author[indexes.currentCommitIndex].Length;
                string author = $"{listOfCommits.Author[indexes.currentCommitIndex]}{new string(' ', authorLength)}";


                if (listWithoutMessage.Length >= Console.WindowWidth - 2)
                {
                    author = $"{author.Substring(0, 2)}..  ";
                    listWithoutMessage = $"{id}{data}{author}";
                    Console.Write($"{author}", Console.ForegroundColor = ConsoleColor.Green);
                }
                else
                {
                    Console.Write($"{author}", Console.ForegroundColor = ConsoleColor.Green);
                }

                Console.ResetColor();

                listWithoutMessage = $"{listOfCommits.Id[indexes.currentCommitIndex]}{data}{author}";
                message = listOfCommits.Message[indexes.currentCommitIndex];
                list = $"{id}{data}{author}{message}";

                var size = Console.WindowWidth - 2;

                if (list.Length > size)
                {
                    message = list.Substring(listWithoutMessage.Length, size - listWithoutMessage.Length - 2).TrimStart();
                }
                else
                {
                    message = list.Substring(listWithoutMessage.Length, message.Length).TrimStart();
                }

                Console.Write($"{message}");
                list = $"{id}{data}{author}{message}";
                addList.Add(list);
                Console.SetCursorPosition(0, indexes.heightPosition);
                Console.Write("│");
            }

            bool clear = false;
            if (indexes.up == true && indexes.heightPosition > 1)
            {
                indexes.currentCommitIndex--;
                indexes.heightPosition--;
            }

            CommitNumber.ReturnCommitNumber(listOfCommits, indexes);
            Cursor.UpdateCursorPositionList(listOfCommits, addList, indexes, blueFond);

            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughConsole(repo, indexes, listOfCommits, height, width);
        }

        public static void PrintNewCommitIfReachLimit(IntPtr repo, Indexes indexes, CommitElements listOfCommits, List<string> addList, int blueFond)
        {
            string message = string.Empty;
            int upAndDownConsole = indexes.heightPosition;
            int index = indexes.currentCommitIndex;
            indexes.startIndex = indexes.currentCommitIndex;
            while (indexes.heightPosition <= Console.WindowHeight - 2)
            {
                Console.SetCursorPosition(1, indexes.heightPosition);
                string list = string.Empty;
                string listWithoutMessage = string.Empty;

                ClearCommitRow(indexes);

                string id = string.Empty;
                id = $"{listOfCommits.Id[indexes.currentCommitIndex]} ";
                Console.Write($"{listOfCommits.Id[indexes.currentCommitIndex]} ", Console.ForegroundColor = ConsoleColor.Magenta);

                string data = string.Empty;
                if ($"{listOfCommits.DateTime[indexes.currentCommitIndex]} ".Length == 9)
                {
                    data = $"{listOfCommits.DateTime[indexes.currentCommitIndex]}{new string(' ', 2)} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }
                else
                {
                    data = $"{listOfCommits.DateTime[indexes.currentCommitIndex]} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }

                int authorLength = 20 - listOfCommits.Author[indexes.currentCommitIndex].Length;
                string author = $"{listOfCommits.Author[indexes.currentCommitIndex]}{new string(' ', authorLength)}";

                listWithoutMessage = $"{listOfCommits.Id[indexes.currentCommitIndex]}{data}{author}";

                if (listWithoutMessage.Length >= Console.WindowWidth / 2 + 7 - 2 && indexes.displayPanel == true)
                {
                    author = $"{author.Substring(0, 2)}..  ";
                    listWithoutMessage = $"{id}{data}{author}";
                    Console.Write($"{author}", Console.ForegroundColor = ConsoleColor.Green);
                }
                else
                {
                    Console.Write($"{author}", Console.ForegroundColor = ConsoleColor.Green);
                }

                Console.ResetColor();


                message = listOfCommits.Message[indexes.currentCommitIndex];
                list = $"{id}{data}{author}{message}";

                var size = new DrawPanel.CommitsPanel();

                if (list.Length > size.width && indexes.displayPanel == true)
                {
                    message = list.Substring(listWithoutMessage.Length, size.width - listWithoutMessage.Length - 2).TrimStart();
                }
                else if (list.Length > Console.WindowWidth - 2 && indexes.displayPanel == false)
                {
                    message = list.Substring(listWithoutMessage.Length, (Console.WindowWidth - 2) - listWithoutMessage.Length - 2).TrimStart();
                }
                else
                {
                    message = list.Substring(listWithoutMessage.Length, message.Length).TrimStart();
                }

                Console.Write($"{message}");
                list = $"{id}{data}{author}{message}";
                addList.Add(list);
                Console.SetCursorPosition(0, indexes.heightPosition);
                Console.Write("│");
                indexes.heightPosition++;
                if (indexes.up == true)
                {
                    indexes.currentCommitIndex++;
                }
                else
                {
                    indexes.currentCommitIndex++;
                }
            }

            indexes.heightPosition--;
            indexes.currentCommitIndex--;

            if (indexes.up == true)
            {
                indexes.heightPosition = upAndDownConsole;
                indexes.currentCommitIndex = index;
            }

            CommitNumber.ReturnCommitNumber(listOfCommits, indexes);

            if (indexes.displayPanel == true)
            {
                ClearMessagePanel();
                ClearFilePanel();
                bool clear = false;
                Navigate.CommitDetail(repo, indexes, listOfCommits, clear);
            }
            
            Cursor.UpdateCursorPositionList(listOfCommits, addList, indexes, blueFond);
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughConsole(repo, indexes, listOfCommits, height, width);
        }

        private static void ClearCommitRow(Indexes indexes)
        {
            if (indexes.displayPanel == true)
            {
                var commitPanel = new DrawPanel.CommitsPanel();
                Console.SetCursorPosition(1, indexes.heightPosition);
                Console.Write(new string(' ', commitPanel.width - 1));
            }
            else
            {
                var commitPanel = Console.WindowWidth - 2;
                Console.SetCursorPosition(1, indexes.heightPosition);
                Console.Write(new string(' ', commitPanel));
            }

            Console.SetCursorPosition(1, indexes.heightPosition);
        }

        private static void ClearMessagePanel()
        {
            var messagePanel = new DrawPanel.MessageBox();

            for (int y = 1; y < messagePanel.height; y++)
            {
                Console.SetCursorPosition(messagePanel.edgeOne + 1, y);
                Console.Write(new string(' ', messagePanel.width));
            }
        }

        private static void ClearFilePanel()
        {

            var filePanel = new DrawPanel.FilesBox();

            for (int x = 1; x < filePanel.height; x++)
            {
                Console.SetCursorPosition(filePanel.edgeOneX + 1, filePanel.edgeOneY + x);
                Console.Write(new string(' ', filePanel.width));
            }
        }
    }
}
