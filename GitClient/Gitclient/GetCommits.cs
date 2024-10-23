
namespace GitClient
{
    public class GetCommits
    {
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public struct Elements
        {
            public string Id;
            public string DateTime;
            public string Author;
            public string Message;
            public string Description;
        }

        public static void PrintCommits(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list)
        {
            GetCommitNumber.ReturnCommitNumber(commitElement, variablesForCommits);
            dimensions = new DrawTabs.Dimensions();
            int blueFond = 0;
            variablesForCommits.height = Console.WindowHeight;
            variablesForCommits.width = Console.WindowWidth;
           
            if (variablesForCommits.displayPanel == false)
            {
                DisplayCommitsOnEntireConsole(variablesForCommits, variablesForFiles, commitElement, list, blueFond);
            }
            else
            {
                DisplayCommitsWithPanel(variablesForCommits, variablesForFiles, commitElement, list, blueFond);
            }
        }

        private static void DisplayCommitsOnEntireConsole(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, int blueFond)
        {
            var element = new Elements();
            variablesForCommits.rigthCursor = 0;
            int index = variablesForCommits.currentCommitIndex;

            if (variablesForCommits.currentCommitIndex > variablesForCommits.heightPosition)
            {
                variablesForCommits.currentCommitIndex = variablesForCommits.currentCommitIndex - variablesForCommits.heightPosition + 1;
            }
            else
            {
                variablesForCommits.currentCommitIndex = 0;
            }

            string text = "";

            while (variablesForCommits.rigthCursor < (variablesForCommits.height - dimensions.tabHeight) - 3)
            {
                Console.SetCursorPosition(1, variablesForCommits.rigthCursor + dimensions.tabHeight + 2);
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
                text = $"{element.Id}{element.DateTime}{author}{element.Message}{element.Description}";
                string listWithoutMessage = $"{element.Id}{element.DateTime}{author}";
                string message = GetMessage(element.Description, element.Id, element.DateTime, author, element.Message);
                
                if (text.Length > Console.WindowWidth - 2)
                {
                    message = message.Substring(0, Console.WindowWidth - 2 - listWithoutMessage.Length - 2).TrimEnd();
                }
                else
                {
                    message = message.Substring(0, element.Message.Length).TrimEnd();
                }

                Console.Write(message);
                text = $"{element.Id}{element.DateTime}{author}{message}";
                
                if (variablesForCommits.currentCommitIndex == index)
                {
                    variablesForCommits.textForBlueFond = text;
                }

                variablesForCommits.currentCommitIndex++;
                variablesForCommits.rigthCursor++;
            }

            variablesForCommits.panelAlreadyDisplayed = false;
            variablesForCommits.currentCommitIndex = index;
            variablesForCommits.logTab = true;
            GetVariablesForTabs tab = new GetVariablesForTabs();
            Cursor.UpdateCursorPositionForCommitsList(commitElement, variablesForCommits, list, blueFond);
            Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElement, list, tab);
        }

        private static void DisplayCommitsWithPanel(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, int blueFond)
        {
            var element = new Elements();
            var size = new DrawPanelRigthSide.CommitsPanel();
            variablesForCommits.rigthCursor = 0;
            variablesForCommits.panelAlreadyDisplayed = true;
            int index = variablesForCommits.currentCommitIndex;
            int i = 0;
            if (variablesForCommits.currentCommitIndex > variablesForCommits.heightPosition)
            {
                variablesForCommits.currentCommitIndex = variablesForCommits.currentCommitIndex - variablesForCommits.heightPosition + 1;
            }
            else
            {
                variablesForCommits.currentCommitIndex = 0;
            }

            while (variablesForCommits.rigthCursor < size.height - dimensions.tabHeight - 1)
            {
                string text = string.Empty;
                string listWithoutMessage = string.Empty;
                Console.SetCursorPosition(1, variablesForCommits.rigthCursor + 1 + dimensions.tabHeight + 1);

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
                string message = GetMessage(element.Description, element.Id, element.DateTime, author, element.Message);
                text = $"{element.Id}{element.DateTime}{author}{message}";
                
                if (text.Length >= size.width)
                {
                    message = text.Substring(listWithoutMessage.Length, size.width - listWithoutMessage.Length - 2);
                }
                else
                {
                    if (listWithoutMessage.Length + 1 + message.Length == text.Length + 1)
                    {
                        message = text.Substring(listWithoutMessage.Length, message.Length);
                    }
                    else
                    {
                        message = text.Substring(listWithoutMessage.Length, message.Length + 1);
                    }
                }

                Console.Write($"{message}");
                text = $"{element.Id}{element.DateTime}{author}{message}";

                if (variablesForCommits.currentCommitIndex == index)
                {
                    variablesForCommits.textForBlueFond = text;
                }

                variablesForCommits.rigthCursor++;
                variablesForCommits.currentCommitIndex++;
                i++;
            }

            variablesForCommits.rigthCursor = variablesForCommits.currentCommitIndex;
            variablesForCommits.currentCommitIndex = index;
            GetVariablesForTabs tabs = new GetVariablesForTabs();
            Cursor.UpdateCursorPositionForCommitsList(commitElement, variablesForCommits, list, blueFond);
            Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElement, list, tabs);
        }

        private static string GetMessage(string description, string id, string data, string author, string message)
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
