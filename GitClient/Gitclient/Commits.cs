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

        public static void PrintCommits(IntPtr repo, ListOfCommits.Indexes indexes, ListOfCommits.CommitElements listOfCommits)
        {
            var addList = new List<string>();
            CommitNumber.ReturnCommitNumber(listOfCommits, indexes);
            var position = new DrawPanel.CommitsPanel();

            if (indexes.displayPanel == false)
            {
                if (indexes.heightPosition > position.height)
                {
                    indexes.index = indexes.heightPosition - position.height;
                }

                DisplayCommitsOnEntireConsole(repo, addList, indexes, listOfCommits);
            }
            else
            {
                DisplayCommitsWithPanel(repo, indexes, addList, listOfCommits);
            }
        }

        private static void DisplayCommitsOnEntireConsole(IntPtr repo, List<string> addList, ListOfCommits.Indexes indexes, ListOfCommits.CommitElements listOfCommits)
        {
            var element = new Elements();

            while (indexes.cursorPosition < Console.WindowHeight - 2 && indexes.index < listOfCommits.Id.Count && indexes.index >= 0)
            {
                Console.SetCursorPosition(1, indexes.cursorPosition + 1);
                element.Id = $"{listOfCommits.Id[indexes.index]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{listOfCommits.DateTime[indexes.index]} ";
                if (element.DateTime.Length == 9)
                {
                    element.DateTime = $"{listOfCommits.DateTime[indexes.index]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - listOfCommits.Author[indexes.index].Length;
                element.Author = $"{listOfCommits.Author[indexes.index]}";
                string author = $"{element.Author}{new string(' ', authorLength)}";
                Console.Write(author, Console.ForegroundColor = ConsoleColor.Green);
                Console.ResetColor();

                element.Message = $"{listOfCommits.Message[indexes.index]}";

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
                indexes.index++;
                indexes.cursorPosition++;
            }

            indexes.panelAlreadyDisplayed = false;
            indexes.rightCursor = indexes.index;
            Cursor.UpdateCursorPositionList(listOfCommits, addList, indexes);
            Navigate.NavigateThroughConsole(repo, indexes, listOfCommits);
        }

        private static void DisplayCommitsWithPanel(IntPtr repo, ListOfCommits.Indexes indexes, List<string> addList, ListOfCommits.CommitElements listOfCommits)
        {
            var element = new Elements();
            var size = new DrawPanel.CommitsPanel();
            indexes.panelAlreadyDisplayed = true;

            while (indexes.cursorPosition < size.height && indexes.index < listOfCommits.Id.Count && indexes.index >= 0)
            {
                string list = string.Empty;
                string listWithoutMessage = string.Empty;
                Console.SetCursorPosition(1, indexes.cursorPosition + 1);
               
                element.Id = $"{listOfCommits.Id[indexes.index]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{listOfCommits.DateTime[indexes.index]} ";
                if (element.DateTime.Length == 9)
                {
                    element.DateTime = $"{listOfCommits.DateTime[indexes.index]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - listOfCommits.Author[indexes.index].Length;
                element.Author = $"{listOfCommits.Author[indexes.index]}";
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
                element.Message = listOfCommits.Message[indexes.index];
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
                indexes.index++;
                indexes.cursorPosition++;
            }


            indexes.rightCursor = indexes.index;
            Cursor.UpdateCursorPositionList(listOfCommits, addList, indexes);
            Navigate.NavigateThroughConsole(repo, indexes, listOfCommits);
        } 
    }
}
