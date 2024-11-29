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
        public static void DrawLargePanel()
        {
            
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            for (int i = 1; i < dimensions.width; i++)
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
                Console.Write("║");
            }


            Console.SetCursorPosition(0, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(dimensions.width, dimensions.tabHeight + 1);
            Console.Write("┐");
            Console.SetCursorPosition(0, dimensions.height);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width, dimensions.height);
            Console.Write("┘");
        }
    }
}
