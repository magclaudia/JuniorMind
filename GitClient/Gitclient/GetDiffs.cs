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
            if (variablesForCommits.logTab == true)
            {
                if (variablesForCommits.pressRight == 1)
                {
                    width = (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 5;
                }
                else
                {
                    width = Console.WindowWidth - 4;
                }
            }
            else
            {
                if (variablesForCommits.right == true)
                {
                    if (variablesForCommits.logTab == true)
                    {
                        width = Console.WindowWidth - 2;
                    }
                    else
                    {
                        width = Console.WindowWidth - 4;
                    }
                }
                else
                {
                    width = Console.WindowWidth / 2 - 3;
                }
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

        public static void CleaningEntireDiffPanel(GetVariablesForCommits variablesForCommits)
        {
            int height = Console.WindowHeight;

            int x = 0;
            int y = Console.WindowWidth;

            for (int i = 3; i <= height - 1; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write(new string(' ', y));
            }

            Console.SetCursorPosition(x, 3);
        }

        public static void CleaningHalfOfDiffPanel(GetVariablesForCommits variablesForCommits)
        {
            int x = 0;
            int y = 0;
            int z = 0;
            int height = 0;

            if (variablesForCommits.logTab == true)
            {
                height = Console.WindowHeight - 2;
                x = Console.WindowWidth / 2 + 3;
                y = (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4;
                z = 3;
            }
            else
            {
                height = Console.WindowHeight - 2;
                x = Console.WindowWidth / 2 + 1;
                y = Console.WindowWidth / 2 - 2;
                z = 3;
            }

            for (int i = z; i <= height; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write(new string(' ', y));
            }

            Console.SetCursorPosition(x, z);
        }
    }

    public class DiffHelper
    {
        private static string fileName = string.Empty;
        private static GetCertainList list = new GetCertainList();
        private static GetVariablesForFiles variablesForFiles = new GetVariablesForFiles();
        private static string content = string.Empty;
        private static GetVariablesForCommits variablesForCommit = new GetVariablesForCommits();
        private static GetVariablesForTabs tab = new GetVariablesForTabs();
        private static DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();

        public static void PrintDiff(IntPtr diff, GetCertainList filesList, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFile, CommitElements commitElements)
        {
            if (variablesForCommits.pressRight == 1)
            {
                list = filesList;
                variablesForCommit = variablesForCommits;
                variablesForFiles = variablesForFile;
                
                if (variablesForFiles.nextFile == false && list.listOfAllDiffs.Count == 0)
                {
                    int result = DiffCallbackForeachFuntion.ReturnForeachCallback(diff, filesList, variablesForFile, variablesForCommits, tab);

                    if (result != 0)
                    {
                        throw new Exception("Failed to iterate over diff.");
                    }

                    variablesForFile.fileIndex = 0;
                }
                
                GetDiffForSmallPanel.GetDiffRelatedToTheSelectedFile(list, variablesForFile, variablesForCommits, commitElements);

            }
            else
            {
                string currentFileName = list.listOfFiles[variablesForFiles.fileIndex];
                variablesForFiles.down = false;
                variablesForFile.diffMoves = false;
                Print(variablesForCommits, variablesForFiles, commitElements, list);
            }
        }

        public static void Print(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElements, GetCertainList list)
        {
            variablesForFiles.height = Console.WindowHeight;
            variablesForFiles.width = Console.WindowWidth;
            string text = string.Empty;
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (variablesForFiles.row == 3 && variablesForFiles.currentLine > Console.WindowHeight - 2 && variablesForFiles.diffMoves == false)
            {
                variablesForFiles.down = false;
            }

            int x;
            List<List<string>> filesDiff = new List<List<string>>();
            string fileFullName = "";
            int height = 0;

            Cursor.UpdateCursorPositionForDiffsList(variablesForCommits, variablesForFiles, list);

            if (variablesForCommits.logTab == true)
            {
                if (variablesForFiles.diffMoves == false)
                {
                    variablesForFiles.row = dimensions.tabHeight + 1;
                }

                if (variablesForFiles.index == 0 && variablesForFiles.down == false && variablesForFiles.up == false)
                {
                    variablesForFiles.stop = 0;
                }

                variablesForFiles.totalLines = list.listOfAllDiffs[variablesForFiles.indexDiff].Count;
                GetDiffsLine.GetLineThroughtDiffsLines(variablesForCommits, variablesForFiles.currentLine, variablesForFiles.totalLines, list);

                filesDiff = list.listOfAllDiffs;
                fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
                height = variablesForFiles.height - 2;
                
                if (variablesForCommits.pressRight == 1)
                {
                    x = variablesForFiles.width / 2 + 3;
                }
                else
                {
                    x = 1;
                }
            }
            else
            {
                if (variablesForCommits.right == true)
                {
                    x = 1;

                    if (variablesForFiles.down == false && variablesForFiles.up == false)
                    {
                        variablesForFiles.row = dimensions.tabHeight + 1;
                    }
                }
                else
                {
                    x = Console.WindowWidth / 2 + 1;
                    variablesForFiles.row = dimensions.tabHeight + 1;
                }

                if (variablesForFiles.unstageChanges == true)
                {
                    filesDiff = list.unstagedChangesDiff;
                    fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
                }
                else
                {
                    filesDiff = list.stagedChangesDiff;
                    fileFullName = list.stagedChangesFiles[variablesForFiles.fileIndex];

                }

                height = (Console.WindowHeight - 2) - (dimensions.tabHeight + 1);
            }

            for (int i = variablesForFiles.index; i < filesDiff[variablesForFiles.indexDiff].Count; i++)
            {
                if (variablesForFiles.row == Console.WindowHeight - 2)
                {
                    variablesForFiles.numberOfNavigations++;
                    break;
                }
                else if (variablesForCommits.esc == true && variablesForCommits.pressRight == 1)
                {
                    break;
                }

                if (filesDiff[variablesForFiles.indexDiff][i].Contains(fileFullName.Remove(0, 5)))
                {
                    text = "filePath";
                }
                else if (filesDiff[variablesForFiles.indexDiff][i].StartsWith('@'))
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
                            if (variablesForFiles.down == false && variablesForFiles.row == dimensions.tabHeight + 1)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            variablesForFiles.row++;
                            Console.SetCursorPosition(x, variablesForFiles.row);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            string textOutput = GetDiffs.ResizeTextToFitInPanel(filesDiff[variablesForFiles.indexDiff][i], variablesForCommits);
                            Console.Write(textOutput);
                            Console.ResetColor();
                        }
                        break;
                    case "hunk":
                        {
                            if (variablesForFiles.row == dimensions.tabHeight + 1 && variablesForFiles.down == false  && variablesForFiles.up == false)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            variablesForFiles.row++;
                            Console.SetCursorPosition(x, variablesForFiles.row);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            string textOutput = GetDiffs.ResizeTextToFitInPanel(filesDiff[variablesForFiles.indexDiff][i], variablesForCommits);
                            Console.Write(textOutput);
                            Console.ResetColor();
                        }
                        break;
                    case "filesCode":
                        {
                            if (variablesForFiles.row == dimensions.tabHeight + 1 && variablesForFiles.down == false)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            variablesForFiles.row++;
                            Console.SetCursorPosition(x, variablesForFiles.row);
                            content = GetDiffs.ResizeTextToFitInPanel(filesDiff[variablesForFiles.indexDiff][i], variablesForCommits);
                            SetColorForLinesOfCode(content, variablesForCommits);
                        }
                        break;
                }


                if (variablesForFiles.down == false && variablesForFiles.up == false && variablesForCommit.logTab == true)
                {
                    variablesForFiles.stop++;
                }
                

                if (variablesForFiles.up == false && variablesForFiles.index < filesDiff[variablesForFiles.indexDiff].Count && variablesForFiles.down == true)
                {
                    variablesForFiles.index++;
                }

                if (variablesForFiles.down == true || variablesForFiles.up == true)
                {
                    if (variablesForFiles.index < height)
                    {
                        TextFitInPanel(fileFullName, variablesForFiles, variablesForCommits, list, commitElements);
                    }
                    else
                    {
                        TextExceedingPanelHeight(fileFullName, variablesForFiles, variablesForCommits, list, commitElements);
                    }
                }
            }

            variablesForFiles.down = true;

            if (variablesForCommit.logTab == false)
            {
                if (variablesForFiles.row < Console.WindowHeight - 2)
                {
                    variablesForFiles.down = false;
                }

                variablesForFiles.row = dimensions.tabHeight + 1;
            }


            if (variablesForCommits.pressRight > 1 && variablesForCommit.logTab == true || tab.initialState == false)
            {
                Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list, tab);
            }
        }

        public static int MaxValue(GetCertainList list, GetVariablesForFiles variablesForFiles)
        {
            int height = Console.WindowHeight - 2 - 4;
            return list.listOfAllDiffs[variablesForFiles.indexDiff].Count > height ? height : list.listOfAllDiffs[variablesForFiles.indexDiff].Count;
        }

        public static void FilesBackground(string fileFullName, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits)
        {
            int y = 0;
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (variablesForCommit.logTab == true)
            {
                variablesForFiles.nextFile = true;
                DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
                y = size.height - dimensions.tabHeight + 1;
            }

            Cursor.UpdateCursorPositionForFilesList(variablesForFiles, variablesForCommits, list, size);

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
                variablesForFiles.indexForLog = 1;
                fileFullName = list.listOfFiles[variablesForFiles.fileIndex];
            }

            if (list.listOfFiles.Count > y && variablesForFiles.fileIndex == y && variablesForFiles.up == false)
            {
                CleaningFilePanel();
                PrintRemaingingFiles();
                variablesForFiles.filesReachPanelLimit = true;
            }

            if (variablesForCommits.logTab == false && variablesForCommits.esc == true)
            {
                variablesForCommits.esc = false;
            }

            Console.SetCursorPosition(1, variablesForFiles.fileRow);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(fileFullName);
            Console.ResetColor();

            if (variablesForFiles.filesReachPanelLimit == false && variablesForFiles.fileIndex >= 1 && variablesForFiles.fileIndex <= list.listOfFiles.Count - 1 || variablesForFiles.up == true )
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

        public static void CodeBackground(GetVariablesForCommits variablesForCommits, GetCertainList list, GetVariablesForFiles variablesForFiles, CommitElements commitElements, string fileFullName)
        {
            int height = Console.WindowHeight;

            List<List<string>> filesDiff = new List<List<string>>();

            if (variablesForCommits.logTab == true)
            {
                filesDiff = list.listOfAllDiffs;
            }
            else
            {
                if (variablesForFiles.unstageChanges == true)
                {
                    filesDiff = list.unstagedChangesDiff;
                }
                else
                {
                    filesDiff = list.stagedChangesDiff;
                }
            }


            int x;
            if (variablesForCommits.pressRight == 1 && variablesForCommits.logTab == true)
            {
                x = variablesForFiles.width / 2 + 3;
            }
            else
            {
                x = 1;
            }


            if (variablesForFiles.row < height - 2)
            {
                Console.SetCursorPosition(x, variablesForFiles.row + 1);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                string text = GetDiffs.ResizeTextToFitInPanel(filesDiff[variablesForFiles.indexDiff][variablesForFiles.index], variablesForCommits);
                Console.Write(text);
                Console.ResetColor();

                if (variablesForCommits.logTab == true)
                {
                    GetDiffsLine.GetLineThroughtDiffsLines(variablesForCommits, variablesForFiles.currentLine, variablesForFiles.totalLines, list);
                }
            }
            else
            {
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
                Console.SetCursorPosition(x, variablesForFiles.row);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                string text = GetDiffs.ResizeTextToFitInPanel(filesDiff[variablesForFiles.indexDiff][variablesForFiles.index - 1], variablesForCommits);
                Console.Write(text);
            }

            Console.ResetColor();
        }

        public static void TextExceedingPanelHeight(string fileFullName, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list, CommitElements commitElements)
        {
            if (variablesForFiles.up == true)
            {
                variablesForFiles.index--;
                variablesForFiles.row = variablesForFiles.row - 2;
            }

            CodeBackground(variablesForCommits, list, variablesForFiles, commitElements,fileFullName);
            Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list, tab);
        }

        public static void TextFitInPanel(string fileFullName, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetCertainList list, CommitElements commitElements)
        {
            if (variablesForFiles.down == true && variablesForFiles.row < Console.WindowHeight - 2 || variablesForFiles.up == true)
            {
                if (variablesForFiles.up == true)
                {
                    variablesForFiles.index--;
                    variablesForFiles.row = variablesForFiles.row - 2;
                }

                CodeBackground(variablesForCommits, list, variablesForFiles, commitElements, fileFullName);
                Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list, tab);
            }

            if (variablesForFiles.end == true)
            {
                Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list, tab);
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
                        Console.ResetColor();
                    }
                    break;
            }
        }
    }
}

