using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class DrawStatus
    {
        public static void DrawPanelsForStatus()
        {
            Changes();
        }

        public static void Changes()
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            for (int i = dimensions.tabHeight + 2; i < dimensions.height; i++)
            {
                Console.SetCursorPosition(dimensions.width, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width / 2, i);
                Console.Write("│");
            }

            for (int i = dimensions.tabHeight + 2; i < dimensions.height / 2 + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width / 2 - 1, i);
                Console.Write("│");
            }

            for (int i = dimensions.height / 2 + 3; i < dimensions.height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width / 2 - 1, i);
                Console.Write("│");
            }

            for (int i = 1; i < dimensions.width / 2 - 1; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, dimensions.changesPanelHeight + 2);
                Console.Write("─");
                Console.SetCursorPosition(i, dimensions.changesPanelHeight + 3);
                Console.Write("─");
                Console.SetCursorPosition(i, dimensions.height);
                Console.Write("─");
            }

            for (int i = dimensions.width / 2 + 1; i < dimensions.width; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, dimensions.height);
                Console.Write("─");
            }

            Console.SetCursorPosition(dimensions.width / 2, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(dimensions.width / 2, dimensions.height);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width, dimensions.height);
            Console.Write("┘");
            Console.SetCursorPosition(dimensions.width, dimensions.tabHeight + 1);
            Console.Write("┐");

            Console.SetCursorPosition(0, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(0, dimensions.height / 2 + 1);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.tabHeight + 1);
            Console.Write("┐");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.height / 2 + 1);
            Console.Write("┘");
            Console.SetCursorPosition(0, dimensions.height / 2 + 2);
            Console.Write("┌");
            Console.SetCursorPosition(0, dimensions.height);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.height);
            Console.Write("┘");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.height / 2 + 2);
            Console.Write("┐");
        }
    }
}
