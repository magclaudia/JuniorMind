using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetDiffs
    {
        public static void GetFileContent(IntPtr repo, UIntPtr numDeltas, IntPtr diff, VariablesForFiles indexes, GetCertainList filesList)
        {
            DiffHelper.PrintDiff(repo, diff, filesList, indexes);
        }

        public static string ResizeTextToFitInPanel(string line)
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

        public static void CleanCodePanel()
        {
            for (int i = 1; i <= Console.WindowHeight - 2; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, i);
                Console.Write(new string(' ', (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4));
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 1);
        }
    }

    public class DiffHelper
    {
        private static int row = 0;
        private static int fileRow = Console.WindowHeight / 2 + 4;
        private static int index = 0;
        private static bool nextFile;
        private static string fileName = string.Empty;
        private static GetCertainList list = new GetCertainList();
        private static string content = string.Empty;
        private static VariablesForFiles indexes = new VariablesForFiles();
        private static int j = 1;

        public static void PrintDiff(IntPtr repo, IntPtr diff, GetCertainList filesList, VariablesForFiles indexes)
        {
            nextFile = indexes.nextFile;
            list.filesNames = filesList.filesNames;
            list.listOfFiles = filesList.listOfFiles;
            list.listOfDiff = filesList.listOfDiff;
            list.diffForEachFile = filesList.diffForEachFile;
            int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);

            if (result != 0)
            {
                throw new Exception("Failed to iterate over diff.");
            }

            index = list.listOfDiffsForEachFiles.Count;
            PrintNewFileContain(indexes, filesList, index);

        }

        public static int DiffFileCallback(LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string fileFullName = list.listOfFiles[indexes.fileIndex];
            fileName = list.filesNames[indexes.fileIndex];
            if (newFilePath!.Contains(fileName))
            {
                list.listOfDiff.Add(newFilePath);
                list.filePath.Add(newFilePath);
            }

            indexes.fileIndex++;
            return 0;
        }

        public static int DiffBinaryCallback(LibGit2Wrapper.GitDiffDelta delta, IntPtr binary, IntPtr payload)
        {
            return 0;
        }

        public static int DiffHunkCallback(LibGit2Wrapper.GitDiffDelta delta, LibGit2Wrapper.GitDiffHunk hunk, IntPtr payload)
        {
            string hunkHeader = new string(hunk.header, 0, (int)hunk.header_len);
            string text = GetDiffs.ResizeTextToFitInPanel($"{hunkHeader}");
            list.listOfDiff.Add(text);
            list.hunks.Add(text);
            return 0;
        }

        public static int DiffLineCallback(LibGit2Wrapper.GitDiffDelta delta, LibGit2Wrapper.GitDiffHunk hunk, LibGit2Wrapper.GitDiffLine line, IntPtr payload)
        {
            string content = Marshal.PtrToStringAnsi(line.content, (int)line.content_len);
            string text = GetDiffs.ResizeTextToFitInPanel($"{(char)line.origin} {content}");
            list.listOfDiff.Add(text);
            list.filesCode.Add(text);
            list.origin.Add(line.origin);
            indexes.nextFile = true;
            return 0;
        }

        public static void Print(VariablesForFiles indexes, int index, int row,  string fileFullName, int j)
        {
            string text = string.Empty;
            for (int i = 0; i < list.diffForEachFile.Count; i++)
            {
                if (row == Console.WindowHeight - 2)
                {
                    Navigate.NavigateThroughDiffsContent(row, index, list, indexes, fileFullName, j);
                }

                if (list.filePath.Contains(list.diffForEachFile[index]))
                {
                    text = "filePath";
                }
                else if (list.hunks.Contains(list.diffForEachFile[index]))
                {
                    text = "hunk";
                }
                else if (list.filesCode.Contains(list.diffForEachFile[index]))
                {
                    text = "filesCode";
                }

                switch (text)
                {
                    case "filePath":
                        {
                            indexes.filesCode = false;

                            if (indexes.down == false && row == 0)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            row++;
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, row);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(list.diffForEachFile[index]);
                            Console.ResetColor();
                        }
                        break;
                    case "hunk":
                        {
                            indexes.filesCode = false;
                            row++;
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, row);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(list.diffForEachFile[index]);
                            Console.ResetColor();

                        }
                        break;
                    case "filesCode":
                        {
                            if (indexes.down == true)
                            {
                                indexes.filesCode = true;
                            }

                            row++;
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, row);
                            content = list.diffForEachFile[index];
                            SetColorForLinesOfCode(content);
                        }
                        break;
                }

                if (index < list.diffForEachFile.Count - 1 && indexes.down == false && row < Console.WindowHeight - 2)
                {
                    index++;
                }
                else
                {
                    break;
                }
                
            }

            if (list.diffForEachFile.Count < Console.WindowHeight - 2)
            {
                TextFitInPanel(fileFullName, index, row, indexes, j);
            }
            else
            {
                TextBiggerThenPanel(fileFullName, index, row, indexes, j);
            }

            Navigate.NavigateThroughDiffsContent(row, index, list, indexes, fileFullName, j);
        }

        public static void PrintNewFileContain(VariablesForFiles indexes, GetCertainList list, int index)
        {
            string currentFileName = list.listOfFiles[indexes.fileIndex];
            FilesBackground(currentFileName, indexes);
            
            if (indexes.fileIndex < list.listOfFiles.Count - 1)
            {
                string completeFileName = "";

                indexes.fileIndex++;
                completeFileName = list.listOfFiles[indexes.fileIndex];
                string fileName = completeFileName.Remove(0, 5);

                for (int i = indexes.diffIndex; i < list.listOfDiff.Count; i++)
                {
                    if (list.listOfDiff[i].Contains(fileName))
                    {
                        index = 0;
                        indexes.down = false;
                        Print(indexes, index, row, currentFileName, j);
                    }

                    list.diffForEachFile.Add(list.listOfDiff[i]);
                }
            }

            if (indexes.fileIndex == list.listOfFiles.Count - 1)
            {
                for (int i = indexes.diffIndex; i < list.listOfDiff.Count; i++)
                {
                    list.diffForEachFile.Add(list.listOfDiff[i]);
                }

                index = 0;
                indexes.down = false;
                indexes.fileIndex++;
                Print(indexes, index, row, currentFileName, j);
            }
        }
        public static void FilesBackground(string fileFullName, VariablesForFiles indexes)
        {
            if (indexes.fileIndex <= list.listOfFiles.Count - 1)
            {
                Console.SetCursorPosition(1, fileRow);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(fileFullName);
                Console.ResetColor();

                if (indexes.fileIndex >= 1)
                {
                    Console.SetCursorPosition(1, fileRow - 1);
                    Console.BackgroundColor = ConsoleColor.Black;
                    fileFullName = list.listOfFiles[indexes.fileIndex - 1];
                    SetColoForFiles(fileFullName, indexes);
                }

                fileRow++;
            }
        }

        private static void CodeBackground(int index, int row, VariablesForFiles indexes, int j)
        {
            if (index <= list.diffForEachFile.Count - 1)
            {
                if (row < Console.WindowHeight - 2)
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, row + 1);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(list.diffForEachFile[index]);
                }
                else
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(list.diffForEachFile[index - 1]);
                }

                Console.ResetColor();
                if (index == list.diffForEachFile.Count - 1 || row >= Console.WindowHeight - 3 && list.diffForEachFile.Count - 1 > Console.WindowHeight - 2)
                {
                    indexes.end = true;
                }
            }
        }

        public static void TextBiggerThenPanel(string fileFullName, int index, int row, VariablesForFiles indexes, int j)
        {
            
            if (row == Console.WindowHeight - 2 && indexes.end == true)
            {
                indexes.down = true;
            }

            if (indexes.down == true && row <= Console.WindowHeight - 2)
            {
                index++;
                CodeBackground(index, row, indexes, j);
                Console.ResetColor();
                Navigate.NavigateThroughDiffsContent(row, index, list, indexes, fileFullName, j);
            }
            else if (indexes.end == true)
            {
                Navigate.NavigateThroughDiffsContent(row, index, list, indexes, fileFullName, j);
            }
        }

        public static void TextFitInPanel(string fileFullName, int index, int row, VariablesForFiles indexes, int j)
        {
            Console.ResetColor();
            if (indexes.down == true && row < Console.WindowHeight - 2)
            {
                index++;
                CodeBackground(index, row, indexes, j);
                Navigate.NavigateThroughDiffsContent(row, index, list, indexes, fileFullName, j);
            }

            if (indexes.end == true)
            {
                Navigate.NavigateThroughDiffsContent(row, index, list, indexes, fileFullName, j);
            }

        }

        private static void SetColoForFiles(string fileFullName, VariablesForFiles indexes)
        {
            string symbol = fileFullName.Substring(0, 1);
            switch (symbol)
            {
                case "+":
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(fileFullName);
                        Console.ResetColor();
                    }
                    break;
                case "-":
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(fileFullName);
                        Console.ResetColor();
                    }
                    break;
                case "M":
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(fileFullName);
                        Console.ResetColor();
                    }
                    break;
            }
        }

        private static void SetColorForLinesOfCode(string content)
        {
            var firstchar = content.First();
            switch(firstchar)
            {
                case ' ':
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(content);
                        Console.ResetColor();
                    }
                    break;
                case '+':
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(content);
                        Console.ResetColor();
                    }
                    break;
                case '-':
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(content);
                        Console.ResetColor();
                    }
                    break;
            }
        }
    }
}

