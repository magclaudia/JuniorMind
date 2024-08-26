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

        public static void PrintCommits(IntPtr repo, GetVariablesForCommits variablesForCommits, CommitElements listOfCommits)
        {
            GetCertainList list = new GetCertainList();
            GetCommitNumber.ReturnCommitNumber(listOfCommits, variablesForCommits);
            var position = new DrawPanelRigthSide.CommitsPanel();
            int blueFond = 0;
            if (variablesForCommits.displayPanel == false)
            {
                DisplayCommitsOnEntireConsole(repo, list, variablesForCommits, listOfCommits, blueFond);
            }
            else
            {
                DisplayCommitsWithPanel(repo, variablesForCommits, list, listOfCommits, blueFond);
            }
        }

        private static void DisplayCommitsOnEntireConsole(IntPtr repo, GetCertainList list, GetVariablesForCommits variablesForCommits, CommitElements listOfCommits, int blueFond)
        {
            var element = new Elements();
            variablesForCommits.rigthCursor = 0;
            int index = variablesForCommits.currentCommitIndex;
            variablesForCommits.currentCommitIndex = variablesForCommits.startIndex;

            while (variablesForCommits.rigthCursor < Console.WindowHeight - 2 && variablesForCommits.startIndex < listOfCommits.Id.Count && variablesForCommits.startIndex >= 0)
            {
                Console.SetCursorPosition(1, variablesForCommits.rigthCursor + 1);
                element.Id = $"{listOfCommits.Id[variablesForCommits.currentCommitIndex]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]} ";
                if (element.DateTime.Length == 9)
                {
                    element.DateTime = $"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - listOfCommits.Author[variablesForCommits.currentCommitIndex].Length;
                element.Author = $"{listOfCommits.Author[variablesForCommits.currentCommitIndex]}";
                string author = $"{element.Author}{new string(' ', authorLength)}";
                Console.Write(author, Console.ForegroundColor = ConsoleColor.Green);
                Console.ResetColor();

                element.Message = $"{listOfCommits.Message[variablesForCommits.currentCommitIndex]}".TrimEnd();
                element.Description = $"{listOfCommits.Description[variablesForCommits.currentCommitIndex]}".TrimEnd();
                string text = $"{element.Id}{element.DateTime}{author}{element.Message}{element.Description}";
                string listWithoutMessage = $"{element.Id}{element.DateTime}{author}";
                string message = CheckList(element.Description, element.Id, element.DateTime, author, element.Message);
                if (text.Length > Console.WindowWidth - 2)
                {
                    message = message.Substring(0, Console.WindowWidth - 2 - listWithoutMessage.Length - 2);
                }
                else
                {
                    message = message.Substring(0, element.Message.Length);
                }

                Console.Write(message);
                text = $"{element.Id}{element.DateTime}{author}{message}";
                list.addList.Add(text);
                variablesForCommits.currentCommitIndex++;
                variablesForCommits.rigthCursor++;
            }

            variablesForCommits.panelAlreadyDisplayed = false;
            variablesForCommits.currentCommitIndex = index;
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Cursor.UpdateCursorPositionList(listOfCommits, list.addList, variablesForCommits, blueFond);
            Navigate.NavigateThroughCommits(repo, variablesForCommits, listOfCommits, /*list,*/ height, width);
        }

        private static void DisplayCommitsWithPanel(IntPtr repo, GetVariablesForCommits variablesForcommits, GetCertainList list, CommitElements listOfCommits, int blueFond)
        {
            var element = new Elements();
            var size = new DrawPanelRigthSide.CommitsPanel();
            variablesForcommits.rigthCursor = 0;
            variablesForcommits.panelAlreadyDisplayed = true;
            int index = variablesForcommits.currentCommitIndex;
            variablesForcommits.currentCommitIndex = variablesForcommits.startIndex;

            while (variablesForcommits.rigthCursor < size.height && variablesForcommits.startIndex < listOfCommits.Id.Count && variablesForcommits.startIndex >= 0)
            {
                string text = string.Empty;
                string listWithoutMessage = string.Empty;
                Console.SetCursorPosition(1, variablesForcommits.rigthCursor + 1);
               
                element.Id = $"{listOfCommits.Id[variablesForcommits.currentCommitIndex]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{listOfCommits.DateTime[variablesForcommits.currentCommitIndex]} ";
                if (element.DateTime.Length == 9)
                {
                    element.DateTime = $"{listOfCommits.DateTime[variablesForcommits.currentCommitIndex]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - listOfCommits.Author[variablesForcommits.currentCommitIndex].Length;
                element.Author = $"{listOfCommits.Author[variablesForcommits.currentCommitIndex]}";
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

                
                element.Message = listOfCommits.Message[variablesForcommits.currentCommitIndex].TrimEnd();
                element.Description = listOfCommits.Description[variablesForcommits.currentCommitIndex].TrimEnd();
                string message = CheckList(element.Description, element.Id, element.DateTime, author, element.Message); 
                text = $"{element.Id}{element.DateTime}{author}{message}";
                if (text.Length >= size.width)
                {
                    message = text.Substring(listWithoutMessage.Length, size.width - listWithoutMessage.Length - 3);
                }
                else
                {
                    message = text.Substring(listWithoutMessage.Length, element.Message.Length);
                }

                Console.Write($"{message}");
                text = $"{element.Id}{element.DateTime}{author}{message}";
                list.addList.Add(text);
                variablesForcommits.rigthCursor++;
                variablesForcommits.currentCommitIndex++;
            }

            variablesForcommits.rigthCursor = variablesForcommits.currentCommitIndex;
            variablesForcommits.currentCommitIndex = index;
            Cursor.UpdateCursorPositionList(listOfCommits, list.addList, variablesForcommits, blueFond);
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughCommits(repo, variablesForcommits, listOfCommits,/* list, */height, width);
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
