using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace GitClient
{
    public class GetDiffs
    {
        public static void GetFileContent(IntPtr diff, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, CommitElements commitElements, GetCertainList filesList)
        {
            DiffHelper.PrintDiff(diff, filesList, variablesForCommits, variablesForFiles, commitElements);
        }

        public static string ResizeTextToFitInPanel(string line, GetVariablesForCommits variablesForCommits)
        {
            int width;
            if (variablesForCommits.pressRight == 1)
            {
                width = (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4;
            }
            else
            {
                width = Console.WindowWidth - 3;
            }

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

        public static void CleaningDiffPanel(GetVariablesForCommits variablesForCommits)
        {
            int height = Console.WindowHeight;
            int width = Console.WindowWidth;

            int x;
            int y;

            if (variablesForCommits.pressRight == 1)
            {
                x = width / 2 + 3;
                y = (width - 2) - (width / 2) - 4;
            }
            else
            {
                x = 1;
                y = height - 2;
            }

            for (int i = 1; i <= height - 2; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write(new string(' ', y));
            }

            Console.SetCursorPosition(x, 1);
        }
    }

    public class DiffHelper
    {
        private static string fileName = string.Empty;
        private static GetCertainList list = new GetCertainList();
        private static GetVariablesForFiles variablesForFiles = new GetVariablesForFiles();
        private static string content = string.Empty;
        private static GetVariablesForCommits variablesForCommit = new GetVariablesForCommits();

        public static void PrintDiff(IntPtr diff, GetCertainList filesList, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElements)
        {
            if (variablesForCommits.pressRight == 1)
            {
                if (variablesForFiles.nextFile == false)
                {
                    list.filesNames = filesList.filesNames;
                    list.listOfFiles = filesList.listOfFiles;
                    list.startingIndexes = filesList.startingIndexes;
                    list.listStartAt = filesList.listStartAt;
                    variablesForCommit = variablesForCommits;
                    list.listOfAllDiffs = filesList.listOfAllDiffs;

                    int i = 0;
                    while (i < list.listOfFiles.Count)
                    {
                        list.listOfAllDiffs.Add(new List<string>());
                        list.startingIndexes.Add(new List<int>());
                        i++;
                    }

                    int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);

                    if (result != 0)
                    {
                        throw new Exception("Failed to iterate over diff.");
                    }
                }

                GetDiffSmallPanel.GetDiffRelatedToTheSelectedFile(list, variablesForFiles, variablesForCommits, commitElements);

            }
            else
            {
                string currentFileName = list.listOfFiles[variablesForFiles.fileIndex];
                variablesForFiles.down = false;
                Print(variablesForCommits, commitElements, currentFileName);
            }
        }

        public static int DiffFileCallback(ref LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            //string fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
            fileName = list.filesNames[variablesForFiles.fileIndex];
            //list.filePath.Add(newFilePath!);
            string text = newFilePath!;

            if (newFilePath!.Contains(fileName))
            {
                variablesForFiles.indexDiff++;
                list.listOfAllDiffs[variablesForFiles.indexDiff].Add(text);
                //list.listOfDiff.Add(text);
                //list.startingIndexes[variablesForFiles.indexDiff].Add();
                //list.filePath.Add(text);
            }

            variablesForFiles.fileIndex++;
            return 0;
        }

        public static int DiffBinaryCallback(ref LibGit2Wrapper.GitDiffDelta delta, IntPtr binary, IntPtr payload)
        {
            return 0;
        }

        public static int DiffHunkCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, IntPtr payload)
        {
            byte[] filteredHeader = hunk.header.Where(c => c != '\0' && c != '0').ToArray();
            string hunkHeader = System.Text.Encoding.UTF8.GetString(filteredHeader);
            string text = hunkHeader;
            list.listOfAllDiffs[variablesForFiles.indexDiff].Add(text);
            //list.listOfDiff.Add(text);
            //list.hunks.Add(text);
            return 0;
        }

        public static int DiffLineCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, ref LibGit2Wrapper.GitDiffLine line, IntPtr payload)
        {
            string content = Marshal.PtrToStringAnsi(line.content, (int)line.content_len);
            if (content.StartsWith('\t'))
            {
                string output = content.Replace("\t", new string(' ', 4));
                content = output + content;
            }

            if (content.Contains("\n\t"))
            {
                content = content[..content.IndexOf("\n\t")];
            }

            string text = $"{(char)line.origin} {content}";

            list.listOfAllDiffs[variablesForFiles.indexDiff].Add(text);
            //list.listOfDiff.Add(text);
            //list.filesCode.Add(text);
            variablesForFiles.nextFile = true;
            return 0;
        }

        public static void Print(GetVariablesForCommits variablesForCommits, CommitElements commitElements, string fileFullName)
        {
            variablesForFiles.height = Console.WindowHeight;
            variablesForFiles.width = Console.WindowWidth;
            string text = string.Empty;
            //if (list.listOfFiles.Count == 1)
            //{
            //    variablesForFiles.totalLines = list.listOfAllDiffs[variablesForFiles.indexDiff].Count;
            //}
            //else
            //{
            //    variablesForFiles.totalLines = list.listOfAllDiffs[variablesForFiles.indexDiff].Count
            //    //variablesForFiles.totalLines = list.startingIndexes[variablesForFiles.indexDiff][variablesForFiles.fileIndex] - list.startingIndexes[variablesForFiles.indexDiff][variablesForFiles.fileIndex - 1];
            //}

            if (variablesForCommits.pressRight > 1)
            {
                variablesForFiles.row = 0;
                variablesForFiles.down = false;
            }

            variablesForFiles.totalLines = list.listOfAllDiffs[variablesForFiles.indexDiff].Count;

            GetDiffsLine.GetLineThroughtDiffsLines(variablesForCommits, variablesForFiles.currentLine, variablesForFiles.totalLines, list);
            Cursor.UpdateCursorPositionForDiffsList(variablesForCommits, variablesForFiles, list);

            int x;
            if (variablesForCommits.pressRight == 1)
            {
                x = variablesForFiles.width / 2 + 3;
            }
            else
            {
                x = 1;
            }


            for (int i = variablesForFiles.index; i < MaxValue(list, variablesForFiles); i++)
            {
                if (list.listOfAllDiffs[variablesForFiles.indexDiff][i].Contains(fileFullName.Remove(0, 5)))
                {
                    text = "filePath";
                }
                else if (list.listOfAllDiffs[variablesForFiles.indexDiff][i].StartsWith('@'))
                {
                    text = "hunk";
                }
                else
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
                            Console.SetCursorPosition(x, variablesForFiles.row);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            string textOutput = GetDiffs.ResizeTextToFitInPanel(list.listOfAllDiffs[variablesForFiles.indexDiff][i], variablesForCommits);
                            Console.Write(textOutput);
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
                            Console.SetCursorPosition(x, variablesForFiles.row);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            string textOutput = GetDiffs.ResizeTextToFitInPanel(list.listOfAllDiffs[variablesForFiles.indexDiff][i], variablesForCommits);
                            Console.Write(textOutput);
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
                            Console.SetCursorPosition(x, variablesForFiles.row);
                            content = GetDiffs.ResizeTextToFitInPanel(list.listOfAllDiffs[variablesForFiles.indexDiff][i], variablesForCommits);
                            SetColorForLinesOfCode(content, variablesForCommits);
                        }
                        break;
                }

                if (variablesForFiles.index < variablesForFiles.fileIndex && variablesForFiles.up == false)
                {
                    variablesForFiles.index++;
                }

                if (variablesForFiles.down == true)
                {
                    if (variablesForFiles.fileIndex <= variablesForFiles .height - 2)
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
            if (variablesForCommits.pressRight > 1)
            {
                Navigate.NavigateThroughDiffsContent(list, variablesForFiles, variablesForCommits, commitElements, fileFullName);
            }
        }

        public static int MaxValue(GetCertainList list, GetVariablesForFiles variablesForFiles)
        {
            return list.listOfAllDiffs[variablesForFiles.indexDiff].Count > variablesForFiles.height
                ? variablesForFiles.height : list.listOfAllDiffs[variablesForFiles.indexDiff].Count;
        }

        public static void FilesBackground(string fileFullName, GetVariablesForFiles variablesForFiles)
        {
            variablesForFiles.nextFile = true;
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            Cursor.UpdateCursorPositionForFilesList(variablesForFiles, list, size);

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
                PrintRemaingingFiles();
                variablesForFiles.filesReachPanelLimit = true;
            }
            
            Console.SetCursorPosition(1, variablesForFiles.fileRow);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(fileFullName);
            Console.ResetColor();

            if (variablesForFiles.filesReachPanelLimit == false && variablesForFiles.fileIndex >= 1 && variablesForFiles.fileIndex <= list.listOfFiles.Count - 1 || variablesForFiles.up == true)
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

            variablesForFiles.filesReachPanelLimit = false;
        }

        private static void PrintRemaingingFiles()
        {
            int position = variablesForFiles.fileRow;
            for (int i = variablesForFiles.fileIndex; i < list.listOfFiles.Count; i++) 
            {
                if (position == variablesForFiles.height)
                {
                    break;
                }

                Console.SetCursorPosition(1, position);
                Console.Write(list.listOfFiles[i]);
                Console.SetCursorPosition(1, position + 1);
                position++;
            }

        }

        private static void CleaningFilePanel()
        {
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            int width = Console.WindowWidth;
            int maxHeight = Console.WindowHeight - 1;
            int maxPosition = 0;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                maxPosition = 5;
            }
            else
            {
                maxPosition = 6;
            }

            for (int i = size.height + maxPosition; i < maxHeight; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(new string(' ', (width - 2) - (width / 2) - 4));
            }

            variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;
            Console.SetCursorPosition(1, size.height + maxPosition);
        }

        private static void CodeBackground(GetVariablesForCommits variablesForCommits, CommitElements commitElements, string fileFullName)
        {
            int height = Console.WindowHeight;

            int x;
            if (variablesForCommits.pressRight == 1)
            {
                x = variablesForFiles.width / 2 + 3;
            }
            else
            {
                x = 1;
            }

            if (variablesForFiles.index < list.startingIndexes[variablesForFiles.indexDiff][variablesForFiles.fileIndex])
            {
                if (variablesForFiles.row < height - 2)
                {
                    Console.SetCursorPosition(x, variablesForFiles.row + 1);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    string text = GetDiffs.ResizeTextToFitInPanel(list.listOfAllDiffs[variablesForFiles.indexDiff][variablesForFiles.index], variablesForCommits);
                    Console.Write(text);
                    Console.ResetColor();
                    GetDiffsLine.GetLineThroughtDiffsLines(variablesForCommits, variablesForFiles.currentLine, variablesForFiles.totalLines, list);
                }
                else
                {
                    DiffHelper.Print(variablesForCommits, commitElements, fileFullName);
                    Console.SetCursorPosition(x, variablesForFiles.row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    string text = GetDiffs.ResizeTextToFitInPanel(list.listOfAllDiffs[variablesForFiles.indexDiff][variablesForFiles.index - 1], variablesForCommits);
                    Console.Write(text);
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

        public static void SetColorForLinesOfCode(string content, GetVariablesForCommits variablesForCommits)
        {
            var firstChar = content.First();
            int x;
            if (variablesForCommits.pressRight == 1)
            {
                x = variablesForFiles.width / 2 + 3;
            }
            else
            {
                x = 1;
            }


            switch (firstChar)
            {
                case '+':
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(content);
                        Console.SetCursorPosition(x, variablesForFiles.row);
                        Console.ResetColor();
                    }
                    break;

                case '-':
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(content);
                        Console.SetCursorPosition(x, variablesForFiles.row);
                        Console.ResetColor();
                    }
                    break;

                default:
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(content);
                       // Console.SetCursorPosition(x, variablesForFiles.row);
                        Console.ResetColor();
                    }
                    break;
            }
        }
    }
}

