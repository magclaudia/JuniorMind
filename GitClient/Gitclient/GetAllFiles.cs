using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GitClient
{
    public class GetAllFiles
    {
        public static void PrintAllFiles(IntPtr repo, UIntPtr numDeltas, IntPtr diff, int a, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements, GetVariablesForTabs tab)
        {
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();

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
                string filePath;

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
                string fileWithSymbol;
                if (variablesForCommits.right == true && variablesForCommits.esc == false)
                {
                    list.filesNames.Add(fileName);
                }

                switch (delta.status)
                {
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_ADDED:
                        fileWithSymbol = $"+    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, variablesForCommits, variablesForFiles, tab);
                        Console.ForegroundColor = ConsoleColor.Green;
                        PrintEachFile(fileWithSymbol, size, i, ref a, variablesForCommits, list, variablesForFiles);
                        break;

                    case LibGit2Wrapper.GitDelta.GIT_DELTA_MODIFIED:
                        fileWithSymbol = $"M    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, variablesForCommits, variablesForFiles, tab);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        PrintEachFile(fileWithSymbol, size, i, ref a, variablesForCommits, list, variablesForFiles);
                        break;

                    case LibGit2Wrapper.GitDelta.GIT_DELTA_DELETED:
                        fileWithSymbol = $"-    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, variablesForCommits, variablesForFiles, tab);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        PrintEachFile(fileWithSymbol, size, i, ref a, variablesForCommits, list, variablesForFiles);
                        break;
                }

                Console.ResetColor();
            }

            if (variablesForCommits.right == true)
            {
                GetVariablesForFiles variablesForFile = new GetVariablesForFiles();
                GetDiffs.GetFileContent(diff, variablesForFile, variablesForCommits, commitElements, list);
            }
        }

        public static void PrintRemaingingFiles(GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list)
        {
            int position;
            var filelist = new List<string>();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (variablesForCommits.logTab == true)
            {
                position = variablesForFiles.fileRow;
            }
            else
            {
                position = Console.WindowHeight / 2;
            }

            int y = 0;
            if (variablesForFiles.fileIndex <= list.unstagedChangesFiles.Count - 1)
            {
                filelist = list.unstagedChangesFiles;
                variablesForFiles.fileRow = dimensions.unstagedStart;
                y = variablesForFiles.fileRow;
            }
            else
            {
                filelist = list.stagedChangesFiles;
            }

            for (int i = variablesForFiles.fileIndex; i < filelist.Count; i++)
            {
                if (y >= position)
                {
                    break;
                }

                Console.SetCursorPosition(1, y);
                ChooseColorForFiles(variablesForFiles, list, y, i);
                Console.SetCursorPosition(1, y + 1);
                y++;
            }
        }

        public static void PrintStatusFilesIfAlreadyReceived(GetVariablesForFiles variablesForFiles, GetCertainList list, int y, int i)
        {
            Console.SetCursorPosition(1, y - 1);
            Console.Write(variablesForFiles.projName);
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            int j = 0;

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
                if (i == list.unstagedChangesFiles.Count - 1)
                {
                    i = dimensions.changesPanelHeight - 3;
                    y = dimensions.unstagedStart;
                }


                while (j < list.unstagedChangesFiles.Count)
                {
                    if (j == dimensions.changesPanelHeight - 3 || i > list.unstagedChangesFiles.Count - 1)
                    {
                        break;
                    }

                    ChooseColorForFiles(variablesForFiles, list, y, i);
                    j++;
                    i++;
                    y++;
                }
            }
            else
            {
                Console.SetCursorPosition(1, 4);
                string text = Tabs.SetTabTextLength("No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                Console.WriteLine(text);
            }

            if (list.stagedChangesFiles.Count > 0)
            {
                j = 0;
                i = 0;

                while (j < list.stagedChangesFiles.Count)
                {
                    if (j == dimensions.changesPanelHeight - 3)
                    {
                        break;
                    }

                    ChooseColorForFiles(variablesForFiles, list, variablesForFiles.fileRow, i);
                    variablesForFiles.fileRow++;
                    j++;
                    i++;
                }
            }
            else
            {
                Console.SetCursorPosition(1, dimensions.stagedStart - 1);
                Console.Write(variablesForFiles.projName);
                Console.SetCursorPosition(1, dimensions.stagedStart);
                string text = Tabs.SetTabTextLength("No changes found in the staging area.", Console.WindowWidth / 2 - 3);
                Console.WriteLine(text);
            }
        }

        public static void ChooseColorForFiles(GetVariablesForFiles variablesForFiles, GetCertainList list, int y, int i)
        {
            var filelist = new List<string>();
            if (variablesForFiles.fileIndex <= list.unstagedChangesFiles.Count - 1)
            {
                filelist = list.unstagedChangesFiles;
            }
            else
            {
                filelist = list.stagedChangesFiles;
            }

            switch (filelist[i][0])
            {
                case 'M':
                    {
                        Console.SetCursorPosition(1, y);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(filelist[i]);
                        Console.ResetColor();
                    }
                    break;
                case '+':
                    {
                        Console.SetCursorPosition(1, y);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(filelist[i]);
                        Console.ResetColor();
                    }
                    break;
                case '-':
                    {
                        Console.SetCursorPosition(1, y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(filelist[i]);
                        Console.ResetColor();
                    }
                    break;
            }
        }

        private static void PrintProjectName(DrawPanelRigthSide.FilesBox size, ulong i, string filePath, string fileName, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetVariablesForTabs tab)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (i == 0)
            {
                int x = 0;
                int y = 0;
                int width = 0;
                int height = 0;

                if (variablesForCommits.logTab == true)
                {
                    x = size.edgeOneX + 1;
                    y = size.edgeOneY + 1;
                    width = size.width;
                    height = size.height - 3;
                }
                else
                {
                    if (variablesForFiles.unstageChanges == true)
                    {
                        x = 1;
                        y = 4;
                    }
                    else
                    {
                        x = 1;
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            y = Console.WindowHeight / 2 + 3;
                        }
                        else
                        {
                            y = Console.WindowHeight / 2 + 2;
                        }
                    }

                    width = dimensions.changesPanelWidth;
                    height = (Console.WindowHeight - 1) - y;
                }

                int firstIndex = 0;
                int fullPathLength = filePath!.Length;
                if (variablesForCommits.right == true)
                {
                    Console.SetCursorPosition(1, y);
                }
                else
                {
                    Console.SetCursorPosition(x, y);
                }

                string projectFolderName;
                string projectFolderWithSymbol;
                string projectFolder = string.Empty;

                if (fullPathLength > fileName.Length)
                {
                    projectFolderName = filePath.Substring(firstIndex, fullPathLength - fileName.Length - 1);
                    projectFolderWithSymbol = $"  ▾{projectFolderName}";
                    if (projectFolderWithSymbol.Length > width)
                    {
                        projectFolder = projectFolderWithSymbol.Substring(firstIndex, width);
                    }
                    else
                    {
                        projectFolder = projectFolderWithSymbol.Substring(firstIndex, projectFolderWithSymbol.Length);
                    }
                }
                else
                {
                    fileName = $"  ▾{fileName}";
                    if (fileName.Length > width)
                    {

                        projectFolder = fileName.Substring(0, width);

                    }
                    else
                    {
                        projectFolder = fileName.Substring(0, fileName.Length);
                    }

                }

                variablesForFiles.projName = projectFolder;
                Console.Write(projectFolder);
            }
        }

        private static void PrintEachFile(string fileWithSymbol, DrawPanelRigthSide.FilesBox size, ulong i, ref int step, GetVariablesForCommits variablesForCommits, GetCertainList list, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions(); 
            int lengthForNow = 0;
            int firstIndex = 0;
            string file = "";
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            if (variablesForCommits.logTab == true)
            {
                x = size.edgeOneX + 1;
                y = size.edgeOneY + 1;
                width = size.width;
                height = size.height - 3;
                i++;
            }
            else
            {
                if (variablesForFiles.unstageChanges == true)
                {
                    x = 1;
                    y = dimensions.unstagedStart;
                }
                else
                {
                    x = 1;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        y = Console.WindowHeight / 2 + 3;
                    }
                    else
                    {
                        y = Console.WindowHeight / 2 + 2;
                    }
                }

                width = dimensions.changesPanelWidth - 2;
                height = dimensions.changesPanelHeight - 4;
            }

            if (fileWithSymbol.Length - lengthForNow > width)
            {
                if (variablesForCommits.right == true && (int)i <= height)
                {
                    Console.SetCursorPosition(1, y + (int)i);
                }
                else if (variablesForCommits.right == false && (int)i <= height)
                {
                    Console.SetCursorPosition(x, y + (int)i);
                }

                file = fileWithSymbol.Substring(firstIndex, width);
                firstIndex++;
            }
            else
            {
                if (variablesForCommits.right == true && (int)i <= height)
                {
                    Console.SetCursorPosition(1, y + (int)i);
                }
                else if (variablesForCommits.right == false && (int)i <= height)
                {
                    Console.SetCursorPosition(x, y + (int)i);
                }

                file = fileWithSymbol.Substring(firstIndex, fileWithSymbol.Length - lengthForNow);
            }

            if (variablesForCommits.right == true && variablesForCommits.esc == false)
            {
                list.listOfFiles.Add(file);
            }

            if (variablesForFiles.unstageChanges == true)
            {
                list.unstagedChangesFiles.Add(file);
            }
            else if (variablesForFiles.stageChanges == true)
            {
                list.stagedChangesFiles.Add(file);
            }

            if ((int)i <= height)
            {
                Console.Write(file);
            }

            firstIndex += file.Length - 1;
        }
    }
}
