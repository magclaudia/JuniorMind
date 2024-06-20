using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class Cursor
    {
        public static void UpdateCursorPositionList(ListOfCommits.CommitElements listOfCommits, List<string> addList, ListOfCommits.Indexes indexes)
        {
            Console.CursorVisible = false;
            int indicatorPosition = (indexes.upOrDownOneStep * (Console.WindowHeight - 2)) / listOfCommits.Id.Count;
            
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

        private static void DisplayCustomCursor(List<string> addList, ConsoleColor color, ListOfCommits.CommitElements listOfCommits, ListOfCommits.Indexes indexes)
        {
            Console.ForegroundColor = color;
            char cursorSymbol = '█';
            Console.Write(cursorSymbol);
            Console.ResetColor();
            BlueBackground.DisplayBlueBox(addList, indexes, listOfCommits);
        }
    }
}
