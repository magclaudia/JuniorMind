using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class Commits
    {
        public struct Elements
        {
            public string Id;
            public string DateTime;
            public string Author;
            public string Message;
        }

        public static void PrintCommits(IntPtr repo, Indexes indexes, CommitElements listOfCommits)
        {
            var addList = new List<string>();
            CommitNumber.ReturnCommitNumber(listOfCommits, indexes);
            var position = new DrawPanel.CommitsPanel();
            int blueFond = 0;
            if (indexes.displayPanel == false)
            {
                DisplayCommitsOnEntireConsole(repo, addList, indexes, listOfCommits, blueFond);
            }
            else
            {
                DisplayCommitsWithPanel(repo, indexes, addList, listOfCommits, blueFond);
            }
        }

        private static void DisplayCommitsOnEntireConsole(IntPtr repo, List<string> addList, Indexes indexes, CommitElements listOfCommits, int blueFond)
        {
            var element = new Elements();
            indexes.cursorPosition = 0;
            while (indexes.cursorPosition < Console.WindowHeight - 2 && indexes.numberOfCommits < listOfCommits.Id.Count && indexes.numberOfCommits >= 0)
            {
                Console.SetCursorPosition(1, indexes.cursorPosition + 1);
                element.Id = $"{listOfCommits.Id[indexes.currentCommitIndex]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{listOfCommits.DateTime[indexes.currentCommitIndex]} ";
                if (element.DateTime.Length == 9)
                {
                    element.DateTime = $"{listOfCommits.DateTime[indexes.currentCommitIndex]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - listOfCommits.Author[indexes.currentCommitIndex].Length;
                element.Author = $"{listOfCommits.Author[indexes.currentCommitIndex]}";
                string author = $"{element.Author}{new string(' ', authorLength)}";
                Console.Write(author, Console.ForegroundColor = ConsoleColor.Green);
                Console.ResetColor();

                element.Message = $"{listOfCommits.Message[indexes.currentCommitIndex]}";

                string message = string.Empty;
                string list = $"{element.Id}{element.DateTime}{author}{element.Message}";
                string listWithoutMessage = $"{element.Id}{element.DateTime}{author}";

                if (list.Length > Console.WindowWidth - 2)
                {
                    message = element.Message.Substring(0, Console.WindowWidth - 2 - listWithoutMessage.Length - 1);
                }
                else
                {
                    message = element.Message.Substring(0, element.Message.Length);
                }

                Console.Write(message);
                list = $"{element.Id}{element.DateTime}{author}{message}";
                addList.Add(list);
                indexes.numberOfCommits++;
                indexes.currentCommitIndex++;
                indexes.cursorPosition++;
            }

            indexes.cursorPosition = 1;
            indexes.panelAlreadyDisplayed = false;
            indexes.currentCommitIndex = 0;
            indexes.rightCursor = indexes.currentCommitIndex;
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Cursor.UpdateCursorPositionList(listOfCommits, addList, indexes, blueFond);
            Navigate.NavigateThroughConsole(repo, indexes, listOfCommits, height, width);
        }

        private static void DisplayCommitsWithPanel(IntPtr repo, Indexes indexes, List<string> addList, CommitElements listOfCommits, int blueFond)
        {
            var element = new Elements();
            var size = new DrawPanel.CommitsPanel();
            indexes.cursorPosition = 0;
            indexes.panelAlreadyDisplayed = true;
            while (indexes.cursorPosition < size.height && indexes.numberOfCommits < listOfCommits.Id.Count && indexes.numberOfCommits >= 0)
            {
                string list = string.Empty;
                string listWithoutMessage = string.Empty;
                Console.SetCursorPosition(1, indexes.cursorPosition + 1);
               
                element.Id = $"{listOfCommits.Id[indexes.currentCommitIndex]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{listOfCommits.DateTime[indexes.currentCommitIndex]} ";
                if (element.DateTime.Length == 9)
                {
                    element.DateTime = $"{listOfCommits.DateTime[indexes.currentCommitIndex]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - listOfCommits.Author[indexes.currentCommitIndex].Length;
                element.Author = $"{listOfCommits.Author[indexes.currentCommitIndex]}";
                string author = $"{element.Author}{new string(' ', authorLength)}";

                listWithoutMessage = $"{element.Id}{element.DateTime}{author}";

                if (listWithoutMessage.Length >= Console.WindowWidth / 2 + 7 - 2)
                {
                    author = $"{element.Author.Substring(0, 2)}..  ";
                    listWithoutMessage = $"{element.Id}{element.DateTime}{author}";
                    Console.Write($"{author}", Console.ForegroundColor = ConsoleColor.Green);
                }
                else
                {
                    Console.Write($"{author}", Console.ForegroundColor = ConsoleColor.Green);
                }

                Console.ResetColor();

                string message;
                element.Message = listOfCommits.Message[indexes.currentCommitIndex];
                list = $"{element.Id}{element.DateTime}{author}{element.Message}";


                if (list.Length > size.width)
                {
                    message = list.Substring(listWithoutMessage.Length, size.width - listWithoutMessage.Length - 2);
                }
                else
                {
                    message = list.Substring(listWithoutMessage.Length, element.Message.Length);
                }

                Console.Write($"{message}");
                list = $"{element.Id}{element.DateTime}{author}{message}";
                addList.Add(list);
                indexes.numberOfCommits++;
                indexes.cursorPosition++;
                indexes.currentCommitIndex++;
            }

            indexes.cursorPosition = 1;
            indexes.rightCursor = indexes.currentCommitIndex;
            indexes.currentCommitIndex = 0;
            Cursor.UpdateCursorPositionList(listOfCommits, addList, indexes, blueFond);
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughConsole(repo, indexes, listOfCommits, height, width);
        }
    } 
}
