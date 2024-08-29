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

        public static void PrintCommits(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement)
        {
            var addList = new List<string>();
            GetCommitNumber.ReturnCommitNumber(commitElement, variablesForCommits);
            var position = new DrawPanelRigthSide.CommitsPanel();
            int blueFond = 0;
            if (variablesForCommits.displayPanel == false)
            {
                DisplayCommitsOnEntireConsole(addList, variablesForCommits, variablesForFiles, commitElement, blueFond);
            }
            else
            {
                DisplayCommitsWithPanel(variablesForCommits, variablesForFiles, addList, commitElement, blueFond);
            }
        }

        private static void DisplayCommitsOnEntireConsole(List<string> addList, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, int blueFond)
        {
            var element = new Elements();
            variablesForCommits.rigthCursor = 0;
            int index = variablesForCommits.currentCommitIndex;
            variablesForCommits.currentCommitIndex = variablesForCommits.startIndex;

            while (variablesForCommits.rigthCursor < Console.WindowHeight - 2 && variablesForCommits.startIndex < commitElement.Id.Count && variablesForCommits.startIndex >= 0)
            {
                Console.SetCursorPosition(1, variablesForCommits.rigthCursor + 1);
                element.Id = $"{commitElement.Id[variablesForCommits.currentCommitIndex]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]} ";
                if (element.DateTime.Length == 9)
                {
                    element.DateTime = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - commitElement.Author[variablesForCommits.currentCommitIndex].Length;
                element.Author = $"{commitElement.Author[variablesForCommits.currentCommitIndex]}";
                string author = $"{element.Author}{new string(' ', authorLength)}";
                Console.Write(author, Console.ForegroundColor = ConsoleColor.Green);
                Console.ResetColor();

                element.Message = $"{commitElement.Message[variablesForCommits.currentCommitIndex]}".TrimEnd();
                element.Description = $"{commitElement.Description[variablesForCommits.currentCommitIndex]}".TrimEnd();
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
                addList.Add(text);
                variablesForCommits.currentCommitIndex++;
                variablesForCommits.rigthCursor++;
            }

            variablesForCommits.panelAlreadyDisplayed = false;
            variablesForCommits.currentCommitIndex = index;
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Cursor.UpdateCursorPositionForCommitsList(commitElement, addList, variablesForCommits, blueFond);
            Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElement, height, width);
        }

        private static void DisplayCommitsWithPanel(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, List<string> addList, CommitElements commitElement, int blueFond)
        {
            var element = new Elements();
            var size = new DrawPanelRigthSide.CommitsPanel();
            variablesForCommits.rigthCursor = 0;
            variablesForCommits.panelAlreadyDisplayed = true;
            int index = variablesForCommits.currentCommitIndex;
            variablesForCommits.currentCommitIndex = variablesForCommits.startIndex;

            while (variablesForCommits.rigthCursor < size.height && variablesForCommits.startIndex < commitElement.Id.Count && variablesForCommits.startIndex >= 0)
            {
                string text = string.Empty;
                string listWithoutMessage = string.Empty;
                Console.SetCursorPosition(1, variablesForCommits.rigthCursor + 1);

                element.Id = $"{commitElement.Id[variablesForCommits.currentCommitIndex]} ";
                Console.Write(element.Id, Console.ForegroundColor = ConsoleColor.Magenta);

                element.DateTime = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]} ";
                if (element.DateTime.Length == 9)
                {
                    element.DateTime = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]}{new string(' ', 2)} ";
                }

                Console.Write(element.DateTime, Console.ForegroundColor = ConsoleColor.Cyan);

                int authorLength = 20 - commitElement.Author[variablesForCommits.currentCommitIndex].Length;
                element.Author = $"{commitElement.Author[variablesForCommits.currentCommitIndex]}";
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


                element.Message = commitElement.Message[variablesForCommits.currentCommitIndex].TrimEnd();
                element.Description = commitElement.Description[variablesForCommits.currentCommitIndex].TrimEnd();
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
                addList.Add(text);
                variablesForCommits.rigthCursor++;
                variablesForCommits.currentCommitIndex++;
            }

            variablesForCommits.rigthCursor = variablesForCommits.currentCommitIndex;
            variablesForCommits.currentCommitIndex = index;
            Cursor.UpdateCursorPositionForCommitsList(commitElement, addList, variablesForCommits, blueFond);
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElement, height, width);
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
