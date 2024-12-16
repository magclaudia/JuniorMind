using Gitclient.model;
using Gitclient.repository;
using Gitclient.service;
using Gitclient.ui;
using GitClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class UnstagedChangesPanel
    {
        private UnstagedChangesDiffService unstagedChangesDiffService;
        private UnstagedChangesService unstagedChangesService;
        private int totalNumberOfFiles;
        private int startIndex;
        private int currentIndex;
        private int endIndex;
        private int fileNumber;
        private int x;
        private int y;
        private List<UnstagedChange> currentUnstagedChanges = new List<UnstagedChange>();
        private DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
        private BlueBox blueBox = new BlueBox();
        private Indicator indicator = new Indicator();
        private TabsPanel tabs = new TabsPanel();
        private UnstagedDiffPanel diff;


        public UnstagedChangesPanel(UnstagedChangesService unstagedChangesService, UnstagedChangesDiffService unstagedChangesDiffService)
        {
            this.unstagedChangesService = unstagedChangesService;
            this.unstagedChangesDiffService = unstagedChangesDiffService;
            totalNumberOfFiles = unstagedChangesService.GetAllUnstagedChanges().Count;
            startIndex = GetStartIndex();
            currentIndex = GetCurrentIndex();
            endIndex = GetEndIndex();
            fileNumber = GetCurrentFile();
            x = 1;
            y = dimensions.unstagedStart;
            diff = new UnstagedDiffPanel(unstagedChangesDiffService, unstagedChangesService);
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
                : totalNumberOfFiles - 1;
        }

        public int GetCurrentFile()
        {
            return 1;
        }

        public void Show()
        {
            Refresh();
            Navigate();
        }

        private void Refresh()
        {
            if (totalNumberOfFiles > 0)
            {
                currentUnstagedChanges = unstagedChangesService.GetCurrentUnstagedChanges(startIndex, endIndex);
               
                if (currentIndex == 0)
                {
                    GetAllFiles(currentUnstagedChanges);
                }
            }
            else
            {
                Console.SetCursorPosition(4, dimensions.changesPanelHeight / 2 + dimensions.tabHeight);
                string text = TextSettings.GetTextLength("No changes found in the unstaged area", dimensions.changesPanelWidth - 4);
                Console.Write(text);
            }

            DrawPanel(currentUnstagedChanges);
        }

        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ClearConsoleChoosenSpace clear = new ClearConsoleChoosenSpace();
            int indexForDiff = 0;

            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            if (fileNumber < totalNumberOfFiles && fileNumber > 0)
                            {
                                ButtomPress.Type.down = true;
                                ButtomPress.Type.up = false;
                                clear.ClearFiles(currentIndex, x, y, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd);
                                clear.ClearDiff(y);

                                if (y == dimensions.unstagedEnd && endIndex < totalNumberOfFiles - 1)
                                {
                                    startIndex++;
                                    endIndex++;
                                    indexForDiff++;
                                    Refresh();
                                }
                                else
                                {
                                    GetUnstagedFiles(currentUnstagedChanges);
                                    indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                                }

                                if (y < dimensions.unstagedEnd)
                                {
                                    currentIndex++;
                                    y++;
                                    indexForDiff++;
                                }

                                if (fileNumber < totalNumberOfFiles && fileNumber > 0)
                                {
                                    fileNumber++;
                                }

                                blueBox.SetBlueBox((1, y), currentIndex, currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                                diff.Show(indexForDiff);
                            }
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        {
                            if (fileNumber > 1)
                            {
                                ButtomPress.Type.down = false;
                                ButtomPress.Type.up = true;

                                clear.ClearFiles(currentIndex, x, y, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd);
                                clear.ClearDiff(y);

                                if (y == dimensions.unstagedStart && startIndex > 0)
                                {
                                    startIndex--;
                                    endIndex--;
                                    indexForDiff--;
                                    Refresh();
                                }
                                else
                                {
                                    GetUnstagedFiles(currentUnstagedChanges);
                                }

                                if (y > dimensions.unstagedStart)
                                {
                                    currentIndex--;
                                    y--;
                                    indexForDiff--;
                                }

                                if (fileNumber > 0)
                                {
                                    fileNumber--;
                                }

                                blueBox.SetBlueBox((1, y), currentIndex, currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                                diff.Show(indexForDiff);
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            if (ButtomPress.Type.right == false)
                            {
                                if (totalNumberOfFiles > 0)
                                {
                                    ButtomPress.Type.right = true;
                                    clear.ClearDiff(y);
                                    diff.Show(indexForDiff);
                                    diff.Navigate();
                                    Restore();
                                    ButtomPress.Type.escape = false;
                                    blueBox.SetBlueBox((1, y), currentIndex, currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        {
                            CloseApplication();
                        }
                        break;
                }

            } while (keyInfo.Key != ConsoleKey.Escape);
        }

        public void Restore()
        {
            Refresh();
            diff.Show(currentIndex);
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

            string text = TextSettings.GetTextLength("Unstaged Changes: ", dimensions.changesPanelWidth);
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
            if (y == dimensions.unstagedEnd && ButtomPress.Type.down == true || y == dimensions.unstagedStart && ButtomPress.Type.up == true || ButtomPress.Type.escape == true)
            {
                GetAllFiles(currentUnstagedChanges);
                //indicator.GetIndicator(fileNumber, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
            }
            else
            {
                GetOneFileAtTime(currentUnstagedChanges);
                
                if (ButtomPress.Type.deleted == false)
                {
                    blueBox.SetBlueBox((1, y), currentIndex, currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                    indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                }
            }
        }

        public void GetAllFiles(List<UnstagedChange> currentUnstagedChanges)
        {
            for (int i = 0; i < currentUnstagedChanges.Count; i++)
            {
                int y = dimensions.unstagedStart + i;
                string displayText = TextSettings.GetTextLength(currentUnstagedChanges[i].Display(), dimensions.changesPanelWidth - 2);
                Console.SetCursorPosition(1, y);
                Console.ForegroundColor = TextSettings.SetColor(displayText[0]);
                Console.Write(displayText);
                Console.ResetColor();
            }
        }

        private void GetOneFileAtTime(List<UnstagedChange> currentUnstagedChanges)
        {
            string displayText = TextSettings.GetTextLength(currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            Console.SetCursorPosition(1, y);
            Console.ForegroundColor = TextSettings.SetColor(displayText[0]);
            Console.Write(displayText);
            Console.ResetColor();
        }

        private void UnstagedPath()
        {
            GetProjectPath projectPath = new GetProjectPath();

            string path = $"  ▾{projectPath.ProjectPath(Environment.CurrentDirectory)}";
            path = TextSettings.GetTextLength(path, dimensions.changesPanelWidth - 1);
            Console.SetCursorPosition(1, dimensions.unstagedStart - 1);
            Console.Write(path);
            Console.ResetColor();
        }

        private void CloseApplication()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
