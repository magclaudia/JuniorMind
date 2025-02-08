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
        public event EventHandler<FileSelectionChangedEventArgs> FileSelectionChanged;
        private PanelCommunicationService communicationService;
        private StatusDiffService statusDiffService;
        private StatusService statusService;
        private List<ChangeAttribute> currentUnstagedChanges;
        private DrawTabs.Dimensions dimensions;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private int totalNumberOfFiles;
        private static int startIndex;
        private static int currentIndex;
        private static int endIndex;
        private static int fileNumber;
        private int x;
        private static int y;
        private int countingIndex;
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
            totalNumberOfFiles = statusService.GetAllUnstagedChanges().Count;
            startIndex = GetStartIndex();
            currentIndex = GetCurrentIndex();
            endIndex = GetEndIndex();
            fileNumber = 1;
            x = 1;
            y = dimensions.unstagedStart;
            countingIndex = 0;
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
        public int GetStartIndex()
        {
            if (ReadButtons.Enter == true)
            {
                if (statusService.GetAllUnstagedChanges().Count > dimensions.unstagedEnd - dimensions.unstagedStart + 1)
                {
                    startIndex = statusService.GetAllUnstagedChanges().Count - (dimensions.unstagedEnd - dimensions.unstagedStart + 1);
                }
            }
            
            return startIndex;
        }
        public int GetCurrentIndex()
        {
            return currentIndex;
        }
        public int GetEndIndex()
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
                currentUnstagedChanges = statusService.GetCurrentChanges(GetStartIndex(), GetEndIndex(), "unstage");
                GetAllFiles(currentUnstagedChanges);

                if (ReadButtons.WorkingInUnstagePanel == false && ReadButtons.WorkingInStagePanel == false)
                {
                    if (ReadButtons.Enter == true)
                    {
                        currentIndex = currentUnstagedChanges.Count - 1;
                        fileNumber = statusService.GetAllUnstagedChanges().Count;
                        SetY();
                    }

                    blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                }
                else if (ReadButtons.WorkingInUnstagePanel == true)
                {
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

                GoToLog();
                
                
                
            }
        }

        private void GoToLog()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2)
            {
                communicationService.DisplayLog();
            }
        }

        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ClearConsoleChoosenSpace clear = new ClearConsoleChoosenSpace();
            ReadButtons.Enter = false;

            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                       {
                            if (fileNumber < totalNumberOfFiles)
                            {
                                ReadButtons.WorkingInUnstagePanel = true;
                                ReadButtons.Down = true;
                                ReadButtons.Up = false;
                                ReadButtons.Enter = false;

                                //
                                ReadButtons.RightOnce = true;
                                clear.ClearDiff(y);
                                ReadButtons.RightOnce = false;
                                //

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
                                    indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
                                }

                                if (y < dimensions.unstagedEnd)
                                {
                                    currentIndex++;
                                    y++;
                                }

                                if (fileNumber < totalNumberOfFiles && fileNumber > 0)
                                {
                                    fileNumber++;
                                }

                                communicationService.SetCurrentFileName(currentUnstagedChanges[currentIndex].GetFileName());
                                countingIndex++;    
                                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd);
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
                                panel.DrawUnstagePanel(currentUnstagedChanges, totalNumberOfFiles, currentIndex);
                               //
                                ReadButtons.RightOnce = true;
                                clear.ClearDiff(y);
                                ReadButtons.RightOnce = false;
                                //
                                communicationService.SetLastUnstageFileName(currentUnstagedChanges.Last().GetFileName());
                                string firstStageFileName = statusService.GetCurrentChanges(0, 1, "stage")[0].GetFileName();
                                OnFileSelectionChanged(firstStageFileName, isStaged: true);
                                ReadButtons.WorkingInStagePanel = true;
                                communicationService.NavigateToStagedPanel();
                            }
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        {
                            if (fileNumber > 1)
                            {
                                ReadButtons.Down = false;
                                ReadButtons.Up = true;

                                //
                                ReadButtons.RightOnce = true;
                                clear.ClearDiff(y);
                                ReadButtons.RightOnce = false;
                                //

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

                                if (fileNumber > 0)
                                {
                                    fileNumber--;
                                }

                                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                indicator.GetIndicator(currentIndex, dimensions.unstagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.unstagedStart - 1, dimensions.unstagedEnd + 1);
                                OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: false);
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            ReadButtons.Enter = true;
                            ReadButtons.Up = false;
                            ReadButtons.Down = false;
                            statusService.StageFile(currentUnstagedChanges[currentIndex]);
                            currentUnstagedChanges = statusService.GetCurrentChanges(GetStartIndex(), GetEndIndex(), "unstage");
                            clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                            clear.ClearFiles(x, dimensions.stagedStart, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                            //
                            ReadButtons.RightOnce = true;
                            clear.ClearDiff(y);
                            ReadButtons.RightOnce = false;
                            //
                            fileNumber--;

                            if (currentIndex > 0 && currentUnstagedChanges.Count > 0)
                            {
                                y--;
                                currentIndex--;


                                if (currentIndex == 0)
                                {
                                    fileNumber = 1;
                                }
                                else
                                {
                                    fileNumber--;
                                }

                                endIndex--;
                                OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: false);
                                Refresh();
                                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                            }
                            else if (currentIndex == 0 && currentUnstagedChanges.Count > 0)
                            {
                                fileNumber = 1;
                                Refresh();
                                OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: false);
                                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                            }
                            else
                            {
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
                            
                            communicationService.NavigateToStagedPanel();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            if (ReadButtons.RightStatus == false)    
                            {
                                if (totalNumberOfFiles > 0)
                                {
                                    //ReadButtons.RightStatus = true;
                                    ReadButtons.RightOnce = true;
                                    Console.Clear();
                                    communicationService.NavigateToTabPanel();
                                    communicationService.SetCurrentIndex(currentIndex);
                                    communicationService.NavigateToDiffPanel();
                                }
                            }
                        }
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
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
        private void SetY()
        {
            if (totalNumberOfFiles > dimensions.unstagedEnd)
            {
                y = dimensions.unstagedEnd;
            }
            else
            {
                y = dimensions.tabHeight + totalNumberOfFiles + 2;
            }
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
        public void GetAllFiles(List<ChangeAttribute> currentUnstagedChanges)
        {
            for (int i = 0; i < currentUnstagedChanges.Count; i++)
            {
                int index = dimensions.unstagedStart + i;
                string displayText = TextSettings.GetTextLength(currentUnstagedChanges[i].Display(), dimensions.changesPanelWidth - 2);
                Console.SetCursorPosition(1, index);
                Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
                Console.Write(displayText);
                Console.ResetColor();
                
                if (i == currentUnstagedChanges.Count - 1 && ReadButtons.WorkingInStagePanel == true)
                {
                    y = index;
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
