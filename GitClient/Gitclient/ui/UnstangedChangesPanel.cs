using Gitclient.model;
using Gitclient.repository;
using Gitclient.ui;
using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class UnstangedChangesPanel
    {
        private int totalNumberOfFiles;
        private int startIndex;
        private int currentIndex;
        private int endIndex;
        private int panelSize;
        private int x;
        private int y;
        //private (int x, int y) cursorPosition;
        private UnstagedChangesService unstagedChangesService;
        private DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
        private SetTextLegth textLegth = new SetTextLegth();
        private GetColorForText color = new GetColorForText();
        private BlueBox blueBox = new BlueBox();
        private Indicator indicator = new Indicator();

        public UnstangedChangesPanel(UnstagedChangesService unstagedChangesService)
        {
            this.totalNumberOfFiles = unstagedChangesService.GetAllUnstagedChanges().Count;
            this.startIndex = GetStartIndex();
            this.currentIndex = GetCurrentIndex();
            this.endIndex = GetEndIndex();
            this.x = 1;
            this.y = dimensions.unstagedStart;
            //this.cursorPosition = (x, y + 1);
            this.unstagedChangesService = unstagedChangesService;
            this.panelSize = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
        }

        public int GetStartIndex()
        {
            return 0;
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public int GetEndIndex()
        {
            return currentIndex < totalNumberOfFiles ? dimensions.unstagedEnd - dimensions.unstagedStart
                : totalNumberOfFiles;
        }

        public void Show()
        {
            Refresh();
            
            if (totalNumberOfFiles > 0)
            {
                Navigate();
            }
        }

        private void Refresh()
        {
            List<UnstagedChange> unstagedChanges = new List<UnstagedChange>();
            
            if (totalNumberOfFiles > 0)
            {
                unstagedChanges = unstagedChangesService.GetCurrentUnstagedChanges(startIndex, endIndex);
            }

            DrawPanel(unstagedChanges);
        }

        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ClearConsoleChoosenSpace clear = new ClearConsoleChoosenSpace();
            
            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                       
                        if (currentIndex < totalNumberOfFiles - 1)
                        {
                            currentIndex++;
                            //cursorPosition = (x, y);
                            
                            if (y < dimensions.unstagedEnd || y == dimensions.unstagedEnd)
                            {
                                y++;
                            }

                            if (currentIndex > endIndex)
                            {
                                startIndex++;
                                endIndex++;
                            }
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        if (currentIndex > 0)
                        {
                            currentIndex--;
                           // cursorPosition = (x, y);
                            
                            if (y <= dimensions.unstagedEnd)
                            {
                                y--;
                            }

                            if (currentIndex < startIndex && startIndex > 0)
                            {
                                startIndex--;
                                endIndex--;
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        {
                            CloseApplication();
                        }
                        break;
                }

                if (currentIndex >= 1 && currentIndex < totalNumberOfFiles && y >= 1)
                {
                    clear.Clear(currentIndex - 1, x, y - 1, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd);
                }

                Refresh();
                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1);

            } while (keyInfo.Key != ConsoleKey.Escape);
        }


        private void DrawPanel(List<UnstagedChange> currentUnstagedChanges)
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

            string text = textLegth.Text("Unstaged Changes: ", dimensions.changesPanelWidth);
            Console.SetCursorPosition(1, dimensions.tabHeight + 1);
            Console.Write(text);

            if (totalNumberOfFiles > 0)
            {
                UnstagedPath();
                GetUnstagedFiles(currentUnstagedChanges);
            }
        }

        private void GetUnstagedFiles(List<UnstagedChange> currentUnstagedChanges)
        {
            if (y <= dimensions.unstagedEnd && currentIndex > 0)
            {
                y--;
                currentIndex--;

                string displayText = textLegth.Text(currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                Console.SetCursorPosition(1, y);
                Console.ForegroundColor = color.SetColor(displayText[0]);
                Console.Write(displayText);
                y++;
                currentIndex++;
                displayText = textLegth.Text(currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                blueBox.SetBlueBox((1, y), displayText, dimensions.changesPanelWidth - 2);
            }
            else
            {
                for (int i = 0; i < currentUnstagedChanges.Count; i++)
                {
                    int y = dimensions.unstagedStart + i;
                    string displayText = textLegth.Text(currentUnstagedChanges[i].Display(), dimensions.changesPanelWidth - 2);

                    if (startIndex + i == currentIndex)
                    {
                        blueBox.SetBlueBox((1, y), displayText, dimensions.changesPanelWidth - 2);
                    }
                    else
                    {
                        Console.SetCursorPosition(1, y);
                        Console.ForegroundColor = color.SetColor(displayText[0]);
                        Console.Write(displayText);
                    }
                }
            }

            indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1);
            Console.ResetColor();
        }

        private void UnstagedPath()
        {
            GetProjectPath projectPath = new GetProjectPath();

            string path = $"  ▾{projectPath.ProjectPath(Environment.CurrentDirectory)}";
            path = textLegth.Text(path, dimensions.changesPanelWidth - 1);
            Console.SetCursorPosition(1, dimensions.unstagedStart - 1);
            Console.Write(path);
        }

        private void CloseApplication()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
