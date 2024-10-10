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
        private static string path = string.Empty;

        public static void PrintTabs(string repoPath)
        {
            Console.Clear();
            path = repoPath;
            SetInitialState();
            ChooseTab();
        }

        public static void GetTabsNames()
        {
            Console.SetCursorPosition(1, 1);
            string statusTab = "Status [1]";
            string outputText = SetTabTextLength(statusTab, dimensions.tabWidth);
            Console.Write(outputText);

            Console.SetCursorPosition(dimensions.tabWidth + 1, 1);
            string logTab = "Log [2]";
            outputText = SetTabTextLength(logTab, dimensions.tabWidth);
            Console.Write(outputText);
        }

        public static void GetRepoPath(string repoPath)
        {
            Console.SetCursorPosition((Console.WindowWidth / 2) + 3, 1);
            Console.ForegroundColor = ConsoleColor.Red;
            string text = SetTabTextLength(repoPath, Console.WindowWidth - (dimensions.tabWidth * 2) - 2);
            Console.Write(text);
            Console.ResetColor();
        }

        public static void SetInitialState()
        {
            DrawTabs.DrawOnlyTabs(0, Console.WindowWidth - 1);
            GetRepoPath(path);
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
                            GetRepoPath(path);
                            DrawTabs.DrawOnlyTabs(0, dimensions.tabWidth * 2);
                            DrawLogPanel.DrawLargePanel();
                            GetTabsNames();
                            SetColorForChosenTab("Log [2]", dimensions.tabWidth, dimensions.tabWidth * 2);
                            
                        }
                        break;

                    default: break;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

        private static string SetTabTextLength(string name, int maxValue)
        {
            string text;
            if (name.Length < maxValue)
            {
                int freeSpace = (maxValue - name.Length) / 2;
                text = new string(' ', freeSpace) + name;
            }
            else
            {
                text = name.Substring(0, maxValue);
            }

            return text;
        }

        private static void SetColorForChosenTab(string tabName, int x, int y)
        {
            Console.SetCursorPosition(x, 1);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = x; i < y; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("─");
                Console.SetCursorPosition(i, 1);
                Console.Write("─");
                Console.SetCursorPosition(i, dimensions.tabHeight);
                Console.Write("─");
            }

            Console.SetCursorPosition(x + 1, 1);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(SetTabTextLength(tabName, dimensions.tabWidth));
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
            int start = dimensions.tabHeight;
            for(int i = start + 1; i < Console.WindowHeight; i++) 
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
    }
}
