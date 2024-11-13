using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class FilesPrintStatusFiles
    {
        public static void PrintFilesForStatus(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            
            int y = 0;
            int x = 1;
            int height = 0;

            if (list.unstagedChangesFiles.Count > 0)
            {
                Console.SetCursorPosition(1, dimensions.unstagedStart - 1);
                string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
                variablesForFiles.projName = $"  ▾{directoryName}";
                Console.ForegroundColor = ConsoleColor.White; 
                Console.Write(variablesForFiles.projName);
                Console.ResetColor();
                
                y = dimensions.tabHeight + 3;
                height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
                
                if (height > list.unstagedChangesFiles.Count)
                {
                    height = list.unstagedChangesFiles.Count;
                }

                if (list.unstagedFilesStartAt.Count == 0)
                {
                    int index = height;
                    list.unstagedFilesStartAt.Add(0);

                    while (index < list.unstagedChangesFiles.Count)
                    {
                        list.unstagedFilesStartAt.Add(index);
                        index += index;
                    }
                }

                int i = 0;

                if (variablesForFiles.stageChanges == true)
                {
                    i = list.unstagedFilesStartAt[list.unstagedFilesStartAt.Count - 1];
                }
                else
                {
                    i = list.unstagedFilesStartAt[variablesForFiles.filesStartAt];
                }
                
                int count = 0;

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

            if (variablesForFiles.stageChanges == true || list.stagedChangesFiles.Count > 0)
            {
                if (list.unstagedChangesFiles.Count == 0)
                {
                    Console.SetCursorPosition(1, dimensions.unstagedStart);
                    string text = Tabs.SetStatusTextLength(" No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                    Console.WriteLine(text);
                }

                Console.SetCursorPosition(1, dimensions.stagedStart - 1);
                string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
                variablesForFiles.projName = $"  ▾{directoryName}";
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(variablesForFiles.projName);
                Console.ResetColor();

                y = dimensions.stagedStart;
                height = dimensions.stagedEnd - dimensions.stagedStart;

                if (height > list.stagedChangesFiles.Count)
                {
                    height = list.stagedChangesFiles.Count;
                }

                int i = 0;

                if (variablesForFiles.stageChanges == true)
                {
                    i = list.unstagedFilesStartAt[variablesForFiles.filesStartAt];
                }
                else
                {
                    i = 0;
                }

                if (list.stagedFilesStartAt.Count == 0)
                {
                    int index = height;
                    list.stagedFilesStartAt.Add(0);

                    while (index < list.stagedChangesFiles.Count)
                    {
                        list.stagedFilesStartAt.Add(index);
                        index += index;
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

            if (variablesForFiles.unstageChanges == true && variablesForFiles.fileIndex < list.unstagedChangesFiles.Count)
            {
                Console.SetCursorPosition(1, dimensions.unstagedStart - 1);
                string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
                variablesForFiles.projName = $"  ▾{directoryName}";
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(variablesForFiles.projName);
                Console.ResetColor();

                int height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
                int i = list.unstagedFilesStartAt[variablesForFiles.filesStartAt];
                int count = 0;
                int y = dimensions.unstagedStart;
                int x = 1;

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


            if (variablesForFiles.stageChanges == true || list.stagedChangesFiles.Count > 0)
            {
                Console.SetCursorPosition(1, dimensions.stagedStart - 1);
                string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
                variablesForFiles.projName = $"  ▾{directoryName}";
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(variablesForFiles.projName);
                Console.ResetColor();
                int i = 0;

                if (variablesForFiles.stageChanges == true)
                {
                    i = list.stagedFilesStartAt[variablesForFiles.filesStartAt];
                }

                int height = dimensions.stagedEnd - dimensions.stagedStart;
                int count = 0;
                int y = dimensions.stagedStart;
                int x = 1;

                while (count < height)
                {
                    if (i == list.stagedChangesFiles.Count)
                    {
                        break;
                    }

                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, list.stagedChangesFiles);
                    i++;
                    count++;
                    y++;
                }
            }
        }
    }
}
