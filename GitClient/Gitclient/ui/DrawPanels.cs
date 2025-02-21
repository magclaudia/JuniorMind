using GitClient.model;
using GitClient.repository;
using GitClient.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class DrawPanels
    {
        private DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
        private Indicator indicator = new Indicator();
        private int yEndInfo = 0;
        private int yEndMessage = 0;


        public void DrawUnstagePanel(List<ChangeAttribute> currentUnstagedChanges, int totalNumberOfFiles, int currentIndex)
        {
            for (int i = dimensions.tabHeight + 2; i < dimensions.height / 2 + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width / 2 - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < dimensions.changesPanelWidth; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, dimensions.changesPanelHeight + 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(0, dimensions.height / 2 + 1);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.tabHeight + 1);
            Console.Write("┐");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.height / 2 + 1);
            Console.Write("┘");

            string text = TextSettings.GetTextLength("Unstaged Changes: ", dimensions.changesPanelWidth);
            Console.SetCursorPosition(1, dimensions.tabHeight + 1);
            Console.Write(text);


            if (totalNumberOfFiles > 0)
            {
                if (currentIndex == 0)
                {
                    indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                }

                ProjectPath(1, dimensions.unstagedStart - 1);
            }
        }
        public void DrawStagePanel(List<ChangeAttribute> currentStagedChanges, int totalNumberOfFiles)
        {
            for (int i = dimensions.unstagedEnd + 3; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width / 2 - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < dimensions.changesPanelWidth; i++)
            {
                Console.SetCursorPosition(i, dimensions.unstagedEnd + 2);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, dimensions.unstagedEnd + 2);
            Console.Write("┌");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.unstagedEnd + 2);
            Console.Write("┐");
            Console.SetCursorPosition(dimensions.width / 2 - 1, Console.WindowHeight - 1);
            Console.Write("┘");

            string text = TextSettings.GetTextLength("Staged Changes: ", dimensions.changesPanelWidth);
            Console.SetCursorPosition(1, dimensions.unstagedEnd + 2);
            Console.Write(text);

            if (totalNumberOfFiles > 0)
            {
                ProjectPath(1, dimensions.stagedStart - 1);
            }
        }
        public void DrawDiffPanel(List<string> currentDiff, int x, int currentIndex)
        {
            int width = 0;
            int positionOfDiffName = 0;

            if (ReadButtons.RightOnce == true)
            {
                x = 0;
                width = Console.WindowWidth - 1;
                positionOfDiffName = 1;
            }
            else
            {
                x = dimensions.width / 2 + 1;
                width = Console.WindowWidth - dimensions.changesPanelWidth - 3;
                positionOfDiffName = dimensions.width / 2 + 2;
            }

            for (int i = dimensions.tabHeight + 2; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < width; i++)
            {
                Console.SetCursorPosition(x + i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(x + i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(x, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(x, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, dimensions.tabHeight + 1);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("┘");

            string text = TextSettings.GetTextLength("Diff: ", dimensions.width / 2 - 2);
            Console.SetCursorPosition(positionOfDiffName, dimensions.tabHeight + 1);
            Console.Write(text);

            if (ReadButtons.RightOnce == true && currentIndex == 0)
            {
                indicator.GetIndicator(currentIndex, Console.WindowHeight - 1, currentDiff.Count(), Console.WindowWidth - 1, dimensions.tabHeight + 2, Console.WindowHeight - 2);
            }
        }
        public void DrawBorderForFullSizeCommitList()
        {
            for (int i = 3; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, 2);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, 2);
            Console.Write("┌");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, 2);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("┘");
        }
        public void DrawBorderForCommitsAfterPressingEnter()
        {
            for (int i = 3; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth / 2 + 7, i);
                Console.Write("║");
            }

            for (int i = 1; i < Console.WindowWidth / 2 + 7; i++)
            {
                Console.SetCursorPosition(i, 2);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, 2);
            Console.Write("┌");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 7, 2);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 7, Console.WindowHeight - 1);
            Console.Write("┘");

            for (int i = 3; i < Console.WindowHeight / 4; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 9, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            for (int i = Console.WindowWidth / 2 + 10; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, 2);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight / 4);
                Console.Write("─");
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, 2);
            Console.Write("Info ");

            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, 2);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight / 4);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, 2);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 4);
            Console.Write("┘");

            for (int i = Console.WindowHeight / 4 + 3; i < Console.WindowHeight / 2 + 2; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 9, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            for (int i = Console.WindowWidth / 2 + 10; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, Console.WindowHeight / 4 + 2);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight / 2 + 2);
                Console.Write("─");
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, Console.WindowHeight / 4 + 2);
            Console.Write("Message ");

            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight / 4 + 2);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight / 2 + 2);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 4 + 2);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 2 + 2);
            Console.Write("┘");

            for (int i = Console.WindowHeight / 2 + 5; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 9, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            for (int i = Console.WindowWidth / 2 + 10; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, Console.WindowHeight / 2 + 4);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight / 2 + 4);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 2 + 4);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("┘");
        }
        public void DrawBorderForCommitsAfterPressingRightOnce()
        {
            yEndInfo = Console.WindowHeight / 3 - 2;

            for (int i = dimensions.tabHeight + 2; i < yEndInfo; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width / 2 - 1, i);
                Console.Write("│");
            }

            for (int i = 1; i < Console.WindowWidth / 2 - 1; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight / 3 - 2);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, 2);
            Console.Write("┌");
            Console.SetCursorPosition(0, Console.WindowHeight / 3 - 2);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width / 2 - 1, 2);
            Console.Write("┐");
            Console.SetCursorPosition(dimensions.width / 2 - 1, Console.WindowHeight / 3 - 2);
            Console.Write("┘");

            Console.SetCursorPosition(1, 2);
            Console.Write("Info ");

            yEndMessage = Console.WindowHeight / 2 + 2;

            for (int i = yEndInfo + 2; i < Console.WindowHeight / 2 + 2; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width / 2 - 1, i);
                Console.Write("│");
            }

            for (int i = 1; i < dimensions.width / 2 - 1; i++)
            {
                Console.SetCursorPosition(i, yEndInfo + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, yEndMessage);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, yEndInfo + 1);
            Console.Write("┌");
            Console.SetCursorPosition(0, yEndMessage);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width / 2 - 1, yEndInfo + 1);
            Console.Write("┐");
            Console.SetCursorPosition(dimensions.width / 2 - 1, yEndMessage);
            Console.Write("┘");

            Console.SetCursorPosition(1, yEndInfo + 1);
            Console.Write("Message ");

            for (int i = yEndMessage + 2; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 2, i);
                Console.Write("║");
            }

            for (int i = 1; i < dimensions.width / 2 - 1; i++)
            {
                Console.SetCursorPosition(i, yEndMessage + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, yEndMessage + 1);
            Console.Write("┌");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width / 2 - 1, yEndMessage + 1);
            Console.Write("┐");
            Console.SetCursorPosition(dimensions.width / 2 - 1, Console.WindowHeight - 1);
            Console.Write("┘");

            for (int i = dimensions.tabHeight + 2; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(dimensions.width / 2 + 1, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            for (int i = 2; i < Console.WindowWidth / 2; i++)
            {
                Console.SetCursorPosition(dimensions.width / 2 + i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(dimensions.width / 2 + i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(dimensions.width / 2 + 1, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(dimensions.width / 2 + 1, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, dimensions.tabHeight + 1);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("┘");

            Console.SetCursorPosition(dimensions.width / 2 + 2, dimensions.tabHeight + 1);
            Console.Write("Diff: ");
        }
        private void ProjectPath(int x, int y)
        {
            GetProjectPath projectPath = new GetProjectPath();
            string path = $"  ▾{projectPath.ProjectPath(Environment.CurrentDirectory)}";
            path = TextSettings.GetTextLength(path, dimensions.changesPanelWidth - 1);
            Console.SetCursorPosition(x, y);
            Console.Write(path);
            Console.ResetColor();
        }
    }
}
