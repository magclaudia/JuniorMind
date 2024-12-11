namespace GitClient
{
    public class ReplaceEachCommitOneByOne
    {
        public static void PrintNewCommitIfPanel(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, bool reachLimit, int blueFond)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    variablesForCommits.heightPosition++;
                    variablesForCommits.currentCommitIndex++;
                }

                Console.SetCursorPosition(1, variablesForCommits.heightPosition);

                if (i == 0)
                {
                    ClearCommitRow(variablesForCommits);
                }

                string id = $"{commitElement.Id[variablesForCommits.currentCommitIndex]} ";
                Console.Write($"{commitElement.Id[variablesForCommits.currentCommitIndex]} ", Console.ForegroundColor = ConsoleColor.Magenta);

                string data;
                if ($"{commitElement.DateTime[variablesForCommits.currentCommitIndex]} ".Length == 9)
                {
                    data = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]}{new string(' ', 2)} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }
                else
                {
                    data = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }

                int authorLength = 20 - commitElement.Author[variablesForCommits.currentCommitIndex].Length;
                string author = $"{commitElement.Author[variablesForCommits.currentCommitIndex]}{new string(' ', authorLength)}";

                string listWithoutMessage = $"{commitElement.Id[variablesForCommits.currentCommitIndex]}{data}{author}";
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

                string message = commitElement.Message[variablesForCommits.currentCommitIndex].TrimEnd();
                string description = commitElement.Description[variablesForCommits.currentCommitIndex].TrimEnd();
                message = ReturnMessage(description, message);
                string text = $"{id}{data}{author}{message}";
                var size = new DrawPanelRigthSide.CommitsPanel();

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
                        message = text.Substring(listWithoutMessage.Length + 1, message.Length);
                    }
                }

                Console.Write($"{message.TrimStart()}");
                text = $"{id}{data}{author}{message.TrimStart()}";

                if (i == 0 && variablesForCommits.up == true)
                {
                    variablesForCommits.textForBlueFond = text;
                }
                else if (i == 1 && variablesForCommits.down == true)
                {
                    variablesForCommits.textForBlueFond = text;
                }
            }


            ClearMessagePanel();
            ClearFilePanel();
            bool clear = false;

            if (variablesForCommits.up == true && variablesForCommits.heightPosition > 1)
            {
                variablesForCommits.currentCommitIndex--;
                variablesForCommits.heightPosition--;
            }

            GetCommitNumber.ReturnCommitNumber(commitElement, variablesForCommits);
            Navigate.GetCommitDetails(variablesForCommits, variablesForFiles, commitElement, list, clear);
            Cursor.UpdateCursorPositionForCommitsList(commitElement, variablesForCommits, list, blueFond);
            Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElement, list);
        }

        public static void PrintNewCommitIfNoPanel(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, bool reachLimit, int blueFond)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    variablesForCommits.heightPosition++;
                    variablesForCommits.currentCommitIndex++;
                }

                Console.SetCursorPosition(1, variablesForCommits.heightPosition);
                string listWithoutMessage = string.Empty;

                if (i == 0)
                {
                    ClearCommitRow(variablesForCommits);
                }

                string id = $"{commitElement.Id[variablesForCommits.currentCommitIndex]} ";
                Console.Write($"{commitElement.Id[variablesForCommits.currentCommitIndex]} ", Console.ForegroundColor = ConsoleColor.Magenta);

                string data;

                if ($"{commitElement.DateTime[variablesForCommits.currentCommitIndex]} ".Length == 9)
                {
                    data = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]}{new string(' ', 2)} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }
                else
                {
                    data = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }

                int authorLength = 20 - commitElement.Author[variablesForCommits.currentCommitIndex].Length;
                string author = $"{commitElement.Author[variablesForCommits.currentCommitIndex]}{new string(' ', authorLength)}";

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

                listWithoutMessage = $"{commitElement.Id[variablesForCommits.currentCommitIndex]}{data}{author}";
                string message = commitElement.Message[variablesForCommits.currentCommitIndex].TrimEnd();
                string description = commitElement.Description[variablesForCommits.currentCommitIndex].TrimEnd();
                message = ReturnMessage(description, message);
                string text = $"{id}{data}{author}{message}";
                var size = Console.WindowWidth - 2;

                if (text.Length > size)
                {
                    message = text.Substring(listWithoutMessage.Length, size - listWithoutMessage.Length - 2).TrimStart();
                }
                else
                {
                    message = text.Substring(listWithoutMessage.Length, message.Length + 1).TrimStart();
                }

                Console.Write($"{message}");

                text = $"{id}{data}{author}{message}";

                if (i == 1 && variablesForCommits.down == true)
                {
                    variablesForCommits.textForBlueFond = text;
                }
                else if (i == 0 && variablesForCommits.up == true)
                {
                    variablesForCommits.textForBlueFond = text;
                }
            }

            if (variablesForCommits.up == true && variablesForCommits.heightPosition > 1)
            {
                variablesForCommits.currentCommitIndex--;
                variablesForCommits.heightPosition--;
            }

            GetCommitNumber.ReturnCommitNumber(commitElement, variablesForCommits);
            Cursor.UpdateCursorPositionForCommitsList(commitElement, variablesForCommits, list, blueFond);
            Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElement, list);
        }

        public static void PrintNewCommitIfReachLimit(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, int blueFond)
        {
            variablesForCommits.height = Console.WindowHeight;
            variablesForCommits.width = Console.WindowWidth;
            int upAndDownConsole = variablesForCommits.heightPosition;
            int index = variablesForCommits.currentCommitIndex;
            int i = 0;
            int count = 0;

            if (variablesForCommits.down == true)
            {
                i = 1;
            }

            while (variablesForCommits.heightPosition <= variablesForCommits.height - 2)
            {
                if (variablesForCommits.heightPosition == 3)
                {
                    variablesForCommits.firstCommitInLine = variablesForCommits.currentCommitIndex;
                }

                Console.SetCursorPosition(1, variablesForCommits.heightPosition);
                ClearCommitRow(variablesForCommits);

                string id = $"{commitElement.Id[variablesForCommits.currentCommitIndex]} ";
                Console.Write($"{commitElement.Id[variablesForCommits.currentCommitIndex]} ", Console.ForegroundColor = ConsoleColor.Magenta);

                string data;
                if ($"{commitElement.DateTime[variablesForCommits.currentCommitIndex]} ".Length == 9)
                {
                    data = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]}{new string(' ', 2)} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }
                else
                {
                    data = $"{commitElement.DateTime[variablesForCommits.currentCommitIndex]} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }

                int authorLength = 20 - commitElement.Author[variablesForCommits.currentCommitIndex].Length;
                string author = $"{commitElement.Author[variablesForCommits.currentCommitIndex]}{new string(' ', authorLength)}";

                string listWithoutMessage = $"{commitElement.Id[variablesForCommits.currentCommitIndex]}{data}{author}";
                if (listWithoutMessage.Length >= Console.WindowWidth / 2 + 7 - 2 && variablesForCommits.displayPanel == true)
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

                string message = commitElement.Message[variablesForCommits.currentCommitIndex].TrimEnd();
                string description = commitElement.Description[variablesForCommits.currentCommitIndex].TrimEnd();
                message = ReturnMessage(description, message);
                string text = $"{id}{data}{author}{message}";
                var size = new DrawPanelRigthSide.CommitsPanel();

                if (text.Length >= size.width && variablesForCommits.displayPanel == true)
                {
                    message = text.Substring(listWithoutMessage.Length, size.width - listWithoutMessage.Length - 2);
                }
                else if (text.Length > Console.WindowWidth - 2 && variablesForCommits.displayPanel == false)
                {
                    message = text.Substring(listWithoutMessage.Length, (Console.WindowWidth - 2) - listWithoutMessage.Length - 2);
                }
                else if (text.Length - listWithoutMessage.Length == message.Length)
                {
                    message = text.Substring(listWithoutMessage.Length, message.Length);
                }
                else
                {
                    message = text.Substring(listWithoutMessage.Length, message.Length + 1);
                }

                Console.Write($"{message.TrimStart()}");
                text = $"{id}{data}{author}{message.TrimStart()}";

                if (i == 1 && variablesForCommits.down == true)
                {
                    variablesForCommits.textForBlueFond = text;
                }
                else if (count == 0 && variablesForCommits.up == true)
                {
                    variablesForCommits.textForBlueFond = text;
                }

                variablesForCommits.heightPosition++;
                variablesForCommits.currentCommitIndex++;
                count++;
            }

            variablesForCommits.heightPosition--;
            variablesForCommits.currentCommitIndex--;

            if (variablesForCommits.up == true)
            {
                variablesForCommits.heightPosition = upAndDownConsole;
                variablesForCommits.currentCommitIndex = index;
            }

            GetCommitNumber.ReturnCommitNumber(commitElement, variablesForCommits);

            if (variablesForCommits.displayPanel == true)
            {
                ClearMessagePanel();
                ClearFilePanel();
                bool clear = false;
                Navigate.GetCommitDetails(variablesForCommits, variablesForFiles, commitElement, list, clear);
            }

            Cursor.UpdateCursorPositionForCommitsList(commitElement, variablesForCommits, list, blueFond);
            Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElement, list);
        }

        public static void ClearCommitRow(GetVariablesForCommits variablesForCommits)
        {
            if (variablesForCommits.displayPanel == true)
            {
                var commitPanel = new DrawPanelRigthSide.CommitsPanel();
                Console.SetCursorPosition(1, variablesForCommits.heightPosition);
                Console.Write(new string(' ', commitPanel.width - 1));
            }
            else
            {
                var commitPanel = Console.WindowWidth - 2;
                Console.SetCursorPosition(1, variablesForCommits.heightPosition);
                Console.Write(new string(' ', commitPanel));
            }

            Console.SetCursorPosition(1, variablesForCommits.heightPosition);
        }

        public static void ClearMessagePanel()
        {
            var messagePanel = new DrawPanelRigthSide.MessageBox();
            int height = Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2 - 1);
            for (int y = height; y < messagePanel.height + 1; y++)
            {
                Console.SetCursorPosition(messagePanel.edgeOne + 1, y);
                Console.Write(new string(' ', messagePanel.width));
            }
        }

        public static void ClearFilePanel()
        {
            var filePanel = new DrawPanelRigthSide.FilesBox();

            for (int x = 1; x < filePanel.height - 1; x++)
            {
                Console.SetCursorPosition(filePanel.edgeOneX + 1, filePanel.edgeOneY + x);
                Console.Write(new string(' ', filePanel.width));
            }
        }

        public static string ReturnMessage(string description, string message)
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
