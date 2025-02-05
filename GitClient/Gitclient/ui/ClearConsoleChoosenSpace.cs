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
            ReadButtons.Deleted = true;
            
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

            if (ReadButtons.DiffMovements == true && y < EndAt && ReadButtons.Down == true || y > dimensions.tabHeight + 2 && ReadButtons.Up == true && ReadButtons.DiffMovements == true)
            {
                x = 1;
                Console.SetCursorPosition(x, y);
                Console.Write(new string(' ', Console.WindowWidth - 3));
            }
            else if (ReadButtons.RightStatus == true)
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
            else if (ReadButtons.DiffMovements == true && y == EndAt || y == 3 && ReadButtons.DiffMovements == true)
            {
                width = Console.WindowWidth - 3;
                startFrom = 3;

                while (startFrom <= EndAt)
                {
                    Console.SetCursorPosition(1, startFrom);
                    Console.Write(new string(' ', width));
                    startFrom++;
                }
            }
            else
            {
                if (ReadButtons.RightOnce == true)
                {
                    x = Console.WindowWidth / 2 + 2;
                }
               
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
            int index = ReadButtons.Down == true ? y - 1 : y + 1; 
            Console.SetCursorPosition(x, index);
            Console.Write(new string(' ', width));
        }

        public void ClearCommitFullWindow(int x, int y, int width, int heigth)
        {
            int startFrom = 2;
            while (startFrom < heigth)
            {
                Console.SetCursorPosition(x, startFrom);
                Console.Write(new string(' ', width));
                startFrom++;
            }
        }

        public void ClearCommitWithDescription(int x, int width, int heigth)
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
           
            while(startFrom < Console.WindowHeight / 4)
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

        public void ClearFilesPanelForCommits()
        {
            int startFrom = Console.WindowHeight / 2 + 6;

            while (startFrom < Console.WindowHeight - 1)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 10, startFrom);
                Console.Write(new string(' ', Console.WindowWidth - (Console.WindowWidth / 2 + 11)));
                startFrom++;
            }
        }
    }
}
