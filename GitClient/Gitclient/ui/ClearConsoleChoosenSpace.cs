using GitClient.model;
using GitClient.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class ClearConsoleChoosenSpace
    {
        private DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public void ClearFiles(int x, int y, int startFrom, int width, int EndAt, string cleaningArea)
        {
            ButtomPress.Type.deleted = true;
            
            if (cleaningArea == "cleaningAllPanelArea")
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

        public void ClearDiff (int y)
        {
            int startFrom = dimensions.tabHeight + 2;
            int EndAt = Console.WindowHeight - 2;
            int x = 0;
            int width = 0;

            if (ButtomPress.Type.diffMovements == true && y < EndAt && ButtomPress.Type.down == true || y > dimensions.tabHeight + 2 && ButtomPress.Type.up == true && ButtomPress.Type.diffMovements == true)
            {
                x = 1;
                Console.SetCursorPosition(x, y);
                Console.Write(new string(' ', Console.WindowWidth - 3));
            }
            else if (ButtomPress.Type.right == true)
            {
                startFrom = dimensions.tabHeight + 1;
                x = 0;
                width = Console.WindowWidth;
                EndAt = Console.WindowHeight - 1;

                while (startFrom <= EndAt)
                {
                    Console.SetCursorPosition(x, startFrom);
                    Console.Write(new string(' ', Console.WindowWidth));
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

        public void ClearOneCommit(int x, int y, int width)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(new string(' ', width));
        }

        public void ClearCommitFullWindow(int x, int y, int width, int heigth)
        {
            int startFrom = 3;
            while (startFrom < heigth)
            {
                Console.SetCursorPosition(x, startFrom);
                Console.Write(new string(' ', width));
                startFrom++;
            }
        }

        public void ClearCommitPanel()
        {
            int startFrom = 2;
            while (startFrom < Console.WindowHeight)
            {
                Console.SetCursorPosition(0, startFrom);
                Console.Write(new string(' ', Console.WindowWidth));
                startFrom++;
            }
        }

        public void ClearInfoPanel()
        {
            int startFrom = 3;
           
            while(startFrom < 6)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 10, startFrom);
                Console.Write(new string(' ', Console.WindowWidth - (Console.WindowWidth / 2 + 11)));
                startFrom++;
            }
        }

        public void ClearMessagePanel()
        {
            int startFrom = Console.WindowHeight / 4 + 3;

            while(startFrom < Console.WindowHeight / 2 + 2)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 10, startFrom);
                Console.Write(new string(' ', Console.WindowWidth - (Console.WindowWidth / 2 + 11)));
                startFrom++;
            }
        }
    }
}
