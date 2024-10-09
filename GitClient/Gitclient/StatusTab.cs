using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class StatusTab
    {
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
        public static void GetStatusChangesNames()
        {
            string unstaged = SetStatusTabTextLength("Unstaged Changes: ");
            Console.SetCursorPosition(1, dimensions.textPanelHeight);
            Console.Write(unstaged);
            
            string staged = SetStatusTabTextLength("Staged Changes: ");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.SetCursorPosition(1, Console.WindowHeight / 2 + 2);
            }
            else
            {
                Console.SetCursorPosition(1, Console.WindowHeight / 2 + 1);
            }

            Console.Write(staged);
            
            string diff = "Diff: ";
            Console.SetCursorPosition(Console.WindowWidth / 2 + 1, dimensions.textPanelHeight);
            Console.Write(diff);
        }

        private static string SetStatusTabTextLength(string text)
        {
            string outputText;
            if (text.Length < Console.WindowWidth / 2)
            {
                outputText = text;
            }
            else
            {
                outputText = text.Substring(0, Console.WindowWidth / 2);
            }

            return outputText;
        }
    }
}
