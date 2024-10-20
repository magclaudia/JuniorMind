using System.Runtime.InteropServices;

namespace GitClient
{
    public class Cursor
    {
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();


        public static void UpdateCursorPositionForCommitsList(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetCertainList list, int blueFond)
        {
            Console.CursorVisible = true;
            int indicatorPosition = (variablesForCommits.currentCommitIndex * ((Console.WindowHeight - dimensions.tabHeight - 4)) / commitElements.Id.Count);

            if (variablesForCommits.heightPosition >= 1 && variablesForCommits.displayPanel == true && variablesForCommits.up && indicatorPosition < Console.WindowHeight - 3)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition + dimensions.tabHeight + 3);
                Console.Write("║");
            }
            else if (variablesForCommits.heightPosition >= 10 && variablesForCommits.displayPanel == true && variablesForCommits.down == true && indicatorPosition >= 1)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition + 3);
                Console.Write("║");
            }
            else if (variablesForCommits.heightPosition >= 1 && variablesForCommits.displayPanel == false && variablesForCommits.up == true && indicatorPosition < Console.WindowHeight - 3)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + dimensions.tabHeight + 3);
                Console.Write("║");
            }
            else if (variablesForCommits.heightPosition > 10 && variablesForCommits.displayPanel == false && variablesForCommits.down == true && indicatorPosition >= 1)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + dimensions.tabHeight + 1);
                Console.Write("║");
            }

            if (variablesForCommits.displayPanel == false)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + dimensions.tabHeight + 2);
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition + dimensions.tabHeight + 2);
            }

            DisplayCustomCursor(ConsoleColor.DarkBlue, commitElements, variablesForCommits, list);
        }

        private static void DisplayCustomCursor(ConsoleColor color, CommitElements listOfCommits, GetVariablesForCommits variablesForCommits, GetCertainList list)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            char cursorSymbol = '█';
            Console.Write(cursorSymbol);
            Console.ResetColor();
            BlueBackgroundForCommits.DisplayBlueBox(variablesForCommits, listOfCommits, list);
        }

        public static void UpdateCursorPositionForDiffsList(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            int x = 0;
            if (variablesForCommits.pressRight == 1)
            {
                x = Console.WindowWidth - 2;
            }
            else
            {
                x = Console.WindowWidth - 1;
            }

            int indicatorPosition = (variablesForFiles.index * (Console.WindowHeight - 2)) / list.listOfAllDiffs[variablesForFiles.indexDiff].Count;
            Console.SetCursorPosition(x, indicatorPosition + 1);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            char cursorSymbol = '█';
            Console.Write(cursorSymbol);
            Console.ResetColor();
            int i = 1;

            while (i <= indicatorPosition && variablesForFiles.down == true && variablesForFiles.up == false)
            {
                Console.SetCursorPosition(x, i);
                Console.Write("║");
                i++;
            }

            i = 0;
            int stop = (Console.WindowHeight - 2) - indicatorPosition;
            while (variablesForFiles.up == true && i < stop - 1)
            {
                Console.SetCursorPosition(x, Console.WindowHeight - 2 - i);
                Console.Write("║");
                i++;
            }

            if (list.listStartAt.Count > 0)
            {
                if (list.listStartAt[variablesForFiles.indexForLog] == variablesForFiles.index && indicatorPosition >= 0)
                {
                    while (i < stop - 1)
                    {
                        Console.SetCursorPosition(x, Console.WindowHeight - 2 - i);
                        Console.Write("║");
                        i++;
                    }
                }

            }

            Console.SetCursorPosition(x, indicatorPosition);
        }

        public static void UpdateCursorPositionForFilesList(GetVariablesForFiles variablesForFiles, GetCertainList list, DrawPanelRigthSide.FilesBox size)
        {
            int indicatorPosition = (variablesForFiles.fileIndex * (Console.WindowHeight - 1 - (Console.WindowHeight / 2 + 1) - 1) / list.listOfFiles.Count);
            if (variablesForFiles.up == true && variablesForFiles.fileRow == Console.WindowHeight / 2 + 3)
            {
                indicatorPosition = 0;
            }


            if (indicatorPosition < size.height - 2)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 1, Console.WindowHeight / 2 + 2 + indicatorPosition + 1);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                char cursorSymbol = '█';
                Console.Write(cursorSymbol);
                Console.ResetColor();

                int i = 1;
                while (i <= indicatorPosition && variablesForFiles.down == true && variablesForFiles.up == false)
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 - 1, Console.WindowHeight / 2 + 2 + i);
                    Console.Write("║");
                    i++;
                }

                i = 0;
                int stop = (Console.WindowHeight - 1 - (Console.WindowHeight / 2 + 1) - 1) - indicatorPosition;
                while (variablesForFiles.up == true && i < stop - 2)
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 - 1, Console.WindowHeight - 2 - i);
                    Console.Write("║");
                    i++;
                }
            }
        }
    }
}
