using System.Drawing;
using System.Runtime.InteropServices;

namespace GitClient
{
    public class GetFiles
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

        public static void PrintRemaingingFiles(GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            int position;
            var filesList = new List<string>();
            int x = 1;
            int y = 0;

            if (variablesForCommits.logTab == true)
            {
                filesList = list.listOfFiles;
                position = Console.WindowHeight - 2;
                x = 1;
                y = variablesForFiles.fileRow;
            }
            else if (variablesForFiles.unstageChanges == true)
            {
                position = dimensions.unstagedEnd;
                filesList = list.unstagedChangesFiles;
                y = dimensions.unstagedStart;
                x = 1;
            }
            else
            {
                position = dimensions.stagedEnd;
                filesList = list.stagedChangesFiles;
                variablesForFiles.fileRow = dimensions.stagedStart;
                y = variablesForFiles.fileRow;
                x = 1;
            }

            for (int i = list.fileListStartAt[variablesForFiles.startAt]; i < filesList.Count; i++)
            {
                if (y >= position + 1)
                {
                    break;
                }

                ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, filesList);
                Console.SetCursorPosition(1, y + 1);
                y++;
            }

        }

        public static void FilesBackground(string fileFullName, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list)
        {
            int y = 0;
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();

            if (variablesForCommits.logTab == true)
            {
                variablesForFiles.nextFile = true;
                y = size.height - dimensions.tabHeight + 1;
            }

            Cursor.UpdateCursorPositionForFilesList(variablesForFiles, variablesForCommits, list, size);

            if (variablesForFiles.fileRow + 1 == Console.WindowHeight / 2 + 4 && variablesForFiles.up == true)
            {
                CleaningFilePanel(variablesForFiles);
                variablesForFiles.fileIndex = 0;

                while (variablesForFiles.fileRow <= Console.WindowHeight - 2)
                {
                    fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
                   // SetColorForFiles(fileFullName, variablesForFiles);
                    variablesForFiles.fileIndex++;
                    variablesForFiles.fileRow++;
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                }

                variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;
                variablesForFiles.fileIndex = 0;
                variablesForFiles.indexForLog = 1;
                fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
            }

            if (list.listOfFiles.Count > y && variablesForFiles.fileIndex == y && variablesForFiles.up == false)
            {
                CleaningFilePanel(variablesForFiles);
                PrintRemaingingFiles(variablesForFiles, variablesForCommits, list);
                variablesForFiles.filesReachPanelLimit = true;
            }

            if (variablesForCommits.logTab == false && variablesForCommits.esc == true)
            {
                variablesForCommits.esc = false;
            }

            Console.SetCursorPosition(1, variablesForFiles.fileRow);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(fileFullName);
            Console.ResetColor();

            if (variablesForFiles.filesReachPanelLimit == false && variablesForFiles.fileIndex >= 1 && variablesForFiles.fileIndex <= list.listOfFiles.Count - 1 || variablesForFiles.up == true)
            {
                if (variablesForFiles.up == true)
                {
                    variablesForFiles.fileRow++;
                    variablesForFiles.fileIndex++;
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                    Console.BackgroundColor = ConsoleColor.Black;
                    fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
                   // SetColorForFiles(fileFullName, variablesForFiles);
                }
                else
                {
                    Console.SetCursorPosition(1, variablesForFiles.fileRow - 1);
                    Console.BackgroundColor = ConsoleColor.Black;
                    fileFullName = list.listOfFiles[variablesForFiles.fileIndex - 1];
                   // SetColorForFiles(fileFullName, variablesForFiles);
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

        private static string ResizeFilesNamesToFitInPanel(string text, DrawTabs.Dimensions dimensions, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            if (variablesForCommits.logTab == true)
            {
                if (text.Length > dimensions.widthLogRight)
                {
                    text = text.Substring(0, dimensions.widthLogRight);
                }
            }
            else
            {
                if (text.Length > dimensions.changesPanelWidth - 2)
                {
                    text = text.Substring(0, dimensions.changesPanelWidth - 2);
                }
            }

            return text;
        }

        private static void CleaningFilePanel(GetVariablesForFiles variablesForFiles)
        {
            int width = Console.WindowWidth;
            int maxHeight = Console.WindowHeight - 1;
            int maxPosition = 0;
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();

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
}
