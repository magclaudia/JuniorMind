using Gitclient.ui;
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
    public class UnstagedChangesPanel : UiComponent
    {
        public event EventHandler<FileSelectionChangedEventArgs> FileSelectionChanged;
        private PanelCommunicationService communicationService;

        private StatusDiffService statusDiffService;
        private StatusService statusService;
        private List<ChangeAttribute> currentUnstagedChanges;
        private DrawTabs.Dimensions dimensions;
        private BlueBox blueBox;
        private Indicator indicator;
        private int totalNumberOfFiles;
        private static int startIndex;
        private static int currentIndex;
        private static int lastEndIndexValue = -1;
        private int endIndex;
        private static int lastFileNumberValue = 1;
        private int fileNumber;
        private static int indexForDiff;
        private int x;
        private static int y;
        private int countingIndex;
        private int checkLastFile;


        public UnstagedChangesPanel(StatusService statusService, StatusDiffService statusDiffService)
        {
            this.statusService = statusService;
            this.statusDiffService = statusDiffService;
            currentUnstagedChanges = new List<ChangeAttribute>();
            dimensions = new DrawTabs.Dimensions();
            blueBox = new BlueBox();
            indicator = new Indicator();
            totalNumberOfFiles = statusService.GetAllUnstagedChanges().Count;
            startIndex = GetStartIndex();
            currentIndex = GetCurrentIndex();
            endIndex = lastEndIndexValue >= 0 && lastEndIndexValue < totalNumberOfFiles ? lastEndIndexValue : GetEndIndex();
            fileNumber = lastFileNumberValue;
            indexForDiff = 0;
            x = 1;
            y = dimensions.unstagedStart;
            countingIndex = 0;
            checkLastFile = 0;
        }

        public void SetCommunicationService(PanelCommunicationService service)
        {
            communicationService = service;
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

        public override void Show()
        {
            Refresh();

            if (ButtomPress.Type.workingInStagePanel == false)
            {
                indexForDiff = communicationService.GetLastIndexForDiff();
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

                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd + 1);
            }
            else
            {
                Console.SetCursorPosition(2, dimensions.changesPanelHeight / 2 + dimensions.tabHeight);
                string text = TextSettings.GetTextLength("No changes found in the unstaged area.", dimensions.changesPanelWidth - 4);
                Console.Write(text);
            }

            DrawPanel(currentUnstagedChanges);

            if (totalNumberOfFiles == 0)
            {
                OnFileSelectionChanged(indexForDiff, isStaged: true);
                ButtomPress.Type.workingInStagePanel = true;
                communicationService.NavigateToStagedPanel();
            }
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
                                ButtomPress.Type.enter = false;
                                clear.ClearFiles(x, y, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningOnFile");
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
                                OnFileSelectionChanged(indexForDiff, isStaged: false);
                            }
                            
                            if (fileNumber == totalNumberOfFiles)
                            {
                                checkLastFile++;
                            }

                            if (checkLastFile == 2 && statusService.GetAllStageChanges().Count > 0)
                            {
                                checkLastFile = 0;
                                clear.ClearFiles(x, y, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningOnFile");
                                GetOneFileAtTime(currentUnstagedChanges);
                                DrawPanel(currentUnstagedChanges);
                                clear.ClearDiff(y);
                                communicationService.SetLastIndexForDiff(indexForDiff);
                                indexForDiff = 0;
                                OnFileSelectionChanged(indexForDiff, isStaged: true);
                                ButtomPress.Type.workingInStagePanel = true;
                                communicationService.NavigateToStagedPanel();
                            }
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        {
                            if (fileNumber > 1)
                            {
                                ButtomPress.Type.down = false;
                                ButtomPress.Type.up = true;
                                
                                clear.ClearFiles(x, y, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningOneFile");
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
                                OnFileSelectionChanged(indexForDiff, isStaged: false);
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            ButtomPress.Type.enter = true;
                            ButtomPress.Type.up = false;
                            ButtomPress.Type.down = false;
                            statusService.StageFile(currentUnstagedChanges[currentIndex]);
                            clear.ClearFiles(x, dimensions.unstagedStart - 1, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                            clear.ClearFiles(x, dimensions.stagedStart, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                            clear.ClearDiff(y);

                            if (indexForDiff > 0)
                            {
                                y--;
                                currentIndex--;
                                indexForDiff--;
                                fileNumber--;
                                OnFileSelectionChanged(indexForDiff, isStaged: false);
                                Refresh();
                                communicationService.SetLastIndexForDiff(indexForDiff);
                            }
                            else
                            {
                                OnFileSelectionChanged(indexForDiff, isStaged: true);
                                ButtomPress.Type.workingInStagePanel = true;
                            }

                            if (statusService.GetAllUnstagedChanges().Count == 0)
                            {
                                fileNumber = 0;
                            }
                            
                            
                            indexForDiff = 0;
                            communicationService.NavigateToStagedPanel();
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

        protected virtual void OnFileSelectionChanged(int fileIndex, bool isStaged) 
        {
            FileSelectionChanged?.Invoke(this, new FileSelectionChangedEventArgs(indexForDiff, isStaged));
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


            if (totalNumberOfFiles > 0)
            {
                UnstagedPath();
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
