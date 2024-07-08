using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class PrintAllFiles
    {
        public static void PrintAllFilesAffectedByCommit(UIntPtr numDeltas, IntPtr diff, int a, Indexes indexes)
        {
            var size = new DrawPanelRigthSide.FilesBox();
            for (UIntPtr i = 0; i < numDeltas.ToUInt64(); i++)
            {
                if ((int)i == size.height - 3)
                {
                    break;
                }

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
                switch (delta.status)
                {
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_ADDED:
                        fileWithSymbol = $"+    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, indexes);
                        Console.ForegroundColor = ConsoleColor.Green;
                        PrintEveryFile(fileWithSymbol, size, i, ref a, indexes);
                        break;
                   
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_MODIFIED:
                        fileWithSymbol = $"M    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, indexes);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        PrintEveryFile(fileWithSymbol, size, i, ref a, indexes);
                        break;
                    
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_DELETED:
                        fileWithSymbol = $"-    {fileName}";
                        PrintProjectName(size, i, filePath, fileName, indexes);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        PrintEveryFile(fileWithSymbol, size, i, ref a, indexes);
                        break;
                }

                Console.ResetColor();
            }
        }

        private static void PrintProjectName(DrawPanelRigthSide.FilesBox size, ulong i, string filePath, string fileName, Indexes indexes)
        {
            if (i == 0)
            {
                int firstIndex = 0;
                int fullPathLength = filePath!.Length;
                if (indexes.rigth == true)
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

        private static void PrintEveryFile(string fileWithSymbol, DrawPanelRigthSide.FilesBox size, ulong i, ref int step, Indexes indexes)
        {
            int lengthForNow = 0;
            int firstIndex = 0;

            if (step < size.height - 1)
            {
                string file;
                i++;
                if (fileWithSymbol.Length - lengthForNow > size.width)
                {
                    if (indexes.rigth == true)
                    {
                        Console.SetCursorPosition(1, size.edgeOneY + 1 + (int)i);
                    }
                    else
                    {
                        Console.SetCursorPosition(size.edgeOneX + 1, size.edgeOneY + 1 + (int)i);
                    }

                    file = fileWithSymbol.Substring(firstIndex, size.width);
                    firstIndex++;
                }
                else
                {
                    if (indexes.rigth == true)
                    {
                        Console.SetCursorPosition(1, size.edgeOneY + 1 + (int)i);
                    }
                    else
                    {
                        Console.SetCursorPosition(size.edgeOneX + 1, size.edgeOneY + 1 + (int)i);
                    }

                    file = fileWithSymbol.Substring(firstIndex, fileWithSymbol.Length - lengthForNow);
                }

                Console.Write(file);
                firstIndex += file.Length - 1;
                step++;
            }
        }
    }
}
