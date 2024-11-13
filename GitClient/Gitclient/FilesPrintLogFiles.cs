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
            var position = new DrawPanelRigthSide.FilesBox();
            string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
            variablesForFiles.projName = $"  ▾{directoryName}";

            Console.ForegroundColor = ConsoleColor.White;
            if (variablesForCommits.right == true)
            {
                Console.SetCursorPosition(1, position.edgeOneY);
                Console.Write($"Files: {list.listOfFiles.Count} ");
                Console.SetCursorPosition(1, position.edgeOneY + 1);
                Console.WriteLine(variablesForFiles.projName);
            }
            else
            {
                Console.SetCursorPosition(position.edgeOneX + 1, position.edgeOneY);
                Console.Write($"Files: {list.listOfFiles.Count} ");
                Console.SetCursorPosition(position.edgeOneX + 1, position.edgeOneY + 1);
                Console.WriteLine(variablesForFiles.projName);
            }

            Console.ResetColor();

            int y = 0;
            int x = 0;
            int height = Console.WindowHeight - variablesForFiles.fileRow - 1;

            if (height > list.listOfFiles.Count)
            {
                height = list.listOfFiles.Count;
            }

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

            for (int i = 0; i < height; i++)
            {
                GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, list.listOfFiles);
                y++;
            }
        }
    }
}
