using GitClient.model;
using GitClient.service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  

namespace GitClient.ui
{
    public class DiffPanel
    {
        private StatusDiffService unstagedChangesDiffService;
        private StatusService unstagedChangesService;
        private DrawTabs.Dimensions dimensions;
        private List<Diffs> currentDiff;
        private BlueBox blueBox;
        private int height;
        private Indicator indicator;
        private int x;
        private int y = 4;
        private int startIndex;
        private int currentIndex;
        private int diffSize;


        public DiffPanel(StatusDiffService unstagedChangesDiffService, StatusService unstagedChangesService) 
        {
            this.unstagedChangesDiffService = unstagedChangesDiffService;
            this.unstagedChangesService = unstagedChangesService;
            dimensions = new DrawTabs.Dimensions();
            currentDiff = new List<Diffs>();
            blueBox = new BlueBox();
            height = Console.WindowHeight - dimensions.tabHeight;
            indicator = new Indicator();
            x = 1;
            y = dimensions.tabHeight + 2;
            startIndex = GetStartIndex();
            currentIndex = GetCurrentIndex();
            diffSize = unstagedChangesDiffService.GetAllUnstageDiffs().Count;
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public void Show(int currentIndex)
        {
            y = dimensions.tabHeight + 2;

            if (ButtomPress.Type.workingInStagePanel == true)
            {
                diffSize = unstagedChangesDiffService.GetAllStageDiffs().Count;
                currentDiff = unstagedChangesDiffService.GetCurrentStageDiff(currentIndex);
            }
            else
            {
                diffSize = unstagedChangesDiffService.GetAllUnstageDiffs().Count;
                currentDiff = unstagedChangesDiffService.GetCurrentUnstageDiff(currentIndex);
            }

            Refresh(currentIndex);
        }

        public void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ClearConsoleChoosenSpace clear = new ClearConsoleChoosenSpace();

            do
            {
                keyInfo = Console.ReadKey();
                ButtomPress.Type.diffMovements = true;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            if (currentIndex < currentDiff[0].diffs.Count - 1)
                            {
                                ButtomPress.Type.down = true;
                                ButtomPress.Type.up = false;
                                clear.ClearDiff(y);
                                DrawDiffPanel(currentDiff);

                                if (y == height - 1)
                                {
                                    startIndex++;
                                }

                                GetNewLineDiff(currentDiff);

                                if (y < height)
                                {
                                    y++;
                                }

                                currentIndex++;
                                string textForBluexBox = currentDiff[0].diffs[currentIndex];
                                blueBox.SetBlueBox((1, y), textForBluexBox, Console.WindowWidth - 3);
                                indicator.GetIndicator(currentIndex, height - 1, diffSize, Console.WindowWidth - 1, y - 1, height - 1);
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            if (currentIndex > 0)
                            {
                                ButtomPress.Type.down = false;
                                ButtomPress.Type.up = true;
                                clear.ClearDiff(y);
                                DrawDiffPanel(currentDiff);

                                if (y == dimensions.tabHeight + 2)
                                {
                                    startIndex--;
                                }

                                GetNewLineDiff(currentDiff);

                                if (y > dimensions.tabHeight + 2)
                                {
                                    y--;
                                }

                                currentIndex--;
                                string textForBluexBox = currentDiff[0].diffs[currentIndex];
                                blueBox.SetBlueBox((1, y), textForBluexBox, Console.WindowWidth - 3);
                                indicator.GetIndicator(currentIndex, height - 1, diffSize, Console.WindowWidth - 1, y, height);
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        {
                            ButtomPress.Type.diffMovements = false;
                            ButtomPress.Type.escape = true;
                            Console.Clear();
                            ButtomPress.Type.right = false;
                            ButtomPress.Type.escape = false;
                            ButtomPress.Type.deleted = false;
                            Ui ui = new Ui();
                            ui.Show();
                        }
                        break;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

        private int GetStartIndex()
        {
            return 0;
        }

        private void Refresh(int currentIndex)
        {
            //diffSize = unstagedChangesDiffService.GetAllUnstageDiffs().Count;

            if (diffSize > 0)
            {
                DisplayDiff(currentDiff, currentIndex);
                
                if (ButtomPress.Type.right == true)
                {
                    string textForBluexBox = currentDiff[0].Display();
                    blueBox.SetBlueBox((1, y), textForBluexBox, Console.WindowWidth - 3);
                }
            }

            DrawDiffPanel(currentDiff);
        }

        private void DisplayDiff(List<Diffs> currentDiff, int currentIndex)
        {
            int stopAt = currentDiff[0].diffs.Count() > height ? height : currentDiff[0].diffs.Count();
            int width = 0;

            if (ButtomPress.Type.right == true)
            {
                x = 1;
                width = Console.WindowWidth - 3;
            }
            else
            {
                x = dimensions.changesPanelWidth + 4;
                width = Console.WindowWidth - dimensions.changesPanelWidth - 6;
            }

            for (int i = 0; i < stopAt; i++)
            {
                int y = dimensions.unstagedStart - 1 + i;
                
                if (y < height)
                {
                    string displayText = TextSettings.GetTextLength(currentDiff[0].diffs[i], width);
                    Console.SetCursorPosition(x, y);
                    
                    if (displayText == "")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = i == 0 ? ConsoleColor.Gray : TextSettings.SetColor(displayText[0]);
                    }

                    Console.Write(displayText);
                    Console.ResetColor();
                }
            }
        }

        private void DrawDiffPanel(List<Diffs> currentDiff)
        {
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

            string text = TextSettings.GetTextLength("Diff: ", dimensions.width / 2 - 2);
            Console.SetCursorPosition(positionOfDiffName, dimensions.tabHeight + 1);
            Console.Write(text);

            if (ButtomPress.Type.right == true && currentIndex == 0)
            {
                indicator.GetIndicator(currentIndex, Console.WindowHeight - 1, currentDiff[0].diffs.Count(), Console.WindowWidth - 1, dimensions.tabHeight + 2, Console.WindowHeight - 2);
            }
        }

        private void GetNewLineDiff(List<Diffs> currentDiff)
        {
            if (y == height - 1 && ButtomPress.Type.down == true || y == 3 && ButtomPress.Type.up == true)
            {
                y = dimensions.tabHeight + 2;
                GetAllDiffLines(currentDiff);
                
                if (ButtomPress.Type.up)
                {
                    y = dimensions.tabHeight + 2;

                }
                else
                {
                    y = height - 2;
                }
            }
            else
            {
                GetOneLineAtTime(currentDiff);
            }
        }

        private void GetAllDiffLines(List<Diffs> currentDiff)
        {
            int index = startIndex; 
            for (int i = 0; i < height - 1; i++)
            {
                if (i == currentDiff[0].diffs.Count || y == height || index == currentDiff[0].diffs.Count)
                {
                    break;
                }

                string displayText = TextSettings.GetTextLength(currentDiff[0].diffs[index], Console.WindowWidth - 3);
                Console.SetCursorPosition(1, y);
                
                if (displayText != "")
                {
                    Console.ForegroundColor = TextSettings.SetColor(displayText[0]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(displayText);
                Console.ResetColor();
                y++;
                index++;
            }
        }

        private void GetOneLineAtTime(List<Diffs> currentDiff)
        {
            string displayText = TextSettings.GetTextLength(currentDiff[0].diffs[currentIndex], Console.WindowWidth - 3);
            Console.SetCursorPosition(1, y);
            
            if (displayText != "")
            {
                Console.ForegroundColor = displayText.EndsWith(".cs") ? ConsoleColor.DarkGray : TextSettings.SetColor(displayText[0]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write(displayText);
            Console.ResetColor();
        }
    }
}
