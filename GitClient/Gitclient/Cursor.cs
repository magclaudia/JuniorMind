using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class Cursor
    {
        public static void UpdateCursorPosition(bool displayPanel, int heightPosition, List<string> listOfCommits, int index)
        {
            Console.CursorVisible = false;
            int indicatorPosition = (index * (Console.WindowHeight - 2)) / listOfCommits.Count;
            if (displayPanel == false)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + 1);
                DisplayCustomCursor(heightPosition, ConsoleColor.DarkBlue, listOfCommits, index);
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition + 1);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                char cursorSymbol = '█';
                Console.Write(cursorSymbol);
                Console.ResetColor();
            }
        }

        private static void DisplayCustomCursor(int cursorPosition, ConsoleColor color, List<string> listOfCommits, int index)
        {
            Console.ForegroundColor = color;
            char cursorSymbol = '█';
            Console.Write(cursorSymbol);
            Console.ResetColor();
            BlueBackground.DisplayBlueBox(cursorPosition, index, listOfCommits);
        }
    }
}
