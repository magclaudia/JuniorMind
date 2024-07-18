using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class FileContentReader
    {
        public static void GetFileContent(IntPtr repo, UIntPtr numDeltas, IntPtr diff, Indexes indexes)
        {
            var size = new DrawPanelRigthSide.FilesBox();
            int startingIndex = 0;
            int moveNext = 1;
            for (UIntPtr i = (uint)indexes.indexForFiles; i < numDeltas.ToUInt64(); i++)
            {
                if(i > 0)
                {
                    CleanCodePanel();
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

                var commitOid = new GitOid();
                if (delta.new_file.id.Id.All(id => id == 0))
                {
                    commitOid = delta.old_file.id;
                }
                else
                {
                    commitOid = delta.new_file.id;
                }
                
                if (LibGit2Wrapper.git_blob_lookup(out IntPtr blobPtr, repo, ref commitOid) != 0)
                {
                    throw new Exception("Failed to lookup commit.");
                }

                IntPtr rawContent = LibGit2Wrapper.git_blob_rawcontent(blobPtr);
                long rawSize = LibGit2Wrapper.git_blob_rawsize(blobPtr);
                byte[] content = new byte[rawSize];
                Marshal.Copy(rawContent, content, 0, (int)rawSize);
                LibGit2Wrapper.git_blob_free(blobPtr);

                string contentReturn = Encoding.UTF8.GetString(content);
                int j = 1;
                DisplayFileContentFirstSeen(contentReturn, filePath, startingIndex, indexes, moveNext, j, repo, numDeltas, diff);
            }
        }

        public static void DisplayFileContentFirstSeen(string content, string filePath, int startingIndex, Indexes indexes, int moveNext, int i, IntPtr repo, UIntPtr numDeltas, IntPtr diff)
        {
            string[] lines = content.Split('\n');
            for (i = 1; i < lines.Length; i++)
            {
                if (i > Console.WindowHeight - 2)
                {
                    break;
                }

                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, i);
                Console.Write(new string(' ', (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4));
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, i);
                string line = ResizeTextToFitInPanel(lines[startingIndex]);
                Console.Write(line);
                startingIndex++;
                indexes.addLines.Add(line);
            }

            DisplayFileContentInPanel(content, filePath, startingIndex, indexes, moveNext, i, repo, numDeltas, diff);
        }

        public static void DisplayFileContentInPanel(string content, string filePath, int startingIndex, Indexes indexes, int moveNext, int i, IntPtr repo, UIntPtr numDeltas, IntPtr diff)
        {
            string[] lines = content.Split('\n');
            while (i < lines.Length)
            {
                if (i >= Console.WindowHeight - 1)
                {
                    PrintBackground(startingIndex, content, filePath, indexes, moveNext, i, lines, repo, numDeltas, diff);
                    Navigate.NavigateThroughFilesContent(content, filePath, startingIndex, indexes, moveNext, i, lines, repo, numDeltas, diff);
                }
                
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, i);
                Console.Write(new string(' ', (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4));
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, i);
                string line = ResizeTextToFitInPanel(lines[startingIndex]);
                Console.Write(line);
                startingIndex++;
                if (startingIndex == lines.Length - 1 && indexes.up == false)
                {
                    indexes.nextFile = true;
                    break;
                }

                
                if (indexes.up == false)
                {
                    if (indexes.addLines.Count >= Console.WindowHeight - 2)
                    {
                        indexes.addLines.Clear();
                    }

                    indexes.addLines.Add(line);
                    i++;
                }
                else
                {
                    i--;
                    startingIndex--;
                    PrintBackground(startingIndex, content, filePath, indexes, moveNext, i, lines, repo, numDeltas, diff);
                }
            }

            indexes.indexForFiles++;
            GetFileContent(repo, numDeltas, diff, indexes);
        }

        public static void PrintBackground(int startingIndex, string content, string filePath, Indexes indexes, int moveNext, int i, string[] lines, IntPtr repo, UIntPtr numDeltas, IntPtr diff)
        {
            int j = indexes.indexForFiles;
            while (j < indexes.addLines.Count)
            {
                if (indexes.indexForFiles > 0)
                {
                    Console.ResetColor();
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, indexes.indexForFiles);
                    Console.Write(new string(' ', (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4));
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, indexes.indexForFiles);
                    Console.Write(indexes.addLines[indexes.indexForFiles - 1]);
                }

                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, indexes.indexForFiles + 1);
                Console.Write(new string(' ', (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4));
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, indexes.indexForFiles + 1);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(indexes.addLines[indexes.indexForFiles]);
                indexes.down = true;
                Console.ResetColor();
                Navigate.NavigateThroughFilesContent(content, filePath, startingIndex, indexes, moveNext, j, lines, repo, numDeltas, diff);

            }
        }

        private static string ResizeTextToFitInPanel(string line)
        {
            var width = (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4;
            if (line.Length < width)
            {
                return line;
            }
            else
            {
                line = line.Substring(0, width);
                return line;
            }
        }

        private static void CleanCodePanel()
        {
            for (int i = 1; i <= Console.WindowHeight - 2; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, i);
                Console.Write(new string(' ', (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4));
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 1);
        }
    }
}
