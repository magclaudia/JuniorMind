using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class DrawLogPanel
    {
        private static int width = Console.WindowWidth - 1;
        private static int height = Console.WindowHeight - 1;
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public static void DrawLargePanel()
        {
            for (int i = 1; i < width; i++)
            {
                Console.SetCursorPosition(i, dimensions.textPanelHeight);
                Console.Write("─");
                Console.SetCursorPosition(i, height);
                Console.Write("─");
            }

            for (int i = dimensions.tabHeight + 2; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(width, i);
                Console.Write("│");
            }

            Console.SetCursorPosition(0, dimensions.textPanelHeight);
            Console.Write("┌");
            Console.SetCursorPosition(width, dimensions.textPanelHeight);
            Console.Write("┐");
            Console.SetCursorPosition(0, height);
            Console.Write("└");
            Console.SetCursorPosition(width, height);
            Console.Write("┘");
        }
    }
}
