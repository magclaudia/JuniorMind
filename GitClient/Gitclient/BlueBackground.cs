using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class BlueBackground
    {
        public static void DisplayBlueBox(List<string> addList, ListOfCommits.Indexes indexes, ListOfCommits.CommitElements listOfCommits)
        {

            Console.SetCursorPosition(1, indexes.heightPosition);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write(addList[indexes.heightPosition - 1]);
            Console.ResetColor();
        }
    }
}
