using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class HeaderPanel
    {
        public static void Header(Indexes indexes)
        {
            if (indexes.rigth == true)
            {
                Console.SetCursorPosition(1, 0);
                Console.Write("Info ");
                Console.SetCursorPosition(1, Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2));
                Console.Write("Message ");
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 11, 0);
                Console.Write("Info ");
                Console.SetCursorPosition(Console.WindowWidth / 2 + 11, Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2));
                Console.Write("Message ");
            }
        }
    }
}
