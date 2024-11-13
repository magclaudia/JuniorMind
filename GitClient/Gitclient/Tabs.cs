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
        private static string path = string.Empty;

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
            Console.Clear();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawTabs.DrawOnlyTabs();
            GetRepoPath(path);
            GetTabsNames();
            DrawStatus.DrawPanelsForStatus();
            GetStatusFiles.GetStatusChangesNames();
            GetStatusFiles.GetUnstagedChanges(commitElements, variablesForCommits, variablesForFiles, list);
            variablesForFiles.indexDiff = -1;
            GetStatusFiles.GetStagedChanges(commitElements, variablesForCommits, variablesForFiles, list);
            string fileFullName = "";

            for (int i = 0; i < list.unstagedChangesFiles.Count; i++)
            {
                if (list.stagedChangesFiles.Contains(list.unstagedChangesFiles[i]))
                {
                    list.unstagedChangesFiles.Remove(list.unstagedChangesFiles[i]);
                    list.unstagedChangesDiff.Remove(list.unstagedChangesDiff[i]);
                    i--;
                }

                if (list.unstagedChangesFiles.Count == 0)
                {
                    variablesForFiles.unstageChanges = false;
                }
            }

            FilesPrintStatusFiles.PrintFilesForStatus(list, variablesForCommits, variablesForFiles);
           
            if (list.unstagedChangesFiles.Count > 0)
            {
                fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
                variablesForFiles.fileRow = dimensions.unstagedStart;
            }
            else
            {
                fileFullName = list.stagedChangesFiles[variablesForFiles.fileIndex];
                variablesForFiles.fileRow = dimensions.stagedStart;
            }

            GetFiles.FilesBackground(fileFullName, variablesForFiles, variablesForCommits, list);
            variablesForCommits.stopWorkingOnCommits = true;
            variablesForCommits.pressRight = 1;
            variablesForFiles.initialState = true;
            variablesForFiles.indexDiff = 0;
            DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);

            if (list.unstagedChangesFiles.Count == 0 && list.stagedChangesFiles.Count == 0)
            {
                Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list);
            }
        }

        public static void ChooseStatusTab(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            variablesForFiles.initialState = false;
            Console.Clear();
            DrawTabs.DrawOnlyTabs();
            GetTabsNames();
            GetRepoPath(path);
            DrawStatus.DrawPanelsForStatus();
            GetStatusFiles.GetStatusChangesNames();

            if (list.unstagedChangesFiles.Count == 0)
            {
                if (list.stagedChangesFiles.Count > 0)
                {
                    Console.SetCursorPosition(1, dimensions.stagedStart - 1);
                    string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
                    variablesForFiles.projName = $"  ▾{directoryName}";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(variablesForFiles.projName);
                    Console.ResetColor();
                   
                    int height = dimensions.stagedEnd - dimensions.stagedStart;
                    int count = 0;
                    int i = 0;
                    int x = 1;
                    int y = dimensions.stagedStart;

                    while (count < height)
                    {
                        if (i == list.stagedChangesFiles.Count)
                        {
                            break;
                        }

                        GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, list.stagedChangesFiles);
                        y++;
                        i++;
                        count++;
                    }

                    variablesForFiles.unstageChanges = false;
                    variablesForFiles.stageChanges = true;
                    string fileFullName = list.stagedChangesFiles[variablesForFiles.fileIndex];
                    GetFiles.FilesBackground(fileFullName, variablesForFiles, variablesForCommits, list);
                    variablesForFiles.down = false;
                    DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
                }
            }
            else
            {
                if (list.stagedChangesFiles.Count == 0)
                {
                    Console.SetCursorPosition(1, dimensions.stagedStart);
                    string text = Tabs.SetStatusTextLength("No changes found in the staging area.", Console.WindowWidth / 2 - 3);
                    Console.WriteLine(text);
                }

                if (variablesForFiles.fileIndex <= dimensions.unstagedEnd - dimensions.unstagedStart)
                {
                    FilesPrintStatusFiles.PrintFilesForStatus(list, variablesForCommits, variablesForFiles);
                }
                else
                {
                    FilesPrintStatusFiles.ScrollThrouthFilesList(list, variablesForCommits, variablesForFiles);
                }

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

                GetFiles.FilesBackground(fileFullName, variablesForFiles, variablesForCommits, list);
                variablesForFiles.down = false;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
            }
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
