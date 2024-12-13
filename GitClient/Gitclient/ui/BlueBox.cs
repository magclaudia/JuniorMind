using Gitclient.model;
using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.ui
{
    public class BlueBox
    {
        private SetTextLegth textLegth = new SetTextLegth();
        private DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public void SetBlueBox((int x, int y) cursorPosition, int currentIndex, List<UnstagedChange> currentUnstagedChanges, int panelWidth)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(cursorPosition.x, cursorPosition.y);
            string displayText = textLegth.Text(currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            string paddedText = displayText.PadRight(panelWidth); 
            Console.Write(paddedText);
            Console.ResetColor();
        }
    }
}
