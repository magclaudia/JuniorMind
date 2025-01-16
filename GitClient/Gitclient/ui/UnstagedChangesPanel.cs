using Gitclient.ui;
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

        public int GetStartIndex()
        {
            if (ButtomPress.Type.enter == true)
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
            if (ButtomPress.Type.enter == true && ButtomPress.Type.workingInUnstagePanel == false && ButtomPress.Type.workingInStagePanel == false)
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
            Refresh();

            if (ButtomPress.Type.workingInStagePanel == false)
            {
                ButtomPress.Type.workingInUnstagePanel = true;
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

                if (ButtomPress.Type.workingInUnstagePanel == false && ButtomPress.Type.workingInStagePanel == false)
                {
                    if (ButtomPress.Type.enter == true)
                    {
                        currentIndex = currentUnstagedChanges.Count - 1;
                        string fileName = currentUnstagedChanges.Last().GetFileName();
                        OnFileSelectionChanged(fileName, isStaged: false);
                        fileNumber = statusService.GetAllUnstagedChanges().Count;
                    }

                    blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                }
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
                //OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: true);
                ButtomPress.Type.workingInStagePanel = true;
                communicationService.NavigateToStagedPanel();
            }
        }

        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ClearConsoleChoosenSpace clear = new ClearConsoleChoosenSpace();
            ButtomPress.Type.enter = false;

            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                       {
                            if (fileNumber < totalNumberOfFiles)
                            {
                                ButtomPress.Type.workingInUnstagePanel = true;
                                ButtomPress.Type.down = true;
                                ButtomPress.Type.up = false;
                                ButtomPress.Type.enter = false;
                                
                                clear.ClearDiff(y);

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
                            
                            if (currentIndex == totalNumberOfFiles - 1)
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
                                
                                communicationService.SetLastUnstageFileName(currentUnstagedChanges.Last().GetFileName());
                                string firstStageFileName = statusService.GetCurrentChanges(0, 1, "stage")[0].GetFileName();
                                OnFileSelectionChanged(firstStageFileName, isStaged: true);
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
                                
                                clear.ClearDiff(y);

                                if (y == dimensions.unstagedStart && startIndex > 0)
                                {
                                    startIndex--;
                                    //endIndex--;
                                    //indexForDiff--;
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
                                    //indexForDiff--;
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
                            ButtomPress.Type.enter = true;
                            ButtomPress.Type.up = false;
                            ButtomPress.Type.down = false;
                            statusService.StageFile(currentUnstagedChanges[currentIndex]);
                           
                            clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart-1, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                            clear.ClearFiles(x, dimensions.stagedStart, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                            clear.ClearDiff(y);
                            totalNumberOfFiles = statusService.GetAllUnstagedChanges().Count;

                            if (currentIndex > 0 && totalNumberOfFiles > 0)
                            {
                                y--;
                                currentIndex--;
                                //indexForDiff--;
                               // fileNumber--;
                                endIndex--;
                                OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: false);
                                Refresh();
                                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                //communicationService.SetLastIndexForDiff(indexForDiff);
                            }
                            else if (currentIndex == 0 && totalNumberOfFiles > 0)
                            {
                                Refresh();
                                OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: false);
                                blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);

                                //communicationService.SetLastIndexForDiff(indexForDiff);
                            }
                            else
                            {
                                Console.SetCursorPosition(2, dimensions.changesPanelHeight / 2 + dimensions.tabHeight);
                                string text = TextSettings.GetTextLength("No changes found in the unstaged area.", dimensions.changesPanelWidth - 4);
                                Console.Write(text);
                                OnFileSelectionChanged(currentUnstagedChanges[currentIndex].GetFileName(), isStaged: true);
                                ButtomPress.Type.workingInStagePanel = true;
                            }

                            if (statusService.GetAllUnstagedChanges().Count() == 0)
                            {
                                fileNumber = 0;
                            }
                            
                            //

                            //indexForDiff = 0;
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

        protected virtual void OnFileSelectionChanged(string fileName, bool isStaged) 
        {
            FileSelectionChanged?.Invoke(this, new FileSelectionChangedEventArgs(fileName, isStaged));
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

            //if (ButtomPress.Type.enter == true)
            //{
            //    blueBox.SetBlueBox((1, y), currentUnstagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            //    ButtomPress.Type.enter = false;
            //}
        }
            
        public void GetAllFiles(List<ChangeAttribute> currentUnstagedChanges)
        {
            for (int i = 0; i < currentUnstagedChanges.Count; i++)
            {
                int index = dimensions.unstagedStart + i;
                string displayText = TextSettings.GetTextLength(currentUnstagedChanges[i].Display(), dimensions.changesPanelWidth - 2);
                Console.SetCursorPosition(1, index);
                Console.ForegroundColor = TextSettings.SetColor(displayText[0]);
                Console.Write(displayText);
                Console.ResetColor();
                
                if (i == currentUnstagedChanges.Count - 1 && ButtomPress.Type.workingInStagePanel == true)
                {
                    y = index;
                }
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
