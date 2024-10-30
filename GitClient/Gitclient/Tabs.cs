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
        }

        public static void GetTabsNames()
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            Console.SetCursorPosition(1, 0);
            string statusTab = "Status [1]";
            string outputText = SetTabTextLength(statusTab, dimensions.tabWidth);
            Console.Write(outputText);

            Console.SetCursorPosition(dimensions.tabWidth + 1, 0);
            string logTab = "Log [2]";
            outputText = SetTabTextLength(logTab, dimensions.tabWidth);
            Console.Write(outputText);
        }

        public static void GetRepoPath(string repoPath)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            Console.SetCursorPosition((Console.WindowWidth / 2) + 3, 0);
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
            DrawStatus.DrawPanelsForStatus();
            StatusTab.GetStatusChangesNames();
            StatusTab.GetUnstagedChanges(commitElements, variablesForCommits, variablesForFiles, list, tabs);
            variablesForFiles.indexDiff = -1;
            StatusTab.GetStagedChanges(commitElements, variablesForCommits, variablesForFiles, list, tabs);
            string fileFullName = "";

            if (list.unstagedChangesFiles.Count > 0)
            {
                fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
                variablesForFiles.fileRow = dimensions.unstagedStart;
                DiffHelper.FilesBackground(fileFullName, variablesForFiles, variablesForCommits);
                variablesForCommits.stopWorkingOnCommits = true;
                variablesForCommits.pressRight = 1;
                tabs.initialState = true;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
            }
            else if (list.stagedChangesFiles.Count > 0)
            {
                fileFullName = list.stagedChangesFiles[variablesForFiles.fileIndex];
                variablesForFiles.fileRow = dimensions.stagedStart;
                DiffHelper.FilesBackground(fileFullName, variablesForFiles, variablesForCommits);
                variablesForCommits.stopWorkingOnCommits = true;
                variablesForCommits.pressRight = 1;
                tabs.initialState = true;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
            }

            if (list.unstagedChangesFiles.Count == 0 && list.stagedChangesFiles.Count == 0)
            {
                Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list, tabs);
            }
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
                if (list.stagedChangesFiles.Count > 0)
                {
                    variablesForFiles.unstageChanges = false;
                    variablesForFiles.stageChanges = true;
                    GetAllFiles.PrintStatusFilesIfAlreadyReceived(variablesForFiles, variablesForCommits, list, 3, 0);
                    string fileFullName = list.stagedChangesFiles[variablesForFiles.fileIndex];
                    DiffHelper.FilesBackground(fileFullName, variablesForFiles, variablesForCommits);
                    variablesForFiles.down = false;
                    DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
                }
            }
            else
            {
                GetAllFiles.PrintStatusFilesIfAlreadyReceived(variablesForFiles, variablesForCommits, list, 3, 0);
                string fileFullName = "";
               
                if (variablesForFiles.unstageChanges == true)
                {
                    fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
                    variablesForFiles.unstageChanges = true;
                    variablesForFiles.stageChanges = false;
                }
                else
                {
                    fileFullName = list.stagedChangesFiles[variablesForFiles.fileIndex];
                    variablesForFiles.unstageChanges = false;
                    variablesForFiles.stageChanges = true;
                }

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
