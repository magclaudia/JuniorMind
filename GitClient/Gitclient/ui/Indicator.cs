using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.ui
{
    public class Indicator
    {
        private DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public void GetIndicator(int currentIndex, int heigth, int totalNumberOfFiles, int x, int y, int endAt)
        {
            int i = y;

            while (i <= endAt)
            {
                Console.SetCursorPosition(x, i);
                Console.Write("║");
                i++;
            }

            Console.CursorVisible = false;
            int indicatorPosition = (currentIndex * heigth) / totalNumberOfFiles;
            int positionOfIndicator = y + indicatorPosition <= heigth ? y + indicatorPosition : heigth;
            Console.SetCursorPosition(x, positionOfIndicator);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            char cursorSymbol = '█';
            Console.Write(cursorSymbol);
            Console.ResetColor();
        }
    }
}
