using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class Tabs
    {
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public static void PrintTabs(string repoPath)
        {
            DrawTabs.DrawBorders();
            GetTabsNames();
            GetRepoPath(repoPath);
            SetInitialState();
            ChooseTab();
        }

        public static void GetTabsNames()
        {
            Console.SetCursorPosition(1, 1);
            string statusTab = "Status [1]";
            string outputText = SetTabTextLength(statusTab);
            Console.Write(outputText);

            Console.SetCursorPosition(dimensions.tabWidth + 1, 1);
            string logTab = "Log [2]";
            outputText = SetTabTextLength(logTab);
            Console.Write(outputText);
        }

        public static void GetRepoPath(string repoPath)
        {
            Console.SetCursorPosition(1, dimensions.tabHeight + 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(repoPath);
            Console.ResetColor();
        }

        public static void SetInitialState()
        {
            CleanTabs(dimensions.tabWidth * 2);
            DrawTabs.DrawOnlyTabs(0, dimensions.tabWidth * 2);
            GetTabsNames();
            SetColorForChosenTab("Status [1]", 0, dimensions.tabWidth);
            DrawStatus.DrawPanelsForStatus();
            StatusTab.GetStatusChangesNames();
        }

        public static void ChooseTab()
        {
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            CleanTabs(dimensions.tabWidth * 2);
                            DrawTabs.DrawOnlyTabs(0, dimensions.tabWidth * 2);
                            GetTabsNames();
                            SetColorForChosenTab("Status [1]", 0, dimensions.tabWidth);
                            DrawStatus.DrawPanelsForStatus();
                            StatusTab.GetStatusChangesNames();
                        }
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            CleanTabs(dimensions.tabWidth * 2);
                            CleanTextPanel();
                            DrawTabs.DrawOnlyTabs(0, dimensions.tabWidth * 2);
                            DrawStatus.DrawLargePanel();
                            GetTabsNames();
                            SetColorForChosenTab("Log [2]", dimensions.tabWidth, dimensions.tabWidth * 2);
                            
                        }
                        break;

                    default: break;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

        private static string SetTabTextLength(string name)
        {
            string text;
            if (name.Length < dimensions.tabWidth)
            {
                int freeSpace = (dimensions.tabWidth - name.Length) / 2;
                text = new string(' ', freeSpace) + name;
            }
            else
            {
                text = name.Substring(0, dimensions.tabWidth);
            }

            return text;
        }

        private static void SetColorForChosenTab(string tabName, int x, int y)
        {
            Console.SetCursorPosition(x, 1);
            Console.BackgroundColor = ConsoleColor.Gray;
            DrawTabs.DrawOnlyTabs(x, y);

            Console.SetCursorPosition(x + 1, 1);
            Console.Write(new string(' ', dimensions.tabWidth));
            Console.SetCursorPosition(x + 1, 1);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(SetTabTextLength(tabName));
            Console.ResetColor();
        }

        private static void CleanTabs(int stop)
        {
            for (int i = 0; i <= dimensions.tabHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', stop));
            }
        }

        private static void CleanTextPanel()
        {
            int start = dimensions.tabHeight + dimensions.repoPathHeight;
            for(int i = start; i < Console.WindowHeight - 1; i++) 
            {
                Console.SetCursorPosition(1, i);
                Console.Write(new string(' ', Console.WindowWidth - 2));
            }
        }
    }
}
