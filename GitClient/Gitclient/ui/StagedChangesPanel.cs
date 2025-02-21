using GitClient.model;
using GitClient.repository;
using GitClient.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class StagedChangesPanel : UiComponent
    {
        public event EventHandler<FileSelectionChangedEventArgs>? FileSelectionChanged;
        private PanelCommunicationService communicationService;
        private StatusDiffService diffService;
        private StatusService statusService;
        private List<ChangeAttribute> currentStagedChanges;
        private DrawTabs.Dimensions dimensions;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private ClearConsoleChoosenSpace clear;
        private int totalNumberOfFiles;
        private int startIndex;
        private int currentIndex;
        private int endIndex;
        private int fileNumber;
        private int countingNumberOfPressingUp;
        private int x;
        private int y;

        public StagedChangesPanel(StatusDiffService diffService, StatusService statusService, PanelCommunicationService communicationService)
        {
            this.diffService = diffService;
            this.statusService = statusService;
            this.communicationService = communicationService;
            currentStagedChanges = new List<ChangeAttribute>();
            dimensions = new DrawTabs.Dimensions();
            blueBox = new BlueBox();
            indicator = new Indicator();    
            panel = new DrawPanels();
            clear = new ClearConsoleChoosenSpace();
            totalNumberOfFiles = statusService.GetAllStageChanges().Count;
            startIndex = GetStartIndex();
            currentIndex = GetCurrentIndex();
            endIndex = GetEndIndex();
            fileNumber = 1;
            countingNumberOfPressingUp = 0;
            x = 1;
            y = dimensions.stagedStart;
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
            return endIndex < dimensions.stagedEnd - dimensions.stagedStart ? dimensions.stagedEnd - dimensions.stagedStart
               : startIndex + dimensions.stagedEnd - dimensions.stagedStart;
        }
        public void SetCommunicationService(PanelCommunicationService service)
        {
            communicationService = service;
        }
        public override void Show()
        {
            panel.DrawStagePanel(currentStagedChanges, totalNumberOfFiles);
            Refresh();

            if (ReadButtons.WorkingInStagePanel == true)
            {
                Navigate();
            } 
        }
        private void Refresh()
        {
            totalNumberOfFiles = statusService.GetAllStageChanges().Count;

            if (totalNumberOfFiles > 0)
            {
                currentStagedChanges.Clear();
                currentStagedChanges = statusService.GetCurrentChanges(startIndex, endIndex, "stage");
                GetAllFiles(currentStagedChanges);
                ProjectPath(1, dimensions.stagedStart - 1);

                if (ReadButtons.WorkingInStagePanel == true)
                {
                     blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                     indicator.GetIndicator(currentIndex, dimensions.stagedEnd - 1, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.stagedStart - 1, dimensions.stagedEnd);
                }
            }
            else
            {
                Console.SetCursorPosition(2, dimensions.stagedStart + (dimensions.stagedEnd - dimensions.stagedStart) / 2);
                string text = TextSettings.GetTextLength("No changes found in the stage area.", dimensions.changesPanelWidth - 4);
                Console.Write(text);
            }
        }
        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            totalNumberOfFiles = statusService.GetAllStageChanges().Count;
            ReadButtons.WorkingInStagePanel = true;
            ReadButtons.WorkingInUnstagePanel = false;

            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            if (currentIndex < totalNumberOfFiles - 1 && fileNumber > 0)
                            {
                                HandleFilesDownMoves();
                            }
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        {
                            HandleFilesUpMoves();
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            HandlePressingEnter();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            if (ReadButtons.RightStatus == false)
                            {
                                if (totalNumberOfFiles > 0)
                                {
                                    ReadButtons.RightOnce = true;
                                    Console.Clear();
                                    communicationService.NavigateToTabPanel();
                                    communicationService.SetFileIndex(currentIndex);
                                    communicationService.NavigateToDiffPanel();
                                }
                            }
                        }
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            ReadButtons.WorkingInStagePanel = true;
                            communicationService.SetFileIndex(currentIndex);
                            communicationService.DisplayLog();
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

        private void HandleFilesDownMoves()
        {
            ReadButtons.Down = true;
            ReadButtons.Up = false;
            clear.ClearFiles(x, y, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd, "cleaningOneFile");
            clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);

            if (y == dimensions.stagedEnd - 1 && endIndex < totalNumberOfFiles)
            {
                startIndex++;
                endIndex++;
                currentIndex = currentStagedChanges.Count - 1;
                clear.ClearFiles(x, y, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                Refresh();
                GetAllFiles(currentStagedChanges);
            }
            else
            {
                GetOneFileAtTime(currentStagedChanges);
            }

            if (y < dimensions.stagedEnd - 1)
            {
                currentIndex++;
                y++;
            }

            if (fileNumber < totalNumberOfFiles && fileNumber > 0)
            {
                fileNumber++;
            }

            communicationService.SetCurrentFileName(currentStagedChanges[currentIndex].GetFileName());
            blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            indicator.GetIndicator(currentIndex, dimensions.stagedEnd - 1, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.stagedStart - 1, dimensions.stagedEnd);
            OnFileSelectionChanged(currentStagedChanges[currentIndex].GetFileName(), isStaged: true);
        }
        private void HandleFilesUpMoves()
        {
            if (fileNumber > 1)
            {
                ReadButtons.Down = false;
                ReadButtons.Up = true;
                clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);

                if (y == dimensions.stagedStart && startIndex > 0)
                {
                    clear.ClearFiles(x, y, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                    startIndex--;
                    endIndex--;
                    Refresh();
                }
                else
                {
                    clear.ClearFiles(x, y, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningOneFile");
                    GetOneFileAtTime(currentStagedChanges);
                }

                if (y > dimensions.stagedStart)
                {
                    currentIndex--;
                    y--;
                }

                if (fileNumber > 1)
                {
                    fileNumber--;
                }

                blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                indicator.GetIndicator(currentIndex, dimensions.stagedEnd - 1, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.stagedStart, dimensions.stagedEnd);
                OnFileSelectionChanged(currentStagedChanges[currentIndex].GetFileName(), isStaged: true);
            }

            if (fileNumber == 1 && statusService.GetAllUnstagedChanges().Count > 0)
            {
                countingNumberOfPressingUp++;
            }

            if (fileNumber == 1 && statusService.GetAllUnstagedChanges().Count > 0 && countingNumberOfPressingUp == 2)
            {
                clear.ClearFiles(x, y, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningOneFile");
                GetOneFileAtTime(currentStagedChanges);
                panel.DrawStagePanel(currentStagedChanges, totalNumberOfFiles);
                ReadButtons.Deleted = false;
                clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
                string lastUntageFileName = statusService.GetAllUnstagedChanges().Last().GetFileName();
                OnFileSelectionChanged(lastUntageFileName, isStaged: false);
                ReadButtons.WorkingInStagePanel = false;
                ReadButtons.WorkingInUnstagePanel = false;
                clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                countingNumberOfPressingUp = 0;
                communicationService.NavigateToUnstagedPanel();
            }
        }
        private void HandlePressingEnter()
        {
            ReadButtons.Enter = true;
            ReadButtons.Up = false;
            ReadButtons.Down = false;
            statusService.UnstageFile(currentStagedChanges[currentIndex]);
            currentStagedChanges = statusService.GetCurrentChanges(startIndex, endIndex, "stage");

            if (currentStagedChanges.Count == 0)
            {
                totalNumberOfFiles = statusService.GetAllStageChanges().Count;
                clear.ClearStagePanel(0, dimensions.stagedStart - 2, dimensions.changesPanelWidth + 1, dimensions.stagedEnd + 1);
                clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
                panel.DrawStagePanel(currentStagedChanges, totalNumberOfFiles);
            }
            else
            {
                clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                clear.ClearFiles(x, dimensions.stagedStart, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
            }

            if (currentIndex > 0 && currentStagedChanges.Count > 0)
            {
                y--;
                currentIndex--;
                Refresh();
                OnFileSelectionChanged(currentStagedChanges[currentIndex].GetFileName(), isStaged: true);
            }
            else if (currentIndex == 0 && currentStagedChanges.Count > 0)
            {
                OnFileSelectionChanged(currentStagedChanges[currentIndex].GetFileName(), isStaged: true);
                Refresh();
            }
            else
            {
                Console.SetCursorPosition(2, dimensions.stagedStart + 1);
                string text = TextSettings.GetTextLength("No changes found in the staged area.", dimensions.changesPanelWidth - 4);
                Console.Write(text);
                OnFileSelectionChanged(statusService.GetAllUnstagedChanges().Last().GetFileName(), isStaged: false);
                ReadButtons.WorkingInStagePanel = false;
            }

            communicationService.NavigateToUnstagedPanel();
        }
        public void OnFileSelectionChanged(string fileName, bool isStaged)
        {
            FileSelectionChanged?.Invoke(this, new FileSelectionChangedEventArgs(fileName, isStaged));
        }
        private void GetAllFiles(List<ChangeAttribute> currentStagedChanges)
        {
            clear.ClearFiles(x, y, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");

            for (int i = 0; i < currentStagedChanges.Count; i++)
            {
                int y = dimensions.stagedStart + i;
                string displayText = TextSettings.GetTextLength(currentStagedChanges[i].Display(), dimensions.changesPanelWidth - 2);
                Console.SetCursorPosition(1, y);
                Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
                Console.Write(displayText);
                Console.ResetColor();
            }
        }
        private void GetOneFileAtTime(List<ChangeAttribute> currentStagedChanges)
        {
            string displayText = TextSettings.GetTextLength(currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            Console.SetCursorPosition(1, y);
            Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
            Console.Write(displayText);
            Console.ResetColor();
        }
        private void CloseApplication()
        {
            Console.Clear();
            Environment.Exit(0);
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
