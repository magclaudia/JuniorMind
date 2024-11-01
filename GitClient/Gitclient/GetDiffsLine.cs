namespace GitClient
{
    public class GetDiffsLine
    {
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public static void GetLineThroughtDiffsLines(GetVariablesForCommits variablesForCommits, int row, int totalRows, GetCertainList list)
        {
            int x = 0;
            int length = $" Line: {row}/{totalRows}".Length;

            if (variablesForCommits.pressRight == 1)
            {
                x = Console.WindowWidth - length - 4;
            }
            else 
            {
                x = 1;
            }

            Console.SetCursorPosition(x, dimensions.tabHeight + 1);
            Console.Write(new string(' ', length + 1));
            Console.SetCursorPosition(x, dimensions.tabHeight + 1);
            Console.Write($" Line: {row}/{totalRows}");
        }

        public static void GetLineIfDownMoves(GetCertainList list, GetVariablesForFiles variables)
        {
            list.start.Add(variables.currentLine);
            if (list.start.Count >= 2 && list.start[list.start.Count - 1] - 1 == list.start[list.start.Count - 2])
            {
                list.start.RemoveAt(list.start.Count - 1);
            }
        }

        public static void GetLineIfUpMoves(int index, GetCertainList list, GetVariablesForFiles variablesForFiles)
        {
            //if (variablesForFiles.nextFile == true && !list.listOfDiff[index].StartsWith(list.filePath[variablesForFiles.fileIndex - 1]))
            //{
            //    variablesForFiles.currentLine = list.start[list.start.Count - 1];
            //    list.start.RemoveAt(list.start.Count - 1);
            //}
            //else
            //{
            //    variablesForFiles.currentLine = variablesForFiles.currentLine - (Console.WindowHeight - 2);
            //}
        }
    }
}
