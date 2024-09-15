using System;
using System.Runtime.InteropServices;

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

        public static void CleaningCodePanel()
        {
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;

            for (int i = 1; i <= height - 2; i++)
            {
                Console.SetCursorPosition(width / 2 + 3, i);
                Console.Write(new string(' ', (width - 2) - (width / 2) - 4));
            }

            Console.SetCursorPosition(width / 2 + 3, 1);
        }
    }

    public class DiffHelper
    {
        private static string fileName = string.Empty;
        private static GetCertainList list = new GetCertainList();
        private static GetVariablesForFiles variablesForFiles = new GetVariablesForFiles();
        private static string content = string.Empty;

        public static void PrintDiff(IntPtr repo, IntPtr diff, GetCertainList filesList, GetVariablesForCommits variablesForCommits, CommitElements commitElements)
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

            PrintNewFileContain(variablesForCommits, commitElements);
        }

        public static int DiffFileCallback(LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
            fileName = list.filesNames[variablesForFiles.fileIndex];
            list.filePath.Add(newFilePath!);
           
            string text = GetDiffs.ResizeTextToFitInPanel($"{newFilePath}");
           
            
            if (newFilePath!.Contains(fileName))
            {
                list.listOfDiff.Add(text);
                list.startingIndexes.Add(list.listOfDiff.Count - 1);
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
            byte[] filteredHeader = hunk.header.Where(c => c != '\0' && c != '0').ToArray();
            string hunkHeader = System.Text.Encoding.UTF8.GetString(filteredHeader);
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

        public static void PrintNewFileContain(GetVariablesForCommits variablesForCommits, CommitElements commitElements)
        {
            if (list.listOfDiff[variablesForFiles.index].StartsWith("=") || list.listOfDiff[variablesForFiles.index].StartsWith("<") || list.listOfDiff[variablesForFiles.index].StartsWith(">"))
            {
                list.listOfDiff.RemoveAt(list.listOfDiff.IndexOf("="));
                list.listOfDiff.RemoveAt(list.listOfDiff.IndexOf("<"));
                list.listOfDiff.RemoveAt(list.listOfDiff.IndexOf(">"));
            }

            variablesForFiles.fileIndex = 0;
            string currentFileName = list.listOfFiles[variablesForFiles.fileIndex];
            FilesBackground(currentFileName, variablesForFiles);

            if (variablesForFiles.up == false)
            {
                variablesForFiles.fileIndex++;
            }

            if (variablesForFiles.fileIndex <= list.listOfFiles.Count)
            {
                list.startingIndexes.Add(list.listOfDiff.Count);
            }

            variablesForFiles.a = list.listOfFiles.Count;
            Print(variablesForCommits, commitElements, currentFileName);
        }  

        public static void Print(GetVariablesForCommits variablesForCommits, CommitElements commitElements, string fileFullName)
        {
            variablesForFiles.height = Console.WindowHeight;
            variablesForFiles.width = Console.WindowWidth;
            string text = string.Empty;
            if (list.listOfFiles.Count == 1)
            {
                variablesForFiles.totalLines = list.listOfDiff.Count;
            }
            else
            {
                variablesForFiles.totalLines = list.startingIndexes[variablesForFiles.fileIndex] - list.startingIndexes[variablesForFiles.fileIndex - 1];
            }

            GetDiffsLine.GetLineThroughtDiffsLines(variablesForFiles.currentLine, variablesForFiles.totalLines, variablesForFiles, list);
            Cursor.UpdateCursorPositionForDiffsList(variablesForFiles, list);

            for (int i = variablesForFiles.index; i < list.startingIndexes[variablesForFiles.fileIndex]; i++)
            {
                if (i == 0 && variablesForFiles.up == false && !list.listStartAt.Contains(variablesForFiles.index))
                {
                    list.listStartAt.Add(variablesForFiles.index);
                }

                if (variablesForFiles.row == variablesForFiles.height - 2)
                {
                    Navigate.NavigateThroughDiffsContent(list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
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
                            Console.SetCursorPosition(variablesForFiles.width / 2 + 3, variablesForFiles.row);
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
                            Console.SetCursorPosition(variablesForFiles.width / 2 + 3, variablesForFiles.row);
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
                            Console.SetCursorPosition(variablesForFiles.width / 2 + 3, variablesForFiles.row);
                            content = list.listOfDiff[i];
                            SetColorForLinesOfCode(content);
                        }
                        break;
                }

                if (variablesForFiles.index == list.startingIndexes[variablesForFiles.fileIndex] - 1 && variablesForFiles.up == false || variablesForFiles.row == variablesForFiles.height - 2 && variablesForFiles.up == false)
                {
                    variablesForFiles.end = true;
                    variablesForFiles.nextFile = false;
                    variablesForFiles.numberOfNavigations++;

                    if (variablesForFiles.up == false && !list.listStartAt.Contains(variablesForFiles.index + 1))
                    {
                        list.listStartAt.Add(variablesForFiles.index + 1);
                    }

                    variablesForFiles.index = list.listStartAt[variablesForFiles.x];
                    variablesForFiles.row = 0;
                    Console.SetCursorPosition(variablesForFiles.width / 2 + 3, 1);
                    break;
                }

                if (variablesForFiles.index < list.startingIndexes[variablesForFiles.fileIndex] && variablesForFiles.up == false)
                {
                    variablesForFiles.index++;
                }

                if (variablesForFiles.down == true)
                {
                    if (list.startingIndexes[variablesForFiles.fileIndex] <= variablesForFiles.height - 2)
                    {
                        TextFitInPanel(fileFullName, variablesForFiles, variablesForCommits, commitElements);
                    }
                    else
                    {
                        TextExceedingPanelHeight(fileFullName, variablesForFiles, variablesForCommits, commitElements);
                    }
                }
            }

            variablesForFiles.down = true;
            Navigate.NavigateThroughDiffsContent(list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
        }

        public static void FilesBackground(string fileFullName, GetVariablesForFiles variablesForFiles)
        {
            variablesForFiles.nextFile = true;
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            Cursor.UpdateCursorPositionForFilesList(variablesForFiles, list);

            if (variablesForFiles.fileRow + 1 == Console.WindowHeight / 2 + 4 && variablesForFiles.up == true)
            {
                CleaningFilePanel();
                variablesForFiles.fileIndex = 0;
                while (variablesForFiles.fileRow <= Console.WindowHeight - 2)
                {
                    fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
                    SetColorForFiles(fileFullName, variablesForFiles);
                    variablesForFiles.fileIndex++;
                    variablesForFiles.fileRow++;
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                }

                variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;
                variablesForFiles.fileIndex = 0;
                variablesForFiles.x = 1;
                fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
            }

            if (list.listOfFiles.Count > size.height - 3 && variablesForFiles.fileIndex == size.height - 3 && variablesForFiles.up == false)
            {
                CleaningFilePanel();
            }
            
            Console.SetCursorPosition(1, variablesForFiles.fileRow);
            Console.Write(new string(' ', (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4));
            Console.SetCursorPosition(1, variablesForFiles.fileRow);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(fileFullName);
            Console.ResetColor();
            
            if (variablesForFiles.fileIndex >= 1 && variablesForFiles.fileIndex <= list.listOfFiles.Count - 1 || variablesForFiles.up == true)
            {
                if (variablesForFiles.up == true)
                {
                    variablesForFiles.fileRow++;
                    variablesForFiles.fileIndex++;
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                    Console.BackgroundColor = ConsoleColor.Black;
                    fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
                    SetColorForFiles(fileFullName, variablesForFiles);
                }
                else
                {
                    Console.SetCursorPosition(1, variablesForFiles.fileRow - 1);
                    Console.BackgroundColor = ConsoleColor.Black;
                    fileFullName = list.listOfFiles[variablesForFiles.fileIndex - 1];
                    SetColorForFiles(fileFullName, variablesForFiles);
                }
            }

            if (variablesForFiles.up == false && list.listOfFiles.Count > 1)
            {
                variablesForFiles.fileRow++;
            }
        }

        private static void CleaningFilePanel()
        {
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            int width = Console.WindowWidth;
            int maxHeight = Console.WindowHeight - 1;

            for (int i = size.height + 5; i < maxHeight; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(new string(' ', (width - 2) - (width / 2) - 4));
            }

            variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;
            Console.SetCursorPosition(1, size.height + 5);
        }

        private static void CodeBackground(GetVariablesForCommits variablesForCommits, CommitElements commitElements, string fileFullName)
        {
            int height = Console.WindowHeight;

            if (variablesForFiles.index < list.startingIndexes[variablesForFiles.fileIndex])
            {
                if (variablesForFiles.row < height - 2)
                {
                    Console.SetCursorPosition(variablesForFiles.width / 2 + 3, variablesForFiles.row + 1);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(list.listOfDiff[variablesForFiles.index]);
                    Console.ResetColor();
                    GetDiffsLine.GetLineThroughtDiffsLines(variablesForFiles.currentLine, variablesForFiles.totalLines, variablesForFiles, list);
                }
                else
                {
                    DiffHelper.Print(variablesForCommits, commitElements, fileFullName);
                    Console.SetCursorPosition(variablesForFiles.width / 2 + 3, variablesForFiles.row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(list.listOfDiff[variablesForFiles.index - 1]);
                }

                Console.ResetColor();
            }
        }

        public static void TextExceedingPanelHeight(string fileFullName, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, CommitElements commitElements)
        {
            if (variablesForFiles.up == true)
            {
                variablesForFiles.index--;
                variablesForFiles.row = variablesForFiles.row - 2;
            }

            CodeBackground(variablesForCommits, commitElements, fileFullName);
            Console.ResetColor();
            Navigate.NavigateThroughDiffsContent(list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
        }

        public static void TextFitInPanel(string fileFullName, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, CommitElements commitElements)
        {
            if (variablesForFiles.down == true && variablesForFiles.row < Console.WindowHeight - 2)
            {
                if (variablesForFiles.up == true)
                {
                    variablesForFiles.index--;
                    variablesForFiles.row = variablesForFiles.row - 2;
                }

                CodeBackground(variablesForCommits, commitElements, fileFullName);
                Navigate.NavigateThroughDiffsContent(list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
            }

            if (variablesForFiles.end == true)
            {
                Navigate.NavigateThroughDiffsContent(list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
            }
        }

        private static void SetColorForFiles(string fileFullName, GetVariablesForFiles variablesForFiles)
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
            var firstChar = content.First();
            switch (firstChar)
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

