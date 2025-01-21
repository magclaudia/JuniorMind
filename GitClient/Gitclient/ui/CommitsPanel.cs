using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gitclient.model;
using GitClient.model;
using GitClient.repository;
using GitClient.service;
using static GitClient.DrawTabs;

namespace GitClient.ui
{
    public class CommitsPanel : UiComponent
    {
        private CommitsLisLibGit2Repository libgit2Repository;
        private CommitsListService commitsListService;
        private int startIndex;
        private int endIndex;
        private int currentIndex;
        private List<CommitsElements> currentCommits;
        private BlueBox blueBox;
        private Indicator indicator;
        private int totalCommits;
        private int y = 3;
        private int commitNumber;


        public CommitsPanel()
        {
            libgit2Repository = new CommitsLisLibGit2Repository();
            commitsListService = new CommitsListService(libgit2Repository);
            startIndex = GetStartIndex();
            endIndex = GetEndIndex();
            currentIndex = CurrentIndex();
            currentCommits = new List<CommitsElements>();
            blueBox = new BlueBox();
            indicator = new Indicator();
            totalCommits = commitsListService.GetAllCommits().Count;
            commitNumber = 1;
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
            DrawBorderForFullSizeCommitList();
            Refresh();
            Navigate();
        }

        public void Refresh() 
        {
            commitsListService.GetAllCommits();
            
            if (CommitLayouts.IsFullListOFCommits == true)
            {
                currentCommits = commitsListService.GetCurrentListOfCommits(startIndex, endIndex);
                DisplayCommitsOnEntireWindow();
                indicator.GetIndicator(currentIndex, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, y, Console.WindowHeight - 2);
                blueBox.SetBlueBox((1, y), currentCommits[currentIndex].Display(), Console.WindowWidth - 3);
                GetCommitNumber();
            }
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
                                ButtomPress.Type.down = true;
                                ButtomPress.Type.up = false;

                                if (y == Console.WindowHeight - 2 && endIndex < totalCommits)
                                {
                                    clear.ClearCommitFullWindow(1, y, Console.WindowWidth - 2, Console.WindowHeight - 1);
                                    startIndex++;
                                    endIndex++;
                                    Refresh();
                                }
                                else
                                {
                                    clear.ClearOneCommit(1, y, Console.WindowWidth - 2);
                                    DisplayOneCommit();
                                }

                                if (y < Console.WindowHeight - 2)
                                {
                                    currentIndex++;
                                    y++;
                                }

                                if (commitNumber < totalCommits && commitNumber > 0)
                                {
                                    commitNumber++;
                                }

                                GetCommitNumber();
                                blueBox.SetBlueBox((1, y), currentCommits[currentIndex].Display(), Console.WindowWidth - 3);
                                indicator.GetIndicator(currentIndex, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, y, Console.WindowHeight - 2);
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            if (commitNumber > 1)
                            {
                                ButtomPress.Type.down = false;
                                ButtomPress.Type.up = true;

                                if (y == 3 && startIndex > 0)
                                {
                                    startIndex--;
                                    clear.ClearCommitFullWindow(1, y, Console.WindowWidth - 2, Console.WindowHeight - 1);
                                    Refresh();
                                }
                                else
                                {
                                    clear.ClearOneCommit(1, y, Console.WindowWidth - 2);
                                    DisplayOneCommit();
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

                                GetCommitNumber();
                                blueBox.SetBlueBox((1, y), currentCommits[currentIndex].Display(), Console.WindowWidth - 3);
                                indicator.GetIndicator(currentIndex, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, y, Console.WindowHeight - 1);
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
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

        private void DisplayOneCommit()
        {
            int width = Console.WindowWidth - 3;
            int height = Console.WindowHeight;
            string displayText = TextSettings.GetTextLength(currentCommits[currentIndex].Display(), width);
            Console.SetCursorPosition(1, y);
            TextSettings.SetColorLog(displayText);
        }

        private void DisplayCommitsOnEntireWindow()
        {
            int width = Console.WindowWidth - 3;
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

        private void DrawBorderForFullSizeCommitList()
        {
            for (int i = 3; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, 2);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, 2);
            Console.Write("┌");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, 2);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("┘");
        }

        private void GetCommitNumber()
        {
            Console.SetCursorPosition(1, 2);
            Console.Write($"Commit: {commitNumber}/{totalCommits}");
        }

        private void CloseApplication()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
