using GitClient.model;
using GitClient.repository;
using GitClient.service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GitClient.ui
{
    public class UnstagedChangesPanel
    {
        private StatusDiffService statusDiffService;
        private StatusService statusService;
        private List<ChangeAttribute> currentUnstagedChanges;
        private DrawTabs.Dimensions dimensions;
        private BlueBox blueBox;
        private Indicator indicator;
        private DiffPanel unstagedDiff;
        private int totalNumberOfFiles;
        private static int startIndex;
        private static int currentIndex;
        private static int lastEndIndexValue = -1;
        private int endIndex;
        private static int lastFileNumberValue = 1;
        private int fileNumber;
        private static int lastValueOfY;
        private int indexForDiff;
        private static int lastValueOfindexForDiff;
        private int x;
        private static int y;
        private int countingIndex;


        public UnstagedChangesPanel(StatusService statusService, StatusDiffService statusDiffService)
        {
            this.statusService = statusService;
            this.statusDiffService = statusDiffService;
            currentUnstagedChanges = new List<ChangeAttribute>();
            dimensions = new DrawTabs.Dimensions();
            blueBox = new BlueBox();
            indicator = new Indicator();
            unstagedDiff = new DiffPanel(statusDiffService, statusService);
            totalNumberOfFiles = statusService.GetAllUnstagedChanges().Count;
            startIndex = GetStartIndex();
            currentIndex = GetCurrentIndex();
            endIndex = lastEndIndexValue >= 0 && lastEndIndexValue < totalNumberOfFiles ? lastEndIndexValue : GetEndIndex();
            fileNumber = lastFileNumberValue;
            indexForDiff = lastValueOfindexForDiff;
            lastValueOfY = y;
            x = 1;
            y = dimensions.unstagedStart;
            countingIndex = 0;
        }

        public int SaveLastIndexForDiff()
        {
           return lastValueOfindexForDiff = indexForDiff;
        }

        public void SaveLastEndIndexValue()
        {
            lastEndIndexValue = endIndex;
        }

        public int SaveFileNumberLastValue()
        {
            return lastFileNumberValue = fileNumber;
        }

        public int SaveLastYValue()
        {
            return lastValueOfY > 0 ? y = lastValueOfY : y = dimensions.unstagedStart;
        }

        public int GetStartIndex()
        {
            return startIndex;
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

        public void Show()
        {
            Refresh();
            
            if (ButtomPress.Type.workingInStagePanel == false)
            {
                Navigate();
            }
        }

        private void Refresh()
        {
            currentUnstagedChanges.Clear();
            totalNumberOfFiles = statusService.GetAllUnstagedChanges().Count;
           
            if (totalNumberOfFiles > 0)
            {
                currentUnstagedChanges = statusService.GetCurrentChanges(startIndex, endIndex, "unstage");
                GetAllFiles(currentUnstagedChanges);

                //if (currentIndex == 0 || ButtomPress.Type.enter == true)
                //{
                //    GetAllFiles(currentUnstagedChanges);
                //}
            }
            else
            {
                Console.SetCursorPosition(2, dimensions.changesPanelHeight / 2 + dimensions.tabHeight);
                string text = TextSettings.GetTextLength("No changes found in the unstaged area", dimensions.changesPanelWidth - 4);
                Console.Write(text);
            }

            DrawPanel(currentUnstagedChanges);
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
                       {
                            if (fileNumber < totalNumberOfFiles && fileNumber > 0)
                            {
                                ButtomPress.Type.workingInUnstagePanel = true;
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

                                countingIndex++;    
                                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                                unstagedDiff.Show(indexForDiff);
                            }
                            
                            
                            if (fileNumber == totalNumberOfFiles + 1 && statusService.GetAllStageChanges().Count > 0)
                            {
                                SaveFileNumberLastValue();
                                SaveLastEndIndexValue();
                                SaveLastIndexForDiff();
                                Console.SetCursorPosition(1, dimensions.unstagedEnd);
                                Console.Write(new string(' ', dimensions.changesPanelWidth - 1));
                                GetOneFileAtTime(currentUnstagedChanges);
                                ButtomPress.Type.workingInStagePanel = true;
                                ButtomPress.Type.workingInUnstagePanel = false;
                                DrawPanel(currentUnstagedChanges);
                                clear.ClearDiff(y);
                                unstagedDiff.Show(0);
                                StagedChangesPanel stagedChanges = PanelFactory.CreateStagedChangesPanel();
                                stagedChanges.SaveFileNumberLastValue();
                                stagedChanges.Show();
                            }

                            if (fileNumber == totalNumberOfFiles)
                            {
                                fileNumber++;
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

                                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd + 1);
                                unstagedDiff.Show(indexForDiff);
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            ButtomPress.Type.enter = true;
                            ButtomPress.Type.up = false;
                            ButtomPress.Type.down = false;
                            statusService.StageFile(currentUnstagedChanges[currentIndex]);
                            clear.ClearFiles(currentIndex, x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd);
                            clear.ClearFiles(currentIndex, x, dimensions.stagedStart, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1);
                            clear.ClearDiff(y);
                            Ui ui = new Ui();
                            ui.Show(); 
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
                                    lastValueOfY = y;
                                    SaveFileNumberLastValue();
                                    SaveLastEndIndexValue();
                                    SaveLastIndexForDiff();
                                    unstagedDiff.Show(indexForDiff);
                                    unstagedDiff.Navigate();
                                    blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
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

        private void DrawPanel(List<ChangeAttribute> currentUnstagedChanges)
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

            if (totalNumberOfFiles > 0 && countingIndex < totalNumberOfFiles - 1)
            {
                UnstagedPath();
                if (ButtomPress.Type.workingInStagePanel == false)
                {
                    GetUnstagedFiles(currentUnstagedChanges);
                }
            }
        }

        private void GetUnstagedFiles(List<ChangeAttribute> currentUnstagedChanges)
        {
            if (y == dimensions.unstagedEnd && ButtomPress.Type.down == true || y == dimensions.unstagedStart && ButtomPress.Type.up == true || ButtomPress.Type.escape == true)
            {
                GetAllFiles(currentUnstagedChanges);
                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
            }
            else
            {
                GetOneFileAtTime(currentUnstagedChanges);

                if (ButtomPress.Type.deleted == false)
                {
                    indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                    blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                }
            }

            if (ButtomPress.Type.enter == true)
            {
                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                ButtomPress.Type.enter = false;
            }
        }
            
        public void GetAllFiles(List<ChangeAttribute> currentUnstagedChanges)
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

        private void GetOneFileAtTime(List<ChangeAttribute> currentUnstagedChanges)
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
