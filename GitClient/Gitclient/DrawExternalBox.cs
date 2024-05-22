using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient
{
    public class DrawExternalBox
    {
        public static void DrawBox()
        {
            var width = Console.WindowWidth - 1;
            var height = Console.WindowHeight - 1;

            Console.SetCursorPosition(0, 0);
            Console.Write("┌");
            Console.SetCursorPosition(width, 0);
            Console.Write("┐");
            Console.SetCursorPosition(0, height);
            Console.Write("└");
            Console.SetCursorPosition(width, height);
            Console.Write("┘");

            for (int i = 1; i < width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("─");
                Console.SetCursorPosition(i, height);
                Console.Write("─");
            }

            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(width, i);
                Console.Write("║");
            }
        }
    }
}
