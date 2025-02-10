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

namespace GitClient.ui
{
    public class CommitsWithDescription
    {
        private CommitsService commitsService;
        private List<Commit> currentCommits;
        private List<ChangeAttribute> currentFiles;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panel;
        private List<ChangeAttribute> listOfFiles;
        private ClearConsoleChoosenSpace clear;
        private int totalCommits;
        private int width;
        private int lastYPosition;

        public CommitsWithDescription(CommitsService commitsService, PanelCommunicationService panelCommunicationService) 
        {
            this.commitsService = commitsService;
            currentCommits = new List<Commit>();
            currentFiles = new List<ChangeAttribute>();
            blueBox = new BlueBox();
            indicator = new Indicator();
            panel = new DrawPanels();
            listOfFiles = new List<ChangeAttribute>();
            clear = new ClearConsoleChoosenSpace();
            totalCommits = commitsService.GetAllCommits().Count;
            width = 0;
        }


        public void SubscribeToPanel(CommitsNavigation commitsNavigation)
        {
            commitsNavigation.CommitSelectionChanged += HandleCommitsSelectionChanged!;
        }
        private void HandleCommitsSelectionChanged(object sender, CommitSelectionChangedEventArgs e)
        {
            ButtonStates buttonStates = e.ButtonStates;

            if (buttonStates.Enter == true && buttonStates.GitLog == false)
            {
                listOfFiles = commitsService.GetAllFilesForCommit(e.CommitNumber - 1);
                currentCommits = commitsService.GetCurrentListOfCommits(e.StartIndex, e.EndIndex);
                currentFiles = commitsService.GetCurrentFiles(e.FilesStartingIndex, e.CommitNumber - 1);
                HandlePressingEnterButton(e);
            }
            else if (buttonStates.Right == true)
            {
                currentFiles = commitsService.GetCurrentFiles(e.FilesStartingIndex, e.CommitNumber - 1);
                HandlePressingRightButtonOnce(e);
            }
            else if (buttonStates.Left == true)
            {
                currentFiles = commitsService.GetCurrentFiles(0, e.CommitNumber - 1);
                HandlePressingLeftButton(e);
            }
            else if (buttonStates.Diff == true)
            {
                HandleDisplayingDiff(e);
            }
        }
        private void HandlePressingEnterButton(CommitSelectionChangedEventArgs e)
        {
            width = Console.WindowWidth / 2 + 5;
            int height = Console.WindowHeight - 1;
            int x = Console.WindowWidth / 2 + 10;
            ButtonStates buttonStates = e.ButtonStates;

            if (ReadButtons.DisplayListOfCommitsOnEntirePanel == true)
            {
                panel.DrawBorderForCommitsAfterPressingEnter();
                DisplayCommits();
            }
            else
            {
                if (e.Y == Console.WindowHeight - 2 || e.Y == 3 || buttonStates.Left == true)
                {
                    clear.ClearCommitWithDescription(1, width, height);
                    DisplayCommits();
                }
                else
                {
                    clear.ClearOneCommit(1, e.Y, width);
                    DisplayOneCommit(e);
                }

                clear.ClearInfoPanel();
                clear.ClearMessagePanel();
                clear.ClearFilesPanelForCommits();
            }

            indicator.GetIndicator(e.CommitNumber, height - 1, totalCommits, width +  2, 3, height);
            blueBox.SetBlueBox((1, e.Y), currentCommits[e.CommitIndex].Display(), width);
            GetInfo(x, Console.WindowWidth - (Console.WindowWidth / 2 + 11), Console.WindowHeight / 4, e);
            GetMessage(x, Console.WindowWidth / 2 - 11, Console.WindowHeight / 2 - 1, e);
            GetCommitNumber(e);
            GetPath(x, Console.WindowHeight / 2 + 5);
            GetFiles(x, Console.WindowWidth - (Console.WindowWidth / 2 + 11), height, Console.WindowHeight / 2 + 6, e.StartIndex, e.Y);
            GetFilesNumber(x, Console.WindowHeight / 2 + 4);
        }
        private void HandlePressingRightButtonOnce(CommitSelectionChangedEventArgs e)
        {
            int x = 1;
            width = (Console.WindowWidth / 2) - 2;
            int height = Console.WindowHeight - (Console.WindowHeight / 2 + 4);

            if (ReadButtons.Enter == true)
            {
                lastYPosition = e.Y;
                e.Y = Console.WindowHeight / 2 + 5;
                panel.DrawBorderForCommitsAfterPressingRightOnce();
                GetInfo(x, width, Console.WindowHeight / 4, e);
                GetMessage(x, width, Console.WindowHeight / 2 + 1, e);
                GetPath(x, Console.WindowHeight / 2 + 4);
                GetFiles(x, width, Console.WindowHeight - 1, Console.WindowHeight / 2 + 5, e.FileIndex, e.Y);
                GetFilesNumber(x, Console.WindowHeight / 2 + 3);
                
                if (ReadButtons.Esc == true)
                {
                    e.Y = lastYPosition;
                    indicator.GetIndicator(e.FileIndex, Console.WindowHeight - height, listOfFiles.Count, width + 1, lastYPosition, Console.WindowHeight - 1);
                    blueBox.SetBlueBox((1, e.Y), currentFiles[e.FileIndex].Display(), width - 3);
                }
                else
                {
                    indicator.GetIndicator(e.FileIndex, Console.WindowHeight - height, listOfFiles.Count, width + 1, e.Y, Console.WindowHeight - 1);
                    blueBox.SetBlueBox((1, e.Y), currentFiles[e.FileIndex].Display(), width - 3);
                }

                GetFileDiff(Console.WindowWidth / 2 + 2, width - 1, e);
            }
            else if (e.FileNumber >= currentFiles.Count - 1 && ReadButtons.Down == true || e.FileIndex == -1 && ReadButtons.Up == true)
            {
                GetFiles(x, width, Console.WindowHeight - 1, Console.WindowHeight / 2 + 5, e.FilesStartingIndex, Console.WindowHeight / 2 + 5);
                indicator.GetIndicator(e.FileIndex, Console.WindowHeight - height, listOfFiles.Count, width, e.Y, Console.WindowHeight - 1);
                
                if (e.FileIndex == -1)
                {
                    e.FileIndex = 0;
                }
                
                blueBox.SetBlueBox((1, e.Y), currentFiles[e.FileIndex].Display(), width - 2);
                GetFileDiff(Console.WindowWidth / 2 + 2, width - 1, e);
            }
            else
            {
                GetOneFile(width  - 2, e.Y, e);
                GetFileDiff(Console.WindowWidth / 2 + 2, width - 1, e);
            }
        }
        private void HandlePressingLeftButton(CommitSelectionChangedEventArgs e)
        {
            panel.DrawBorderForCommitsAfterPressingEnter();
            HandlePressingEnterButton(e);
        }
        private void HandleDisplayingDiff(CommitSelectionChangedEventArgs e)
        {
            List<string> currentDiff = commitsService.GetCurrentDiffForSelectedFile(e.CommitNumber - 1, currentFiles[e.FileIndex].GetFileName());
            int index = e.DiffIndex;

            if (e.DiffIndex == 0 || e.Y == Console.WindowHeight - 2 || e.Y == 3 && ReadButtons.Up == true)
            {
                GetFileDiff(1, Console.WindowWidth - 3, e);
                
                if (e.Y == 3 && ReadButtons.Up == true)
                {
                    index = e.DiffStartingIndex;
                }
            }
            else
            {
                GetOneLineOfDiff(currentDiff, e);
            }
            
            blueBox.SetBlueBox((1, e.Y), currentDiff[index], Console.WindowWidth - 3);
            indicator.GetIndicator(e.DiffIndex, Console.WindowHeight - 2, currentDiff.Count, Console.WindowWidth - 1, e.Y, Console.WindowHeight - 2);
        }
        private void DisplayOneCommit(CommitSelectionChangedEventArgs e)
        {
            (int index, int yNewPosition) = ReadButtons.Down ? (e.CommitIndex - 1, e.Y - 1) : (e.CommitIndex + 1, e.Y + 1);
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
        private void GetOneFile(int width, int y, CommitSelectionChangedEventArgs e)
        {
            string fileName;

            if (ReadButtons.Down == true)
            {
                fileName = currentFiles[e.FileIndex - 1].Display();
                Console.SetCursorPosition(1, y - 1);
            }
            else
            {
                
                fileName = currentFiles[e.FileIndex + 1].Display();
                Console.SetCursorPosition(1, y + 1);
            }

            string displayText = TextSettings.GetTextLength(fileName, width);
            Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
            Console.Write(displayText);
            Console.ResetColor();
            indicator.GetIndicator(e.FileIndex, Console.WindowHeight - Console.WindowHeight / 2 + 3, listOfFiles.Count, width + 2, Console.WindowHeight / 2 + 4, Console.WindowHeight - 1);
            blueBox.SetBlueBox((1, y), currentFiles[e.FileIndex].Display(), width);
        }
        private void GetFileDiff(int x, int width, CommitSelectionChangedEventArgs e)
        {
            int index = e.DiffStartingIndex;
            List<string> currentDiff = commitsService.GetCurrentDiffForSelectedFile(e.CommitNumber - 1, currentFiles[e.FileIndex].GetFileName());
            
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

            if (ReadButtons.Down == true)
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
        private void GetInfo(int x, int width, int height, CommitSelectionChangedEventArgs e)
        {
            Dictionary<string, string> display = new Dictionary<string, string>
            {
                { "Author: ", currentCommits[e.CommitIndex].Author },
                { "Date/Time: ", currentCommits[e.CommitIndex].DateAndTime },
                { "sha: ", currentCommits[e.CommitIndex].Id }
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
                else
                {
                    break;
                }
            }
        }
        private void GetMessage(int x, int width, int height, CommitSelectionChangedEventArgs e)
        {
            int y = 0;
            
            if (ReadButtons.Enter == true && ReadButtons.RightOnce == false)
            {
                y = Console.WindowHeight / 4 + 3;
            }
            else
            {
                y = Console.WindowHeight / 3;
            }
             
            int index = 0;

            if (currentCommits[e.CommitIndex].Message.Length >= width)
            {
                for (int i = 0; i < height; i++)
                {
                    if (index == currentCommits[e.CommitIndex].Message.Length)
                    {
                        break;
                    }

                    if (currentCommits[e.CommitIndex].Message.Length - index < width)
                    {
                        width = currentCommits[e.CommitIndex].Message.Length - index;
                    }

                    Console.SetCursorPosition(x, y);
                    string displayText = currentCommits[e.CommitIndex].Message.Substring(index, width);
                    Console.Write(displayText.TrimStart());
                    index += width;
                    y++;
                }
            }
            else
            {
                Console.SetCursorPosition(x, y);
                Console.Write(currentCommits[e.CommitIndex].Message);
            }
        }
        private void GetCommitNumber(CommitSelectionChangedEventArgs e)
        {
            Console.SetCursorPosition(1, 2);
            Console.Write($"Commit: {e.CommitNumber} / {totalCommits} ");
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
