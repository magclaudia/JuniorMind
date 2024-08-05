using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetDiffsLine
    {
        public static void GetRowThroughtDiffsLines(int row, int totalRows)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 + 3, 0);
            Console.Write($"Line: {row + 1}/{totalRows} ");
        }
    }
}
