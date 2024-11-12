using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetDiffForSmallPanel
    {

        public static void GetDiffRelatedToTheSelectedFile(GetCertainList list, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, CommitElements commitElements)
        {
            variablesForFiles.indexDiff = variablesForFiles.fileIndex;
            
            string currentFileName = list.listOfFiles[variablesForFiles.fileIndex];
            
            if (variablesForFiles.fileIndex == 0)
            {
                GetFiles.FilesBackground(currentFileName, variablesForFiles, variablesForCommits, list);
            }

            Print(list, variablesForCommits, variablesForFiles, commitElements, currentFileName);
        }

        public static void Print(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElements, string fileFullName)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            variablesForFiles.totalLines = list.listOfAllDiffs[variablesForFiles.indexDiff].Count;

            GetDiffsLine.GetLineThroughtDiffsLines(variablesForCommits, variablesForFiles.currentLine, variablesForFiles.totalLines, list);
            Cursor.UpdateCursorPositionForDiffsList(variablesForCommits, variablesForFiles, list);

            int x;

            if (variablesForCommits.pressRight == 1)
            {
                x = variablesForFiles.width / 2 + 4;
            }
            else
            {
                x = 1;
            }

            int stop = DiffHelper.MaxValue(list, variablesForFiles);
            string text;

            for (int i = variablesForFiles.index; i < stop; i++)
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
                            if (variablesForFiles.down == false && variablesForFiles.row == dimensions.tabHeight + 1)
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
                            if (variablesForFiles.row == dimensions.tabHeight + 1 && variablesForFiles.down == false)
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
                            if (variablesForFiles.row == dimensions.tabHeight + 1 && variablesForFiles.down == false)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            variablesForFiles.row++;
                            Console.SetCursorPosition(x, variablesForFiles.row);
                            string content = GetDiffs.ResizeTextToFitInPanel(list.listOfAllDiffs[variablesForFiles.indexDiff][i], variablesForCommits);
                            DiffHelper.SetColorForLinesOfCode(content, variablesForCommits);
                        }
                        break;
                }
            }
        }
    }
}
