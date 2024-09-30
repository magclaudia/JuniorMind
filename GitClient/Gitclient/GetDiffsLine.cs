namespace GitClient
{
    public class GetDiffsLine
    {
        public static void GetLineThroughtDiffsLines(GetVariablesForCommits variablesForCommits, int row, int totalRows, GetCertainList list)
        {
            int x = 0;
            if (variablesForCommits.pressRight == 1)
            {
                x = Console.WindowWidth / 2 + 3;
            }
            else 
            {
                x = 1;
            }

            int length = $"Line: {row}/{totalRows} ".Length;
            Console.SetCursorPosition(x, 0);
            Console.Write(new string(' ', length + 2));
            Console.SetCursorPosition(x, 0);
            Console.Write($"Line: {row}/{totalRows} ");
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
