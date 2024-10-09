using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    internal class DrawStatus
    {
        private static int width = Console.WindowWidth - 1;
        private static int height = Console.WindowHeight - 1;
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public static void DrawPanelsForStatus()
        {
            Changes();
        }

       
        public static void Changes()
        {
            for (int i = dimensions.textPanelHeight + 1; i < height; i++)
            {
                Console.SetCursorPosition(width, i);
                Console.Write("│");
                Console.SetCursorPosition(width / 2, i);
                Console.Write("│");
            }

            for (int i = dimensions.textPanelHeight + 1; i < height / 2 + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(width / 2 - 1, i);
                Console.Write("│");
            }

            for (int i = height / 2 + 3; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(width / 2 - 1, i);
                Console.Write("│");
            }

            for (int i = 1; i < width / 2 - 1; i++)
            {
                Console.SetCursorPosition(i, dimensions.textPanelHeight);
                Console.Write("─");
                Console.SetCursorPosition(i, height / 2 + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, height / 2 + 2);
                Console.Write("─");
                Console.SetCursorPosition(i, height);
                Console.Write("─");
            }

            for (int i = width / 2 + 1; i < width; i++)
            {
                Console.SetCursorPosition(i, dimensions.textPanelHeight);
                Console.Write("─");
                Console.SetCursorPosition(i, height);
                Console.Write("─");
            }

            Console.SetCursorPosition(width / 2, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(width / 2, height);
            Console.Write("└");
            Console.SetCursorPosition(width, height);
            Console.Write("┘");
            Console.SetCursorPosition(width, dimensions.tabHeight + 1);
            Console.Write("┐");

            Console.SetCursorPosition(0, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(0, height / 2 + 1);
            Console.Write("└");
            Console.SetCursorPosition(width / 2 - 1, dimensions.tabHeight + 1);
            Console.Write("┐");
            Console.SetCursorPosition(width / 2 - 1, height / 2 + 1);
            Console.Write("┘");
            Console.SetCursorPosition(0, height / 2 + 2);
            Console.Write("┌");
            Console.SetCursorPosition(0, height);
            Console.Write("└");
            Console.SetCursorPosition(width / 2 - 1, height);
            Console.Write("┘");
            Console.SetCursorPosition(width / 2 - 1, height / 2 + 2);
            Console.Write("┐");


        }
    }
}
