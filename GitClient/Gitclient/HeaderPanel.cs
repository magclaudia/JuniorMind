using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class HeaderPanel
    {
        public static void Header()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 + 11, 0);
            Console.Write("Info ");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 11, Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2));
            Console.Write("Message ");
        }
    }
}
