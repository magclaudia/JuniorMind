using Gitclient.model;
using Gitclient.service;
using GitClient;
using GitClient.ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.ui
{
    public class UnstagedDiff
    {
        private UnstagedChangesDiffService unstagedChangesDiffService;
        private DrawTabs.Dimensions dimensions;
        private List<Diff> currentDiff;
        private int currentIndex;
        private SetTextLegth textLegth;
        private GetColorForText color;
        private int height;

        public UnstagedDiff(UnstagedChangesDiffService unstagedChangesDiffService) 
        {
            this.unstagedChangesDiffService = unstagedChangesDiffService;
            this.dimensions = new DrawTabs.Dimensions();
            this.currentDiff = new List<Diff>();
            this.currentIndex = UnstagedChangesPanel.GetCurrentIndex();
            this.textLegth = new SetTextLegth();
            this.color = new GetColorForText();
            this.height = Console.WindowHeight - dimensions.tabHeight;
        }

        public void Show()
        {
            Refresh();
        }

        private void Refresh()
        {
            currentDiff = unstagedChangesDiffService.GetCurrentDiff();
           
            if (currentIndex == 0)
            {
                DisplayDiff(currentDiff);
            }

            DrawDiffPanel(currentDiff);
        }

        private void DisplayDiff(List<Diff> currentDiff)
        {
            int stopAt = currentDiff[currentIndex].diffs.Count() > height ? height : currentDiff[currentIndex].diffs.Count();

            for (int i = 0; i < stopAt; i++)
            {
                int y = dimensions.unstagedStart - 1 + i;

                if (y < stopAt)
                {
                    string displayText = textLegth.Text(currentDiff[currentIndex].diffs[i], Console.WindowWidth - dimensions.changesPanelWidth - 6);
                    Console.SetCursorPosition(dimensions.changesPanelWidth + 4, y);
                    Console.ForegroundColor = i == 0 ? ConsoleColor.Gray : color.SetColor(displayText[0]);
                    Console.Write(displayText);
                    Console.ResetColor();
                }
            }
        }

        private void DrawDiffPanel(List<Diff> currentDiff)
        {
            for (int i = dimensions.tabHeight + 2; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(dimensions.width / 2 + 1, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < (Console.WindowWidth - dimensions.changesPanelWidth - 3); i++)
            {
                Console.SetCursorPosition(dimensions.width / 2 + 1 + i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(dimensions.width / 2 + 1 + i, Console.WindowHeight - 1);
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

            string text = textLegth.Text("Diff: ", dimensions.width / 2 - 2);
            Console.SetCursorPosition(dimensions.width / 2 + 2, dimensions.tabHeight + 1);
            Console.Write(text);
        }
    }
}
