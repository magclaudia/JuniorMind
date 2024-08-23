using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetDiffsLine
    {
        public static void GetLineThroughtDiffsLines(int row, int totalRows, GetVariablesForFiles indexes, GetCertainList list)
        {
            int length = $"Line: {row}/{totalRows} ".Length;
            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 0);
            Console.Write(new string(' ', length + 2));
            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 0);
            Console.Write($"Line: {row}/{totalRows} ");
        }

        public static void GetLineIfDownMoves(GetCertainList list, GetVariablesForFiles variables)
        {
            list.start.Add(variables.currentLine - 1);
            if (list.start.Count >= 2 && list.start[list.start.Count - 1] - 1 == list.start[list.start.Count - 2])
            {
                list.start.RemoveAt(list.start.Count - 2);
            }
        }

        public static void GetLineIfUpMoves(int index, GetCertainList list, GetVariablesForFiles variables)
        {
            if (variables.nextFile == true && !list.listOfDiff[index].StartsWith(list.filePath[variables.fileIndex - 1]))
            {
                variables.currentLine = list.start[list.start.Count - 1];
                list.start.RemoveAt(list.start.Count - 1);
            }
            else
            {
                variables.currentLine = variables.currentLine - (Console.WindowHeight - 2);
            }
        }
    }
}
