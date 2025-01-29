using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private  int currentIndex;
        private List<CommitsElements> currentCommits;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private int totalCommits;
        private int y = 3;
        private int commitNumber;
        private int width = Console.WindowWidth - 3;
        private int enterCount = 0;
        

        public CommitsPanel(CommitsLibGit2Repository libGit2Repository, CommitsService commitsService, PanelCommunicationService panelCommunicationService)
        {
            this.libgit2Repository = libGit2Repository;
            this.commitsService = commitsService;
            this.panelCommunicationService = panelCommunicationService;
            commitsWithDescription = new CommitsWithDescription(libgit2Repository, this.commitsService, panelCommunicationService);
            startIndex = GetStartIndex();
            endIndex = GetEndIndex();
            currentIndex = CurrentIndex();
            currentCommits = new List<CommitsElements>();
            blueBox = new BlueBox();
            indicator = new Indicator();
            panel = new DrawPanels();
            totalCommits = this.commitsService.GetAllCommits().Count;
            commitNumber = 1;
        }

        public void SetCommunicationService(PanelCommunicationService service)
        {
            panelCommunicationService = service;
        }

        protected virtual void OnCommitSelectionChanged(bool enter, bool right, bool left, bool diff, int startIndex, int endIndex, int currentIndex, int y, int commitNumber)
        {
            CommitSelectionChanged.Invoke(this, new CommitSelectionChangedEventArgs(enter, right, left, diff, startIndex, endIndex, currentIndex, y, commitNumber));
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

        public int CurrentIndex()
        {
            return currentIndex;
        }

        public override void Show()
        {
            Refresh();
            Navigate();
        }

        public void Refresh() 
        {
            CommitLayouts.IsFullListOFCommits = true;
            panel.DrawBorderForFullSizeCommitList();
            commitsService.GetAllCommits();
            currentCommits = commitsService.GetCurrentListOfCommits(startIndex, endIndex);
            DisplayCommits(width);
            indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, y, Console.WindowHeight - 2);
            blueBox.SetBlueBox((1, y), currentCommits[currentIndex].Display(), Console.WindowWidth - 3);
            GetCommitNumber();
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
                            if (commitNumber < totalCommits)
                            {
                                ReadButtonsPressing.Type.down = true;
                                ReadButtonsPressing.Type.up = false;

                                if (y == Console.WindowHeight - 2 && endIndex < totalCommits)
                                {
                                    startIndex++;
                                    endIndex++;

                                    if (CommitLayouts.IsFullListOFCommits == true)
                                    {
                                        clear.ClearCommitFullWindow(1, y, Console.WindowWidth - 1, Console.WindowHeight);
                                        Refresh();
                                    }
                                }
                                else
                                {
                                    if (CommitLayouts.IsFullListOFCommits == true)
                                    {
                                        clear.ClearOneCommit(1, y + 1, Console.WindowWidth - 2);
                                        DisplayOneCommit(width);
                                    }
                                }

                                if (y < Console.WindowHeight - 2)
                                {
                                    currentIndex++;
                                    y++;
                                }

                                if (commitNumber < totalCommits)
                                {
                                    commitNumber++;
                                }



                                if (y <= Console.WindowHeight - 2 && CommitLayouts.IsFullListOFCommits == false)
                                {
                                    OnCommitSelectionChanged(enter: true, right: false, left: false, diff: false, startIndex, endIndex, currentIndex, y, commitNumber);
                                }
                                else
                                {
                                    GetCommitNumber();
                                    blueBox.SetBlueBox((1, y), currentCommits[currentIndex].Display(), width);
                                    indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, 3, Console.WindowHeight - 1);
                                }
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            if (commitNumber > 1)
                            {
                                ReadButtonsPressing.Type.down = false;
                                ReadButtonsPressing.Type.up = true;

                                if (y == 3 && startIndex > 0)
                                {
                                    startIndex--;
                                    endIndex--;

                                    if (CommitLayouts.IsFullListOFCommits == true)
                                    {
                                        clear.ClearCommitFullWindow(1, y, Console.WindowWidth - 1, Console.WindowHeight);
                                        Refresh();
                                    }
                                }
                                else
                                {
                                    if (CommitLayouts.IsFullListOFCommits == true)
                                    {
                                        clear.ClearOneCommit(1, y - 1, Console.WindowWidth - 1);
                                        DisplayOneCommit(width);
                                    }
                                }

                                if (y > 3)
                                {
                                    currentIndex--;
                                    y--;
                                }

                                if (commitNumber > 1)
                                {
                                    commitNumber--;
                                }

                                if (y >= 3 && CommitLayouts.IsFullListOFCommits == false)
                                {
                                    OnCommitSelectionChanged(enter: true, right: false, left: false, diff: false, startIndex, endIndex, currentIndex, y, commitNumber);
                                }
                                else if (CommitLayouts.IsFullListOFCommits == true)
                                {
                                    GetCommitNumber();
                                    blueBox.SetBlueBox((1, y), currentCommits[currentIndex].Display(), width);
                                    indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, y, Console.WindowHeight - 1);
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            ReadButtonsPressing.Type.enter = true;
                            enterCount++;
                           
                            if (enterCount == 1)
                            {
                                clear.ClearCommitPanel();
                                OnCommitSelectionChanged(enter: true, right: false, left: false, diff: false, startIndex, endIndex, currentIndex, y, commitNumber);
                                CommitLayouts.IsFullListOFCommits = false;
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
                            if (ReadButtonsPressing.Type.enter == true)
                            {
                                clear.ClearCommitPanel();
                                OnCommitSelectionChanged(enter: false, right: true, left: false, diff: false, startIndex, endIndex, currentIndex, y, commitNumber);
                                ReadButtonsPressing.Type.enter = false;
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

        private void DisplayOneCommit(int width)
        {
            int height = Console.WindowHeight;
            string displayText = TextSettings.GetTextLength(currentCommits[currentIndex].Display(), width);
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
