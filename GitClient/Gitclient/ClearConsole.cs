using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class ClearConsole
    {
        public static void Clear()
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            int j = dimensions.tabHeight + 1;

            while (j < Console.WindowHeight)
            {
                Console.SetCursorPosition(0, j);
                Console.Write(new string(' ', dimensions.width + 1));
                j++;
            }
        }
    }
}
