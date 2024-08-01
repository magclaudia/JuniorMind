using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class GetCommits
    {
        public struct Elements
        {
            public string Id;
            public string DateTime;
            public string Author;
            public string Message;
            public string Description;
        }

        public static void PrintCommits(IntPtr repo, VariablesForCommits indexes, CommitElements listOfCommits)
        {
            var addList = new List<string>();
            GetCommitNumber.ReturnCommitNumber(listOfCommits, indexes);
            var position = new DrawPanelRigthSide.CommitsPanel();
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

        private static void DisplayCommitsOnEntireConsole(IntPtr repo, List<string> addList, VariablesForCommits indexes, CommitElements listOfCommits, int blueFond)
        {
            var element = new Elements();
            indexes.rigthCursor = 0;
            int index = indexes.currentCommitIndex;
            indexes.currentCommitIndex = indexes.startIndex;

            while (indexes.rigthCursor < Console.WindowHeight - 2 && indexes.startIndex < listOfCommits.Id.Count && indexes.startIndex >= 0)
            {
                Console.SetCursorPosition(1, indexes.rigthCursor + 1);
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

                element.Message = $"{listOfCommits.Message[indexes.currentCommitIndex]}".TrimEnd();
                element.Description = $"{listOfCommits.Description[indexes.currentCommitIndex]}".TrimEnd();
                string list = $"{element.Id}{element.DateTime}{author}{element.Message}{element.Description}";
                string listWithoutMessage = $"{element.Id}{element.DateTime}{author}";
                string message = CheckList(element.Description, element.Id, element.DateTime, author, element.Message);
                if (list.Length > Console.WindowWidth - 2)
                {
                    message = message.Substring(0, Console.WindowWidth - 2 - listWithoutMessage.Length - 2);
                }
                else
                {
                    message = message.Substring(0, element.Message.Length);
                }

                Console.Write(message);
                list = $"{element.Id}{element.DateTime}{author}{message}";
                addList.Add(list);
                indexes.currentCommitIndex++;
                indexes.rigthCursor++;
            }

            indexes.panelAlreadyDisplayed = false;
            indexes.currentCommitIndex = index;
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Cursor.UpdateCursorPositionList(listOfCommits, addList, indexes, blueFond);
            Navigate.NavigateThroughCommits(repo, indexes, listOfCommits, height, width);
        }

        private static void DisplayCommitsWithPanel(IntPtr repo, VariablesForCommits indexes, List<string> addList, CommitElements listOfCommits, int blueFond)
        {
            var element = new Elements();
            var size = new DrawPanelRigthSide.CommitsPanel();
            indexes.rigthCursor = 0;
            indexes.panelAlreadyDisplayed = true;
            int index = indexes.currentCommitIndex;
            indexes.currentCommitIndex = indexes.startIndex;

            while (indexes.rigthCursor < size.height && indexes.startIndex < listOfCommits.Id.Count && indexes.startIndex >= 0)
            {
                string list = string.Empty;
                string listWithoutMessage = string.Empty;
                Console.SetCursorPosition(1, indexes.rigthCursor + 1);
               
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

                
                element.Message = listOfCommits.Message[indexes.currentCommitIndex].TrimEnd();
                element.Description = listOfCommits.Description[indexes.currentCommitIndex].TrimEnd();
                string message = CheckList(element.Description, element.Id, element.DateTime, author, element.Message); 
                list = $"{element.Id}{element.DateTime}{author}{message}";
                if (list.Length >= size.width)
                {
                    message = list.Substring(listWithoutMessage.Length, size.width - listWithoutMessage.Length - 3);
                }
                else
                {
                    message = list.Substring(listWithoutMessage.Length, element.Message.Length);
                }

                Console.Write($"{message}");
                list = $"{element.Id}{element.DateTime}{author}{message}";
                addList.Add(list);
                indexes.rigthCursor++;
                indexes.currentCommitIndex++;
            }

            indexes.rigthCursor = indexes.currentCommitIndex;
            indexes.currentCommitIndex = index;
            Cursor.UpdateCursorPositionList(listOfCommits, addList, indexes, blueFond);
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughCommits(repo, indexes, listOfCommits, height, width);
        }

        private static string CheckList(string description, string id, string data, string author, string message)
        {
            if (description != "")
            {
                message = $"{message}.Description: {description}";
            }
            else
            {
                message = $"{message}";
            }

            return message;
        }
    } 
}
