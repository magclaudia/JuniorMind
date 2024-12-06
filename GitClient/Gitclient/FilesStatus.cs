using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class FilesStatus
    {
        public static void PrintFilesForStatus(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            
            int y = 0;
            int x = 1;
            int height = 0;
            int index = 0;
            int row = 0;
            
            if (variablesForFiles.unstageChanges == true)
            {
                y = dimensions.tabHeight + 3;
                height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
                index = variablesForFiles.unstagedIndex;
                row = variablesForFiles.fileRowUnstaged;
                variablesForFiles.indexDiff = variablesForFiles.unstagedIndex;
            }
            else
            {
                y = dimensions.stagedEnd;
                height = dimensions.stagedEnd - dimensions.stagedStart;
                index = variablesForFiles.stagedIndex;
                row = variablesForFiles.fileRowStaged;
                variablesForFiles.indexDiff = variablesForFiles.stagedIndex;
            }
            
            if (list.unstagedChangesFiles.Count > 0)
            {
                if (list.stagedChangesFiles.Count == 0)
                {
                    Console.SetCursorPosition(1, dimensions.stagedStart);
                    string text = Tabs.SetStatusTextLength(" No changes found in the staging area.", Console.WindowWidth / 2 - 3);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(text);
                    Console.ResetColor();
                    variablesForFiles.stageChanges = false;
                    variablesForFiles.unstageChanges = true;
                }

                Console.SetCursorPosition(1, dimensions.unstagedStart - 1);
                string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
                variablesForFiles.projName = $"  ▾{directoryName}";
                Console.ForegroundColor = ConsoleColor.White; 
                Console.Write(variablesForFiles.projName);
                Console.ResetColor();
                
                if (height > list.unstagedChangesFiles.Count)
                {
                    height = list.unstagedChangesFiles.Count;
                }

                int i = 0;

                if (list.unstagedFilesStartAt.Count > 0)
                {
                    if (variablesForFiles.unstagedIndex == 0)
                    {
                        i = 0;
                    }
                    else
                    {
                        if (list.unstagedChangesFiles.Count == list.unstagedFilesStartAt[0])
                        {
                            list.unstagedFilesStartAt[0] = variablesForFiles.unstagedIndex;
                            i = list.unstagedFilesStartAt[0];
                        }

                        i = list.unstagedFilesStartAt[0];
                       
                        if (i < 0)
                        {
                            i = 0;
                        }
                    }
                }

                int count = 0;
                y = dimensions.unstagedStart;
                height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;

                while (count < height)
                {
                    if (i == list.unstagedChangesFiles.Count)
                    {
                        break;
                    }

                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, list.unstagedChangesFiles);
                    i++;
                    count++;
                    y++;
                }
            }

            if (variablesForFiles.stageChanges == true && list.stagedChangesFiles.Count > 0 || list.stagedChangesFiles.Count > 0)
            {
                if (list.unstagedChangesFiles.Count == 0)
                {
                    Console.SetCursorPosition(1, dimensions.unstagedStart);
                    string text = Tabs.SetStatusTextLength(" No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                    Console.WriteLine(text);
                    variablesForFiles.unstageChanges = false;
                    variablesForFiles.stageChanges = true;
                }

                Console.SetCursorPosition(1, dimensions.stagedStart - 1);
                string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
                variablesForFiles.projName = $"  ▾{directoryName}";
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(variablesForFiles.projName);
                Console.ResetColor();

                y = dimensions.stagedStart;
                height = dimensions.stagedEnd - dimensions.stagedStart;
                int i = 0;

                if (variablesForFiles.stageChanges == true)
                {
                    if (list.stagedChangesFiles.Count > height && list.stagedFilesStartAt.Count > 0)
                    {

                        if (list.stagedFilesStartAt[0] < 0)
                        {
                            list.stagedFilesStartAt[0] = 0;
                        }

                        i = list.stagedFilesStartAt[0];
                    }
                }
               
                int count = 0;

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
            }
        }

        public static void ScrollThrouthFilesList(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            List<string> filesList = new List<string>();
            List<int> startingIndexes = new List<int>();
           
            int y = 0;
            int x = 1;
            int index = 0;
            int height = 0;
            int startingFrom = 0;
            int row = 0;

            if (variablesForFiles.unstageChanges == true)
            {
                filesList = list.unstagedChangesFiles;
                height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
                startingFrom = dimensions.unstagedStart;
                y = dimensions.unstagedStart;
                index = variablesForFiles.unstagedIndex;
                row = variablesForFiles.fileRowUnstaged;
                startingIndexes = list.unstagedFilesStartAt;
                list.unstagedFilesStartAt.Clear();
            }
            else
            {
                filesList = list.stagedChangesFiles;
                height = dimensions.stagedEnd - dimensions.stagedStart;
                startingFrom = dimensions.stagedStart;
                y = dimensions.stagedStart;
                index = variablesForFiles.stagedIndex;
                row = variablesForFiles.fileRowStaged;
                startingIndexes = list.stagedFilesStartAt;
                list.stagedFilesStartAt.Clear();
            }

            if (index < filesList.Count)
            {
                int i = 1;

                if (variablesForFiles.down == true)
                {
                    if (index >= height)
                    {
                        i = index - height + 1;
                        startingIndexes.Add(i);
                    }
                }
                else
                {
                    i = index;
                }

                int count = 0;

                while (count < height)
                {
                    if (i == filesList.Count)
                    {
                        //row = y - 1;
                        break;
                    }

                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, filesList);
                    i++;
                    count++;
                    y++;
                }
            }

            variablesForFiles.down = false;
            if (variablesForFiles.unstageChanges == true)
            {
                variablesForFiles.unstagedIndex = index;
                variablesForFiles.fileRowUnstaged = row;
            }
            else
            {
                variablesForFiles.stagedIndex = index;
                variablesForFiles.fileRowStaged = row;
            }
        }
    }
}
