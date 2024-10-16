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
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public static void PrintTabs(string repoPath, CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            Console.Clear();
            path = repoPath;
            SetInitialState(commitElements, variablesForCommits, variablesForFiles, list);
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

        public static void SetInitialState(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            DrawTabs.DrawOnlyTabs();
            GetRepoPath(path);
            GetTabsNames();
            DrawStatus.DrawPanelsForStatus();
            StatusTab.GetStatusChangesNames();
            StatusTab.GetUnstagedChanges(commitElements, variablesForCommits, variablesForFiles, list, tabs);
            StatusTab.GetStagedChanges(commitElements, variablesForCommits, variablesForFiles, list, tabs);
            string fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
            variablesForFiles.fileRow = dimensions.unstagedStart;
            DiffHelper.FilesBackground(fileFullName, variablesForFiles, variablesForCommits);
            variablesForCommits.stopWorkingOnCommits = true;
            variablesForCommits.pressRight = 1;
            tabs.initialState = true;
            DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
        }

        public static void ChooseStatusTab(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            tabs.initialState = false;
            Console.Clear();
            DrawTabs.DrawOnlyTabs();
            GetTabsNames();
            GetRepoPath(path);
            DrawStatus.DrawPanelsForStatus();
            StatusTab.GetStatusChangesNames();
            
            if (list.unstagedChangesFiles.Count == 0)
            {
                StatusTab.GetUnstagedChanges(commitElements, variablesForCommits, variablesForFiles, list, tabs);
                StatusTab.GetStagedChanges(commitElements, variablesForCommits, variablesForFiles, list, tabs);
                string fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
                DiffHelper.FilesBackground(fileFullName, variablesForFiles, variablesForCommits);
                variablesForFiles.down = false;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
            }
            else
            {
                GetAllFiles.PrintStatusFilesIfAlreadyReceived(variablesForFiles, list, 5, 0);
                string fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
                DiffHelper.FilesBackground(fileFullName, variablesForFiles, variablesForCommits);
                variablesForFiles.down = false;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
            }
        }

        public static void ChooseLogTab(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            tabs.initialState = false;
            Console.Clear();
            GetRepoPath(path);
            DrawTabs.DrawOnlyTabs();
            DrawLogPanel.DrawLargePanel();
            GetTabsNames();
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
    }
}
