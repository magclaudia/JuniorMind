using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        public static void GetFileContent(IntPtr repo, UIntPtr numDeltas, IntPtr diff, GetVariablesForFiles indexes, GetCertainList filesList)
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
        private static int index = 0;
        private static string fileName = string.Empty;
        private static GetCertainList list = new GetCertainList();
        private static string content = string.Empty;
        private static GetVariablesForFiles indexes = new GetVariablesForFiles();

        public static void PrintDiff(IntPtr repo, IntPtr diff, GetCertainList filesList, GetVariablesForFiles indexes)
        {
            list.filesNames = filesList.filesNames;
            list.listOfFiles = filesList.listOfFiles;
            list.listOfDiff = filesList.listOfDiff;
            list.filePath = filesList.filePath;
            list.hunks = filesList.hunks;
            list.filesCode = filesList.filesCode;
            list.startingIndexes = filesList.startingIndexes;
            list.listStartAt = filesList.listStartAt;
            int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);

            if (result != 0)
            {
                throw new Exception("Failed to iterate over diff.");
            }

            PrintNewFileContain(indexes, filesList, index);
        }

        public static int DiffFileCallback(LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string fileFullName = list.listOfFiles[indexes.fileIndex];
            fileName = list.filesNames[indexes.fileIndex];
            string text = GetDiffs.ResizeTextToFitInPanel($"{newFilePath}");
            if (newFilePath!.Contains(fileName))
            {
                list.listOfDiff.Add(text);
                list.filePath.Add(text);
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
            indexes.nextFile = true;
            return 0;
        }

        public static void PrintNewFileContain(GetVariablesForFiles indexes, GetCertainList list, int index)
        {
            if (list.listOfDiff.Contains("= \n\\ No newline at end of file\n"))
            {
                list.listOfDiff.RemoveAt(list.listOfDiff.IndexOf("= \n\\ No newline at end of file\n"));
            }

            string currentFileName = list.listOfFiles[indexes.fileIndex];
            FilesBackground(currentFileName, indexes);

            if (indexes.up == false)
            {
                indexes.fileIndex++;
            }

            if (indexes.fileIndex < list.listOfFiles.Count)
            {
                string completeFileName = "";
                completeFileName = list.listOfFiles[indexes.fileIndex];
                string fileName = completeFileName.Remove(0, 5);

                for (int i = index; i <= list.filePath.Count - 1; i++)
                {
                    var ddd = list.listOfDiff.IndexOf(list.listOfDiff.Find(x => x.Contains(list.filePath[i]))!);
                    list.startingIndexes.Add(ddd);
                }

                list.startingIndexes.Add(list.listOfDiff.Count);
            }
            
            Print(indexes, index, currentFileName, list);
        }

        public static void Print(GetVariablesForFiles indexes, int index, string fileFullName, GetCertainList list)
        {
            string text = string.Empty;
            GetDiffsLine.GetLineThroughtDiffsLines(indexes.currentLine, list.startingIndexes[indexes.fileIndex] - list.startingIndexes[indexes.fileIndex - 1], indexes, list);

            for (int i = index; i <= list.startingIndexes[indexes.fileIndex]; i++)
            {
                if (i == 0 && indexes.up == false && !list.listStartAt.Contains(index))
                {
                    list.listStartAt.Add(index);
                }

                if (indexes.row == Console.WindowHeight - 2)
                {
                    Navigate.NavigateThroughDiffsContent(index, list, indexes, fileFullName);
                }

                if (list.filePath.Contains(list.listOfDiff[i]))
                {
                    text = "filePath";
                }
                else if (list.hunks.Contains(list.listOfDiff[i]))
                {
                    text = "hunk";
                }
                else if (list.filesCode.Contains(list.listOfDiff[i]))
                {
                    text = "filesCode";
                }

                switch (text)
                {
                    case "filePath":
                        {
                            if (indexes.down == false && indexes.row == 0)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            indexes.row++;
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, indexes.row);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(list.listOfDiff[i]);
                            Console.ResetColor();
                        }
                        break;
                    case "hunk":
                        {
                            if (indexes.row == 0 && indexes.down == false)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            indexes.row++;
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, indexes.row);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(list.listOfDiff[i]);
                            Console.ResetColor();
                        }
                        break;
                    case "filesCode":
                        {
                            if (indexes.row == 0 && indexes.down == false)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            indexes.row++;
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, indexes.row);
                            content = list.listOfDiff[i];
                            SetColorForLinesOfCode(content);
                        }
                        break;
                }

                if (index == list.startingIndexes[indexes.fileIndex] - 1 && indexes.up == false || indexes.row == Console.WindowHeight - 2 && indexes.up == false)
                {
                    indexes.end = true;
                    indexes.nextFile = false;
                    indexes.numberOfNavigations++;

                    if (indexes.up == false && !list.listStartAt.Contains(index + 1))
                    {
                        list.listStartAt.Add(index + 1);
                    }

                    var d = list.listStartAt.IndexOf(index + 1) - 1;
                    index = list.listStartAt[indexes.x];
                    indexes.row = 0;
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 1);
                    break;
                }

                if (index < list.startingIndexes[indexes.fileIndex] && indexes.up == false)
                {
                    index++;
                }

                if (indexes.down == true)
                {
                    if (list.startingIndexes[indexes.fileIndex] <= Console.WindowHeight - 2)
                    {
                        TextFitInPanel(fileFullName, index, indexes);
                    }
                    else
                    {
                        TextExceedingPanelHeight(fileFullName, index, indexes);
                    }
                }
            }

            indexes.down = true;
            Navigate.NavigateThroughDiffsContent(index, list, indexes, fileFullName);
        }

        public static void FilesBackground(string fileFullName, GetVariablesForFiles indexes)
        {
            indexes.nextFile = true;
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            if (indexes.fileIndex <= list.listOfFiles.Count - 1)
            {
                if (indexes.fileIndex == size.height - 3)
                {

                }

                Console.SetCursorPosition(1, indexes.fileRow);
                Console.Write(new string(' ', (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4));
                Console.SetCursorPosition(1, indexes.fileRow);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(fileFullName);
                Console.ResetColor();

                if (indexes.fileIndex >= 1 || indexes.up == true)
                {
                    if (indexes.up == true)
                    {
                        indexes.fileRow++;
                        indexes.fileIndex++;
                        Console.SetCursorPosition(1, indexes.fileRow);
                        Console.BackgroundColor = ConsoleColor.Black;
                        fileFullName = list.listOfFiles[indexes.fileIndex];
                    }
                    else
                    {
                        Console.SetCursorPosition(1, indexes.fileRow - 1);
                        Console.BackgroundColor = ConsoleColor.Black;
                        fileFullName = list.listOfFiles[indexes.fileIndex - 1];
                    }

                    SetColoForFiles(fileFullName, indexes);
                }

                if (indexes.up == false)
                {
                    indexes.fileRow++;
                }
            }
        }

        private static void CodeBackground(int index, GetVariablesForFiles indexes, string fileFullName)
        {
            if (index < list.startingIndexes[indexes.fileIndex])
            {
                if (indexes.row < Console.WindowHeight - 2)
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, indexes.row + 1);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(list.listOfDiff[index]);
                    Console.ResetColor();
                    GetDiffsLine.GetLineThroughtDiffsLines(indexes.currentLine/* + 1*/, list.startingIndexes[indexes.fileIndex] - list.startingIndexes[indexes.fileIndex - 1], indexes, list);
                }
                else
                {
                    DiffHelper.Print(indexes, index, fileFullName, list);
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, indexes.row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(list.listOfDiff[index - 1]);
                }

                Console.ResetColor();
            }
        }

        public static void TextExceedingPanelHeight(string fileFullName, int index, GetVariablesForFiles indexes)
        {
            if (indexes.up == true)
            {
                index--;
                indexes.row = indexes.row - 2;
            }

            CodeBackground(index, indexes, fileFullName);
            Console.ResetColor();
            Navigate.NavigateThroughDiffsContent(index, list, indexes, fileFullName);
        }

        public static void TextFitInPanel(string fileFullName, int index, GetVariablesForFiles indexes)
        {
            if (indexes.down == true && indexes.row < Console.WindowHeight - 2)
            {
                if (indexes.up == true)
                {
                    index--;
                    indexes.row = indexes.row - 2;
                }

                CodeBackground(index, indexes, fileFullName);
                Navigate.NavigateThroughDiffsContent(index, list, indexes, fileFullName);
            }

            if (indexes.end == true)
            {
                Navigate.NavigateThroughDiffsContent(index, list, indexes, fileFullName);
            }
        }

        private static void SetColoForFiles(string fileFullName, GetVariablesForFiles indexes)
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
            switch (firstchar)
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

