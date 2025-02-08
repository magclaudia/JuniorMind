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
    public class LogPanel /*: UiComponent*/
    {
        private CommitsService commitsService;
        private PanelCommunicationService panelCommunicationService;
        private List<Commit> currentCommits;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private int commitStartingIndex;
        private int commitEndIndex;
        private int commitIndex;
        private int totalCommits;
        private int y;
        private int commitNumber;
        private int width = Console.WindowWidth - 3;

        public LogPanel(CommitsService commitsService, PanelCommunicationService panelCommunicationService)
        {
            this.commitsService = commitsService;
            this.panelCommunicationService = panelCommunicationService;
            commitStartingIndex = 0;
            commitEndIndex = GetEndIndex();
            commitIndex = 0;
            y = 3;
            currentCommits = new List<Commit>();
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

        public void SubscribeToPanel(CommitsNavigation commitsNavigation)
        {
            commitsNavigation.CommitSelectionChanged += HandleCommitsSelectionChanged!;
        }

        private void HandleCommitsSelectionChanged(object sender, CommitSelectionChangedEventArgs e)
        {
            ButtonStates buttonStates = e.ButtonStates;
            
            if (buttonStates.GitLog == true && buttonStates.Enter == false && buttonStates.Down == false && buttonStates.Up == false)
            {
                Refresh(e);
            }
            else if (buttonStates.Enter == true && buttonStates.GitLog == true)
            {
                Refresh(e);
            }
            else if (buttonStates.Down == true && buttonStates.GitLog == true || buttonStates.Up == true && buttonStates.GitLog == true)
            {
                if (e.Y < Console.WindowHeight - 3)
                {
                    DisplayOneCommit(width, e);
                }
                else
                {
                    DisplayCommits(width, e);
                }
            }
        }


        //public override void Show()
        //{
        //    Refresh();
        //}

        public void Refresh(CommitSelectionChangedEventArgs e) 
        {
            ReadButtons.DisplayListOfCommitsOnEntirePanel = true;
            ReadButtons.RightStatus = false;
            panel.DrawBorderForFullSizeCommitList();
            commitsService.GetAllCommits();
            currentCommits = commitsService.GetCurrentListOfCommits(e.StartIndex, e.EndIndex);
            DisplayCommits(width, e);
            indicator.GetIndicator(e.CommitNumber, Console.WindowHeight - 2, totalCommits, Console.WindowWidth - 1, e.Y, Console.WindowHeight - 2);
            blueBox.SetBlueBox((1, e.Y), currentCommits[e.CommitIndex].Display(), Console.WindowWidth - 3);
            GetCommitNumber(e);
        }

        private int GetEndIndex()
        {
            return commitEndIndex < Console.WindowHeight - 3 ? Console.WindowHeight - 3
                : commitStartingIndex + Console.WindowHeight - 3;
        }


        private void DisplayOneCommit(int width, CommitSelectionChangedEventArgs e)
        {
            int height = Console.WindowHeight;
            string displayText = TextSettings.GetTextLength(currentCommits[e.CommitIndex].Display(), width);
            Console.SetCursorPosition(1, e.Y);
            TextSettings.SetColorLog(displayText);
        }

        private void DisplayCommits(int width, CommitSelectionChangedEventArgs e)
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

        private void GetCommitNumber(CommitSelectionChangedEventArgs e)
        {
            Console.SetCursorPosition(1, 2);
            Console.Write($"Commit: {e.CommitNumber} / {totalCommits} ");
        }
    }
}
