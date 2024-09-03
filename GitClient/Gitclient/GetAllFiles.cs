using System.Runtime.InteropServices;

namespace GitClient
{
    public class GetAllFiles
    {
        public static void PrintAllFilesAffectedByCommit(IntPtr repo, UIntPtr numDeltas, IntPtr diff, int a, GetVariablesForCommits variablesForCommits, CommitElements commitElements)
        {
            var size = new DrawPanelRigthSide.FilesBox();
            var files = new GetCertainList();

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
                files.filesNames.Add(fileName);
                switch (delta.status)
                {
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_ADDED:
                        fileWithSymbol = $"+    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, variablesForCommits);
                        Console.ForegroundColor = ConsoleColor.Green;
                        PrintEachFile(fileWithSymbol, size, i, ref a, variablesForCommits, files);
                        break;

                    case LibGit2Wrapper.GitDelta.GIT_DELTA_MODIFIED:
                        fileWithSymbol = $"M    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, variablesForCommits);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        PrintEachFile(fileWithSymbol, size, i, ref a, variablesForCommits, files);
                        break;

                    case LibGit2Wrapper.GitDelta.GIT_DELTA_DELETED:
                        fileWithSymbol = $"-    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, variablesForCommits);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        PrintEachFile(fileWithSymbol, size, i, ref a, variablesForCommits, files);
                        break;
                }

                Console.ResetColor();
            }

            if (variablesForCommits.right == true)
            {
                GetVariablesForFiles variablesForFiles = new GetVariablesForFiles();
                GetDiffs.GetFileContent(repo, numDeltas, diff, variablesForFiles, variablesForCommits, commitElements, files);
            }
        }

        private static void PrintProjectName(DrawPanelRigthSide.FilesBox size, ulong i, string filePath, string fileName, GetVariablesForCommits variablesForCommits)
        {
            if (i == 0)
            {
                int firstIndex = 0;
                int fullPathLength = filePath!.Length;
                if (variablesForCommits.right == true)
                {
                    Console.SetCursorPosition(1, size.edgeOneY + 1);
                }
                else
                {
                    Console.SetCursorPosition(size.edgeOneX + 1, size.edgeOneY + 1);
                }

                string projectFolderName;
                string projectFolderWithSymbol;
                string projectFolder = string.Empty;

                if (fullPathLength > fileName.Length)
                {
                    projectFolderName = filePath.Substring(firstIndex, fullPathLength - fileName.Length - 1);
                    projectFolderWithSymbol = $"  ▾{projectFolderName}";
                    if (projectFolderWithSymbol.Length > size.width)
                    {
                        projectFolder = projectFolderWithSymbol.Substring(firstIndex, size.width);
                    }
                    else
                    {
                        projectFolder = projectFolderWithSymbol.Substring(firstIndex, projectFolderWithSymbol.Length);
                    }
                }
                else
                {
                    fileName = $"  ▾{fileName}";
                    if (fileName.Length > size.width)
                    {

                        projectFolder = fileName.Substring(0, size.width);

                    }
                    else
                    {
                        projectFolder = fileName.Substring(0, fileName.Length);
                    }

                }

                Console.Write(projectFolder);
            }
        }

        private static void PrintEachFile(string fileWithSymbol, DrawPanelRigthSide.FilesBox size, ulong i, ref int step, GetVariablesForCommits variablesForCommits, GetCertainList files)
        {
            int lengthForNow = 0;
            int firstIndex = 0;
            string file = "";
            i++;

            if (fileWithSymbol.Length - lengthForNow > size.width)
            {
                if (variablesForCommits.right == true && (int)i <= size.height - 3)
                {
                    Console.SetCursorPosition(1, size.edgeOneY + 1 + (int)i);
                }
                else if (variablesForCommits.right == false && (int)i <= size.height - 3)
                {
                    Console.SetCursorPosition(size.edgeOneX + 1, size.edgeOneY + 1 + (int)i);
                }

                file = fileWithSymbol.Substring(firstIndex, size.width);
                firstIndex++;
            }
            else
            {
                if (variablesForCommits.right == true && (int)i <= size.height - 3)
                {
                    Console.SetCursorPosition(1, size.edgeOneY + 1 + (int)i);
                }
                else if (variablesForCommits.right == false && (int)i <= size.height - 3)
                {
                    Console.SetCursorPosition(size.edgeOneX + 1, size.edgeOneY + 1 + (int)i);
                }

                file = fileWithSymbol.Substring(firstIndex, fileWithSymbol.Length - lengthForNow);
            }

            files.listOfFiles.Add(file);
            if ((int)i <= size.height - 3)
            {
                Console.Write(file);
            }

            firstIndex += file.Length - 1;

        }

    }
}
