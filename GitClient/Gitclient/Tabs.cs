using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class Tabs
    {
        private static string path = string.Empty;
        private static GetVariablesForTabs tabs = new GetVariablesForTabs();

        public static void PrintTabs(string repoPath, CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            Console.Clear();
            path = repoPath;
            SetInitialState(commitElements, variablesForCommits, variablesForFiles, list);
            ChooseTab(commitElements, variablesForCommits, variablesForFiles, list);
        }

        public static void GetTabsNames()
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
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
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            Console.SetCursorPosition((Console.WindowWidth / 2) + 3, 1);
            Console.ForegroundColor = ConsoleColor.Red;
            string text = SetTabTextLength(repoPath, Console.WindowWidth - (dimensions.tabWidth * 2) - 2);
            Console.Write(text);
            Console.ResetColor();
        }

        public static void SetInitialState(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawTabs.DrawOnlyTabs();
            GetRepoPath(path);
            GetTabsNames();
            SetColorForChosenTab("Status [1]", 0, dimensions.tabWidth);
            DrawStatus.DrawPanelsForStatus();
            StatusTab.GetStatusChangesNames();
            StatusTab.GetUnstagedChanges(commitElements, variablesForCommits, variablesForFiles, list, tabs);
            StatusTab.GetStagedChanges(commitElements, variablesForCommits, list, tabs);
            if (list.unstagedChangesFiles.Count > 0)
            {
                string fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
                DiffHelper.FilesBackground(fileFullName, variablesForFiles);
            }
            
            variablesForCommits.stopWorkingOnCommits = true;
            variablesForCommits.pressRight = 1;
            DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
        }

        public static void ChooseTab(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            Console.Clear();
                            DrawTabs.DrawOnlyTabs();
                            GetTabsNames();
                            SetColorForChosenTab("Status [1]", 0, dimensions.tabWidth);
                            GetRepoPath(path);
                            DrawStatus.DrawPanelsForStatus();
                            StatusTab.GetStatusChangesNames();
                            StatusTab.GetUnstagedChanges(commitElements, variablesForCommits, variablesForFiles, list, tabs);
                            StatusTab.GetStagedChanges(commitElements, variablesForCommits, list, tabs);
                            
                            string fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
                            DiffHelper.FilesBackground(fileFullName, variablesForFiles);
                            DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
                           
                        }
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            Console.Clear();
                            GetRepoPath(path);
                            DrawTabs.DrawOnlyTabs();
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

        public static string SetTabTextLength(string name, int maxValue)
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
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
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
    }
}
