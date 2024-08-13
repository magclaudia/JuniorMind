using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetDiffsLine
    {
        public static void GetRowThroughtDiffsLines(int row, int totalRows, VariablesForFiles indexes, GetCertainList list)
        {
            int length = $"Line: {row + 1}/{totalRows} ".Length;
            if (row == 0)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 0);
                Console.Write(new string(' ', length + 2));
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 0);
                Console.Write($"Line: {row + 1}/{totalRows} ");

            }
            else if (row <= indexes.diffForEachFileIndex + 1 && row >= 1)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 0);
                Console.Write(new string(' ', length + 2));
                Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 0);
                Console.Write($"Line: {row}/{totalRows} ");
            }
        }
    }
}
