using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient
{
    public class HeaderPanel
    {
        public static void Header(int numberOfFiles)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 + 11, 0);
            Console.Write("Message ");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 11, Console.WindowHeight / 2 + 1);
            Console.Write($"Files: {numberOfFiles} ");
        }
    }
}
