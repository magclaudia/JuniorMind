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
            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, 0);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth - 1, 0);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("┘");

            for (int i = Console.WindowWidth / 2 + 11; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight / 2 - 1);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight / 2 + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            for (int i = 1; i < Console.WindowHeight / 2-1; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 10, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, Console.WindowHeight / 2 - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 2 - 1);
            Console.Write("┘");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, Console.WindowHeight / 2 + 1);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 2 + 1);
            Console.Write("┐");


            for (int i = Console.WindowHeight / 2 + 2; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 10, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            BigPanel();
        }

        private static void BigPanel()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 8, 0);
            Console.Write("┐");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 8, Console.WindowHeight - 1);
            Console.Write("┘");

            for (int i = 1; i < Console.WindowWidth / 2 + 8; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, i);
                Console.Write("║");
                Console.SetCursorPosition(1, i);
            }
        }
    }
}
