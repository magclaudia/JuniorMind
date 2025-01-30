using GitClient.model;
using GitClient.service;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  

namespace GitClient.ui
{
    public class DiffPanel : UiComponent
    {
        private StatusDiffService statusDiffService;
        private StatusService statusService;
        private HunksService hunksService;
        private PanelCommunicationService communicationService;
        private DrawTabs.Dimensions dimensions;
        private List<string> currentDiff;
        private List<string> hunk;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panels;
        private int height;
        private int x;
        private int y;
        private int startIndex;
        private int currentIndex;
        private int hunkIndex;


        public DiffPanel(StatusDiffService statusDiffService, StatusService statusService, HunksService hunksService, PanelCommunicationService communicationService) 
        {
            this.statusDiffService = statusDiffService;
            this.statusService = statusService;
            this.hunksService = hunksService;
            this.communicationService = communicationService;
            dimensions = new DrawTabs.Dimensions();
            currentDiff = new List<string>();
            hunk = new List<string>();
            blueBox = new BlueBox();
            height = Console.WindowHeight - dimensions.tabHeight;
            indicator = new Indicator();
            panels = new DrawPanels();
            x = 1;
            y = dimensions.tabHeight + 2;
            startIndex = GetStartIndex();
            currentIndex = GetCurrentIndex();
            hunkIndex = 0;
        }

        public void SubcribeToPanel(UnstagedChangesPanel unstagedChangesPanel, StagedChangesPanel stagedChangesPanel)
        {
            unstagedChangesPanel.FileSelectionChanged += HandleFileSelectionChanged!;
            stagedChangesPanel.FileSelectionChanged += HandleFileSelectionChanged!;
        }

        public void SetCommunicationService(PanelCommunicationService service)
        {
            communicationService = service;
        }

        public override void Show()
        {
            y = dimensions.tabHeight + 2;
            List<FileDiff> fileDiffs = new List<FileDiff>();
            string currentPanel;
            hunk.Clear();

            if (ReadButtonsPressingOrActions.Type.workingInStagePanel == true)
            {
                fileDiffs = statusDiffService.GetAllStageDiffs();
                currentPanel = "stage";
            }
            else
            {
                fileDiffs = statusDiffService.GetAllUnstageDiffs();
                currentPanel = "unstage";
            }
            
            if (fileDiffs.Count > 0)
            {
                currentIndex = communicationService.GetCurrentIndex();
                string fileName = fileDiffs[currentIndex].fileName;
                currentDiff = fileName != "" ? currentDiff = statusDiffService.GetCurrentDiff(fileName, currentPanel)[0].diffs : new List<string>();
                currentIndex = 0;
            }

            Refresh(currentDiff);

            if (ReadButtonsPressingOrActions.Type.rightOnce == true)
            {
                Navigate();
            }
        }

        public void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ClearConsoleChoosenSpace clear = new ClearConsoleChoosenSpace();

            do
            {
                keyInfo = Console.ReadKey();
                ReadButtonsPressingOrActions.Type.diffMovements = true;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            if (currentIndex < currentDiff.Count - 1)
                            {
                                ReadButtonsPressingOrActions.Type.down = true;
                                ReadButtonsPressingOrActions.Type.up = false;
                                clear.ClearDiff(y);
                                panels.DrawDiffPanel(currentDiff, x, currentIndex); 

                                if (y == height - 1)
                                {
                                    startIndex++;
                                }

                                GetNewLineDiff(currentDiff);

                                if (y < height)
                                {
                                    y++;
                                }

                                currentIndex++;
                                IsHunkHeader(currentDiff[currentIndex]);
                                blueBox.SetBlueBox((1, y), currentDiff[currentIndex], Console.WindowWidth - 3);
                                indicator.GetIndicator(currentIndex, height - 1, currentDiff.Count, Console.WindowWidth - 1, y - 1, height - 1);
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            if (currentIndex > 0)
                            {
                                ReadButtonsPressingOrActions.Type.down = false;
                                ReadButtonsPressingOrActions.Type.up = true;
                                clear.ClearDiff(y);
                                panels.DrawDiffPanel(currentDiff, x, currentIndex);

                                if (y == dimensions.tabHeight + 2)
                                {
                                    startIndex--;
                                }

                                GetNewLineDiff(currentDiff);

                                if (y > dimensions.tabHeight + 2)
                                {
                                    y--;
                                }

                                currentIndex--;
                                blueBox.SetBlueBox((1, y), currentDiff[currentIndex], Console.WindowWidth - 3);
                                indicator.GetIndicator(currentIndex, height - 1, currentDiff.Count, Console.WindowWidth - 1, y, height);
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            if (currentIndex > 0)
                            {
                                int fileIndex = communicationService.GetCurrentIndex();
                                var filePath = statusService.GetAllUnstagedChanges()[fileIndex].GetFilePath();
                                communicationService.SetFilePath(filePath);
                                SetListsOfHunks(currentDiff[currentIndex]);
                                communicationService.SetHunkIndex(hunkIndex);
                                communicationService.SetHunkToBeTransfer(hunk);
                                communicationService.GetCurrentIndex();
                                hunksService.GetHunks();
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        {
                            ReadButtonsPressingOrActions.Type.diffMovements = false;
                            Console.Clear();
                            ReadButtonsPressingOrActions.Type.rightOnce = false;
                            currentIndex = communicationService.GetCurrentIndex();
                            startIndex = 0;
                            communicationService.NavigateToStatusInitialState();
                        }
                        break;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

        private void Refresh(List<string> currentDiff)
        {
            if (currentDiff.Count > 0)
            {
                DisplayDiff(currentDiff);

                if (ReadButtonsPressingOrActions.Type.rightOnce == true)
                {
                    string textForBluexBox = currentDiff[0];
                    blueBox.SetBlueBox((1, y), textForBluexBox, Console.WindowWidth - 3);
                }
            }

            panels.DrawDiffPanel(currentDiff, x, currentIndex);
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }


        private int GetStartIndex()
        {
            return 0;
        }

        private void SetListsOfHunks(string line)
        {
            int index = currentIndex;
            
            List<List<string>> listOfHunks = new List<List<string>>();

            listOfHunks = currentDiff.Aggregate(new List<List<string>>(), (acc, line) =>
            {
                if (line.StartsWith("@@"))
                {
                    acc.Add(new List<string> { line });
                }
                else if (acc.Any())
                {
                    acc.Last().Add(line);
                }

                return acc;
            });

            hunk = listOfHunks.First(list => list.Contains(line));
            hunkIndex = listOfHunks.IndexOf(hunk);
        }

        private void IsHunkHeader(string line)
        {
            if (line.StartsWith("@@") && !hunk.Contains(line))
            {
                hunkIndex++;
            }
        }

        private void HandleFileSelectionChanged(object sender, FileSelectionChangedEventArgs e)
        {
            List<string> currentDiff = e.IsStaged ? statusDiffService.GetCurrentDiff(e.FileName, "stage")[0].diffs
                : statusDiffService.GetCurrentDiff(e.FileName, "unstage")[0].diffs;

            Refresh(currentDiff);
        }

        private void DisplayDiff(List<string> currentDiff)
        {
            int stopAt = currentDiff.Count() > height - 2 ? height - 2 : currentDiff.Count();
            int width = 0;

            if (ReadButtonsPressingOrActions.Type.rightOnce == true)
            {
                x = 1;
                width = Console.WindowWidth - 3;
            }
            else
            {
                x = dimensions.changesPanelWidth + 4;
                width = Console.WindowWidth - dimensions.changesPanelWidth - 6;
            }

            for (int i = 0; i < stopAt; i++)
            {
                int y = dimensions.unstagedStart - 1 + i;
                
                if (y < height)
                {
                    string displayText = TextSettings.GetTextLength(currentDiff[i], width);
                    Console.SetCursorPosition(x, y);
                    
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

        //private void DrawDiffPanel(List<string> currentDiff)
        //{
        //    int width = 0;
        //    int positionOfDiffName = 0;

        //    if (ReadButtonsPressing.Type.right == true)
        //    {
        //        x = 0;
        //        width = Console.WindowWidth - 1;
        //        positionOfDiffName = 1;
        //    }
        //    else
        //    {
        //        x = dimensions.width / 2 + 1;
        //        width = Console.WindowWidth - dimensions.changesPanelWidth - 3;
        //        positionOfDiffName = dimensions.width / 2 + 2;
        //    }

        //    for (int i = dimensions.tabHeight + 2; i < Console.WindowHeight - 1; i++)
        //    {
        //        Console.SetCursorPosition(x, i);
        //        Console.Write("│");
        //        Console.SetCursorPosition(Console.WindowWidth - 1, i);
        //        Console.Write("║");
        //    }

        //    for (int i = 1; i < width; i++)
        //    {
        //        Console.SetCursorPosition(x + i, dimensions.tabHeight + 1);
        //        Console.Write("─");
        //        Console.SetCursorPosition(x + i, Console.WindowHeight - 1);
        //        Console.Write("─");
        //    }

        //    Console.SetCursorPosition(x, dimensions.tabHeight + 1);
        //    Console.Write("┌");
        //    Console.SetCursorPosition(x, Console.WindowHeight - 1);
        //    Console.Write("└");
        //    Console.SetCursorPosition(Console.WindowWidth - 1, dimensions.tabHeight + 1);
        //    Console.Write("┐");
        //    Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
        //    Console.Write("┘");

        //    string text = TextSettings.GetTextLength("Diff: ", dimensions.width / 2 - 2);
        //    Console.SetCursorPosition(positionOfDiffName, dimensions.tabHeight + 1);
        //    Console.Write(text);

        //    if (ReadButtonsPressing.Type.right == true && currentIndex == 0)
        //    {
        //        indicator.GetIndicator(currentIndex, Console.WindowHeight - 1, currentDiff.Count(), Console.WindowWidth - 1, dimensions.tabHeight + 2, Console.WindowHeight - 2);
        //    }
        //}

        private void GetNewLineDiff(List<string> currentDiff)
        {
            if (y == height - 1 && ReadButtonsPressingOrActions.Type.down == true || y == 3 && ReadButtonsPressingOrActions.Type.up == true)
            {
                y = dimensions.tabHeight + 2;
                GetAllDiffLines(currentDiff);
                
                if (ReadButtonsPressingOrActions.Type.up)
                {
                    y = dimensions.tabHeight + 2;

                }
                else
                {
                    y = height - 2;
                }
            }
            else
            {
                GetOneLineAtTime(currentDiff);
            }
        }

        private void GetAllDiffLines(List<string> currentDiff)
        {
            int index = startIndex; 
            for (int i = 0; i < height - 1; i++)
            {
                if (i == currentDiff.Count || y == height || index == currentDiff.Count)
                {
                    break;
                }

                string displayText = TextSettings.GetTextLength(currentDiff[index], Console.WindowWidth - 3);
                Console.SetCursorPosition(1, y);
                
                if (displayText != "")
                {
                    Console.ForegroundColor = TextSettings.SetColorStatus(displayText[0]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(displayText);
                Console.ResetColor();
                y++;
                index++;
            }
        }

        private void GetOneLineAtTime(List<string> currentDiff)
        {
            string displayText = TextSettings.GetTextLength(currentDiff[currentIndex], Console.WindowWidth - 3);
            Console.SetCursorPosition(1, y);
            
            if (displayText != "")
            {
                Console.ForegroundColor = displayText.EndsWith(".cs") ? ConsoleColor.DarkGray : TextSettings.SetColorStatus(displayText[0]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write(displayText);
            Console.ResetColor();
        }
    }
}
