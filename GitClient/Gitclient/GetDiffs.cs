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
                    width = (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 4;
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
                y = (Console.WindowWidth - 2) - (Console.WindowWidth / 2) - 3;
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
        public static GetCertainList list = new GetCertainList();
        public static GetVariablesForFiles variablesForFiles = new GetVariablesForFiles();
        public static string content = string.Empty;
        public static GetVariablesForCommits variablesForCommit = new GetVariablesForCommits();

        public static void PrintDiff(IntPtr diff, GetCertainList filesList, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFile, CommitElements commitElements)
        {
            List<string> fileslist = new List<string>();
            int index = 0;
            int row = 0;

            if (variablesForFiles.unstageChanges == true)
            {
                fileslist = filesList.unstagedChangesFiles;
                index = variablesForFile.unstagedIndex;
                row = variablesForFile.fileRowUnstaged;
            }
            else
            {
                fileslist = filesList.stagedChangesFiles;
                index = variablesForFile.stagedIndex;
                row = variablesForFile.fileRowStaged;
            }

            if (variablesForCommits.pressRight == 1)
            {
                list = filesList;
                variablesForCommit = variablesForCommits;
                variablesForFiles = variablesForFile;
                
                if (variablesForFiles.nextFile == false && list.listOfAllDiffs.Count == 0)
                {
                    int result = DiffCallbackForeachFuntion.ReturnForeachCallback(diff, filesList, variablesForFile, variablesForCommits);

                    if (result != 0)
                    {
                        throw new Exception("Failed to iterate over diff.");
                    }

                    variablesForFile.fileIndex = 0;
                }
                
                GetDiffForSmallPanel.GetDiffRelatedToTheSelectedFile(list, variablesForFile, variablesForCommits, commitElements);
                Console.SetCursorPosition(1, variablesForFiles.fileRow);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(list.listOfFiles[variablesForFiles.fileIndex]);
                Console.ResetColor();
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

            //Cursor.UpdateCursorPositionForDiffsList(variablesForCommits, variablesForFiles, list);

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
                    fileFullName = list.unstagedChangesFiles[variablesForFiles.unstagedIndex];
                }
                else
                {
                    filesDiff = list.stagedChangesDiff;
                    fileFullName = list.stagedChangesFiles[variablesForFiles.stagedIndex];
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
                            SetColorForLinesOfCode(content, variablesForCommits, variablesForFiles);
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
            variablesForFiles.initialState = false;
                 
            if (variablesForCommit.logTab == false)
            {
                if (variablesForFiles.row < Console.WindowHeight - 2)
                {
                    variablesForFiles.down = false;
                }

                variablesForFiles.row = dimensions.tabHeight + 1;
            }


            if (variablesForCommits.pressRight > 1 && variablesForCommit.logTab == true || variablesForFiles.initialState == false)
            {
                Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list);
            }
        }

        public static int MaxValue(GetCertainList list, GetVariablesForFiles variablesForFiles)
        {
            int height = Console.WindowHeight - 2 - 4;
            return list.listOfAllDiffs[variablesForFiles.indexDiff].Count > height ? height : list.listOfAllDiffs[variablesForFiles.indexDiff].Count;
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
            Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list);
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
                Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list);
            }

            if (variablesForFiles.end == true)
            {
                Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElements, list);
            }
        }

        public static void SetColorForLinesOfCode(string content, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
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

