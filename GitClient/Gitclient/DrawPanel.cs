using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient
{
    public class DrawPanel
    {
        public static void Panel()
        {
            var width = Console.WindowWidth - 1;
            var height = Console.WindowHeight - 1;

            Console.SetCursorPosition(Console.WindowWidth / 2, 0);
            Console.Write("┌");
            Console.SetCursorPosition(width, 0);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth / 2, height);
            Console.Write("└");
            Console.SetCursorPosition(width, height);
            Console.Write("┘");

            for (int i = Console.WindowWidth / 2 + 1; i < width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("-");
                Console.SetCursorPosition(i, height/2 - 1);
                Console.Write("-");
                Console.SetCursorPosition(i, height/2 + 1);
                Console.Write("-");
                Console.SetCursorPosition(i, height);
                Console.Write("-");
            }

            for (int i = 1; i < height/2-1; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2, i);
                Console.Write("|");
                Console.SetCursorPosition(width, i);
                Console.Write("|");
            }

            Console.SetCursorPosition(Console.WindowWidth / 2, height / 2 - 1);
            Console.Write("└");
            Console.SetCursorPosition(width, height / 2 - 1);
            Console.Write("┘");
            Console.SetCursorPosition(Console.WindowWidth / 2, height / 2 + 1);
            Console.Write("┌");
            Console.SetCursorPosition(width, height / 2 + 1);
            Console.Write("┐");


            for (int i = height / 2 + 2; i < height; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2, i);
                Console.Write("|");
                Console.SetCursorPosition(width, i);
                Console.Write("|");
            }
        }
    }
}
