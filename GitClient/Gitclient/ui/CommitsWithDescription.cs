using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                HandlePressingRightButtomOnce(e);
            }
        }

        private void HandlePressingEnterButtom(CommitSelectionChangedEventArgs e)
        {
            width = Console.WindowWidth / 2 + 5;
            int x = Console.WindowWidth / 2 + 10;

            if (ReadButtonsPressingOrActions.Type.displayListOfCommitsOnEntirePanel == true)
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
            blueBox.SetBlueBox((1, y), currentCommits[commitIndex].Display(), width);
            GetInfo(x, Console.WindowWidth - (Console.WindowWidth / 2 + 11), Console.WindowHeight / 4);
            GetMessage(x, Console.WindowWidth / 2 - 11, Console.WindowHeight / 2 - 1);
            GetCommitNumber();
            GetPath(x, Console.WindowHeight / 2 + 5);
            GetFiles(x, Console.WindowWidth - (Console.WindowWidth / 2 + 11), Console.WindowHeight - 1, Console.WindowHeight / 2 + 6, e.StartIndex, y);
            GetFilesNumber(x, Console.WindowHeight / 2 + 4);
        }

        private void HandlePressingRightButtomOnce(CommitSelectionChangedEventArgs e)
        {
            int x = 1;
            width = (Console.WindowWidth / 2) - 2;
            int height = Console.WindowHeight - (Console.WindowHeight / 2 + 4);

            if (ReadButtonsPressingOrActions.Type.enter == true)
            {
                y = Console.WindowHeight / 2 + 5;
                panel.DrawBorderForCommitsAfterPressingRightOnce();
                GetInfo(x, width, Console.WindowHeight / 3 - 3);
                GetMessage(x, width, Console.WindowHeight / 2 + 1);
                GetPath(x, Console.WindowHeight / 2 + 4);
                GetFiles(x, width, Console.WindowHeight - 1, Console.WindowHeight / 2 + 5, e.FileIndex, y);
                GetFilesNumber(x, Console.WindowHeight / 2 + 3);
                indicator.GetIndicator(fileNumber, Console.WindowHeight - height, listOfFiles.Count, width, y, Console.WindowHeight - 1);
                blueBox.SetBlueBox((1, y), currentFiles[fileNumber - 1].Display(), width - 3);
                GetFileDiff(fileNumber - 1);
            }
            else if (fileNumber >= currentFiles.Count - 1 && ReadButtonsPressingOrActions.Type.down == true || fileIndex == -1 && ReadButtonsPressingOrActions.Type.up == true)
            {
                GetFiles(x, width, Console.WindowHeight - 1, Console.WindowHeight / 2 + 5, e.FilesStartingIndex, Console.WindowHeight / 2 + 5);
                indicator.GetIndicator(fileNumber, Console.WindowHeight - height, listOfFiles.Count, width, y, Console.WindowHeight - 1);
                
                if (e.FileIndex == -1)
                {
                    e.FileIndex = 0;
                }
                
                blueBox.SetBlueBox((1, y), currentFiles[e.FileIndex].Display(), width - 2);
                GetFileDiff(e.FileIndex);
            }
            else
            {
                GetOneFile(width  - 2, height, y);
                GetFileDiff(e.FileIndex);
            }
        }
        private void DisplayOneCommit()
        {
            (int index, int yNewPosition) = ReadButtonsPressingOrActions.Type.down ? (commitIndex - 1, y - 1) : (commitIndex + 1, y + 1);
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

            if (ReadButtonsPressingOrActions.Type.down == true)
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
            indicator.GetIndicator(listOfFiles.Count, Console.WindowHeight - height, listOfFiles.Count, width + 2, y, Console.WindowHeight - 1);
            blueBox.SetBlueBox((1, y), currentFiles[fileIndex].Display(), width);
        }
        private void GetFileDiff(int index)
        {
            List<FileDiff> currentDiff = commitsService.GetCurrentDiffForSelectedFile(commitNumber - 1, currentFiles[index].GetFileName());
            
            int height = currentDiff[0].diffs.Count > Console.WindowHeight - 3 ? Console.WindowHeight - 1
                   : (currentDiff[0].diffs.Count == Console.WindowHeight - 3 ? currentDiff[0].diffs.Count
                       : currentDiff[0].diffs.Count + 3);
            width = Console.WindowWidth / 2 + 2;

            for (int i = 0; i < height; i++)
            {
                int y = 3 + i;

                if (y < height)
                {
                    string displayText = TextSettings.GetTextLength(currentDiff[0].diffs[i], width - 6);
                    Console.SetCursorPosition(width, y);

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
                        Console.ForegroundColor = i == 0 ? ConsoleColor.Gray : TextSettings.SetColorStatus(displayText[0]);
                    }

                    Console.Write(displayText);
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }
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
