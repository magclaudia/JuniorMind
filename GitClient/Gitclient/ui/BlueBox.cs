using GitClient.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class BlueBox
    {
        public void SetBlueBox((int x, int y) cursorPosition, string text, int panelWidth)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(cursorPosition.x, cursorPosition.y);
            string displayText = TextSettings.GetTextLength(text, panelWidth);
            string paddedText = displayText.PadRight(panelWidth); 
            Console.Write(paddedText);
            Console.ResetColor();
        }
    }
}
