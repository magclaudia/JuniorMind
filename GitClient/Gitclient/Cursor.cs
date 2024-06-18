using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class Cursor
    {
        public static void UpdateCursorPosition(bool displayPanel, bool panelAlreadyDisplayed, List<string> addList, int heightPosition, ListOfCommits.CommitElements listOfCommits, int index)
        {
            Console.CursorVisible = true;
            int indicatorPosition = (index * (Console.WindowHeight - 2)) / listOfCommits.Id.Count;
            
            if (displayPanel == false)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, indicatorPosition + 1);
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, indicatorPosition + 1);
            }

            DisplayCustomCursor(heightPosition, panelAlreadyDisplayed, addList, ConsoleColor.DarkBlue, listOfCommits, index);

        }

        private static void DisplayCustomCursor(int cursorPosition, bool panelAlreadyDisplayed, List<string> addList, ConsoleColor color, ListOfCommits.CommitElements listOfCommits, int index)
        {
            Console.ForegroundColor = color;
            char cursorSymbol = '█';
            Console.Write(cursorSymbol);
            Console.ResetColor();
            BlueBackground.DisplayBlueBox(cursorPosition, panelAlreadyDisplayed, addList, index, listOfCommits);
        }
    }
}
