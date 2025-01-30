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
using static GitClient.DrawTabs;

namespace GitClient.ui
{
    public class CommitsPanel : UiComponent
    {
        public event EventHandler<CommitSelectionChangedEventArgs> CommitSelectionChanged;
        private CommitsLibGit2Repository libgit2Repository;
        private CommitsService commitsService;
        private CommitsWithDescription commitsWithDescription;
        private PanelCommunicationService panelCommunicationService;
        private  int startIndex;
        private  int endIndex;
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
        private int totalFiles;
        

        public CommitsPanel(CommitsLibGit2Repository libGit2Repository, CommitsService commitsService, PanelCommunicationService panelCommunicationService)
        {
            this.libgit2Repository = libGit2Repository;
            this.commitsService = commitsService;
            this.panelCommunicationService = panelCommunicationService;
            commitsWithDescription = new CommitsWithDescription(libgit2Repository, this.commitsService, panelCommunicationService);
            startIndex = GetStartIndex();
            endIndex = GetEndIndex();
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
            totalFiles = commitsService.GetAllFilesForCommit(commitNumber - 1).Count;
        }

        public void SetCommunicationService(PanelCommunicationService service)
        {
            panelCommunicationService = service;
        }

        protected virtual void OnCommitSelectionChanged(bool enter, bool right, bool left, bool diff, int startIndex, int endIndex, int currentIndex, int fileIndex, int y, int commitNumber)
        {
            CommitSelectionChanged.Invoke(this, new CommitSelectionChangedEventArgs(enter, right, left, diff, startIndex, endIndex, currentIndex, fileIndex, y, commitNumber));
        } 

        public int GetStartIndex()
        {
            return startIndex;
        }

        public int GetEndIndex()
        {
            return endIndex < Console.WindowHeight - 3 ? Console.WindowHeight - 3
                : startIndex + Console.WindowHeight - 3;
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
            ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel = true;
            //CommitLayouts.IsFullListOFCommits = true;
            panel.DrawBorderForFullSizeCommitList();
            commitsService.GetAllCommits();
            currentCommits = commitsService.GetCurrentListOfCommits(startIndex, endIndex);
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
                            ReadButtonsPressingOrActions.Type.down = true;
                            ReadButtonsPressingOrActions.Type.up = false;

                            if (ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == true || ReadButtonsPressingOrActions.Type.enter == true)
                            {
                                CommitsDownMoves();
                            }
                            else
                            {
                                if (y <= 4)
                                {
                                    y = Console.WindowHeight / 2 + 5;
                                }

                                FilesDownMoves();
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            ReadButtonsPressingOrActions.Type.down = false;
                            ReadButtonsPressingOrActions.Type.up = true;

                            if (ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == true || ReadButtonsPressingOrActions.Type.enter == true)
                            {
                                CommitsUpMoves();
                            }
                            else
                            {
                                //if (y <= 4)
                                //{
                                //    y = Console.WindowHeight / 2 + 5;
                                //}

                                FilesDownMoves();
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            ReadButtonsPressingOrActions.Type.enter = true;
                            enterCount++;
                           
                            if (enterCount == 1)
                            {
                                clear.ClearCommitPanel();
                                OnCommitSelectionChanged(enter: true, right: false, left: false, diff: false, startIndex, endIndex, commitIndex, fileIndex, y, commitNumber);
                                /* CommitLayouts.IsFullListOFCommits*/
                                ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel = false;
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
                            ReadButtonsPressingOrActions.Type.rightOnce = true;

                            if (ReadButtonsPressingOrActions.Type.enter == true)
                            {
                                clear.ClearCommitPanel();
                                OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, startIndex, endIndex, commitIndex, fileIndex, y, commitNumber);
                                ReadButtonsPressingOrActions.Type.enter = false;
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

        private void CommitsDownMoves()
        {
            if (commitNumber < totalCommits)
            {
                if (y == Console.WindowHeight - 2 && endIndex < totalCommits)
                {
                    startIndex++;
                    endIndex++;

                    if (ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearCommitFullWindow(1, y, Console.WindowWidth - 1, Console.WindowHeight);
                        Refresh();
                    }
                }
                else
                {
                    if (ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == true)
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

                if (y <= Console.WindowHeight - 2 && ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == false)
                {
                    OnCommitSelectionChanged(enter: true, right: false, left: false, diff: false, startIndex, endIndex, commitIndex, fileIndex, y, commitNumber);
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
            totalFiles = commitsService.GetAllFilesForCommit(commitIndex).Count;
            currentFiles = commitsService.GetCurrentFiles(startIndex, commitIndex);

            if (fileNumber < totalFiles)
            {
                if (y == Console.WindowHeight - 2)
                {
                    clear.ClearFiles(1, y, Console.WindowHeight / 2 + 5, Console.WindowWidth / 2 - 4, Console.WindowHeight - 2, "cleaningAllPanelArea");
                    clear.ClearDiff(y);
                    startIndex++;
                    OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, startIndex, endIndex, commitIndex, fileIndex, y, fileNumber);
                }
                else
                {
                    clear.ClearFiles(1, y, startIndex, Console.WindowWidth / 2 - 4, Console.WindowHeight - 1, "cleaningOneFile");
                    clear.ClearDiff(y);
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

                if (fileNumber <= currentFiles.Count)
                {
                    OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, startIndex, endIndex, commitIndex, fileIndex, y, fileNumber);
                }
            }
        }

        private void CommitsUpMoves()
        {
            if (commitNumber > 1)
            {
                if (y == 3 && startIndex > 0)
                {
                    startIndex--;
                    endIndex--;

                    if (ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == true)
                    {
                        clear.ClearCommitFullWindow(1, y, Console.WindowWidth - 1, Console.WindowHeight);
                        Refresh();
                    }
                }
                else
                {
                    if (ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == true)
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

                if (y >= 3 && ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == false)
                {
                    OnCommitSelectionChanged(enter: true, right: false, left: false, diff: false, startIndex, endIndex, commitIndex, fileIndex, y, commitNumber);
                }
                else if (ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == true)
                {
                    GetCommitNumber();
                    blueBox.SetBlueBox((1, y), currentCommits[commitIndex].Display(), width);
                    indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, y, Console.WindowHeight - 1);
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
