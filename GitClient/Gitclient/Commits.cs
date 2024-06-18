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

        public static void PrintCommits(IntPtr repo, bool panelAlreadyDisplayed, bool displayPanel, int heightPosition, int cursorPosionBiggerThenHeight, int upOrDownOneStep, int index, int rightCursor, int cursorPosition, ListOfCommits.CommitElements listOfCommits)
        {
            var addList = new List<string>();
            CommitNumber.ReturnCommitNumber(listOfCommits, upOrDownOneStep);
            var position = new DrawPanel.CommitsPanel();
            
            if (displayPanel == false)
            {
                if (heightPosition > position.height)
                {
                    index = heightPosition - position.height;
                }

                DisplayCommitsOnEntireConsole(repo, panelAlreadyDisplayed, addList, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition, listOfCommits);
            }
            else
            {
                DisplayCommitsWithPanel(repo, panelAlreadyDisplayed, addList, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition, listOfCommits);
            }
        }

        private static void DisplayCommitsOnEntireConsole(IntPtr repo, bool panelAlreadyDisplayed, List<string> addList, bool displayPanel, int commitNumber, int cursorPosionBiggerThenHeight, int upOrDownOneStep, int index, int rightCursor, int cursorPosition, ListOfCommits.CommitElements listOfCommits)
        {
            var element = new Elements();

            while (cursorPosition < Console.WindowHeight - 2 && index < listOfCommits.Id.Count && index >= 0)
            {
                Console.SetCursorPosition(1, cursorPosition + 1);
                element.Id = $"{listOfCommits.Id[index]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{listOfCommits.DateTime[index]} ";
                if (element.DateTime.Length == 8)
                {
                    element.DateTime = $"{listOfCommits.DateTime[index]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - listOfCommits.Author[index].Length;
                element.Author = $"{listOfCommits.Author[index]}";
                string author = $"{element.Author}{new string(' ', authorLength)}";
                Console.Write(author, Console.ForegroundColor = ConsoleColor.Green);
                Console.ResetColor();

                element.Message = $"{listOfCommits.Message[index]}";

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
                index++;
                cursorPosition++;
            }

            panelAlreadyDisplayed = false;
            rightCursor = index;
            Cursor.UpdateCursorPosition(displayPanel, panelAlreadyDisplayed, addList, commitNumber, listOfCommits, upOrDownOneStep);
            Navigate.NavigateThroughConsole(repo, panelAlreadyDisplayed, displayPanel, commitNumber, listOfCommits, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition);
        }

        private static void DisplayCommitsWithPanel(IntPtr repo, bool panelAlreadyDisplayed, List<string> addList, bool displayPanel, int commitNumber, int cursorPosionBiggerThenHeight, int upOrDownOneStep, int index, int rightCursor, int cursorPosition, ListOfCommits.CommitElements listOfCommits)
        {
            var element = new Elements();
            var size = new DrawPanel.CommitsPanel();
            panelAlreadyDisplayed = true;

            while (cursorPosition < size.height && index < listOfCommits.Id.Count && index >= 0)
            {
                string list = string.Empty;
                string listWithoutMessage = string.Empty;
                Console.SetCursorPosition(1, cursorPosition + 1);
                element.Id = $"{listOfCommits.Id[index]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{listOfCommits.DateTime[index]} ";
                if (element.DateTime.Length == 8)
                {
                    element.DateTime = $"{listOfCommits.DateTime[index]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - listOfCommits.Author[index].Length;
                element.Author = $"{listOfCommits.Author[index]}";
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
                element.Message = listOfCommits.Message[index];
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
                index++;
                cursorPosition++;
            }

            
            rightCursor = index;
            Cursor.UpdateCursorPosition(displayPanel, panelAlreadyDisplayed, addList, commitNumber, listOfCommits, upOrDownOneStep);
            Navigate.NavigateThroughConsole(repo, panelAlreadyDisplayed, displayPanel, commitNumber, listOfCommits, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition);
        }
    }
}