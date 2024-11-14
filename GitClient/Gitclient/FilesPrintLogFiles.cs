using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class FilesPrintLogFiles
    {
        public static void PrintFilesForLog(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            TextToBeResize(list, variablesForCommits, variablesForFiles);
            DrawPanelRigthSide.FilesBox filesBox = new DrawPanelRigthSide.FilesBox();
            int y = 0;
            int x = 0;
            int height = filesBox.edgeFourY - filesBox.edgeOneY - 2;

            //if (height > list.listOfFiles.Count)
            //{
            //    height = list.listOfFiles.Count;
            //}

            if (variablesForCommits.enter == true && variablesForCommits.pressRight < 1)
            {
                y = Console.WindowHeight / 2 + 4;
                x = Console.WindowWidth / 2 + 11;
            }
            else
            {
                y = Console.WindowHeight / 2 + 4;
                x = 1;
            }

            int count = 0;
            int i = 0;

            if (variablesForCommits.pressRight == 1 && list.listOfFiles.Count > height)
            {
                i = list.logFilesStartAt[variablesForFiles.fileLogStartAt];
                //variablesForFiles.fileIndex = i;
            }

            while (count < height)
            {
                if (i == list.listOfFiles.Count)
                {
                    break;
                }

                GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, list.listOfFiles);
                y++;
                i++;
                count++;
            }
        }

        private static void TextToBeResize(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawPanelRigthSide.FilesBox position = new DrawPanelRigthSide.FilesBox();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
            variablesForFiles.projName = $"  ▾{directoryName}";
            Console.ForegroundColor = ConsoleColor.White;

            if (variablesForCommits.right == true)
            {
                Console.SetCursorPosition(1, position.edgeOneY);
                Console.Write($"Files: {list.listOfFiles.Count} ");
                Console.SetCursorPosition(1, position.edgeOneY + 1);
                Console.WriteLine(HandleResizingText(variablesForFiles.projName, dimensions, variablesForCommits, variablesForFiles));
            }
            else
            {
                Console.SetCursorPosition(position.edgeOneX + 1, position.edgeOneY);
                Console.Write($"Files: {list.listOfFiles.Count} ");
                Console.SetCursorPosition(position.edgeOneX + 1, position.edgeOneY + 1);
                Console.WriteLine(HandleResizingText(variablesForFiles.projName, dimensions, variablesForCommits, variablesForFiles));
            }

            Console.ResetColor();
            List<string> filesLog = new List<string>();

            foreach (var file in list.listOfFiles)
            {
                filesLog.Add(HandleResizingText(file, dimensions, variablesForCommits, variablesForFiles));
            }

            list.listOfFiles.Clear();
            list.listOfFiles = filesLog;
            
            if (variablesForCommits.pressRight == 1)
            {
                int height = position.edgeFourY - position.edgeOneY - 2;
                int count = height;
                list.logFilesStartAt.Add(0);

                while (count < list.listOfFiles.Count)
                {
                    list.logFilesStartAt.Add(count);
                    count += height;
                }
            }
        }

        private static string HandleResizingText(string text, DrawTabs.Dimensions dimensions, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            if (variablesForCommits.right == true)
            {
                if (text.Length > dimensions.widthRightLog)
                {
                    text = text.Substring(0, dimensions.widthRightLog);
                }
            }
            else
            {
                if (text.Length > dimensions.widthEnterLog)
                {
                    text = text.Substring(0, dimensions.widthEnterLog);
                }
            }

            return text;
        }
    }
}
