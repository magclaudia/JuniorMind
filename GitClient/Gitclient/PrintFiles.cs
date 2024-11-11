using System.Runtime.InteropServices;

namespace GitClient
{
    public class PrintFiles
    {
        public static void GetListOfAllFiles(IntPtr repo, UIntPtr numDeltas, IntPtr diff, int a, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            List<string> listOFAllFiles = new List<string>();
            string filePath = "";
            
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
                    filePath = newFilePath;
                }
                else if (oldFilePath != null)
                {
                    filePath = oldFilePath!;
                }
                else
                {
                    throw new InvalidOperationException("Both file paths are null");
                }

                string fileName = Path.GetFileName(filePath)!;
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

            string directoryName = Path.GetDirectoryName(filePath)!;
            variablesForFiles.projName = $"  ▾{directoryName}";
            
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

        public static void PrintFilesForLog(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            var position = new DrawPanelRigthSide.FilesBox();
           
            if (variablesForCommits.right == true)
            {
                Console.SetCursorPosition(1, position.edgeOneY);
            }
            else
            {
                Console.SetCursorPosition(position.edgeOneX + 1, position.edgeOneY);
            }

            Console.WriteLine($"Files: {list.listOfFiles.Count} ");
            PrintProjectName(variablesForCommits, variablesForFiles, list);
            int y = 0;
            int x = 0;
            int height = Console.WindowHeight - variablesForFiles.fileRow - 1;

            if (height > list.listOfFiles.Count)
            {
                height = list.listOfFiles.Count;
            }

            if (variablesForCommits.enter == true && variablesForCommits.pressRight < 1)
            {
                y = Console.WindowHeight / 2 + 4;
                x = Console.WindowWidth / 2 + 11;
            }
            else
            {
                y = Console.WindowHeight / 2 + 4;
                x = 1;
            }

            for (int i = 0; i < height; i++)
            {
                ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x,  y, i, list.listOfFiles);
                y++;
            }
        }

        public static void PrintFilesForStatus(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            PrintProjectName(variablesForCommits, variablesForFiles, list);
            int y = 0;
            int x = 1;
            int height = dimensions.unstagedEnd - dimensions.unstagedStart;

            if (list.unstagedChangesFiles.Count > 0)
            {
                y = dimensions.tabHeight + 3;

                if (height > list.unstagedChangesFiles.Count)
                {
                    height = list.unstagedChangesFiles.Count;
                }

                for (int i = 0; i < height; i++)
                {
                    ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, list.unstagedChangesFiles);
                    y++;
                }
            }
            

            if (variablesForFiles.stageChanges == true || list.stagedChangesFiles.Count > 0)
            {
                y = dimensions.stagedStart;

                if (height > list.stagedChangesFiles.Count)
                {
                    height = list.stagedChangesFiles.Count;
                }

                int i = 0;
                if (height == variablesForFiles.fileIndex)
                {
                    i = variablesForFiles.fileIndex;
                }

                while (i < height)
                {
                    ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, list.stagedChangesFiles);
                    y++;
                    i++;
                }
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
                variablesForFiles.fileRow = dimensions.unstagedStart;
                y = variablesForFiles.fileRow;
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

            for (int i = variablesForFiles.fileIndex; i < filesList.Count; i++)
            {
                if (y >= position)
                {
                    break;
                }

                ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, filesList);
                Console.SetCursorPosition(1, y + 1);
                y++;
            }
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

        private static void PrintProjectName(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();

            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;
            string fileName = variablesForFiles.projName;

            if (variablesForCommits.logTab == true)
            {
                x = size.edgeOneX + 1;
                y = size.edgeOneY + 1;
                width = size.width;
                height = size.height - 3;

                if (variablesForCommits.right == true)
                {
                    Console.SetCursorPosition(1, y);
                }
                else
                {
                    Console.SetCursorPosition(x, y);
                }


                if (fileName.Length > width)
                {
                    fileName = fileName.Substring(0, width);
                }
                else
                {
                    fileName = fileName.Substring(0, fileName.Length);
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(fileName);
                Console.ResetColor();

            }
            else
            {
                if (variablesForFiles.unstageChanges == true)
                {
                    x = 1;
                    y = dimensions.unstagedStart - 1;
                }
                else
                {
                    x = 1;
                    y = dimensions.stagedStart - 1;
                }

                width = dimensions.changesPanelWidth;
                height = (Console.WindowHeight - 1) - y;
                
                if (fileName.Length > width)
                {
                    fileName = fileName.Substring(0, width);
                }
                else
                {
                    fileName = fileName.Substring(0, fileName.Length);
                }

                if (list.unstagedChangesFiles.Count > 0 && list.stagedChangesFiles.Count > 0)
                {
                    Console.SetCursorPosition(1, dimensions.tabHeight + 2);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(fileName);
                    Console.ResetColor();
                    Console.SetCursorPosition(1, dimensions.stagedStart - 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(fileName);
                    Console.ResetColor();
                }
                else
                {
                    Console.SetCursorPosition(1, dimensions.tabHeight + 2);
                    string text = Tabs.SetStatusTextLength("No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                    Console.WriteLine(text);
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(fileName);
                    Console.ResetColor();
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
    }
}
