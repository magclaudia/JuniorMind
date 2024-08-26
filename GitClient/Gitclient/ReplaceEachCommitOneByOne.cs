namespace GitClient
{
    public class ReplaceEachCommitOneByOne
    {
        public static void PrintNewCommitIfPanel(IntPtr repo, GetVariablesForCommits variablesForCommits, CommitElements listOfCommits, List<string> addList, bool reachLimit, int blueFond)
        {
            string message = string.Empty;

            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    variablesForCommits.heightPosition++;
                    variablesForCommits.currentCommitIndex++;
                }

                Console.SetCursorPosition(1, variablesForCommits.heightPosition);
                string text = string.Empty;
                string listWithoutMessage = string.Empty;

                if (i == 0)
                {
                    ClearCommitRow(variablesForCommits);
                }

                string id = string.Empty;
                id = $"{listOfCommits.Id[variablesForCommits.currentCommitIndex]} ";
                Console.Write($"{listOfCommits.Id[variablesForCommits.currentCommitIndex]} ", Console.ForegroundColor = ConsoleColor.Magenta);

                string data = string.Empty;
                if ($"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]} ".Length == 9)
                {
                    data = $"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]}{new string(' ', 2)} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }
                else
                {
                    data = $"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }

                int authorLength = 20 - listOfCommits.Author[variablesForCommits.currentCommitIndex].Length;
                string author = $"{listOfCommits.Author[variablesForCommits.currentCommitIndex]}{new string(' ', authorLength)}";

                listWithoutMessage = $"{listOfCommits.Id[variablesForCommits.currentCommitIndex]}{data}{author}";

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

                message = listOfCommits.Message[variablesForCommits.currentCommitIndex].TrimEnd();
                string description = listOfCommits.Description[variablesForCommits.currentCommitIndex].TrimEnd();
                message = ReturnMessage(description, id, data, author, message);
                text = $"{id}{data}{author}{message}";
                var size = new DrawPanelRigthSide.CommitsPanel();

                if (text.Length >= size.width)
                {
                    message = text.Substring(listWithoutMessage.Length, size.width - listWithoutMessage.Length - 3);
                }
                else
                {
                    message = text.Substring(listWithoutMessage.Length + 1, message.Length);
                }

                Console.Write($"{message.TrimStart()}");
                text = $"{id}{data}{author}{message.TrimStart()}";
                /*list.*/addList.Add(text);
            }


            ClearMessagePanel();
            ClearFilePanel();
            bool clear = false;

            if (variablesForCommits.up == true && variablesForCommits.heightPosition > 1)
            {
                variablesForCommits.currentCommitIndex--;
                variablesForCommits.heightPosition--;
            }

            GetCommitNumber.ReturnCommitNumber(listOfCommits, variablesForCommits);
            Navigate.CommitDetail(repo, variablesForCommits, listOfCommits, clear);
            Cursor.UpdateCursorPositionList(listOfCommits, /*list.*/addList, variablesForCommits, blueFond);

            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughCommits(repo, variablesForCommits, listOfCommits,/* list,*/ height, width);
        }

        public static void PrintNewCommitIfNoPanel(IntPtr repo, GetVariablesForCommits variablesForCommits, CommitElements listOfCommits, List<string> addList, bool reachLimit, int blueFond)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    variablesForCommits.heightPosition++;
                    variablesForCommits.currentCommitIndex++;
                }

                Console.SetCursorPosition(1, variablesForCommits.heightPosition);
                string text = string.Empty;
                string listWithoutMessage = string.Empty;

                if (i == 0)
                {
                    ClearCommitRow(variablesForCommits);
                }

                string id = string.Empty;
                id = $"{listOfCommits.Id[variablesForCommits.currentCommitIndex]} ";
                Console.Write($"{listOfCommits.Id[variablesForCommits.currentCommitIndex]} ", Console.ForegroundColor = ConsoleColor.Magenta);

                string data = string.Empty;
                if ($"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]} ".Length == 9)
                {
                    data = $"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]}{new string(' ', 2)} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }
                else
                {
                    data = $"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }

                int authorLength = 20 - listOfCommits.Author[variablesForCommits.currentCommitIndex].Length;
                string author = $"{listOfCommits.Author[variablesForCommits.currentCommitIndex]}{new string(' ', authorLength)}";


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

                listWithoutMessage = $"{listOfCommits.Id[variablesForCommits.currentCommitIndex]}{data}{author}";
                string message = listOfCommits.Message[variablesForCommits.currentCommitIndex].TrimEnd();
                string description = listOfCommits.Description[variablesForCommits.currentCommitIndex].TrimEnd();
                message = ReturnMessage(description, id, data, author, message);
                text = $"{id}{data}{author}{message}";
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
               /* list.*/addList.Add(text);
            }

            bool clear = false;
            if (variablesForCommits.up == true && variablesForCommits.heightPosition > 1)
            {
                variablesForCommits.currentCommitIndex--;
                variablesForCommits.heightPosition--;
            }

            GetCommitNumber.ReturnCommitNumber(listOfCommits, variablesForCommits);
            Cursor.UpdateCursorPositionList(listOfCommits, /*list.*/addList, variablesForCommits, blueFond);

            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughCommits(repo, variablesForCommits, listOfCommits, /*list,*/ height, width);
        }

        public static void PrintNewCommitIfReachLimit(IntPtr repo, GetVariablesForCommits variablesForCommits, CommitElements listOfCommits, List<string> addList, int blueFond)
        {
            int upAndDownConsole = variablesForCommits.heightPosition;
            int index = variablesForCommits.currentCommitIndex;
            variablesForCommits.startIndex = variablesForCommits.currentCommitIndex;
            while (variablesForCommits.heightPosition <= Console.WindowHeight - 2)
            {
                Console.SetCursorPosition(1, variablesForCommits.heightPosition);
                string text = string.Empty;
                string listWithoutMessage = string.Empty;

                ClearCommitRow(variablesForCommits);

                string id = string.Empty;
                id = $"{listOfCommits.Id[variablesForCommits.currentCommitIndex]} ";
                Console.Write($"{listOfCommits.Id[variablesForCommits.currentCommitIndex]} ", Console.ForegroundColor = ConsoleColor.Magenta);

                string data = string.Empty;
                if ($"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]} ".Length == 9)
                {
                    data = $"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]}{new string(' ', 2)} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }
                else
                {
                    data = $"{listOfCommits.DateTime[variablesForCommits.currentCommitIndex]} ";
                    Console.Write(data, Console.ForegroundColor = ConsoleColor.Cyan);
                }

                int authorLength = 20 - listOfCommits.Author[variablesForCommits.currentCommitIndex].Length;
                string author = $"{listOfCommits.Author[variablesForCommits.currentCommitIndex]}{new string(' ', authorLength)}";

                listWithoutMessage = $"{listOfCommits.Id[variablesForCommits.currentCommitIndex]}{data}{author}";

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

                string message = listOfCommits.Message[variablesForCommits.currentCommitIndex].TrimEnd();
                string description = listOfCommits.Description[variablesForCommits.currentCommitIndex].TrimEnd();
                message = ReturnMessage(description, id, data, author, message);
                text = $"{id}{data}{author}{message}";
                var size = new DrawPanelRigthSide.CommitsPanel();

                if (text.Length >= size.width && variablesForCommits.displayPanel == true)
                {
                    message = text.Substring(listWithoutMessage.Length, size.width - listWithoutMessage.Length - 3);
                }
                else if (text.Length > Console.WindowWidth - 2 && variablesForCommits.displayPanel == false)
                {
                    message = text.Substring(listWithoutMessage.Length, (Console.WindowWidth - 2) - listWithoutMessage.Length - 2);
                }
                else
                {
                    message = text.Substring(listWithoutMessage.Length, message.Length + 1);
                }

                Console.Write($"{message.TrimStart()}");
                text = $"{id}{data}{author}{message.TrimStart()}";
                /*list.*/addList.Add(text);
                variablesForCommits.heightPosition++;
                if (variablesForCommits.up == true)
                {
                    variablesForCommits.currentCommitIndex++;
                }
                else
                {
                    variablesForCommits.currentCommitIndex++;
                }
            }

            variablesForCommits.heightPosition--;
            variablesForCommits.currentCommitIndex--;

            if (variablesForCommits.up == true)
            {
                variablesForCommits.heightPosition = upAndDownConsole;
                variablesForCommits.currentCommitIndex = index;
            }

            GetCommitNumber.ReturnCommitNumber(listOfCommits, variablesForCommits);

            if (variablesForCommits.displayPanel == true)
            {
                ClearMessagePanel();
                ClearFilePanel();
                bool clear = false;
                Navigate.CommitDetail(repo, variablesForCommits, listOfCommits, clear);
            }
            
            Cursor.UpdateCursorPositionList(listOfCommits, /*list.*/addList, variablesForCommits, blueFond);
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;
            Navigate.NavigateThroughCommits(repo, variablesForCommits, listOfCommits, /*list,*/ height, width);
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

        private static void ClearMessagePanel()
        {
            var messagePanel = new DrawPanelRigthSide.MessageBox();
            var heigth = Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2 - 1);
            for (int y = heigth; y < messagePanel.height + 1; y++)
            {
                Console.SetCursorPosition(messagePanel.edgeOne + 1, y);
                Console.Write(new string(' ', messagePanel.width));
            }
        }

        private static void ClearFilePanel()
        {
            var filePanel = new DrawPanelRigthSide.FilesBox();

            for (int x = 1; x < filePanel.height - 1; x++)
            {
                Console.SetCursorPosition(filePanel.edgeOneX + 1, filePanel.edgeOneY + x);
                Console.Write(new string(' ', filePanel.width));
            }
        }

        private static string ReturnMessage(string description, string id, string data, string author, string message)
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
