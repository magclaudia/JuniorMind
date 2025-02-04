using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;
using GitClient.repository;
using GitClient.service;
using LibGit2Sharp;

namespace GitClient.ui
{
    public class CommitsWithDescription
    {
        private CommitsLibGit2Repository libgit2Repository;
        private CommitsService commitsService;
        private int commitIndex;
        private int fileIndex;
        private List<CommitsElements> currentCommits;
        private List<ChangeAttribute> currentFiles;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private int totalCommits;
        private int y = 3;
        private int commitNumber;
        private int fileNumber;
        private int filesStartingIndex;
        private List<ChangeAttribute> listOfFiles;
        private ClearConsoleChoosenSpace clear;
        private int width;
        private int diffIndex;

        public CommitsWithDescription(CommitsLibGit2Repository libgit2Repository, CommitsService commitsService, PanelCommunicationService panelCommunicationService) 
        {
            this.libgit2Repository = libgit2Repository;
            this.commitsService = commitsService;
            currentCommits = new List<CommitsElements>();
            currentFiles = new List<ChangeAttribute>();
            blueBox = new BlueBox();
            indicator = new Indicator();
            panel = new DrawPanels();
            totalCommits = commitsService.GetAllCommits().Count;
            commitNumber = 1;
            listOfFiles = new List<ChangeAttribute>();
            clear = new ClearConsoleChoosenSpace();
            width = 0;
            diffIndex = 0;
        }


        public void SubscribeToPanel(CommitsPanel commitsPanel)
        {
            commitsPanel.CommitSelectionChanged += HandleCommitsSelectionChanged!;
        }
        private void HandleCommitsSelectionChanged(object sender, CommitSelectionChangedEventArgs e)
        {
            y = e.Y; 
            commitIndex = e.CurrentIndex;
            fileIndex = e.FileIndex;
            commitNumber = e.CommitNumber;
            filesStartingIndex = e.FilesStartingIndex;
            listOfFiles = commitsService.GetAllFilesForCommit(commitNumber - 1);
            currentCommits = commitsService.GetCurrentListOfCommits(e.StartIndex, e.EndIndex);

            if (e.Enter == true)
            {
                currentFiles = commitsService.GetCurrentFiles(0, commitNumber - 1);
                HandlePressingEnterButtom(e);
            }
            else if (e.Right == true)
            {
                fileNumber = e.FileNumber;
                currentFiles = commitsService.GetCurrentFiles(e.FilesStartingIndex, commitNumber - 1);
                HandlePressingRightButtonOnce(e);
            }
            else if (e.Left == true)
            {
                fileNumber = e.FileNumber;
                currentFiles = commitsService.GetCurrentFiles(0, commitNumber - 1);
                HandlePressingLeftButton(e);
            }
            else if (e.Diff == true)
            {
                HandleDisplayingDiff(e);
            }
        }

        private void HandlePressingEnterButtom(CommitSelectionChangedEventArgs e)
        {
            width = Console.WindowWidth / 2 + 5;
            int x = Console.WindowWidth / 2 + 10;

            if (ReadButtons.Type.displayListOfCommitsOnEntirePanel == true)
            {
                panel.DrawBorderForCommitsAfterPressingEnter();
                DisplayCommits();
            }
            else
            {
                if (y == Console.WindowHeight - 2 || y == 3 || e.Left == true)
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
            blueBox.SetBlueBox((1, y), currentCommits[commitIndex].Display(), width);
            GetInfo(x, Console.WindowWidth - (Console.WindowWidth / 2 + 11), Console.WindowHeight / 4);
            GetMessage(x, Console.WindowWidth / 2 - 11, Console.WindowHeight / 2 - 1);
            GetCommitNumber();
            GetPath(x, Console.WindowHeight / 2 + 5);
            GetFiles(x, Console.WindowWidth - (Console.WindowWidth / 2 + 11), Console.WindowHeight - 1, Console.WindowHeight / 2 + 6, e.StartIndex, y);
            GetFilesNumber(x, Console.WindowHeight / 2 + 4);
        }

        private void HandlePressingRightButtonOnce(CommitSelectionChangedEventArgs e)
        {
            int x = 1;
            width = (Console.WindowWidth / 2) - 2;
            int height = Console.WindowHeight - (Console.WindowHeight / 2 + 4);

            if (ReadButtons.Type.enter == true)
            {
                y = Console.WindowHeight / 2 + 5;
                panel.DrawBorderForCommitsAfterPressingRightOnce();
                GetInfo(x, width, Console.WindowHeight / 3 - 3);
                GetMessage(x, width, Console.WindowHeight / 2 + 1);
                GetPath(x, Console.WindowHeight / 2 + 4);
                GetFiles(x, width, Console.WindowHeight - 1, Console.WindowHeight / 2 + 5, e.FileIndex, y);
                GetFilesNumber(x, Console.WindowHeight / 2 + 3);
                indicator.GetIndicator(e.FileIndex, Console.WindowHeight - height, listOfFiles.Count, width, y, Console.WindowHeight - 1);
                blueBox.SetBlueBox((1, y), currentFiles[fileNumber - 1].Display(), width - 3);
                GetFileDiff(Console.WindowWidth / 2 + 2, width - 1, e);
            }
            else if (fileNumber >= currentFiles.Count - 1 && ReadButtons.Type.down == true || fileIndex == -1 && ReadButtons.Type.up == true)
            {
                GetFiles(x, width, Console.WindowHeight - 1, Console.WindowHeight / 2 + 5, e.FilesStartingIndex, Console.WindowHeight / 2 + 5);
                indicator.GetIndicator(e.FileIndex, Console.WindowHeight - height, listOfFiles.Count, width, y, Console.WindowHeight - 1);
                
                if (e.FileIndex == -1)
                {
                    e.FileIndex = 0;
                }
                
                blueBox.SetBlueBox((1, y), currentFiles[e.FileIndex].Display(), width - 2);
                GetFileDiff(Console.WindowWidth / 2 + 2, width - 1, e);
            }
            else
            {
                GetOneFile(width  - 2, height, y);
                GetFileDiff(Console.WindowWidth / 2 + 2, width - 1, e);
            }
        }
        
        private void HandlePressingLeftButton(CommitSelectionChangedEventArgs e)
        {
            panel.DrawBorderForCommitsAfterPressingEnter();
            HandlePressingEnterButtom(e);
        }

        private void HandleDisplayingDiff(CommitSelectionChangedEventArgs e)
        {
            List<string> currentDiff = commitsService.GetCurrentDiffForSelectedFile(commitNumber - 1, currentFiles[e.FileIndex].GetFileName());
            //panel.DrawDiffPanel(currentDiff, 1, commitIndex);
            int index = e.DiffIndex;

            if (e.DiffIndex == 0 || e.Y == Console.WindowHeight - 2 || e.Y == 3 && ReadButtons.Type.up == true)
            {
                GetFileDiff(1, Console.WindowWidth - 3, e);
                
                if (e.Y == 3 && ReadButtons.Type.up == true)
                {
                    index = e.DiffStartingindex;
                }
            }
            else
            {
                GetOneLineOfDiff(currentDiff, e);
            }
            
            blueBox.SetBlueBox((1, e.Y), currentDiff[index], Console.WindowWidth - 3);
            indicator.GetIndicator(e.DiffIndex, Console.WindowHeight - 2, currentDiff.Count, Console.WindowWidth - 1, e.Y, Console.WindowHeight - 2);
        }
        private void DisplayOneCommit()
        {
            (int index, int yNewPosition) = ReadButtons.Type.down ? (commitIndex - 1, y - 1) : (commitIndex + 1, y + 1);
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
        private void GetFiles(int x, int width, int height, int startFrom, int startIndex, int y)
        {
            for (int i = 0; i < currentFiles.Count; i++)
            {
                y = startFrom + i;

                if (y < height)
                {
                    string displayText = TextSettings.GetTextLength(currentFiles[i].Display(), width - 2);
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
                    Console.Write(displayText);
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }
        }
        private void GetOneFile(int width, int height, int y)
        {
            string fileName;

            if (ReadButtons.Type.down == true)
            {
                fileName = currentFiles[fileIndex - 1].Display();
                Console.SetCursorPosition(1, y - 1);
            }
            else
            {
                
                fileName = currentFiles[fileIndex + 1].Display();
                Console.SetCursorPosition(1, y + 1);
            }

            string displayText = TextSettings.GetTextLength(fileName, width);
            Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
            Console.Write(displayText);
            Console.ResetColor();
            indicator.GetIndicator(fileIndex, Console.WindowHeight - Console.WindowHeight / 2 + 3, listOfFiles.Count, width + 2, Console.WindowHeight / 2 + 4, Console.WindowHeight - 1);
            blueBox.SetBlueBox((1, y), currentFiles[fileIndex].Display(), width);
        }
        private void GetFileDiff(int x, int width, CommitSelectionChangedEventArgs e)
        {
            y = e.Y;
            int index = e.DiffStartingindex;
            List<string> currentDiff = commitsService.GetCurrentDiffForSelectedFile(commitNumber - 1, currentFiles[fileIndex].GetFileName());
            
            int height = currentDiff.Count > Console.WindowHeight - 3 ? Console.WindowHeight - 1
                   : (currentDiff.Count == Console.WindowHeight - 3 ? currentDiff.Count
                       : currentDiff.Count + 3);

            for (int i = 0; i < height; i++)
            {
                int y = 3 + i;

                if (y < height)
                {
                    string displayText = TextSettings.GetTextLength(currentDiff[index], width);
                    Console.SetCursorPosition(x, y);

                    if (displayText == "")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (displayText.StartsWith('=') || displayText.StartsWith('<'))
                    {
                        displayText = ""; 
                    }
                    else
                    {
                        Console.ForegroundColor = index == 0 ? ConsoleColor.Gray : TextSettings.SetColorStatus(displayText[0]);
                    }

                    Console.Write(displayText);
                    Console.ResetColor();
                }
                else
                {
                    break;
                }

                index++;
            }
        }
        private void GetOneLineOfDiff(List<string> currentDiff, CommitSelectionChangedEventArgs e)
        {
            string displayText;

            if (ReadButtons.Type.down == true)
            {
                Console.SetCursorPosition(1, e.Y - 1);
                displayText = TextSettings.GetTextLength(currentDiff[e.DiffIndex - 1], Console.WindowWidth - 3);
            }
            else
            {
                Console.SetCursorPosition(1, e.Y + 1);
                displayText = TextSettings.GetTextLength(currentDiff[e.DiffIndex + 1], Console.WindowWidth - 3);
            }
            
            if (e.DiffIndex == 1)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if (displayText != "")
            {
                Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
            }

            Console.Write(displayText);
            Console.ResetColor();
        }
        private void GetInfo(int x, int width, int height)
        {
            Dictionary<string, string> display = new Dictionary<string, string>
            {
                { "Author: ", currentCommits[commitIndex].Author },
                { "Date/Time: ", currentCommits[commitIndex].DateAndTime },
                { "sha: ", currentCommits[commitIndex].Id }
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

            if (currentCommits[commitIndex].Message.Length >= width)
            {
                for (int i = 0; i < height; i++)
                {
                    if (index == currentCommits[commitIndex].Message.Length)
                    {
                        break;
                    }


                    if (currentCommits[commitIndex].Message.Length - index < width)
                    {
                        width = currentCommits[commitIndex].Message.Length - index;
                    }

                    Console.SetCursorPosition(x, y);
                    string displayText = currentCommits[commitIndex].Message.Substring(index, width);
                    Console.Write(displayText.TrimStart());
                    index += width;
                    y++;
                }
            }
            else
            {
                Console.SetCursorPosition(x, y);
                Console.Write(currentCommits[commitIndex].Message);
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
    }
}
