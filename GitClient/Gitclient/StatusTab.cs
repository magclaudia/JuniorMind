using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GitClient
{
    public class StatusTab
    {
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
        public static void GetStatusChangesNames()
        {
            string unstaged = SetStatusTabTextLength("Unstaged Changes");
            Console.SetCursorPosition(1, dimensions.textPanelHeight + 1);
            Console.Write(unstaged);
            
            string staged = SetStatusTabTextLength("Staged Changes");
            Console.SetCursorPosition((Console.WindowWidth / 4) + 1, dimensions.textPanelHeight + 1);
            Console.Write(staged);
            
            string diff = "Diffs";
            int freeSpace = ((Console.WindowWidth / 2) - diff.Length) / 2;
            Console.SetCursorPosition(Console.WindowWidth / 2 + 1, dimensions.textPanelHeight + 1);
            Console.Write(new string(' ', freeSpace) + diff);
        }

        private static string SetStatusTabTextLength(string text)
        {
            string outputText;
            if (text.Length < Console.WindowWidth / 4)
            {
                int freeSpace = ((Console.WindowWidth / 4) - text.Length) / 2;
                outputText = new string(' ', freeSpace) + text;
            }
            else
            {
                outputText = text.Substring(0, Console.WindowWidth / 4);
            }

            return outputText;
        }
    }
}
