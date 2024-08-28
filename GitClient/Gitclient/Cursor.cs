using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class Cursor
    {
        public static void UpdateCursorPositionForCommitsList(CommitElements commitElements, List<string> addList, GetVariablesForCommits variablesForCommits, int blueFond)
        {
            Console.CursorVisible = true;
            int indicatorPosition = (variablesForCommits.currentCommitIndex * (Console.WindowHeight - 2)) / commitElements.Id.Count;
            
            if (variablesForCommits.heightPosition >= 1 && variablesForCommits.displayPanel == true && variablesForCommits.up && indicatorPosition < Console.WindowHeight - 3)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition + 2);
                Console.Write("║");
            }
            else if (variablesForCommits.heightPosition >= 10 && variablesForCommits.displayPanel == true && variablesForCommits.down == true && indicatorPosition >= 1)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition);
                Console.Write("║");
            }
            else if (variablesForCommits.heightPosition >= 1 && variablesForCommits.displayPanel == false && variablesForCommits.up == true && indicatorPosition < Console.WindowHeight - 3)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + 2);
                Console.Write("║");
            }
            else if (variablesForCommits.heightPosition > 10 && variablesForCommits.displayPanel == false && variablesForCommits.down == true && indicatorPosition >= 1)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition);
                Console.Write("║");
            }

            if (variablesForCommits.displayPanel == false)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + 1);
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition + 1);
            }

            DisplayCustomCursor(addList, ConsoleColor.DarkBlue, commitElements, variablesForCommits);
        }

        private static void DisplayCustomCursor(List<string> addList, ConsoleColor color, CommitElements listOfCommits, GetVariablesForCommits indexes)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            char cursorSymbol = '█';
            Console.Write(cursorSymbol);
            Console.ResetColor();
            BlueBackgroundForCommits.DisplayBlueBox(addList, indexes, listOfCommits);
        }

        public static void UpdateCursorPositionForDiffsList(GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            int indicatorPosition = (variablesForFiles.index * (Console.WindowHeight - 2)) / list.listOfDiff.Count;
            Console.SetCursorPosition(Console.WindowWidth - 2, indicatorPosition + 1);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            char cursorSymbol = '█';
            Console.Write(cursorSymbol);
            Console.ResetColor();
            int i = 1;
            while (i <= indicatorPosition && variablesForFiles.down == true && variablesForFiles.up == false)
            {
                Console.SetCursorPosition(Console.WindowWidth - 2, i);
                Console.Write("║");
                i++;
            }

            i = 0;
            int stop = (Console.WindowHeight - 2) - indicatorPosition;
            while (variablesForFiles.up == true && i < stop - 1)
            {
                Console.SetCursorPosition(Console.WindowWidth - 2, Console.WindowHeight - 2 - i);
                Console.Write("║");
                i++;
            }

            Console.SetCursorPosition(Console.WindowWidth - 2, indicatorPosition);
        }

        public static void UpdateCursorPositionForFilesList(GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            int indicatorPosition = (variablesForFiles.fileIndex * (Console.WindowHeight - 1 - (Console.WindowHeight / 2 + 1) - 1) / list.listOfFiles.Count);
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
