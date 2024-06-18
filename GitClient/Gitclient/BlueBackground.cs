using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class BlueBackground
    {
        public static void DisplayBlueBox(int blueBoxPosition, bool panelAlreadyDisplayed, List<string> addList, int index, ListOfCommits.CommitElements listOfCommits)
        {

            Console.SetCursorPosition(1, blueBoxPosition);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write(addList[blueBoxPosition - 1]);
            Console.ResetColor();
        }
    }
}
