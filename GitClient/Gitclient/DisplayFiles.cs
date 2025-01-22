using System.Drawing;
using System.Runtime.InteropServices;

namespace GitClient
{
    public class DisplayFiles
    {
        public static void GetListOfAllFiles(IntPtr repo, UIntPtr numDeltas, IntPtr diff, int a, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            List<string> listOFAllFiles = new List<string>();
            
            for (UIntPtr i = 0; i < numDeltas.ToUInt64(); i++)
            {
                IntPtr deltaPtr = LibGit2Wrapper.git_diff_get_delta(diff, i);

                if (deltaPtr == IntPtr.Zero)
                {
                    throw new Exception("Failed to get delta.");
                }

                var delta = Marshal.PtrToStructure<LibGit2Wrapper.GitDiffDelta>(deltaPtr);
                string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
                string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);

                if (newFilePath != null)
                {
                   variablesForFiles.filePath = newFilePath;
                }
                else if (oldFilePath != null)
                {
                    variablesForFiles.filePath = oldFilePath!;
                }
                else
                {
                    throw new InvalidOperationException("Both file paths are null");
                }

                string fileName = Path.GetFileName(variablesForFiles.filePath)!;
                string text = "";

                if (variablesForCommits.right == true && variablesForCommits.esc == false)
                {
                    list.filesNames.Add(fileName);
                }

                switch (delta.status)
                {
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_UNTRACKED:
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_ADDED:
                        {
                            text = ResizeFilesNamesToFitInPanel($"+    {fileName}", dimensions, variablesForCommits, variablesForFiles);
                            listOFAllFiles.Add(text);
                        }
                        break;
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_MODIFIED:
                        {
                            text = ResizeFilesNamesToFitInPanel($"M    {fileName}", dimensions, variablesForCommits, variablesForFiles);
                            listOFAllFiles.Add(text);
                        }
                        break;
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_DELETED:
                        {
                            text = ResizeFilesNamesToFitInPanel($"-    {fileName}", dimensions, variablesForCommits, variablesForFiles);
                            listOFAllFiles.Add(text);
                        }
                        break;
                }
            }

            if (variablesForCommits.logTab == true)
            {
                list.listOfFiles = listOFAllFiles;
            }
            else if (variablesForFiles.unstageChanges == true)
            {
                list.unstagedChangesFiles = listOFAllFiles;
            }
            else
            {
                list.stagedChangesFiles = listOFAllFiles;
            }
        }

        public static void FilesBackground(string fileFullName, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list)
        {
            int y = 0;
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();

            int height = 0;
            int index = 0;
            int row = 0;

            if (variablesForFiles.unstageChanges == true)
            {
                y = dimensions.tabHeight + 3;
                height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
                index = variablesForFiles.unstagedIndex;
                row = variablesForFiles.fileRowUnstaged;
            }
            else
            {
                height = dimensions.stagedEnd - dimensions.stagedStart;
                index = variablesForFiles.stagedIndex;
                row = variablesForFiles.fileRowStaged;
            }

            if (variablesForCommits.logTab == true)
            {
                variablesForFiles.nextFile = true;
                y = size.height - dimensions.tabHeight + 1;
            }

            Cursor.UpdateCursorPositionForFilesList(variablesForFiles, variablesForCommits, list, size);

            if (row + 1 == Console.WindowHeight / 2 + 4 && variablesForFiles.up == true)
            {
                CleaningFilePanelForLog(variablesForFiles, variablesForCommits);
                variablesForFiles.fileIndex = 0;

                while (row <= Console.WindowHeight - 2)
                {
                    fileFullName = list.listOfFiles[index];
                    index++;
                    row++;
                    Console.SetCursorPosition(1, row);
                }

                row = Console.WindowHeight / 2 + 4;
                index = 0;
                variablesForFiles.indexForLog = 1;
                fileFullName = list.listOfFiles[index];
            }

            if (list.listOfFiles.Count > y && index == y && variablesForFiles.up == false)
            {
                string path = variablesForFiles.filePath;
                CleaningFilePanelForLog(variablesForFiles, variablesForCommits);
                FilesStatus.PrintFilesForStatus(list, variablesForCommits, variablesForFiles);
                variablesForFiles.filesReachPanelLimit = true;
            }

            if (variablesForCommits.logTab == false && variablesForCommits.esc == true)
            {
                variablesForCommits.esc = false;
            }

            Console.SetCursorPosition(1, row);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(fileFullName);
            Console.ResetColor();

            if (variablesForCommits.logTab == true)
            {
                if (variablesForFiles.filesReachPanelLimit == false && index >= 1 && index <= list.listOfFiles.Count - 1 || variablesForFiles.up == true)
                {
                    if (variablesForFiles.up == true)
                    {
                        row++;
                        index++;
                        Console.SetCursorPosition(1, row);
                        Console.BackgroundColor = ConsoleColor.Black;
                        fileFullName = list.listOfFiles[index];
                    }
                    else
                    {
                        Console.SetCursorPosition(1, row - 1);
                        Console.BackgroundColor = ConsoleColor.Black;
                        fileFullName = list.listOfFiles[index - 1];
                    }
                }
            }

            variablesForFiles.filesReachPanelLimit = false;
        }

        public static void ChooseColorForEachFiles(GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list, int x, int y, int i, List<string> filesList)
        {
            if (filesList.Count > 0)
            {
                switch (filesList[i][0])
                {
                    case 'M':
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(filesList[i]);
                            Console.ResetColor();
                        }
                        break;
                    case '+':
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(filesList[i]);
                            Console.ResetColor();
                        }
                        break;
                    case '-':
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(filesList[i]);
                            Console.ResetColor();
                        }
                        break;
                }
            }
        }

        public static void CleaningFilePanelForLog(GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits)
        {
            int width = Console.WindowWidth;
            int maxHeight = Console.WindowHeight - 1;
            int maxPosition = 0;
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            
            if (variablesForCommits.logTab == true)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    maxPosition = 5;
                }
                else
                {
                    maxPosition = 6;
                }

                for (int i = size.height + maxPosition; i < maxHeight; i++)
                {
                    Console.SetCursorPosition(1, i);
                    Console.Write(new string(' ', (width - 2) - (width / 2) - 4));
                }

                variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;
                Console.SetCursorPosition(1, size.height + maxPosition);
            }
        }

        public static string ResizeFilesNamesToFitInPanel(string text, DrawTabs.Dimensions dimensions, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            if (text.Length > dimensions.changesPanelWidth - 2)
            {
                text = text.Substring(0, dimensions.changesPanelWidth - 2);
            }

            return text;
        }
    }
}
