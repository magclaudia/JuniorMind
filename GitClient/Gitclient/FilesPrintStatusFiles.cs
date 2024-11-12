using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class FilesPrintStatusFiles
    {
        public static void PrintFilesForStatus(GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            
            int y = 0;
            int x = 1;
            int height = 0;

            if (list.unstagedChangesFiles.Count > 0)
            {
                Console.SetCursorPosition(1, dimensions.unstagedStart - 1);
                string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
                variablesForFiles.projName = $"  ▾{directoryName}";
                Console.Write(variablesForFiles.projName);
                
                y = dimensions.tabHeight + 3;
                height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
                int check = height;
                list.fileListStartAt.Add(0);

                while (check < list.unstagedChangesFiles.Count)
                {
                    list.fileListStartAt.Add(check);
                    check += height;
                }

                if (height > list.unstagedChangesFiles.Count)
                {
                    height = list.unstagedChangesFiles.Count;
                }

                for (int i = 0; i < height; i++)
                {
                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, list.unstagedChangesFiles);
                    y++;
                }
            }

            if (variablesForFiles.stageChanges == true || list.stagedChangesFiles.Count > 0)
            {
                Console.SetCursorPosition(1, dimensions.stagedStart - 1);
                string directoryName = Path.GetDirectoryName(variablesForFiles.filePath)!;
                variablesForFiles.projName = $"  ▾{directoryName}";
                Console.Write(variablesForFiles.projName);

                y = dimensions.stagedStart;
                height = dimensions.stagedEnd - dimensions.stagedStart;

                if (height > list.stagedChangesFiles.Count)
                {
                    height = list.stagedChangesFiles.Count;
                }

                int i = 0;

                if (height == variablesForFiles.fileIndex && list.stagedChangesFiles.Count > height)
                {
                    i = variablesForFiles.fileIndex;
                }

                while (i < height)
                {
                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, i, list.stagedChangesFiles);
                    y++;
                    i++;
                }
            }
        }
    }
}
