using GitClient.model;
using GitClient.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class CommitsNavigation : UiComponent
    {
        public event EventHandler<CommitSelectionChangedEventArgs>? CommitSelectionChanged;
        private CommitsService commitsService;
        private PanelCommunicationService panelCommunicationService;
        private List<Commit> currentCommits;
        private List<ChangeAttribute> currentFiles;
        private List<string> currentDiff;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private ClearConsoleChoosenSpace clear;
        private int commitStartingIndex;
        private int commitEndIndex;
        private int commitIndex;
        private int fileIndex;
        private int totalCommits;
        private int y;
        private int yForDiff;
        private int x;
        private int commitNumber;
        private int width = Console.WindowWidth - 3;
        private int enterCount = 0;
        private int fileNumber;
        private int filesStartingIndex;
        private int totalFiles;
        private int retainPositionOfYForCommit;
        private int countingPressingRight;
        private int diffIndex;
        private int diffNumber;
        private int diffStartingIndex;

        public CommitsNavigation(CommitsService commitsService, PanelCommunicationService panelCommunicationService)
        {
            this.commitsService = commitsService;
            this.panelCommunicationService = panelCommunicationService;
            commitStartingIndex = 0;
            commitEndIndex = GetEndIndex();
            commitIndex = 0;
            fileIndex = 0;
            y = 3;
            yForDiff = 3;
            currentCommits = new List<Commit>();
            currentFiles = new List<ChangeAttribute>();
            blueBox = new BlueBox();
            indicator = new Indicator();
            panel = new DrawPanels();
            clear = new ClearConsoleChoosenSpace();
            totalCommits = this.commitsService.GetAllCommits().Count;
            commitNumber = 1;
            fileNumber = 1;
            filesStartingIndex = 0;
            totalFiles = commitsService.GetAllFilesForCommit(commitNumber - 1).Count;
            currentDiff = new List<string>();
            diffNumber = 1;
            diffStartingIndex = 0;
            this.commitsService = commitsService;
        }

        protected virtual void OnCommitSelectionChanged(ButtonStates buttonStates, int startIndex, int endIndex, int currentIndex, int fileIndex, int y, int commitNumber, int fileNumber, int filesStartingIndex, int diffIndex, int diffStartingIndex)
        {
            CommitSelectionChanged?.Invoke(this, new CommitSelectionChangedEventArgs(buttonStates, startIndex, endIndex, currentIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex));
        }

        public override void Show()
        {
            currentCommits = commitsService.GetCurrentListOfCommits(commitStartingIndex, commitEndIndex);
            ButtonStates buttonStates = SetButtonStates(gitLog: true);
            OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
            Navigate();
        }
        public void Navigate()
        {
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            ReadButtons.Down = true;
                            ReadButtons.Up = false;

                            if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true || ReadButtons.Enter == true)
                            {
                                if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true)
                                {
                                    ButtonStates buttonStates = SetButtonStates(gitLog: true);
                                    CommitsDownMoves(buttonStates);
                                }
                                else if (ReadButtons.Enter == true)
                                {
                                    ButtonStates buttonStates = SetButtonStates(enter: true);
                                    CommitsDownMoves(buttonStates);
                                }
                            }
                            else if (ReadButtons.DiffMovements == true)
                            {
                                ButtonStates buttonStates = SetButtonStates(diff: true);
                                DiffDownMoves(buttonStates);
                            }
                            else
                            {
                                ButtonStates buttonStates = SetButtonStates(right: true);
                                FilesDownMoves(buttonStates);
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            ReadButtons.Down = false;
                            ReadButtons.Up = true;

                            if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true || ReadButtons.Enter == true)
                            {
                                if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true)
                                {
                                    ButtonStates buttonStates = SetButtonStates(gitLog: true);
                                    CommitsUpMoves(buttonStates);
                                }
                                else if (ReadButtons.Enter == true)
                                {
                                    ButtonStates buttonStates = SetButtonStates(enter: true);
                                    CommitsUpMoves(buttonStates);
                                }
                            }
                            else if (ReadButtons.DiffMovements == true)
                            {
                                ButtonStates buttonStates = SetButtonStates(diff: true);
                                DiffUpMoves(buttonStates);
                            }
                            else
                            {
                                ButtonStates buttonStates = SetButtonStates(right: true);
                                FilesUpMoves(buttonStates);
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            ReadButtons.Enter = true;
                            enterCount++;
                            ButtonStates buttonStates = SetButtonStates(enter: true);
                            retainPositionOfYForCommit = y;

                            if (enterCount == 1)
                            {
                                clear.ClearCommitPanel();
                                OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                ReadButtons.DisplayListOfCommitsOnEntirePanel = false;
                            }
                            else
                            {
                                enterCount = 0;
                                clear.ClearCommitPanel();
                                buttonStates = SetButtonStates(enter: true, gitLog: true);
                                OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            ReadButtons.RightOnce = true;

                            if (countingPressingRight == 1)
                            {
                                ButtonStates buttonStates = SetButtonStates(diff: true);
                                diffIndex = 0;
                                diffStartingIndex = 0;
                                diffNumber = 1;
                                clear.ClearCommitPanel();
                                ReadButtons.DiffMovements = true;
                                currentFiles = commitsService.GetCurrentFiles(filesStartingIndex, commitNumber - 1);
                                currentDiff = commitsService.GetCurrentDiffForSelectedFile(commitNumber - 1, currentFiles[fileIndex].GetFileName());
                                panel.DrawDiffPanel(currentDiff, 1, commitIndex);
                                OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, yForDiff, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                countingPressingRight = 0;
                                ReadButtons.Enter = false;
                            }

                            if (ReadButtons.Enter == true && ReadButtons.DisplayListOfCommitsOnEntirePanel == false)
                            {
                                ButtonStates buttonStates = SetButtonStates(right: true);
                                countingPressingRight++;
                                retainPositionOfYForCommit = y;
                                clear.ClearCommitPanel();
                                y = Console.WindowHeight / 2 + 5;
                                OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                ReadButtons.Enter = false;
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            if (ReadButtons.RightOnce == true && ReadButtons.DiffMovements == false)
                            {
                                ReadButtons.Left = true;
                                ButtonStates buttonStates = SetButtonStates(left: true);
                                ReadButtons.RightOnce = false;
                                y = retainPositionOfYForCommit;
                                clear.ClearCommitPanel();
                                countingPressingRight = 0;
                                fileIndex = 0;
                                fileNumber = 1;
                                filesStartingIndex = 0;
                                OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                ReadButtons.Enter = true;
                                ReadButtons.Left = false;
                            }
                        }
                        break;
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            if (ReadButtons.RightOnce == true)
                            {
                                y = retainPositionOfYForCommit;
                            }

                            fileIndex = 0;
                            filesStartingIndex = 0;
                            diffIndex = 0;
                            diffStartingIndex = 0;
                            countingPressingRight = 0;
                            fileNumber = 1;
                            yForDiff = 3;
                            ReadButtons.RightOnce = false;
                            ReadButtons.DiffMovements = false;
                            Console.Clear();
                            panelCommunicationService.NavigateToStatusInitialState();
                        }
                        break;
                    case ConsoleKey.Escape:
                        {
                            if (ReadButtons.DiffMovements == true)
                            {
                                ButtonStates buttonStates = SetButtonStates(right: true);
                                ReadButtons.DiffMovements = false;
                                ReadButtons.Enter = true;
                                ReadButtons.Esc = true;
                                clear.ClearCommitPanel();
                                diffIndex = 0;
                                diffNumber = 1;
                                diffStartingIndex = 0;
                                yForDiff = 3;
                                OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                ReadButtons.Esc = false;
                                ReadButtons.Enter = false;
                                countingPressingRight++;
                            }
                            else
                            {
                                CloseApplication();
                            }
                        }
                        break;
                }

            } while (true);
        }
        private int GetEndIndex()
        {
            return commitEndIndex < Console.WindowHeight - 3 ? Console.WindowHeight - 3
                : commitStartingIndex + Console.WindowHeight - 3;
        }
        private void CommitsDownMoves(ButtonStates buttonStates)
        {
            if (commitNumber < totalCommits)
            {
                if (y == Console.WindowHeight - 2 && commitEndIndex < totalCommits)
                {
                    commitStartingIndex++;
                    commitEndIndex++;
                    currentCommits = commitsService.GetCurrentListOfCommits(commitStartingIndex, commitEndIndex);

                    if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearCommitFullWindow(0, 2, Console.WindowWidth, Console.WindowHeight);
                        buttonStates = SetButtonStates(gitLog: true, enter: false);
                        OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                    }
                }
                else
                {
                    if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearOneCommit(1, y + 1, Console.WindowWidth - 2);
                        buttonStates = SetButtonStates(down: true, gitLog: true);
                        OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                    }
                }

                if (y < Console.WindowHeight - 2)
                {
                    commitIndex++;
                    y++;
                }

                if (commitNumber < totalCommits)
                {
                    commitNumber++;
                }

                if (y <= Console.WindowHeight - 2 && ReadButtons.DisplayListOfCommitsOnEntirePanel == false)
                {
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
                else
                {
                    Console.SetCursorPosition(1, 2);
                    Console.Write($"Commit: {commitNumber} / {totalCommits} ");
                    blueBox.SetBlueBox((1, y), currentCommits[commitIndex].Display(), width);
                    indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, 3, Console.WindowHeight - 1);
                }
            }
        }
        private void FilesDownMoves(ButtonStates buttonStates)
        {
            totalFiles = commitsService.GetAllFilesForCommit(commitNumber - 1).Count;
            currentFiles = commitsService.GetCurrentFiles(filesStartingIndex, commitNumber - 1);

            if (fileNumber < totalFiles)
            {
                if (y == Console.WindowHeight - 2)
                {
                    clear.ClearFiles(1, y, Console.WindowHeight / 2 + 5, Console.WindowWidth / 2 - 3, Console.WindowHeight - 2, "cleaningAllPanelArea");
                    clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
                    commitStartingIndex++;
                    filesStartingIndex++;
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
                else
                {
                    clear.ClearFiles(1, y, commitStartingIndex, Console.WindowWidth / 2 - 3, Console.WindowHeight - 1, "cleaningOneFile");
                    clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
                }

                if (y < Console.WindowHeight - 2)
                {
                    fileIndex++;
                    y++;
                }

                if (fileNumber < totalFiles)
                {
                    fileNumber++;
                }

                if (fileNumber <= totalFiles)
                {
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
            }
        }
        private void DiffDownMoves(ButtonStates buttonStates)
        {
            List<ChangeAttribute> files = currentFiles = commitsService.GetCurrentFiles(filesStartingIndex, commitNumber - 1);
            currentDiff = commitsService.GetCurrentDiffForSelectedFile(commitNumber - 1, files[fileIndex].GetFileName());

            if (diffNumber < currentDiff.Count)
            {
                if (yForDiff == Console.WindowHeight - 2)
                {
                    clear.ClearDiff(1, yForDiff, Console.WindowHeight - 2);
                    diffStartingIndex++;
                    diffIndex++;
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, yForDiff, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
                else
                {
                    clear.ClearDiff(1, yForDiff, Console.WindowHeight - 2);
                }

                if (yForDiff < Console.WindowHeight - 2)
                {
                    diffIndex++;
                    yForDiff++;
                }

                if (diffNumber < currentDiff.Count)
                {
                    diffNumber++;
                }

                if (diffNumber <= currentDiff.Count && yForDiff < Console.WindowHeight - 1)
                {
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, yForDiff, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
            }
        }
        private void CommitsUpMoves(ButtonStates buttonStates)
        {
            if (commitNumber > 1)
            {
                if (y == 3 && commitStartingIndex > 0)
                {
                    commitStartingIndex--;
                    commitEndIndex--;
                    currentCommits = commitsService.GetCurrentListOfCommits(commitStartingIndex, commitEndIndex);

                    if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearCommitFullWindow(1, y, Console.WindowWidth - 1, Console.WindowHeight);
                        buttonStates = SetButtonStates(gitLog: true, enter: false);
                        OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                    }
                }
                else
                {
                    if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearOneCommit(1, y - 1, Console.WindowWidth - 2);
                        buttonStates = SetButtonStates(up: true, gitLog: true);
                        OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                    }
                }

                if (y > 3)
                {
                    commitIndex--;
                    y--;
                }

                if (commitNumber > 1)
                {
                    commitNumber--;
                }

                if (y >= 3 && ReadButtons.DisplayListOfCommitsOnEntirePanel == false)
                {
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
                else if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true)
                {
                    Console.SetCursorPosition(1, 2);
                    Console.Write($"Commit: {commitNumber} / {totalCommits} ");
                    blueBox.SetBlueBox((1, y), currentCommits[commitIndex].Display(), width);
                    indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, y, Console.WindowHeight - 1);
                }
            }
        }
        private void FilesUpMoves(ButtonStates buttonStates)
        {
            totalFiles = commitsService.GetAllFilesForCommit(commitIndex).Count;
            currentFiles = commitsService.GetCurrentFiles(fileIndex, commitIndex);

            if (fileNumber > 1)
            {
                bool callOnCommitSelectionChanged = false;

                if (y == Console.WindowHeight / 2 + 5)
                {
                    clear.ClearFiles(1, y, Console.WindowHeight / 2 + 5, Console.WindowWidth / 2 - 2, Console.WindowHeight - 2, "cleaningAllPanelArea");
                    clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
                    commitStartingIndex--;
                    fileIndex = -1;
                    filesStartingIndex--;
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                    callOnCommitSelectionChanged = true;
                    fileIndex = 0;
                }
                else
                {
                    clear.ClearFiles(1, y, commitStartingIndex, Console.WindowWidth / 2 - 2, Console.WindowHeight - 1, "cleaningOneFile");
                    clear.ClearDiff(Console.WindowWidth / 2 + 2, y, Console.WindowHeight - 2);
                }

                if (fileIndex > 0)
                {
                    y--;
                    fileIndex--;
                }

                if (fileNumber <= totalFiles)
                {
                    fileNumber--;
                }

                if (callOnCommitSelectionChanged == false)
                {
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
            }
        }
        private void DiffUpMoves(ButtonStates buttonStates)
        {
            List<ChangeAttribute> files = currentFiles = commitsService.GetCurrentFiles(filesStartingIndex, commitNumber - 1);
            currentDiff = commitsService.GetCurrentDiffForSelectedFile(commitNumber - 1, files[fileIndex].GetFileName());

            if (diffNumber > 1)
            {
                bool callOnCommitSelectionChanged = false;

                if (yForDiff == 3)
                {
                    clear.ClearDiff(1, yForDiff, Console.WindowHeight - 2);
                    diffStartingIndex--;
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, yForDiff, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                    callOnCommitSelectionChanged = true;
                }
                else
                {
                    clear.ClearDiff(1, yForDiff, Console.WindowHeight - 2);
                }

                if (diffIndex > 0)
                {
                    diffIndex--;
                }

                if (yForDiff > 3)
                {
                    yForDiff--;
                }

                if (diffNumber <= currentDiff.Count)
                {
                    diffNumber--;
                }

                if (callOnCommitSelectionChanged == false)
                {
                    OnCommitSelectionChanged(buttonStates, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, yForDiff, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
            }
        }
        private ButtonStates SetButtonStates(bool down = false, bool up = false, bool enter = false, bool right = false, bool left = false, bool diff = false, bool gitLog = false)
        {
            return new ButtonStates(down: down, up: up, enter: enter, right: right, left: left, diff: diff, gitLog);
        }
        private void CloseApplication()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
