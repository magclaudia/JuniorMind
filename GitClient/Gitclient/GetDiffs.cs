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
        public static void GetFileContent(IntPtr repo, UIntPtr numDeltas, IntPtr diff, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, CommitElements commitElements, GetCertainList filesList)
        {
           DiffHelper.PrintDiff(repo, diff, filesList, variablesForCommits, commitElements);
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
        private static GetVariablesForFiles variablesForFiles = new GetVariablesForFiles();
        private static string content = string.Empty;

        public static void PrintDiff(IntPtr repo,IntPtr diff, GetCertainList filesList, GetVariablesForCommits variablesForCommits, CommitElements commitElements)
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

            PrintNewFileContain(variablesForCommits, commitElements, index);
        }

        public static int DiffFileCallback(LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
            fileName = list.filesNames[variablesForFiles.fileIndex];
            string text = GetDiffs.ResizeTextToFitInPanel($"{newFilePath}");
            if (newFilePath!.Contains(fileName))
            {
                list.listOfDiff.Add(text);
                list.filePath.Add(text);
            }

            variablesForFiles.fileIndex++;
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
            variablesForFiles.nextFile = true;
            return 0;
        }

        public static void PrintNewFileContain(GetVariablesForCommits variablesForCommits, CommitElements commitElements, /* GetCertainList list,*/ int index)
        {
            if (list.listOfDiff.Contains("= \n\\ No newline at end of file\n"))
            {
                list.listOfDiff.RemoveAt(list.listOfDiff.IndexOf("= \n\\ No newline at end of file\n"));
            }

            variablesForFiles.fileIndex = 0;
            string currentFileName = list.listOfFiles[variablesForFiles.fileIndex];
            FilesBackground(currentFileName, variablesForFiles);

            if (variablesForFiles.up == false)
            {
                variablesForFiles.fileIndex++;
            }

            if (variablesForFiles.fileIndex < list.listOfFiles.Count)
            {
                string completeFileName = "";
                completeFileName = list.listOfFiles[variablesForFiles.fileIndex];
                string fileName = completeFileName.Remove(0, 5);

                for (int i = index; i <= list.filePath.Count - 1; i++)
                {
                    var ddd = list.listOfDiff.IndexOf(list.listOfDiff.Find(x => x.Contains(list.filePath[i]))!);
                    list.startingIndexes.Add(ddd);
                }

                list.startingIndexes.Add(list.listOfDiff.Count);
            }
            
            Print(variablesForCommits, commitElements, index, currentFileName);
        }

        public static void Print(GetVariablesForCommits variablesForCommits, CommitElements commitElements, int index, string fileFullName/*, GetCertainList list*/)
        {
            string text = string.Empty;
            GetDiffsLine.GetLineThroughtDiffsLines(variablesForFiles.currentLine, list.startingIndexes[variablesForFiles.fileIndex] - list.startingIndexes[variablesForFiles.fileIndex - 1], variablesForFiles, list);

            for (int i = index; i <= list.startingIndexes[variablesForFiles.fileIndex]; i++)
            {
                if (i == 0 && variablesForFiles.up == false && !list.listStartAt.Contains(index))
                {
                    list.listStartAt.Add(index);
                }

                if (variablesForFiles.row == Console.WindowHeight - 2)
                {
                    Navigate.NavigateThroughDiffsContent(index, list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
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
                            if (variablesForFiles.down == false && variablesForFiles.row == 0)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            variablesForFiles.row++;
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, variablesForFiles.row);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(list.listOfDiff[i]);
                            Console.ResetColor();
                        }
                        break;
                    case "hunk":
                        {
                            if (variablesForFiles.row == 0 && variablesForFiles.down == false)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            variablesForFiles.row++;
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, variablesForFiles.row);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(list.listOfDiff[i]);
                            Console.ResetColor();
                        }
                        break;
                    case "filesCode":
                        {
                            if (variablesForFiles.row == 0 && variablesForFiles.down == false)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            variablesForFiles.row++;
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, variablesForFiles.row);
                            content = list.listOfDiff[i];
                            SetColorForLinesOfCode(content);
                        }
                        break;
                }

                if (index == list.startingIndexes[variablesForFiles.fileIndex] - 1 && variablesForFiles.up == false || variablesForFiles.row == Console.WindowHeight - 2 && variablesForFiles.up == false)
                {
                    variablesForFiles.end = true;
                    variablesForFiles.nextFile = false;
                    variablesForFiles.numberOfNavigations++;

                    if (variablesForFiles.up == false && !list.listStartAt.Contains(index + 1))
                    {
                        list.listStartAt.Add(index + 1);
                    }

                    var d = list.listStartAt.IndexOf(index + 1) - 1;
                    index = list.listStartAt[variablesForFiles.x];
                    variablesForFiles.row = 0;
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 1);
                    break;
                }

                if (index < list.startingIndexes[variablesForFiles.fileIndex] && variablesForFiles.up == false)
                {
                    index++;
                }

                if (variablesForFiles.down == true)
                {
                    if (list.startingIndexes[variablesForFiles.fileIndex] <= Console.WindowHeight - 2)
                    {
                        TextFitInPanel(fileFullName, index, variablesForFiles, variablesForCommits, commitElements);
                    }
                    else
                    {
                        TextExceedingPanelHeight(fileFullName, index, variablesForFiles, variablesForCommits, commitElements);
                    }
                }
            }

            variablesForFiles.down = true;
            Navigate.NavigateThroughDiffsContent(index, list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
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

        private static void CodeBackground(int index, GetVariablesForCommits variablesForCommits, CommitElements commitElements, string fileFullName)
        {
            if (index < list.startingIndexes[variablesForFiles.fileIndex])
            {
                if (variablesForFiles.row < Console.WindowHeight - 2)
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, variablesForFiles.row + 1);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(list.listOfDiff[index]);
                    Console.ResetColor();
                    GetDiffsLine.GetLineThroughtDiffsLines(variablesForFiles.currentLine, list.startingIndexes[variablesForFiles.fileIndex] - list.startingIndexes[variablesForFiles.fileIndex - 1], variablesForFiles, list);
                }
                else
                {
                    DiffHelper.Print(variablesForCommits, commitElements, index, fileFullName);
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 3, variablesForFiles.row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(list.listOfDiff[index - 1]);
                }

                Console.ResetColor();
            }
        }

        public static void TextExceedingPanelHeight(string fileFullName, int index, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, CommitElements commitElements )
        {
            if (variablesForFiles.up == true)
            {
                index--;
                variablesForFiles.row = variablesForFiles.row - 2;
            }

            CodeBackground(index, /*variablesForFiles, */variablesForCommits, commitElements, fileFullName);
            Console.ResetColor();
            Navigate.NavigateThroughDiffsContent(index, list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
        }

        public static void TextFitInPanel(string fileFullName, int index, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, CommitElements commitElements)
        {
            if (variablesForFiles.down == true && variablesForFiles.row < Console.WindowHeight - 2)
            {
                if (variablesForFiles.up == true)
                {
                    index--;
                    variablesForFiles.row = variablesForFiles.row - 2;
                }

                CodeBackground(index,/* variablesForFiles, */variablesForCommits, commitElements,  fileFullName);
                Navigate.NavigateThroughDiffsContent(index, list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
            }

            if (variablesForFiles.end == true)
            {
                Navigate.NavigateThroughDiffsContent(index, list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
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

