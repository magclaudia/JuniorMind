using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class Tabs
    {
        public static string path = string.Empty;

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
            string outputText = SetStatusTextLength(statusTab, dimensions.tabWidth);
            Console.Write(outputText);

            Console.SetCursorPosition(dimensions.tabWidth + 1, 0);
            string logTab = "Log [2]";
            outputText = SetStatusTextLength(logTab, dimensions.tabWidth);
            Console.Write(outputText);
        }

        public static void GetRepoPath(string repoPath)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            Console.SetCursorPosition((Console.WindowWidth / 2) + 3, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            string text = SetStatusTextLength(repoPath, Console.WindowWidth - (dimensions.tabWidth * 2) - 2);
            Console.Write(text);
            Console.ResetColor();
        }

        public static void SetInitialState(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            Console.Clear();
            DrawTabs.DrawOnlyTabs();
            GetRepoPath(path);
            GetTabsNames();
            DrawStatus.DrawPanelsForStatus();
            GetStatusFiles.GetStatusChangesNames();

            string pathToDirectory = string.Empty;

            if (variablesForFiles.statusFilesSufferModifications == false)
            {
                if (list.unstagedChangesFiles.Count == 0 && list.stagedChangesFiles.Count == 0)
                {
                    GetStatusFiles.GetUnstagedChanges(commitElements, variablesForCommits, variablesForFiles, list);
                }
            }
            
            if (list.unstagedChangesFiles.Count == 0 && list.stagedChangesFiles.Count == 0)
            {
                string text = "";
                Console.SetCursorPosition(1, dimensions.unstagedStart);
                text = Tabs.SetStatusTextLength(" No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(text);
                Console.ResetColor();

                Console.SetCursorPosition(1, dimensions.stagedStart);
                text = Tabs.SetStatusTextLength(" No changes found in the staging area.", Console.WindowWidth / 2 - 3);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(text);
                Console.ResetColor();

                Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list);
            }
            else
            {
                FilesStatus.PrintFilesForStatus(list, variablesForCommits, variablesForFiles);
                GetFileBackgoundAndDiff(list, variablesForCommits, variablesForFiles, commitElements);
            }
        }

        public static void GetFileBackgoundAndDiff(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElements)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            string fileFullName = string.Empty;

            int height = 0;
            int index = 0;

            if (variablesForFiles.unstageChanges == true)
            {
                height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
                index = variablesForFiles.unstagedIndex;
            }
            else
            {
                height = dimensions.stagedEnd - dimensions.stagedStart;
                index = variablesForFiles.stagedIndex;
            }

            if (list.unstagedChangesFiles.Count > 0 && variablesForFiles.stageChanges == false)
            {
                fileFullName = list.unstagedChangesFiles[index];
                
                if (index == 0)
                {
                    variablesForFiles.fileRowUnstaged = dimensions.unstagedStart;
                }
            }
            else
            {
                fileFullName = list.stagedChangesFiles[index];
                
                if (index == 0)
                {
                    variablesForFiles.fileRowStaged = dimensions.stagedStart;
                }
            }

            DisplayFiles.FilesBackground(fileFullName, variablesForFiles, variablesForCommits, list);
            variablesForCommits.stopWorkingOnCommits = true;
            variablesForCommits.pressRight = 1;
            variablesForFiles.initialState = true;
            variablesForFiles.indexDiff = index;
            
            DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
        }

        public static void ChooseLogTab(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            variablesForFiles.initialState = false;
            Console.Clear();
            GetRepoPath(path);
            DrawTabs.DrawOnlyTabs();
            DrawLogPanel.DrawLargePanel();
            GetTabsNames();
        }

        public static string SetStatusTextLength(string name, int maxValue)
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
