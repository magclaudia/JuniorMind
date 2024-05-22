using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gitclient
{
    public class Cursor
    {
        public static void UpdateCursorPosition(int heightPosition, List<string> listOfCommits, int index)
        {
            int indicatorPosition = (index * (Console.WindowHeight - 2)) / listOfCommits.Count;
            Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + 1);
            DisplayCustomCursor(heightPosition, ConsoleColor.DarkBlue, listOfCommits, index);
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
