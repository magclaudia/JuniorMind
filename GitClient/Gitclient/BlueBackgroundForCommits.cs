using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class BlueBackgroundForCommits
    {
        public static void DisplayBlueBox(List<string> addList, GetVariablesForCommits indexes, CommitElements listOfCommits)
        {
            if (indexes.heightPosition > Console.WindowHeight - 2)
            {
                indexes.heightPosition = Console.WindowHeight - 2;
            }

            Console.SetCursorPosition(1, indexes.heightPosition);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            if (addList.Count > 2)
            {
                Console.Write(addList[indexes.heightPosition - 1]);
            }
            else
            {
                if (indexes.up == true)
                {
                    Console.SetCursorPosition(1, indexes.heightPosition);
                    Console.Write(addList[0]);
                }
                else if (indexes.up == true && indexes.currentCommitIndex > 1)
                {
                    Console.SetCursorPosition(1, 1);
                    Console.Write(addList[0]);
                }
                else
                {
                    Console.Write(addList[1]);
                }
            }

            Console.ResetColor();
        }
    }
}
