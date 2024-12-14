using Gitclient.model;
using GitClient;
using GitClient.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GitClient.DrawTabs;

namespace Gitclient.ui
{
    public class ClearConsoleChoosenSpace
    {
        private DrawTabs.Dimensions dimensions = new Dimensions();

        public void ClearFiles(int curentIndex, int x, int y, int startFrom, int width, int EndAt)
        {
            ButtomPress.Type.deleted = true;
            
            if (y == EndAt && ButtomPress.Type.down == true || y == startFrom && ButtomPress.Type.up == true)
            {
                while (startFrom <= EndAt)
                {
                    Console.SetCursorPosition(x, startFrom);
                    Console.Write(new string(' ', width));
                    startFrom++;
                }
            }
            else
            {
                Console.SetCursorPosition(x, y);
                Console.Write(new string(' ', width));
            }
        }

        public void ClearDiff()
        {
            int startFrom = dimensions.tabHeight + 2;
            int EndAt = Console.WindowHeight - 2;
            int x = 0;
            int width = 0;

            if (ButtomPress.Type.right == true)
            {
                startFrom = dimensions.tabHeight + 1;
                x = 0;
                width = Console.WindowWidth;
                EndAt = Console.WindowHeight - 1;

                while (startFrom <= EndAt)
                {
                    Console.SetCursorPosition(x, startFrom);
                    Console.Write(new string(' ', width));
                    startFrom++;
                }
            }
            else
            {
                x = Console.WindowWidth / 2 + 2;
                width = Console.WindowWidth / 2 - 3;

                while (startFrom <= EndAt)
                {
                    Console.SetCursorPosition(x, startFrom);
                    Console.Write(new string(' ', width));
                    startFrom++;
                }
            }
        }
    }
}
