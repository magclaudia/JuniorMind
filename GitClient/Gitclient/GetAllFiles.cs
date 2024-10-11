using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GitClient
{
    public class GetAllFiles
    {
        public static void PrintAllFiles(IntPtr repo, UIntPtr numDeltas, IntPtr diff, int a, GetVariablesForCommits variablesForCommits, GetCertainList list, CommitElements commitElements, GetVariablesForTabs tab)
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
                        PrintProjectName(size, i, filePath, fileName, variablesForCommits, tab);
                        Console.ForegroundColor = ConsoleColor.Green;
                        PrintEachFile(fileWithSymbol, size, i, ref a, variablesForCommits, list, tab);
                        break;

                    case LibGit2Wrapper.GitDelta.GIT_DELTA_MODIFIED:
                        fileWithSymbol = $"M    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, variablesForCommits, tab);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        PrintEachFile(fileWithSymbol, size, i, ref a, variablesForCommits, list, tab);
                        break;

                    case LibGit2Wrapper.GitDelta.GIT_DELTA_DELETED:
                        fileWithSymbol = $"-    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, variablesForCommits, tab);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        PrintEachFile(fileWithSymbol, size, i, ref a, variablesForCommits, list, tab);
                        break;
                }

                Console.ResetColor();
            }

            if (variablesForCommits.right == true)
            {
                GetVariablesForFiles variablesForFiles = new GetVariablesForFiles();
                GetDiffs.GetFileContent(diff, variablesForFiles, variablesForCommits, commitElements, list);
            }
        }

        public static void PrintRemaingingFiles(GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            int position = variablesForFiles.fileRow;
            for (int i = variablesForFiles.fileIndex; i < list.listOfFiles.Count; i++)
            {
                if (position > variablesForFiles.height)
                {
                    break;
                }

                Console.SetCursorPosition(1, position);
                ChooseColorForFiles(variablesForFiles, list, position, i);
                Console.SetCursorPosition(1, position + 1);
                position++;
            }
        }

        public static void ChooseColorForFiles(GetVariablesForFiles variablesForFiles, GetCertainList list, int y, int i)
        {
            switch (list.listOfFiles[i][0])
            {
                case 'M':
                    {
                        Console.SetCursorPosition(1, y);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(list.listOfFiles[i]);
                        Console.ResetColor();
                    }
                    break;
                case '+':
                    {
                        Console.SetCursorPosition(1, y);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(list.listOfFiles[i]);
                        Console.ResetColor();
                    }
                    break;
                case '-':
                    {
                        Console.SetCursorPosition(1, y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(list.listOfFiles[i]);
                        Console.ResetColor();
                    }
                    break;
            }
        }

        private static void PrintProjectName(DrawPanelRigthSide.FilesBox size, ulong i, string filePath, string fileName, GetVariablesForCommits variablesForCommits, GetVariablesForTabs tab)
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
                    if (tab.unstageChanges == true)
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

                Console.Write(projectFolder);
            }
        }

        private static void PrintEachFile(string fileWithSymbol, DrawPanelRigthSide.FilesBox size, ulong i, ref int step, GetVariablesForCommits variablesForCommits, GetCertainList list, GetVariablesForTabs tab)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions(); 
            int lengthForNow = 0;
            int firstIndex = 0;
            string file = "";
            i++;

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
                if (tab.unstageChanges == true)
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

                width = dimensions.changesPanelWidth - 3;
                height = dimensions.changesPanelHeight - 3;
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

            if (tab.unstageChanges == true)
            {
                list.unstagedChanges.Add(file);
            }
            else if (tab.stageChanges == true)
            {
                list.stagedChanges.Add(file);
            }

            if ((int)i <= height)
            {
                Console.Write(file);
            }

            firstIndex += file.Length - 1;
        }
    }
}
