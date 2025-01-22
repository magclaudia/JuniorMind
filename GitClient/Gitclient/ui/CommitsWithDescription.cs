using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;
using GitClient.repository;
using GitClient.service;
using static GitClient.DrawTabs;

namespace GitClient.ui
{
    public class CommitsWithDescription
    {
        private CommitsLibGit2Repository libgit2Repository;
        private CommitsService commitsListService;
        private int startIndex;
        private int endIndex;
        private int currentIndex;
        private List<CommitsElements> currentCommits;
        private BlueBox blueBox;
        private Indicator indicator;
        private int totalCommits;
        private int y = 3;
        private int commitNumber;
        private int filesNumber;
        private List<ChangeAttribute> listOfFiles;


        public CommitsWithDescription(CommitsLibGit2Repository libgit2Repository, CommitsService commitsService) 
        {
            this.libgit2Repository = libgit2Repository;
            this.commitsListService = commitsService;
            currentCommits = new List<CommitsElements>();
            startIndex = GetStartIndex();
            endIndex = GetEndIndex();
            currentIndex = CurrentIndex();
            currentCommits = new List<CommitsElements>();
            blueBox = new BlueBox();
            indicator = new Indicator();
            totalCommits = commitsService.GetAllCommits().Count;
            commitNumber = 1;
            filesNumber = commitsService.GetAllFilesForCommit(commitNumber - 1).Count;
            listOfFiles = commitsService.GetAllFilesForCommit(commitNumber - 1);
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

        public void Show(int start, int end, int index, int starty, int commitNr)
        {
            CommitLayouts.IsFullListOFCommits = false;
            DrawBorderForCommitListWithDescription();
            startIndex = start;
            endIndex = end;
            currentIndex = index;
            y = starty;
            commitNumber = commitNr;
            Refresh();
            //Navigate();
        }

        public void Refresh()
        {
            commitsListService.GetAllCommits();
            currentCommits = commitsListService.GetCurrentListOfCommits(startIndex, endIndex);
            DisplayCommits();
            indicator.GetIndicator(currentIndex, Console.WindowHeight - 2, totalCommits, Console.WindowWidth / 2 + 7, y, Console.WindowHeight - 2);
            blueBox.SetBlueBox((1, y), currentCommits[currentIndex].Display(), Console.WindowWidth / 2 + 5);
            GetInfo();
            GetMessage();
            
            GetCommitNumber();
            GetFilesNumber();
            DisplayFiles();
        }

        private void DisplayCommits()
        {
            int width = Console.WindowWidth / 2 + 5;
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

        private void DisplayFiles()
        {
            int width = Console.WindowWidth / 3 + 1;
            int height = Console.WindowHeight - 1;
            int stopAt = filesNumber > height ? height : filesNumber;
            GetPath();

            for (int i = 0; i < stopAt; i++)
            {
                int y = Console.WindowHeight / 2 + 6 + i;

                if (y < height)
                {
                    string displayText = TextSettings.GetTextLength(listOfFiles[i].Display(), width);
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 10, y);
                    Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
                    Console.Write(displayText);
                    Console.ResetColor();
                }
            }
        }

        private void GetInfo()
        {
            int width = Console.WindowWidth / 3 + 1;
            int height = Console.WindowHeight / 4;
            Dictionary<string, string> display = new Dictionary<string, string>();
            display.Add("Author: ", currentCommits[currentIndex].Author);
            display.Add("Date/Time: ", currentCommits[currentIndex].DateAndTime);
            display.Add("sha: ", currentCommits[currentIndex].DateAndTime);

            for (int i = 0; i < 3; i++)
            {
                int y = 3 + i;

                if (y < height)
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 + 10, y);
                    string text = display.ElementAt(i).Key + display.ElementAt(i).Value;
                    string displayText = TextSettings.GetTextLength($"{text}", width);
                    Console.Write(displayText);
                }
            }
        }
        private void GetMessage()
        {
            int width = Console.WindowWidth / 3 + 1;
            int height = Console.WindowHeight / 2 + 1;
            int y = Console.WindowHeight / 4 + 3;

            if (currentCommits[currentIndex].Message.Length > width)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 10, y);
                int index = 0;
                string displayText = currentCommits[currentIndex].Message.Substring(index, width);
                Console.Write(displayText);
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 10, y);
                Console.Write(currentCommits[currentIndex].Message);
            }
        }
        private void GetCommitNumber()
        {
            Console.SetCursorPosition(1, 2);
            Console.Write($"Commit: {commitNumber}/{totalCommits}");
        }
        private void GetFilesNumber()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, Console.WindowHeight / 2 + 4);
            Console.Write($"Files: {filesNumber} ");
        }
        private void GetPath()
        {
            GetProjectPath projectPath = new GetProjectPath();
            string path = $"  ▾{projectPath.ProjectPath(Environment.CurrentDirectory)}";
            path = TextSettings.GetTextLength(path, Console.WindowWidth / 3 + 1);
            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, Console.WindowHeight / 2 + 5);
            Console.Write(path);
            Console.ResetColor();
        }
        private void DrawBorderForCommitListWithDescription()
        {
            for (int i = 3; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth / 2 + 7, i);
                Console.Write("║");
            }

            for (int i = 1; i < Console.WindowWidth / 2 + 7; i++)
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
            Console.SetCursorPosition(Console.WindowWidth / 2 + 7, 2);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 7, Console.WindowHeight - 1);
            Console.Write("┘");

            for (int i = 3; i < Console.WindowHeight / 4; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 9, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            for (int i = Console.WindowWidth / 2 + 10; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, 2);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight / 4);
                Console.Write("─");
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, 2);
            Console.Write("Info ");

            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, 2);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight / 4);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, 2);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 4);
            Console.Write("┘");

            for (int i = Console.WindowHeight / 4 + 3; i < Console.WindowHeight / 2 + 2; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 9, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            for (int i = Console.WindowWidth / 2 + 10; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, Console.WindowHeight / 4 + 2);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight / 2 + 2);
                Console.Write("─");
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 10, Console.WindowHeight / 4 + 2);
            Console.Write("Message ");

            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight / 4 + 2);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight / 2 + 2);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 4 + 2);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 2 + 2);
            Console.Write("┘");

            for (int i = Console.WindowHeight / 2 + 5; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 9, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            for (int i = Console.WindowWidth / 2 + 10; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, Console.WindowHeight / 2 + 4);
                Console.Write("─");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight / 2 + 4);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth / 2 + 9, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight / 2 + 4);
            Console.Write("┐");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("┘");
        }
    }
}
