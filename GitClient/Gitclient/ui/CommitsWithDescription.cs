using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;
using GitClient.repository;
using GitClient.service;
using LibGit2Sharp;
using static GitClient.DrawTabs;

namespace GitClient.ui
{
    public class CommitsWithDescription
    {
        private CommitsLibGit2Repository libgit2Repository;
        private CommitsService commitsService;
        //private PanelCommunicationService panelCommunicationService;
        private int currentIndex;
        private List<CommitsElements> currentCommits;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private int totalCommits;
        private int y = 3;
        private int commitNumber;
        private int fileNumber;
        private int filesNumber;
        private List<ChangeAttribute> listOfFiles;
        private List<FileDiff> listOfDiffsForFiles;
        private ClearConsoleChoosenSpace clear;
        private int width;

        public CommitsWithDescription(CommitsLibGit2Repository libgit2Repository, CommitsService commitsService, PanelCommunicationService panelCommunicationService) 
        {
            this.libgit2Repository = libgit2Repository;
            this.commitsService = commitsService;
            //this.panelCommunicationService = panelCommunicationService;
            currentCommits = new List<CommitsElements>();
            blueBox = new BlueBox();
            indicator = new Indicator();
            panel = new DrawPanels();
            totalCommits = commitsService.GetAllCommits().Count;
            commitNumber = 1;
            fileNumber = 1;
            filesNumber = commitsService.GetAllFilesForCommit(commitNumber - 1).Count;
            listOfFiles = commitsService.GetAllFilesForCommit(commitNumber - 1);
            listOfDiffsForFiles = commitsService.GetAllLogDiff();
            clear = new ClearConsoleChoosenSpace();
            width = 0;
        }


        public void SubscribeToPanel(CommitsPanel commitsPanel)
        {
            commitsPanel.CommitSelectionChanged += HandleCommitsSelectionChanged!;
        }
        private void HandleCommitsSelectionChanged(object sender, CommitSelectionChangedEventArgs e)
        {
            currentCommits = commitsService.GetCurrentListOfCommits(e.StartIndex, e.EndIndex);
            y = e.Y;
            currentIndex = e.CurrentIndex;
            commitNumber = e.CommitNumber;

            if (e.Enter == true)
            {
                HandlePressingEnterButtom(e);
            }
            else if (e.Right == true)
            {
                HandlePressingRightButtomOnce(e);
            }
           
        }

        private void HandlePressingEnterButtom(CommitSelectionChangedEventArgs e)
        {
            width = Console.WindowWidth / 2 + 5;
            int x = Console.WindowWidth / 2 + 10;

            if (CommitLayouts.IsFullListOFCommits == true)
            {
                panel.DrawBorderForCommitsAfterPressingEnter();
                DisplayCommits();
            }
            else
            {
                if (y == Console.WindowHeight - 2 || y == 3)
                {
                    clear.ClearCommitWithDescription(1, width, Console.WindowHeight - 1);
                    DisplayCommits();
                }
                else
                {
                    clear.ClearOneCommit(1, y, width);
                    DisplayOneCommit();
                }

                clear.ClearInfoPanel();
                clear.ClearMessagePanel();
                clear.ClearFilesPanelForCommits();
            }

            indicator.GetIndicator(commitNumber, Console.WindowHeight - 2, totalCommits, width +  2, 3, Console.WindowHeight - 1);
            blueBox.SetBlueBox((1, y), currentCommits[currentIndex].Display(), width);
            GetInfo(x, Console.WindowWidth - (Console.WindowWidth / 2 + 11), Console.WindowHeight / 4);
            GetMessage(x, Console.WindowWidth / 2 - 11, Console.WindowHeight / 2 - 1);
            GetCommitNumber();
            GetPath(x, Console.WindowHeight / 2 + 5);
            GetFiles(x, Console.WindowWidth - (Console.WindowWidth / 2 + 11), Console.WindowHeight - 1, Console.WindowHeight / 2 + 6);
            GetFilesNumber(x, Console.WindowHeight / 2 + 4);
        }

        private void HandlePressingRightButtomOnce(CommitSelectionChangedEventArgs e)
        {
            int x = 1;
            width = Console.WindowWidth / 2 - 2;
            y = Console.WindowHeight / 2 + 5;

            if (ReadButtonsPressing.Type.enter == true)
            {
                panel.DrawBorderForCommitsAfterPressingRightOnce();
                GetInfo(x, width, Console.WindowHeight / 3 - 1);
                GetMessage(x, width, Console.WindowHeight / 2 + 1);
                GetPath(x, Console.WindowHeight / 2 + 4);
                GetFiles(x, width, Console.WindowHeight - 1, Console.WindowHeight / 2 + 5);
                GetFilesNumber(x, Console.WindowHeight / 2 + 3);
                int height = Console.WindowHeight - (Console.WindowHeight / 2 + 4);
                indicator.GetIndicator(fileNumber, Console.WindowHeight - height, listOfFiles.Count, width, y, Console.WindowHeight - 1);
                blueBox.SetBlueBox((1, y), listOfFiles[fileNumber - 1].Display(), width - 2);
                GetFileDiff();
            }
        }
        private void DisplayOneCommit()
        {
            (int index, int yNewPosition) = ReadButtonsPressing.Type.down ? (currentIndex - 1, y - 1) : (currentIndex + 1, y + 1);
            Console.SetCursorPosition(1, yNewPosition);
            int height = Console.WindowHeight;
            string displayText = TextSettings.GetTextLength(currentCommits[index].Display(), width);
            TextSettings.SetColorLog(displayText);
        }
        private void DisplayCommits()
        {
            int height = Console.WindowHeight;

            int stopAt = currentCommits.Count() > height ? height : currentCommits.Count();

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
        private void GetFiles(int x, int width, int height, int startFrom)
        {
            listOfFiles = commitsService.GetAllFilesForCommit(commitNumber - 1);
            filesNumber = listOfFiles.Count;
            int stopAt = filesNumber > height ? height : filesNumber;

            for (int i = 0; i < stopAt; i++)
            {
                int y = startFrom + i;

                if (y < height)
                {
                    string displayText = TextSettings.GetTextLength(listOfFiles[i].Display(), width);
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
                    Console.Write(displayText);
                    Console.ResetColor();
                }
            }
        }
        private void GetInfo(int x, int width, int height)
        {
            //int width = Console.WindowWidth - (Console.WindowWidth / 2 + 11);
            //int height = Console.WindowHeight / 4;
            Dictionary<string, string> display = new Dictionary<string, string>
            {
                { "Author: ", currentCommits[currentIndex].Author },
                { "Date/Time: ", currentCommits[currentIndex].DateAndTime },
                { "sha: ", currentCommits[currentIndex].Id }
            };

            for (int i = 0; i < 3; i++)
            {
                int y = 3 + i;

                if (y < height)
                {
                    Console.SetCursorPosition(x, y);
                    string text = display.ElementAt(i).Key + display.ElementAt(i).Value;
                    string displayText = TextSettings.GetTextLength($"{text}", width);
                    Console.Write(displayText);
                }
            }
        }
        private void GetMessage(int x, int width, int height)
        {
            int y = Console.WindowHeight / 4 + 3;
            int index = 0;

            if (currentCommits[currentIndex].Message.Length >= width)
            {
                for (int i = 0; i < height; i++)
                {
                    if (index == currentCommits[currentIndex].Message.Length)
                    {
                        break;
                    }


                    if (currentCommits[currentIndex].Message.Length - index < width)
                    {
                        width = currentCommits[currentIndex].Message.Length - index;
                    }

                    Console.SetCursorPosition(x, y);
                    string displayText = currentCommits[currentIndex].Message.Substring(index, width);
                    Console.Write(displayText.TrimStart());
                    index += width;
                    y++;
                }
            }
            else
            {
                Console.SetCursorPosition(x, y);
                Console.Write(currentCommits[currentIndex].Message);
            }
        }
        private void GetCommitNumber()
        {
            Console.SetCursorPosition(1, 2);
            Console.Write($"Commit: {commitNumber} / {totalCommits} ");
        }
        private void GetFilesNumber(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(new string(' ', $"Files: {listOfFiles.Count} ".Length));
            Console.SetCursorPosition(x, y);
            Console.Write($"Files: {listOfFiles.Count} ");
        }
        private void GetPath(int x, int height)
        {
            GetProjectPath projectPath = new GetProjectPath();
            string path = $"  ▾{projectPath.ProjectPath(Environment.CurrentDirectory)}";
            path = TextSettings.GetTextLength(path, Console.WindowWidth - (Console.WindowWidth / 2 + 11));
            Console.SetCursorPosition(x, height);
            Console.Write(path);
            Console.ResetColor();
        }
        private void GetFileDiff()
        {
            width = Console.WindowWidth / 2 + 1;
            int height = listOfDiffsForFiles[commitNumber - 1].diffs.Count > Console.WindowHeight - 2 ? Console.WindowHeight - 1 : listOfDiffsForFiles[commitNumber - 1].diffs.Count;
            List<FileDiff> currentDiff = commitsService.GetCurrentDiffForSelectedFile(fileNumber - 1, listOfFiles[fileNumber - 1].GetFileName());

            for (int i = 0; i < height; i++)
            {
                int y = 3 + i;

                if (y < height)
                {
                    string displayText = TextSettings.GetTextLength(currentDiff[fileNumber - 1].diffs[i], width - 4);
                    Console.SetCursorPosition(width, y);

                    if (displayText == "")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = i == 0 ? ConsoleColor.Gray : TextSettings.SetColorStatus(displayText[0]);
                    }

                    Console.Write(displayText);
                    Console.ResetColor();
                }
            }

        }
    }
}
