using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class DrawTabs
    {
        private static int width = Console.WindowWidth - 1;
        private static int height = Console.WindowHeight - 1;

        public struct Dimensions
        {
            public int tabWidth;
            public int tabHeight;
            public int textPanelWidth;
            public int textPanelHeight;

            public Dimensions()
            {
                tabWidth = 15;
                tabHeight = 2;
                textPanelWidth = width;
                textPanelHeight = tabHeight + 1;
            }
        }
       
        public static void DrawBorders()
        {
            DrawUpperBorders();
        }

        public static void DrawUpperBorders()
        {
            Dimensions dimensions = new Dimensions();

            for (int i = 1; i < width; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight);
                Console.Write("─");
            }

            Console.SetCursorPosition(dimensions.tabWidth, dimensions.tabHeight - 1);
            Console.Write("│");
        }

        public static void DrawOnlyTabs(int start, int stop)
        {
            Dimensions dimensions = new Dimensions();

            for (int i = start; i < stop; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight);
                Console.Write("─");
            }

            Console.SetCursorPosition(dimensions.tabWidth, dimensions.tabHeight - 1);
            Console.Write("│");
        }
    }
}
