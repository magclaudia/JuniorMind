using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class DrawTabs
    {
        public struct Dimensions
        {
            public int width;
            public int height;
            public int tabWidth;
            public int tabHeight;
            public int changesPanelWidth;
            public int changesPanelHeight;
            public int unstagedStart;
            public int unstagedEnd;
            public int stagedStart;
            public int stagedEnd;

            public Dimensions()
            {
                width = Console.WindowWidth - 1;
                height = Console.WindowHeight - 1;
                tabWidth = 15;
                tabHeight = 2;
                changesPanelWidth = width / 2 - 1;
                changesPanelHeight = (height / 2 + 1) - tabHeight;
                unstagedStart = 5;
                unstagedEnd = changesPanelHeight;
                stagedStart = unstagedStart + unstagedEnd;
                stagedEnd = height;
            }
        }
       
        public static void DrawBorders()
        {
            DrawUpperBorders();
        }

        public static void DrawUpperBorders()
        {
            Dimensions dimensions = new Dimensions();

            for (int i = 1; i < dimensions.width; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight);
                Console.Write("─");
            }

            Console.SetCursorPosition(dimensions.tabWidth, dimensions.tabHeight - 1);
            Console.Write("│");
        }

        public static void DrawOnlyTabs()
        {
            Dimensions dimensions = new Dimensions();

            for (int i = 0; i < dimensions.width; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight);
                Console.Write("─");
            }

            Console.SetCursorPosition(dimensions.tabWidth, dimensions.tabHeight - 1);
            Console.Write("│");
        }

        public static void DrawBigPanelForDiff()
        {
            Dimensions dimensions = new Dimensions();
            for (int i = 0; i < dimensions.width; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, dimensions.height);
                Console.Write("─");
            }

            for (int i = dimensions.tabHeight + 2; i < dimensions.height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width, i);
                Console.Write("│");
            }

            Console.SetCursorPosition(1, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(1, dimensions.height);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width, dimensions.height);
            Console.Write("┘");
            Console.SetCursorPosition(dimensions.width, dimensions.tabHeight + 1);
            Console.Write("┐");
        }
    }
}
