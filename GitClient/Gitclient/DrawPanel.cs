using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class DrawPanel
    {
        private static int plusLegthForLargePanel = 10;
        private static int border = 2;
        public struct MessageBox
        {
            public int edgeOne;
            public int edgeTwo;
            public int edgeTree;
            public int edgeFour;
            public int width;
            public int height;

            public MessageBox() 
            {
                edgeOne = Console.WindowWidth / 2 + plusLegthForLargePanel;
                edgeTwo = Console.WindowWidth - 1;
                edgeTree = Console.WindowWidth / 2 + plusLegthForLargePanel;
                edgeFour = Console.WindowHeight / 2 - 1;
                
                width = Console.WindowWidth - 1 - (Console.WindowWidth / 2 + plusLegthForLargePanel + 1);
                height = Console.WindowHeight / 2 - 1;
            }
        }

        public struct FilesBox
        {
            public int edgeOneX;
            public int edgeOneY;
            public int edgeTwoX;
            public int edgeTwoY;
            public int edgeTreeX;
            public int edgeTreeY;
            public int edgeFourX;
            public int edgeFourY;
            public int width;
            public int height;

            public FilesBox()
            {
                edgeOneX = Console.WindowWidth / 2 + plusLegthForLargePanel;
                edgeOneY = Console.WindowHeight / 2 + 1;
                edgeTwoX = Console.WindowWidth - 1;
                edgeTwoY = Console.WindowHeight / 2 + 1;
                edgeTreeX = Console.WindowWidth / 2 + plusLegthForLargePanel;
                edgeTreeY = Console.WindowHeight - 1;
                edgeFourX = Console.WindowWidth - 1;
                edgeFourY = Console.WindowHeight - 1;
                width = Console.WindowWidth - 1 - (Console.WindowWidth / 2 + plusLegthForLargePanel + 1);
                height = Console.WindowHeight - 1 - (Console.WindowHeight / 2 + 1);
            }
        }

        public struct CommitsPanel
        {
            public int edgeTwoX;
            public int edgeTreeY;
            public int edgeFourX;
            public int edgeFourY;
            public int width;
            public int height;

            public CommitsPanel()
            {
                edgeTwoX = Console.WindowWidth / 2 + plusLegthForLargePanel - border;
                edgeTreeY = Console.WindowHeight - 1;
                edgeFourX = Console.WindowWidth / 2 + plusLegthForLargePanel - border;
                edgeFourY = Console.WindowHeight - 1;
                width = Console.WindowWidth / 2 + plusLegthForLargePanel - border;
                height = Console.WindowHeight - border;
            }
        }

        public static void MessagePanel()
        {
            var messagePanel = new MessageBox();

            Console.SetCursorPosition(messagePanel.edgeOne, 0);
            Console.Write("┌");
            Console.SetCursorPosition(messagePanel.edgeTwo, 0);
            Console.Write("┐");
            Console.SetCursorPosition(messagePanel.edgeTree, messagePanel.edgeFour);
            Console.Write("└");
            Console.SetCursorPosition(messagePanel.edgeTwo, messagePanel.edgeFour);
            Console.Write("┘");

            for (int i = Console.WindowWidth / 2 + plusLegthForLargePanel + 1; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("─");
                Console.SetCursorPosition(i, messagePanel.edgeFour);
                Console.Write("─");
            }

            for (int i = 1; i < messagePanel.edgeFour; i++)
            {
                Console.SetCursorPosition(messagePanel.edgeOne, i);
                Console.Write("│");
                Console.SetCursorPosition(messagePanel.edgeTwo, i);
                Console.Write("│");
            }

            FilePanel();
        }

        public static void FilePanel()
        {
            var filesPanel = new FilesBox();
            Console.SetCursorPosition(filesPanel.edgeOneX, filesPanel.edgeOneY);
            Console.Write("┌");
            Console.SetCursorPosition(filesPanel.edgeTwoX, filesPanel.edgeTwoY);
            Console.Write("┐");
            Console.SetCursorPosition(filesPanel.edgeTreeX, filesPanel.edgeTreeY);
            Console.Write("└");
            Console.SetCursorPosition(filesPanel.edgeFourX, filesPanel.edgeFourY);
            Console.Write("┘");

            for (int i = Console.WindowWidth / 2 + plusLegthForLargePanel + 1; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, filesPanel.edgeOneY);
                Console.Write("─");
                Console.SetCursorPosition(i, filesPanel.edgeTreeY);
                Console.Write("─");
            }

            for (int i = Console.WindowHeight / 2 + border; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(filesPanel.edgeOneX, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            ListOfAllCommitsPanel();
        }

        private static void ListOfAllCommitsPanel()
        {
            var commitsPanel = new CommitsPanel();
            Console.SetCursorPosition(0, 0);
            Console.Write("┌");
            Console.SetCursorPosition(commitsPanel.edgeTwoX, 0);
            Console.Write("┐");
            Console.SetCursorPosition(0, commitsPanel.edgeTreeY);
            Console.Write("└");
            Console.SetCursorPosition(commitsPanel.edgeFourX, commitsPanel.edgeFourY);
            Console.Write("┘");

            for (int i = 1; i < commitsPanel.width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("─");
                Console.SetCursorPosition(i, commitsPanel.edgeTreeY);
                Console.Write("─");
            }

            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(commitsPanel.edgeTwoX, i);
                Console.Write("║");
                Console.SetCursorPosition(1, i);
            }
        }
    }
}
