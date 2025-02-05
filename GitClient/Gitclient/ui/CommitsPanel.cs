using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;
using GitClient.repository;
using GitClient.service;

namespace GitClient.ui
{
    public class CommitsPanel : UiComponent
    {
        public event EventHandler<CommitSelectionChangedEventArgs> CommitSelectionChanged;
        private CommitsLibGit2Repository libgit2Repository;
        private CommitsService commitsService;
        private CommitsWithDescription commitsWithDescription;
        private PanelCommunicationService panelCommunicationService;
        private  int commitStartingIndex;
        private  int commitEndIndex;
        private  int commitIndex;
        private int fileIndex;
        private List<CommitsElements> currentCommits;
        private List<ChangeAttribute> currentFiles;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private ClearConsoleChoosenSpace clear;
        private int totalCommits;
        private int y;
        private int commitNumber;
        private int width = Console.WindowWidth - 3;
        private int enterCount = 0;
        private int fileNumber;
        private int filesStartingIndex;
        private int totalFiles;
        private int retainPositionOfYWhenEnter;
        private int retainPositionOfYWhenRight;
        private int countingPressingRight;
        private int diffIndex;
        private List<string> currentDiff;
        private int diffNumber;
        private int diffStartingIndex;

        public CommitsPanel(CommitsLibGit2Repository libGit2Repository, CommitsService commitsService, PanelCommunicationService panelCommunicationService)
        {
            this.libgit2Repository = libGit2Repository;
            this.commitsService = commitsService;
            this.panelCommunicationService = panelCommunicationService;
            commitsWithDescription = new CommitsWithDescription(this.commitsService, panelCommunicationService);
            commitStartingIndex = GetStartIndex();
            commitEndIndex = GetEndIndex();
            commitIndex = CommitIndex();
            fileIndex = 0;
            y = 3;
            currentCommits = new List<CommitsElements>();
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
        }

        public void SetCommunicationService(PanelCommunicationService service)
        {
            panelCommunicationService = service;
        }

        protected virtual void OnCommitSelectionChanged(bool enter, bool right, bool left, bool diff, int startIndex, int endIndex, int currentIndex, int fileIndex, int y, int commitNumber, int fileNumber, int filesStartingIndex, int diffIndex, int diffStartingIndex)
        {
            CommitSelectionChanged.Invoke(this, new CommitSelectionChangedEventArgs(enter, right, left, diff, startIndex, endIndex, currentIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex));
        } 

        public int GetStartIndex()
        {
            return commitStartingIndex;
        }

        public int GetEndIndex()
        {
            return commitEndIndex < Console.WindowHeight - 3 ? Console.WindowHeight - 3
                : commitStartingIndex + Console.WindowHeight - 3;
        }

        public int CommitIndex()
        {
            return commitIndex;
        }

        public override void Show()
        {
            Refresh();
            Navigate();
        }

        public void Refresh() 
        {
            ReadButtons.Type.displayListOfCommitsOnEntirePanel = true;
            ReadButtons.Type.rightStatus = false;
            panel.DrawBorderForFullSizeCommitList();
            commitsService.GetAllCommits();
            currentCommits = commitsService.GetCurrentListOfCommits(commitStartingIndex, commitEndIndex);
            DisplayCommits(width);
            indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, y, Console.WindowHeight - 2);
            blueBox.SetBlueBox((1, y), currentCommits[commitIndex].Display(), Console.WindowWidth - 3);
            GetCommitNumber();
        }

        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            ReadButtons.Type.down = true;
                            ReadButtons.Type.up = false;

                            if (ReadButtons.Type.displayListOfCommitsOnEntirePanel == true || ReadButtons.Type.enter == true)
                            {
                                CommitsDownMoves();
                            }
                            else if (ReadButtons.Type.diffMovements == true)
                            {
                                DiffDownMoves();
                            }
                            else
                            {
                                FilesDownMoves();
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            ReadButtons.Type.down = false;
                            ReadButtons.Type.up = true;

                            if (ReadButtons.Type.displayListOfCommitsOnEntirePanel == true || ReadButtons.Type.enter == true)
                            {
                                CommitsUpMoves();
                            }
                            else if (ReadButtons.Type.diffMovements == true)
                            {
                                DiffUpMoves();
                            }
                            else
                            {
                                FilesUpMoves();
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            ReadButtons.Type.enter = true;
                            enterCount++;
                           
                            if (enterCount == 1)
                            {
                                clear.ClearCommitPanel();
                                OnCommitSelectionChanged(enter: true, right: false, left: false, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                ReadButtons.Type.displayListOfCommitsOnEntirePanel = false;
                            }
                            else
                            {
                                clear.ClearCommitFullWindow(0, y, Console.WindowWidth, Console.WindowHeight);
                                width = Console.WindowWidth - 3;
                                enterCount = 0;
                                Refresh();
                            }
                        }

                        break;
                    case ConsoleKey.RightArrow:
                        {
                            ReadButtons.Type.rightOnce = true;
                            
                            if (countingPressingRight == 1)
                            {
                                y = 3;
                                diffIndex = 0;
                                diffStartingIndex = 0;
                                diffNumber = 1;
                                clear.ClearCommitPanel();
                                ReadButtons.Type.diffMovements = true;
                                currentFiles = commitsService.GetCurrentFiles(filesStartingIndex, commitNumber - 1);
                                currentDiff = commitsService.GetCurrentDiffForSelectedFile(commitNumber - 1, currentFiles[fileIndex].GetFileName());
                                panel.DrawDiffPanel(currentDiff, 1, commitIndex);
                                OnCommitSelectionChanged(enter: false, right: false, left: false, diff: true, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                countingPressingRight = 0;
                                ReadButtons.Type.enter = false;
                            }


                            if (ReadButtons.Type.enter == true)
                            {
                                countingPressingRight++;
                                retainPositionOfYWhenEnter = y;
                                clear.ClearCommitPanel();
                                y = Console.WindowHeight / 2 + 5;
                                retainPositionOfYWhenRight = y;
                                OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                ReadButtons.Type.enter = false;
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            if (ReadButtons.Type.rightOnce == true && ReadButtons.Type.diffMovements == false)
                            {
                                ReadButtons.Type.rightOnce = false;
                                clear.ClearCommitPanel();
                                y = retainPositionOfYWhenEnter;
                                countingPressingRight = 0;
                                fileIndex = 0;
                                fileNumber = 1;
                                filesStartingIndex = 0;
                                OnCommitSelectionChanged(enter: false, right: false, left: true, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                ReadButtons.Type.enter = true;
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        {
                            if (ReadButtons.Type.diffMovements == true)
                            {
                                ReadButtons.Type.diffMovements = false;
                                ReadButtons.Type.enter = true;
                                ReadButtons.Type.esc = true;
                                clear.ClearCommitPanel();
                                y = retainPositionOfYWhenRight;
                                diffIndex = 0;
                                diffNumber = 1;
                                diffStartingIndex = 0;
                                OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                                ReadButtons.Type.esc = false;
                                ReadButtons.Type.enter = false;
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

        private void CommitsDownMoves()
        {
            if (commitNumber < totalCommits)
            {
                if (y == Console.WindowHeight - 2 && commitEndIndex < totalCommits)
                {
                    commitStartingIndex++;
                    commitEndIndex++;

                    if (ReadButtons.Type.displayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearCommitFullWindow(1, y, Console.WindowWidth - 1, Console.WindowHeight);
                        Refresh();
                    }
                }
                else
                {
                    if (ReadButtons.Type.displayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearOneCommit(1, y + 1, Console.WindowWidth - 2);
                        DisplayOneCommit(width);
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

                if (y <= Console.WindowHeight - 2 && ReadButtons.Type.displayListOfCommitsOnEntirePanel == false)
                {
                    OnCommitSelectionChanged(enter: true, right: false, left: false, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
                else
                {
                    GetCommitNumber();
                    blueBox.SetBlueBox((1, y), currentCommits[commitIndex].Display(), width);
                    indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, 3, Console.WindowHeight - 1);
                }
            }
        }

        private void FilesDownMoves()
        {
            totalFiles = commitsService.GetAllFilesForCommit(commitNumber - 1).Count;
            currentFiles = commitsService.GetCurrentFiles(filesStartingIndex, commitNumber - 1);

            if (fileNumber < totalFiles)
            {

                if (y == Console.WindowHeight - 2)
                {
                    clear.ClearFiles(1, y, Console.WindowHeight / 2 + 5, Console.WindowWidth / 2 - 3, Console.WindowHeight - 2, "cleaningAllPanelArea");
                    clear.ClearDiff(y);
                    commitStartingIndex++;
                    filesStartingIndex++;
                    retainPositionOfYWhenRight = y;
                    OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
                else
                {
                    clear.ClearFiles(1, y, commitStartingIndex, Console.WindowWidth / 2 - 3, Console.WindowHeight - 1, "cleaningOneFile");
                    clear.ClearDiff(y);
                }

                if (y < Console.WindowHeight - 2)
                {
                    fileIndex++;
                    y++;
                    retainPositionOfYWhenRight = y;
                }

                if (fileNumber < totalFiles)
                {
                    fileNumber++;
                }

                if (fileNumber <= totalFiles)
                {
                    OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
            }
        }

        private void DiffDownMoves()
        {
            List<ChangeAttribute> files = commitsService.GetAllFilesForCommit(commitNumber - 1);
            currentDiff = commitsService.GetCurrentDiffForSelectedFile(commitNumber - 1, files[fileIndex].GetFileName());
            
            if (diffNumber < currentDiff.Count)
            {
                if (y == Console.WindowHeight - 2)
                {
                    clear.ClearDiff(y);
                    diffStartingIndex++;
                    diffIndex++;
                    OnCommitSelectionChanged(enter: false, right: false, left: false, diff: true, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
                else
                {
                    clear.ClearDiff(y);
                }

                if (y < Console.WindowHeight - 2)
                {
                    diffIndex++;
                    y++;
                }

                if (diffNumber < currentDiff.Count)
                {
                    diffNumber++;
                }

                if (diffNumber <= currentDiff.Count && y < Console.WindowHeight - 1)
                {
                    OnCommitSelectionChanged(enter: false, right: false, left: false, diff: true, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
            }
        }

        private void CommitsUpMoves()
        {
            if (commitNumber > 1)
            {
                if (y == 3 && commitStartingIndex > 0)
                {
                    commitStartingIndex--;
                    commitEndIndex--;

                    if (ReadButtons.Type.displayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearCommitFullWindow(1, y, Console.WindowWidth - 1, Console.WindowHeight);
                        Refresh();
                    }
                }
                else
                {
                    if (ReadButtons.Type.displayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearOneCommit(1, y - 1, Console.WindowWidth - 1);
                        DisplayOneCommit(width);
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

                if (y >= 3 && ReadButtons.Type.displayListOfCommitsOnEntirePanel == false)
                {
                    OnCommitSelectionChanged(enter: true, right: false, left: false, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
                else if (ReadButtons.Type.displayListOfCommitsOnEntirePanel == true)
                {
                    GetCommitNumber();
                    blueBox.SetBlueBox((1, y), currentCommits[commitIndex].Display(), width);
                    indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, y, Console.WindowHeight - 1);
                }
            }
        }

        private void FilesUpMoves()
        {
            totalFiles = commitsService.GetAllFilesForCommit(commitIndex).Count;
            currentFiles = commitsService.GetCurrentFiles(fileIndex, commitIndex);

            if (fileNumber > 1)
            {
                bool callOnCommitSelectionChanged = false;

                if (y == Console.WindowHeight / 2 + 5)
                {
                    clear.ClearFiles(1, y, Console.WindowHeight / 2 + 5, Console.WindowWidth / 2 - 2, Console.WindowHeight - 2, "cleaningAllPanelArea");
                    clear.ClearDiff(y);
                    commitStartingIndex--;
                    fileIndex = -1;
                    filesStartingIndex--;
                    OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                    callOnCommitSelectionChanged = true;
                    fileIndex = 0;
                }
                else
                {
                    clear.ClearFiles(1, y, commitStartingIndex, Console.WindowWidth / 2 - 2, Console.WindowHeight - 1, "cleaningOneFile");
                    clear.ClearDiff(y);
                }

                if (fileIndex > 0)
                {
                    y--;
                    fileIndex--;
                    retainPositionOfYWhenRight = y;
                }

                if (fileNumber <= totalFiles)
                {
                    fileNumber--;
                }

                if (callOnCommitSelectionChanged == false)
                {
                    OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
            }
        }

        private void DiffUpMoves()
        {
            List<ChangeAttribute> files = commitsService.GetAllFilesForCommit(commitNumber - 1);
            currentDiff = commitsService.GetCurrentDiffForSelectedFile(commitNumber - 1, files[fileIndex].GetFileName());

            if (diffNumber > 1)
            {
                bool callOnCommitSelectionChanged = false;

                if (y == 3)
                {
                    clear.ClearDiff(y);
                    diffStartingIndex--;
                    OnCommitSelectionChanged(enter: false, right: false, left: false, diff: true, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                    callOnCommitSelectionChanged = true;
                }
                else
                {
                    clear.ClearDiff(y);
                }

                if (diffIndex > 0)
                {
                    diffIndex--;
                }

                if (y > 3)
                {
                    y--;
                }

                if (diffNumber <= currentDiff.Count)
                {
                    diffNumber--;
                }

                if (callOnCommitSelectionChanged == false)
                {
                    OnCommitSelectionChanged(enter: false, right: false, left: false, diff: true, commitStartingIndex, commitEndIndex, commitIndex, fileIndex, y, commitNumber, fileNumber, filesStartingIndex, diffIndex, diffStartingIndex);
                }
            }
        }

        private void DisplayOneCommit(int width)
        {
            int height = Console.WindowHeight;
            string displayText = TextSettings.GetTextLength(currentCommits[commitIndex].Display(), width);
            Console.SetCursorPosition(1, y);
            TextSettings.SetColorLog(displayText);
        }

        private void DisplayCommits(int width)
        {
            int height = Console.WindowHeight;
            int stopAt = currentCommits.Count() > height ? height: currentCommits.Count();

            for (int i = 0; i < stopAt; i++)
            {
                int y = 3 + i;

                if (y < height)
                {
                    string displayText = TextSettings.GetTextLength(currentCommits[i].Display(), width);
                    Console.SetCursorPosition(1, y);
                    TextSettings.SetColorLog(displayText);
                }
            }
        }

        private void GetCommitNumber()
        {
            Console.SetCursorPosition(1, 2);
            Console.Write($"Commit: {commitNumber} / {totalCommits} ");
        }

        private void CloseApplication()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
