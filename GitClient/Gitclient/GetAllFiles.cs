using System.Runtime.InteropServices;

namespace GitClient
{
    public class GetAllFiles
    {
        public static void GetListOfAllFiles(IntPtr repo, UIntPtr numDeltas, IntPtr diff, int a, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements, GetVariablesForTabs tab)
        {
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            List<string> listOFAllFiles = new List<string>();
            string filePath = "";
            

            GetStatusFiles obj = new GetStatusFiles();


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

                if (variablesForCommits.right == true && variablesForCommits.esc == false)
                {
                    list.filesNames.Add(fileName);
                }

                switch (delta.status)
                {
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_UNTRACKED:
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_ADDED:
                        {
                            listOFAllFiles.Add($"+    {fileName}");
                        }
                        break;
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_MODIFIED:
                        {
                            listOFAllFiles.Add($"M    {fileName}");
                        }
                        break;
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_DELETED:
                        {
                            listOFAllFiles.Add($"-    {fileName}");
                        }
                        break;
                }
            }

            string g = Path.GetDirectoryName(filePath)!;
            variablesForFiles.projName = $"  ▾{g}";
            
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

        public static void PrintFilesForLog(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetVariablesForTabs tab)
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
            PrintProjectName(variablesForCommits, variablesForFiles, tab, list);
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
                ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x,  y, i);
                y++;
            }
        }

        public static void PrintFilesForStatus(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetVariablesForTabs tab)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            PrintProjectName(variablesForCommits, variablesForFiles, tab, list);
            int y = 0;
            int x = 1;
            int height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;

            if (variablesForFiles.unstageChanges == true)
            {
                y = dimensions.tabHeight + 3;

                if (height > list.unstagedChangesFiles.Count)
                {
                    height = list.unstagedChangesFiles.Count;
                }

                for (int i = 0; i < height; i++)
                {
                    ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i);
                    y++;
                }
            }
            

            if (variablesForFiles.stageChanges == true || list.stagedChangesFiles.Count > 0)
            {
                variablesForFiles.stageChanges = true;
                variablesForFiles.unstageChanges = false;
                y = dimensions.stagedStart;

                if (height > list.stagedChangesFiles.Count)
                {
                    height = list.stagedChangesFiles.Count;
                }

                for (int i = 0; i < height; i++)
                {
                    ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i);
                    y++;
                }

                variablesForFiles.stageChanges = false;
                variablesForFiles.unstageChanges = true;
            }
        }

        public static void PrintRemaingingFiles(GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            int position;
            var filelist = new List<string>();
            int x = 1;
            int y = 0;

            if (variablesForCommits.logTab == true)
            {
                filelist = list.listOfFiles;
                position = Console.WindowHeight - 2;
                x = 1;
                y = variablesForFiles.fileRow;
            }
            else if (variablesForFiles.unstageChanges == true)
            {
                position = dimensions.unstagedEnd;
                filelist = list.unstagedChangesFiles;
                variablesForFiles.fileRow = dimensions.unstagedStart;
                y = variablesForFiles.fileRow;
                x = 1;
            }
            else
            {
                position = dimensions.stagedEnd;
                filelist = list.stagedChangesFiles;
                y = variablesForFiles.fileRow;
                x = 1;
            }

            for (int i = variablesForFiles.fileIndex; i < filelist.Count; i++)
            {
                if (y > position)
                {
                    break;
                }

                ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i);
                Console.SetCursorPosition(1, y + 1);
                y++;
            }
        }

        public static void PrintStatusFilesIfTheyAreAlreadyBeenReceived(GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list, int y, int i)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            int j = 0;
            int x = 1;
            bool unstagedBool = variablesForFiles.unstageChanges;
            bool stagedBool = variablesForFiles.stageChanges;

            if (variablesForFiles.fileIndex < dimensions.changesPanelHeight - 3)
            {
                i = 0;
            }
            else
            {
                i = variablesForFiles.fileIndex;
            }

            if (list.unstagedChangesFiles.Count > 0)
            {
                Console.SetCursorPosition(1, y);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(variablesForFiles.projName);
                Console.ResetColor();
                y++;
                variablesForFiles.unstageChanges = true;
                variablesForFiles.stageChanges = false;

                if (i == list.unstagedChangesFiles.Count)
                {
                    i = dimensions.changesPanelHeight - 3;
                    y = dimensions.unstagedStart;
                }


                while (j < list.unstagedChangesFiles.Count)
                {
                    if (j == dimensions.changesPanelHeight - 3 || i > list.unstagedChangesFiles.Count)
                    {
                        break;
                    }

                    ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i);
                    j++;
                    i++;
                    y++;
                }
            }
            else
            {
                Console.SetCursorPosition(1, 4);
                string text = Tabs.SetStatusTextLength("No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                Console.WriteLine(text);
            }

            if (list.stagedChangesFiles.Count > 0)
            {
                j = 0;
                i = 0;

                variablesForFiles.unstageChanges = false;
                variablesForFiles.stageChanges = true;

                y = dimensions.stagedStart;
                Console.SetCursorPosition(1, dimensions.stagedStart - 1);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(variablesForFiles.projName);
                Console.ResetColor();

                while (j < list.stagedChangesFiles.Count)
                {
                    if (j == dimensions.changesPanelHeight - 3)
                    {
                        break;
                    }

                    ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i);
                    j++;
                    i++;
                    y++;
                }
            }
            else
            {
                dimensions = new DrawTabs.Dimensions();
                Console.SetCursorPosition(1, dimensions.stagedStart);
                string text = Tabs.SetStatusTextLength("No changes found in the staging area.", Console.WindowWidth / 2 - 3);
                Console.WriteLine(text);
            }

            variablesForFiles.unstageChanges = unstagedBool;
            variablesForFiles.stageChanges = stagedBool;
        }

        public static void ChooseColorForEachFiles(GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list, int x, int y, int i)
        {
            var filelist = new List<string>();

            if (variablesForCommits.logTab == true)
            {
                filelist = list.listOfFiles;
            }
            else
            {
                if (variablesForFiles.unstageChanges == true)
                {
                    filelist = list.unstagedChangesFiles;
                }
                else
                {
                    filelist = list.stagedChangesFiles;
                }
            }

            if (filelist.Count > 0)
            {
                switch (filelist[i][0])
                {
                    case 'M':
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(filelist[i]);
                            Console.ResetColor();
                        }
                        break;
                    case '+':
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(filelist[i]);
                            Console.ResetColor();
                        }
                        break;
                    case '-':
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(filelist[i]);
                            Console.ResetColor();
                        }
                        break;
                }
            }
        }

        private static void PrintProjectName(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetVariablesForTabs tab, GetCertainList list)
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
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(fileName);
                    Console.ResetColor();
                }
            }
        }
    }
}
