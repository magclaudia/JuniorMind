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
        public CommitsPanel()
        {
            libgit2Repository = new CommitsLisLibGit2Repository();
            commitsListService = new CommitsListService(libgit2Repository);
            startIndex = GetStartIndex();
            endIndex = GetEndIndex();
            currentIndex = CurrentIndex();
            currentCommits = new List<CommitsElements>();
        }

        public int GetStartIndex()
        {
            return startIndex;
        }

        public int GetEndIndex()
        {
            return endIndex = Console.WindowHeight - 3;
        }

        public int CurrentIndex()
        {
            return currentIndex;
        }

        public override void Show()
        {
            Refresh();
        }

        public void Refresh() 
        {
            commitsListService.GetAllCommits();
            DrawBorderForFullSizeCommitList();
            
            if (CommitLayouts.IsFullListOFCommits == true)
            {
                currentCommits = commitsListService.GetCurrentListOfCommits(startIndex, endIndex);
                DisplayCommits();
            }
        }

        private void DisplayCommits()
        {
            int width = Console.WindowWidth - 4;
            int height = Console.WindowHeight ;
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
    }
}
