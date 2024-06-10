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
            Console.Write("Message ");
        }
    }
}
