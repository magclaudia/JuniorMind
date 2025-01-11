using Gitclient.ui;
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
        private int totalNumberOfFiles;
        private int startIndex;
        private int currentIndex;
       // private static int lastEndIndexValue = -1;
        private int endIndex;
        private int fileNumber;
       // private static int lastFileNumberValue;
        //private static int lastValueOfY;
        private int indexForDiff;
        //private static int lastValueOfindexForDiff;
        private int x;
        private int y;

        public StagedChangesPanel(StatusDiffService diffService, StatusService statusService)
        {
            this.diffService = diffService;
            this.statusService = statusService;
            currentStagedChanges = new List<ChangeAttribute>();
            totalNumberOfFiles = statusService.GetAllStageChanges().Count;
            dimensions = new DrawTabs.Dimensions();
            startIndex = GetStartIndex();
            currentIndex = GetCurrentIndex();
            endIndex = /*lastEndIndexValue >= 0 && lastEndIndexValue < totalNumberOfFiles ? lastEndIndexValue :*/ GetEndIndex();
            fileNumber =/* lastFileNumberValue*/ 1;
            indexForDiff = 0;
            //y = lastValueOfY;
            x = 1;
            y = dimensions.stagedStart;
            blueBox = new BlueBox();
            indicator = new Indicator();
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
            return currentIndex < totalNumberOfFiles ? dimensions.stagedEnd - dimensions.stagedStart
                : totalNumberOfFiles - 1;
        }

        //public void SaveLastEndIndexValue()
        //{
        //    lastEndIndexValue = endIndex;
        //}

        //public int SaveFileNumberLastValue()
        //{
        //    return lastFileNumberValue > 1 ? fileNumber = lastFileNumberValue : fileNumber = 1;
        //}

        //public int SaveLastYValue()
        //{
        //    return lastValueOfY > 0 ? y = lastValueOfY : y = dimensions.stagedStart;
        //}

        public void SetCommunicationService(PanelCommunicationService service)
        {
            communicationService = service;
        }

        public override void Show()
        {
            Refresh();

            if (ButtomPress.Type.workingInStagePanel == true)
            {
                Navigate();
            }
        }

        private void Refresh()
        {
            if (statusService.GetAllStageChanges().Count > 0)
            {
                currentStagedChanges.Clear();
                currentStagedChanges = statusService.GetCurrentChanges(startIndex, endIndex, "stage");
                GetAllFiles(currentStagedChanges);
               
                if (ButtomPress.Type.workingInStagePanel == true)
                {
                    blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                    indicator.GetIndicator(currentIndex, dimensions.stagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.stagedStart - 1, dimensions.stagedEnd);
                }
            }
            else
            {
                Console.SetCursorPosition(2, dimensions.unstagedEnd + 4);
                string text = TextSettings.GetTextLength("No changes found in the stage area.", dimensions.changesPanelWidth - 4);
                Console.Write(text);
            }

            DrawPanel(currentStagedChanges);
        }

        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ClearConsoleChoosenSpace clear = new ClearConsoleChoosenSpace();
            int countingNumberOfPressingUp = 0;
            totalNumberOfFiles = statusService.GetAllStageChanges().Count;



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
                                clear.ClearFiles(x, y, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd, "cleaningOneFile");
                                clear.ClearDiff(y);

                                if (y == dimensions.stagedEnd - 1 && endIndex < totalNumberOfFiles)
                                {
                                    startIndex++;
                                    endIndex++;
                                    indexForDiff++;
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
                                    indexForDiff++;
                                }

                                if (fileNumber < totalNumberOfFiles && fileNumber > 0)
                                {
                                    fileNumber++;
                                }

                                blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                indicator.GetIndicator(currentIndex, dimensions.stagedStart, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.stagedStart, dimensions.stagedEnd - 1);
                                OnFileSelectionChanged(indexForDiff, isStaged: true);
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

                                if (y == dimensions.stagedStart && startIndex > 0)
                                {
                                    clear.ClearFiles(x, y, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                                    startIndex--;
                                    endIndex--;
                                    indexForDiff--;
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
                                    indexForDiff--;
                                }

                                if (fileNumber > 1)
                                {
                                    fileNumber--;
                                }

                                blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                                indicator.GetIndicator(currentIndex, dimensions.stagedEnd - 1, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.stagedStart, dimensions.stagedEnd);
                                OnFileSelectionChanged(indexForDiff, isStaged: true);
                            }

                            if (fileNumber == 1 && statusService.GetAllUnstagedChanges().Count > 0)
                            {
                                countingNumberOfPressingUp++;
                            }

                            if (fileNumber == 1 && statusService.GetAllUnstagedChanges().Count > 0 && countingNumberOfPressingUp == 2)
                            {
                                //SaveLastEndIndexValue();
                                Console.SetCursorPosition(1, dimensions.stagedStart);
                                Console.Write(new string(' ', dimensions.changesPanelWidth - 1));
                                GetOneFileAtTime(currentStagedChanges);
                                DrawPanel(currentStagedChanges);
                                ButtomPress.Type.workingInStagePanel = false;
                                ButtomPress.Type.workingInUnstagePanel = true;
                                ButtomPress.Type.deleted = false;
                                clear.ClearDiff(y);
                                indexForDiff = communicationService.GetLastIndexForDiff();
                                OnFileSelectionChanged(indexForDiff, isStaged: false);
                                clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                                communicationService.NavigateToUnstagedPanel();
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            ButtomPress.Type.enter = true;
                            ButtomPress.Type.up = false;
                            ButtomPress.Type.down = false;
                            statusService.StageFile(currentStagedChanges[currentIndex]);
                            clear.ClearFiles(x, dimensions.unstagedStart, dimensions.unstagedStart, dimensions.changesPanelWidth - 1, dimensions.unstagedEnd, "cleaningAllPanelArea");
                            clear.ClearFiles(x, dimensions.stagedStart, dimensions.stagedStart, dimensions.changesPanelWidth - 1, dimensions.stagedEnd - 1, "cleaningAllPanelArea");
                            clear.ClearDiff(y);
                            //Ui ui = new Ui();
                            //ui.Show();
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
                                    //lastValueOfY = y;
                                    //SaveFileNumberLastValue();
                                    //SaveLastEndIndexValue();
                                    //SaveLastIndexForDiff();
                                    //stageDiff.Show(/*indexForDiff*/);
                                   // stageDiff.Navigate();
                                    blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
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

        public void OnFileSelectionChanged(int fileIndex, bool isStaged)
        {
            FileSelectionChanged?.Invoke(this, new FileSelectionChangedEventArgs(indexForDiff, isStaged));
        }

        private void DrawPanel(List<ChangeAttribute> currentStagedChanges)
        {
            for (int i = dimensions.unstagedEnd + 3; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width / 2 - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < dimensions.changesPanelWidth; i++)
            {
                Console.SetCursorPosition(i, dimensions.unstagedEnd + 2);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, dimensions.unstagedEnd + 2);
            Console.Write("┌");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.unstagedEnd + 2);
            Console.Write("┐");
            Console.SetCursorPosition(dimensions.width / 2 - 1, Console.WindowHeight - 1);
            Console.Write("┘");

            string text = TextSettings.GetTextLength("Staged Changes: ", dimensions.changesPanelWidth);
            Console.SetCursorPosition(1, dimensions.unstagedEnd + 2);
            Console.Write(text);

            if (totalNumberOfFiles > 0)
            {
                StagedPath();
                //GetStageFiles(currentStagedChanges);
            }
        }

        private void GetStageFiles(List<ChangeAttribute> currentStageChanges)
        {
            //if (y == dimensions.stagedEnd && ButtomPress.Type.down == true || y == dimensions.stagedStart && ButtomPress.Type.up == true || ButtomPress.Type.escape == true)
           
            if (ButtomPress.Type.workingInStagePanel == true)
            {
                GetAllFiles(currentStagedChanges);
                blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                indicator.GetIndicator(currentIndex, dimensions.stagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.stagedStart - 1, dimensions.stagedEnd);
            }
            //else
            //{
            //    GetOneFileAtTime(currentStagedChanges);

            //    //if (ButtomPress.Type.workingInStagePanel == true && currentIndex > 0)
            //    //{
            //    //    indicator.GetIndicator(currentIndex, dimensions.stagedEnd, totalNumberOfFiles, dimensions.width / 2 - 1, dimensions.stagedStart - 1, dimensions.stagedEnd);
            //    //    blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            //    //}
            //}

            //if (ButtomPress.Type.enter == true)
            //{
            //    blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            //    ButtomPress.Type.enter = false;
            //}
        }

        private void GetAllFiles(List<ChangeAttribute> currentStagedChanges)
        {
            for (int i = 0; i < currentStagedChanges.Count; i++)
            {
                int y = dimensions.stagedStart + i;
                string displayText = TextSettings.GetTextLength(currentStagedChanges[i].Display(), dimensions.changesPanelWidth - 2);
                Console.SetCursorPosition(1, y);
                Console.ForegroundColor = TextSettings.SetColor(displayText[0]);
                Console.Write(displayText);
                Console.ResetColor();

                //if (currentIndex == 0 && ButtomPress.Type.workingInStagePanel == true && i == 0)
                //{
                //    blueBox.SetBlueBox((1, y), currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
                //}
            }
        }

        private void GetOneFileAtTime(List<ChangeAttribute> currentStagedChanges)
        {
            string displayText = TextSettings.GetTextLength(currentStagedChanges[currentIndex].Display(), dimensions.changesPanelWidth - 2);
            Console.SetCursorPosition(1, y);
            Console.ForegroundColor = TextSettings.SetColor(displayText[0]);
            Console.Write(displayText);
            Console.ResetColor();
        }

        private void StagedPath()
        {
            GetProjectPath projectPath = new GetProjectPath();
            string path = $"  ▾{projectPath.ProjectPath(Environment.CurrentDirectory)}";
            path = TextSettings.GetTextLength(path, dimensions.changesPanelWidth - 1);
            Console.SetCursorPosition(1, dimensions.stagedStart - 1);
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
