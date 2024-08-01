using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class Cursor
    {
        public static void UpdateCursorPositionList(CommitElements listOfCommits, List<string> addList, VariablesForCommits indexes, int blueFond)
        {
            Console.CursorVisible = true;
            int indicatorPosition = (indexes.currentCommitIndex * (Console.WindowHeight - 2)) / listOfCommits.Id.Count;
            
            if (indexes.heightPosition >= 1 && indexes.displayPanel == true && indexes.up && indicatorPosition < Console.WindowHeight - 3)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition + 2);
                Console.Write("║");
            }
            else if (indexes.heightPosition >= 10 && indexes.displayPanel == true && indexes.down == true && indicatorPosition >= 1)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition);
                Console.Write("║");
            }
            else if (indexes.heightPosition >= 1 && indexes.displayPanel == false && indexes.up == true && indicatorPosition < Console.WindowHeight - 3)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + 2);
                Console.Write("║");
            }
            else if (indexes.heightPosition > 10 && indexes.displayPanel == false && indexes.down == true && indicatorPosition >= 1)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition);
                Console.Write("║");
            }

            if (indexes.displayPanel == false)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + 1);
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition + 1);
            }

            DisplayCustomCursor(addList, ConsoleColor.DarkBlue, listOfCommits, indexes);
        }

        private static void DisplayCustomCursor(List<string> addList, ConsoleColor color, CommitElements listOfCommits, VariablesForCommits indexes)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            char cursorSymbol = '█';
            Console.Write(cursorSymbol);
            Console.ResetColor();
            BlueBackgroundForCommits.DisplayBlueBox(addList, indexes, listOfCommits);
        }
    }
}
