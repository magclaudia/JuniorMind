using GitClient.model;
using GitClient.repository;
using GitClient.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class StagedChangesPanel : UiComponent
    {
        public event EventHandler<FileSelectionChangedEventArgs> FileSelectionChanged;
        private PanelCommunicationService communicationService;
        private StatusDiffService diffService;
        private StatusService statusService;
        private List<ChangeAttribute> currentStagedChanges;
        private DrawTabs.Dimensions dimensions;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private int totalNumberOfFiles;
        private int startIndex;
        private int currentIndex;
        private int endIndex;
        private int fileNumber;
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
            totalNumberOfFiles = statusService.GetAllStageChanges().Count;
            startIndex = GetStartIndex();
            currentIndex = GetCurrentIndex();
            endIndex = GetEndIndex();
            fileNumber = 1;
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

            if (ReadButtonsPressingOrActions.Type.workingInStagePanel == true)
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
               
                if (ReadButtonsPressingOrActions.Type.workingInStagePanel == true)
                {
                     blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                     indicator.GetIndicator(currentIndex, dimensions.stagedEnd - 1, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.stagedStart - 1, dimensions.stagedEnd);
                }
            }
            else
            {
                Console.SetCursorPosition(2, dimensions.unstagedEnd + 4);
                string text = TextSettings.GetTextLength("No changes found in the stage area.", dimensions.changesPanelWidth - 4);
                Console.Write(text);
            }
        }

        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ClearConsoleChoosenSpace clear = new ClearConsoleChoosenSpace();
            int countingNumberOfPressingUp = 0;
            totalNumberOfFiles = statusService.GetAllStageChanges().Count;
            ReadButtonsPressingOrActions.Type.workingInStagePanel = true;
            ReadButtonsPressingOrActions.Type.workingInUnstagePanel = false;

            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            if (currentIndex < totalNumberOfFiles - 1 && fileNumber > 0)
                            {
                                ReadButtonsPressingOrActions.Type.down = true;
                                ReadButtonsPressingOrActions.Type.up = false;
                                clear.ClearFiles(x, y, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd, "cleaningOneFile");
                                clear.ClearDiff(y);

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
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        {
                            if (fileNumber > 1)
                            {
                                ReadButtonsPressingOrActions.Type.down = false;
                                ReadButtonsPressingOrActions.Type.up = true;

                                clear.ClearDiff(y);

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
                                ReadButtonsPressingOrActions.Type.deleted = false;
                                clear.ClearDiff(y);
                                string lastUntageFileName = statusService.GetAllUnstagedChanges().Last().GetFileName();
                                OnFileSelectionChanged(lastUntageFileName, isStaged: false);
                                ReadButtonsPressingOrActions.Type.workingInStagePanel = false;
                                ReadButtonsPressingOrActions.Type.workingInUnstagePanel = false;
                                clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                                communicationService.NavigateToUnstagedPanel();
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            ReadButtonsPressingOrActions.Type.enter = true;
                            ReadButtonsPressingOrActions.Type.up = false;
                            ReadButtonsPressingOrActions.Type.down = false;
                            statusService.UnstageFile(currentStagedChanges[currentIndex]);
                            currentStagedChanges = statusService.GetCurrentChanges(startIndex, endIndex, "stage");

                            clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                            clear.ClearFiles(x, dimensions.stagedStart, dimensions.stagedStart - 1, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                            clear.ClearDiff(y);

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
                                ReadButtonsPressingOrActions.Type.workingInStagePanel = false;
                            }

                            communicationService.NavigateToUnstagedPanel();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            if (ReadButtonsPressingOrActions.Type.rightStatus == false)
                            {
                                if (totalNumberOfFiles > 0)
                                {
                                    ReadButtonsPressingOrActions.Type.rightStatus = true;
                                    Console.Clear();
                                    communicationService.NavigateToTabPanel();
                                    communicationService.SetCurrentIndex(currentIndex);
                                    communicationService.NavigateToDiffPanel();
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

        public void OnFileSelectionChanged(string fileName, bool isStaged)
        {
            FileSelectionChanged?.Invoke(this, new FileSelectionChangedEventArgs(fileName, isStaged));
        }

        private void GetAllFiles(List<ChangeAttribute> currentStagedChanges)
        {
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
    }
}
