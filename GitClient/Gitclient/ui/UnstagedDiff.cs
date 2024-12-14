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
        private BlueBox blueBox;
        private int currentIndex;
        private SetTextLegth textLegth;
        private GetColorForText color;
        private int height;
        private Indicator indicator;

        public UnstagedDiff(UnstagedChangesDiffService unstagedChangesDiffService) 
        {
            this.unstagedChangesDiffService = unstagedChangesDiffService;
            this.dimensions = new DrawTabs.Dimensions();
            this.currentDiff = new List<Diff>();
            blueBox = new BlueBox();
            this.textLegth = new SetTextLegth();
            this.color = new GetColorForText();
            this.height = Console.WindowHeight - dimensions.tabHeight;
            this.indicator = new Indicator();
        }

        public void Show(int currentIndex)
        {
            Refresh(currentIndex);
        }

        private void Refresh(int currentIndex)
        {
            if (unstagedChangesDiffService.GetAllUnstagedDiff().Count > 0)
            {
                currentDiff = unstagedChangesDiffService.GetCurrentDiff(currentIndex);
                DisplayDiff(currentDiff);
                
                if (ButtomPress.Type.right == true)
                {
                    string textForBluexBox = currentDiff[0].Display();
                    blueBox.SetBlueBox((2, dimensions.tabHeight + 2), currentIndex, textForBluexBox, dimensions.changesPanelWidth - 2);
                }
            }

            DrawDiffPanel(currentDiff);
        }

        private void DisplayDiff(List<Diff> currentDiff)
        {
            int stopAt = currentDiff[currentIndex].diffs.Count() > height ? height : currentDiff[currentIndex].diffs.Count();

            int x = 0;
            int width = 0;

            if (ButtomPress.Type.right == true)
            {
                x = 2;
                width = Console.WindowWidth - 2;
            }
            else
            {
                x = dimensions.changesPanelWidth + 4;
                width = Console.WindowWidth - dimensions.changesPanelWidth - 6;
            }


            for (int i = 0; i < stopAt; i++)
            {
                int y = dimensions.unstagedStart - 1 + i;

                if (y < stopAt)
                {
                    string displayText = textLegth.Text(currentDiff[currentIndex].diffs[i], width);
                    Console.SetCursorPosition(x, y);
                    
                    if (displayText == "")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = i == 0 ? ConsoleColor.Gray : color.SetColor(displayText[0]);
                    }

                    Console.Write(displayText);
                    Console.ResetColor();
                }
            }
        }

        private void DrawDiffPanel(List<Diff> currentDiff)
        {
            int x = 0;
            int y = 0;
            int width = 0;
            int positionOfDiffName = 0;

            if (ButtomPress.Type.right == true)
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

            string text = textLegth.Text("Diff: ", dimensions.width / 2 - 2);
            Console.SetCursorPosition(positionOfDiffName, dimensions.tabHeight + 1);
            Console.Write(text);

            if (ButtomPress.Type.right == true)
            {
                indicator.GetIndicator(currentIndex, Console.WindowHeight - 1, currentDiff[0].diffs.Count(), Console.WindowWidth - 1, dimensions.tabHeight + 2, Console.WindowHeight - 2);
            }
        }
    }
}
