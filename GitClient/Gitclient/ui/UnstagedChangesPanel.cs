using GitClient.model;
using GitClient.repository;
using GitClient.service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Enumeration;
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
        public event EventHandler<FileSelectionChangedEventArgs>? FileSelectionChanged;
        private PanelCommunicationService communicationService;
        private StatusDiffService statusDiffService;
        private StatusService statusService;
        private List<ChangeAttribute> currentUnstagedChanges;
        private DrawTabs.Dimensions dimensions;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private ClearConsoleChoosenSpace clear;
        private int totalNumberOfFiles;
        private static int startIndex;
        private static int currentIndex;
        private static int endIndex;
        private static int fileNumber;
        private int x;
        private static int y;
        private int checkLastFile;

        public UnstagedChangesPanel(StatusService statusService, StatusDiffService statusDiffService, PanelCommunicationService communicationService)
        {
            this.statusService = statusService;
            this.statusDiffService = statusDiffService;
            this.communicationService = communicationService;
            currentUnstagedChanges = new List<ChangeAttribute>();
            dimensions = new DrawTabs.Dimensions();
            blueBox = new BlueBox();
            indicator = new Indicator();
            panel = new DrawPanels();
            clear = new ClearConsoleChoosenSpace();
            totalNumberOfFiles = statusService.GetAllUnstagedChanges().Count;
            startIndex = GetStartIndex();
            currentIndex = 0;
            endIndex = GetEndIndex();
            fileNumber = 1;
            x = 1;
            y = dimensions.unstagedStart;
            checkLastFile = 0;
        }

        public void SetCommunicationService(PanelCommunicationService service)
        {
            communicationService = service;
        }
        protected virtual void OnFileSelectionChanged(string fileName, bool isStaged)
        {
            FileSelectionChanged?.Invoke(this, new FileSelectionChangedEventArgs(fileName, isStaged));
        }
        private int GetStartIndex()
        {
            if (ReadButtons.Up == true && ReadButtons.WorkingInUnstagePanel == false || fileNumber == totalNumberOfFiles - 1)
            {
                if (statusService.GetAllUnstagedChanges().Count >= dimensions.unstagedEnd - dimensions.unstagedStart + 1)
                {
                    startIndex = statusService.GetAllUnstagedChanges().Count - (dimensions.unstagedEnd - dimensions.unstagedStart + 1);
                }
            }

            return startIndex;
        }
        private int GetEndIndex()
        {
            if (ReadButtons.Enter == true && ReadButtons.WorkingInUnstagePanel == false && ReadButtons.WorkingInStagePanel == false)
            {
                endIndex = statusService.GetAllUnstagedChanges().Count - 1;
                currentIndex = endIndex;
            }
            else
            {
                endIndex = endIndex < dimensions.unstagedEnd - dimensions.unstagedStart + 1 ? dimensions.unstagedEnd - dimensions.unstagedStart + 1
                : startIndex + dimensions.unstagedEnd - dimensions.unstagedStart + 1;
            }

            return endIndex;
        }
        public override void Show()
        {
            if (ReadButtons.WorkingInUnstagePanel == false && ReadButtons.WorkingInStagePanel == false)
            {
                fileNumber = communicationService.GetFileIndex() + 1;
                currentIndex = communicationService.GetFileIndex();
            }

            panel.DrawUnstagePanel(currentUnstagedChanges, totalNumberOfFiles, currentIndex);
            Refresh();

            if (ReadButtons.WorkingInStagePanel == false)
            {
                ReadButtons.WorkingInUnstagePanel = true;
                Navigate();
            }
        }
        private void Refresh()
        {
            currentUnstagedChanges.Clear();
            totalNumberOfFiles = statusService.GetAllUnstagedChanges().Count;

            if (totalNumberOfFiles > 0)
            {
                if (fileNumber == 0)
                {
                    fileNumber = 1;
                }
                
                currentUnstagedChanges = statusService.GetCurrentChanges(GetStartIndex(), GetEndIndex(), "unstage");
                GetAllFiles(currentUnstagedChanges);
                panel.DrawUnstagePanel(currentUnstagedChanges, totalNumberOfFiles, currentIndex);

                if (ReadButtons.WorkingInUnstagePanel == false && ReadButtons.WorkingInStagePanel == false)
                {
                    if (ReadButtons.Enter == true)
                    {
                        currentIndex = currentUnstagedChanges.Count - 1;
                        fileNumber = statusService.GetAllUnstagedChanges().Count;
                    }

                    if (fileNumber <= totalNumberOfFiles && currentIndex > 0)
                    {
                        currentIndex = currentUnstagedChanges.Count - 1;
                        fileNumber = totalNumberOfFiles;
                    }
                   
                    blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                }
                else if (ReadButtons.WorkingInUnstagePanel == true)
                {
                    if (currentIndex >= currentUnstagedChanges.Count)
                    {
                        currentIndex = currentUnstagedChanges.Count - 1;
                        y--;
                        fileNumber--;
                    }

                    blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                }
            }
            else
            {
                Console.SetCursorPosition(2, dimensions.changesPanelHeight / 2 + dimensions.tabHeight);
                string text = TextSettings.GetTextLength("No changes found in the unstage area.", dimensions.changesPanelWidth - 4);
                Console.Write(text);
            }

            if (totalNumberOfFiles == 0)
            {
                ReadButtons.WorkingInStagePanel = true;
                communicationService.NavigateToStagedPanel();
            }
        }
        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ReadButtons.Enter = false;

            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                       {
                            HandleFilesDownMoves();
                       }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            if (fileNumber > 1 || totalNumberOfFiles > 1)
                            {
                                HandleFilesUpMoves();
                            }
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
                                    communicationService.SetFileIndex(fileNumber - 1);
                                    communicationService.NavigateToDiffPanel();
                                }
                            }
                        }
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
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
            if (fileNumber < totalNumberOfFiles && fileNumber > 0)
            {
                checkLastFile = 0;
                ReadButtons.WorkingInUnstagePanel = true;
                ReadButtons.Down = true;
                ReadButtons.Up = false;
                clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);

                if (y == dimensions.unstagedEnd && endIndex < totalNumberOfFiles)
                {
                    startIndex++;
                    endIndex++;
                    clear.ClearFiles(x, y, dimensions.unstagedStart - 1, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                    Refresh();
                }
                else
                {
                    clear.ClearFiles(x, y, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningOnFile");
                    GetUnstagedFiles(currentUnstagedChanges);
                    indicator.GetIndicator(fileNumber - 1, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                }

                if (y < dimensions.unstagedEnd && currentIndex < currentUnstagedChanges.Count - 1)
                {
                    currentIndex++;
                    y++;
                }

                if (fileNumber < totalNumberOfFiles && fileNumber > 0)
                {
                    fileNumber++;
                }

                communicationService.SetCurrentFileName(currentUnstagedChanges[currentIndex].GetFileName());
                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                indicator.GetIndicator(fileNumber, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                OnFileSelectionChanged(communicationService.GetCurrentFileName(), isStaged: false);
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
                clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
                communicationService.SetLastUnstageFileName(currentUnstagedChanges.Last().GetFileName());
                communicationService.SetFileIndex(currentIndex);
                string firstStageFileName = statusService.GetCurrentChanges(0, 1, "stage")[0].GetFileName();
                OnFileSelectionChanged(firstStageFileName, isStaged: true);
                ReadButtons.WorkingInStagePanel = true;
                communicationService.NavigateToStagedPanel();
            }
        }
        private void HandleFilesUpMoves()
        {
            ReadButtons.Down = false;
            ReadButtons.Up = true;
            clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);

            if (y == dimensions.unstagedStart && startIndex > 0)
            {
                startIndex--;
                clear.ClearFiles(x, y, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                Refresh();
            }
            else
            {
                clear.ClearFiles(x, y, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningOneFile");
                GetUnstagedFiles(currentUnstagedChanges);
            }

            if (y > dimensions.unstagedStart)
            {
                currentIndex--;
                y--;
            }

            if (fileNumber > 1)
            {
                fileNumber--;
            }

            blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            indicator.GetIndicator(fileNumber - 1, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd + 1);
            OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: false);
        }
        private void HandlePressingEnter()
        {
            ReadButtons.Enter = true;
            statusService.StageFile(currentUnstagedChanges[currentIndex]);
            currentUnstagedChanges = statusService.GetCurrentChanges(GetStartIndex(), GetEndIndex(), "unstage");
            totalNumberOfFiles = statusService.GetAllUnstagedChanges().Count;

            if (currentUnstagedChanges.Count == 0)
            {
                totalNumberOfFiles = statusService.GetAllUnstagedChanges().Count;
                clear.ClearUnstagePanel(0, 2, dimensions.changesPanelWidth + 1, dimensions.unstagedEnd + 2);
                panel.DrawUnstagePanel(currentUnstagedChanges, totalNumberOfFiles, currentIndex);
                clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
            }
            else
            {
                clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                clear.ClearFiles(x, dimensions.stagedStart, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
            }

            if (fileNumber == totalNumberOfFiles && fileNumber > 1)
            {
                fileNumber--;
            }

            if (currentIndex > 0 && currentUnstagedChanges.Count > 0)
            {
                y--;
                currentIndex--;
                fileNumber--;
                endIndex--;
                OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: false);
                Refresh();
                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            }
            else if (currentIndex == 0 && currentUnstagedChanges.Count > 0)
            {
                Refresh();
                OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: false);
                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            }
            else
            {
                Console.SetCursorPosition(1, 3);
                Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                Console.SetCursorPosition(2, dimensions.changesPanelHeight / 2 + dimensions.tabHeight);
                string text = TextSettings.GetTextLength("No changes found in the unstaged area.", dimensions.changesPanelWidth - 4);
                Console.Write(text);
                OnFileSelectionChanged(statusService.GetAllStageChanges()[currentIndex].GetFileName(), isStaged: true);
                ReadButtons.WorkingInStagePanel = true;
            }

            if (statusService.GetAllUnstagedChanges().Count() == 0)
            {
                fileNumber = 0;
            }

            ReadButtons.Enter = false;
            communicationService.NavigateToStagedPanel();
        }
        private void GetUnstagedFiles(List<ChangeAttribute> currentUnstagedChanges)
        {
            if (y == dimensions.unstagedEnd && ReadButtons.Down == true || y == dimensions.unstagedStart && ReadButtons.Up == true)
            {
                GetAllFiles(currentUnstagedChanges);
                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
            }
            else
            {
                GetOneFileAtTime(currentUnstagedChanges);

                if (ReadButtons.Deleted == false)
                {
                    indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                    blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                }
            }
        }
        private void GetAllFiles(List<ChangeAttribute> currentUnstagedChanges)
        {
            clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");

            for (int i = 0; i < currentUnstagedChanges.Count; i++)
            {
                int position = dimensions.unstagedStart + i;
                string displayText = TextSettings.GetTextLength(currentUnstagedChanges[i].Display(), dimensions.changesPanelWidth - 2);
                Console.SetCursorPosition(1, position);
                Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
                Console.Write(displayText);
                Console.ResetColor();
                
                if (i == currentUnstagedChanges.Count - 1 && ReadButtons.WorkingInStagePanel == true || fileNumber == totalNumberOfFiles && i == currentUnstagedChanges.Count - 1 && ReadButtons.WorkingInUnstagePanel == false)
                {
                    y = position;
                }
            }
        }
        private void GetOneFileAtTime(List<ChangeAttribute> currentUnstagedChanges)
        {
            string displayText = TextSettings.GetTextLength(currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
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
