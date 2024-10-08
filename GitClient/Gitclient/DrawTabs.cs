using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GitClient.DrawTabs;

namespace GitClient
{
    public class DrawTabs
    {
        private static int width = Console.WindowWidth - 1;
        private static int height = Console.WindowHeight - 1;
        private static int numberOfTabs = 2;

        public struct Dimensions
        {
            public int tabWidth;
            public int tabHeight;
            public int repoPathWidth;
            public int repoPathHeight;
            public int textPanelWidth;
            public int textPanelHeight;

            public Dimensions()
            {
                tabWidth = 15;
                tabHeight = 2;
                repoPathWidth = width;
                repoPathHeight = tabHeight + 3;
                textPanelWidth = width;
                textPanelHeight = tabHeight + repoPathHeight - 1;
            }
        }
       
        public static void DrawBorders()
        {
            DrawUpperBorders();
            DrawPanelForRepoPath();
            DrawPanelBordersForText();
        }
        public static void DrawUpperBorders()
        {
            Dimensions dimensions = new Dimensions();

            Console.SetCursorPosition(0, dimensions.tabHeight);
            Console.Write("┗");
            Console.SetCursorPosition(width, dimensions.tabHeight);
            Console.Write("┛");
            Console.SetCursorPosition(0, 0);
            Console.Write("┏");
            Console.SetCursorPosition(width, 0);
            Console.Write("┓");


            for (int i = 1; i < width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("─");

                Console.SetCursorPosition(i, dimensions.tabHeight);
                Console.Write("─");
            }


            Console.SetCursorPosition(0, dimensions.tabHeight - 1);
            Console.Write("┃");
            Console.SetCursorPosition(width, dimensions.tabHeight - 1);
            Console.Write("┃");

            int x = dimensions.tabWidth;
            for (int i = 0; i < numberOfTabs; i++)
            {
                Console.SetCursorPosition(x, dimensions.tabHeight - 1);
                Console.Write("┃");
                Console.SetCursorPosition(x, 0);
                Console.Write("┳");
                Console.SetCursorPosition(x, dimensions.tabHeight);
                Console.Write("┻");
                x = dimensions.tabWidth * 2;
            }
        }

        public static void DrawOnlyTabs(int start, int stop)
        {
            Dimensions dimensions = new Dimensions();

            for (int i = start; i < stop; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("─");

                Console.SetCursorPosition(i, dimensions.tabHeight);
                Console.Write("─");
            }

            Console.SetCursorPosition(start, 1);
            Console.SetCursorPosition(start, dimensions.tabHeight);
            Console.Write("┗");
            Console.SetCursorPosition(stop, dimensions.tabHeight);
            Console.Write("┻");
            Console.SetCursorPosition(start, 0);
            Console.Write("┏");
            Console.SetCursorPosition(stop, 0);
            Console.Write("┳");
            Console.SetCursorPosition(dimensions.tabWidth, 0);
            Console.Write("┳");
            Console.SetCursorPosition(dimensions.tabWidth, dimensions.tabHeight);
            Console.Write("┻");

            Console.SetCursorPosition(start, dimensions.tabHeight - 1);
            Console.Write("┃");
            Console.SetCursorPosition(dimensions.tabWidth, dimensions.tabHeight - 1);
            Console.Write("┃");
            Console.SetCursorPosition(stop, dimensions.tabHeight - 1);
            Console.Write("┃");
        }

        public static void DrawPanelForRepoPath()
        {
            Dimensions dimensions = new Dimensions();
            
            Console.SetCursorPosition(0, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(0, dimensions.repoPathHeight);
            Console.Write("└");
            Console.SetCursorPosition(width, dimensions.tabHeight + 1);
            Console.Write("┐");
            Console.SetCursorPosition(width, dimensions.repoPathHeight);
            Console.Write("┘");

            for (int i = dimensions.tabHeight + 2; i < dimensions.repoPathHeight; i++) 
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(width, i);
                Console.Write("│");
            }

            for (int i = 1; i < width; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, dimensions.repoPathHeight);
                Console.Write("─");
            }
        }

        public static void DrawPanelBordersForText()
        {
            Dimensions dimensions = new Dimensions();

            Console.SetCursorPosition(0, dimensions.textPanelHeight);
            Console.Write("┌");
            Console.SetCursorPosition(width, dimensions.textPanelHeight);
            Console.Write("┐");
            Console.SetCursorPosition(0, height);
            Console.Write("└");
            Console.SetCursorPosition(width, height);
            Console.Write("┘");

            for (int i = 1; i < width; i++)
            {
                Console.SetCursorPosition(i, dimensions.textPanelHeight);
                Console.Write("─");
                Console.SetCursorPosition(i, height);
                Console.Write("─");
            }

            for (int i = dimensions.textPanelHeight + 1; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(width, i);
                Console.Write("│");
            }
        }
    }
}
