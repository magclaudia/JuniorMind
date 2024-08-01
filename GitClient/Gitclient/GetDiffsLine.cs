using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient
{
    public class GetDiffsLine
    {
        public static void GetRowThroughtDiffsLines(int index, int totalRows)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 + 6, 0);
            Console.Write($"Line: {index}/{totalRows} ");

        }
    }
}
