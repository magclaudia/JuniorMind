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
        private List<FileDiff> fileDiffs;
        private List<string> currentDiff;
        private List<string> hunk;
        private BlueBox blueBox;
        private Indicator indicator;
        private DrawPanels panels;
        private int height;
        private int x;
        private int y;
        private int startIndex;
        private int diffIndex;
        private int hunkIndex;


        public DiffPanel(StatusDiffService statusDiffService, StatusService statusService, HunksService hunksService, PanelCommunicationService communicationService) 
        {
            this.statusDiffService = statusDiffService;
            this.statusService = statusService;
            this.hunksService = hunksService;
            this.communicationService = communicationService;
            dimensions = new DrawTabs.Dimensions();
            fileDiffs = new List<FileDiff>();
            currentDiff = new List<string>();
            hunk = new List<string>();
            blueBox = new BlueBox();
            height = Console.WindowHeight - dimensions.tabHeight;
            indicator = new Indicator();
            panels = new DrawPanels();
            x = 1;
            y = dimensions.tabHeight + 2;
            startIndex = 0;
            diffIndex = GetCurrentIndex();
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
            startIndex = 0;
            string currentPanel;
            hunk.Clear();

            if (ReadButtons.WorkingInStagePanel == true && statusDiffService.GetAllStageDiffs().Count > 0 || statusDiffService.GetAllUnstageDiffs().Count == 0)
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
                diffIndex = communicationService.GetFileIndex();
                
                if (diffIndex >= fileDiffs.Count)
                {
                    diffIndex = fileDiffs.Count - 1;
                }
                
                string fileName = fileDiffs[diffIndex].fileName;
                currentDiff = fileName != "" ? currentDiff = statusDiffService.GetCurrentDiff(fileName, currentPanel)[0].diffs : new List<string>();
                diffIndex = 0;
            }

            Refresh(currentDiff);

            if (ReadButtons.RightOnce == true)
            {
                Navigate();
            }
        }
        private void Navigate()
        {
            ConsoleKeyInfo keyInfo;
            ClearConsoleChoosenSpace clear = new ClearConsoleChoosenSpace();

            do
            {
                keyInfo = Console.ReadKey();
                ReadButtons.DiffMovements = true;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            if (diffIndex < currentDiff.Count - 1)
                            {
                                ReadButtons.Down = true;
                                ReadButtons.Up = false;
                                clear.ClearDiff(1, y, Console.WindowHeight - 2);

                                if (y == height - 1)
                                {
                                    startIndex++;
                                }

                                GetNewLineDiff(currentDiff);

                                if (y < height)
                                {
                                    y++;
                                }

                                diffIndex++;
                                IsHunkHeader(currentDiff[diffIndex]);
                                blueBox.SetBlueBox((1, y), currentDiff[diffIndex], Console.WindowWidth - 3);
                                indicator.GetIndicator(diffIndex, height - 1, currentDiff.Count, Console.WindowWidth - 1, 3, height);
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            if (diffIndex > 0)
                            {
                                ReadButtons.Down = false;
                                ReadButtons.Up = true;
                                clear.ClearDiff(1, y, Console.WindowHeight - 2);

                                if (y == dimensions.tabHeight + 2)
                                {
                                    startIndex--;
                                }

                                GetNewLineDiff(currentDiff);

                                if (y > dimensions.tabHeight + 2)
                                {
                                    y--;
                                }

                                diffIndex--;
                                blueBox.SetBlueBox((1, y), currentDiff[diffIndex], Console.WindowWidth - 3);
                                indicator.GetIndicator(diffIndex, height - 1, currentDiff.Count, Console.WindowWidth - 1, 3, height);
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            if (diffIndex > 0)
                            {
                                int fileIndex = communicationService.GetFileIndex();
                                string filePath = "";
                                string fileName = "";

                                if (ReadButtons.WorkingInUnstagePanel == true)
                                {
                                    if (statusService.GetAllUnstagedChanges().Count == 0 || fileIndex < 0 || fileIndex >= statusService.GetAllUnstagedChanges().Count)
                                    {
                                        break;
                                    }

                                    filePath = statusService.GetAllUnstagedChanges()[fileIndex].GetFilePath();
                                    fileName = statusService.GetAllUnstagedChanges()[fileIndex].GetFileName();
                                }
                                else
                                {
                                    if (statusService.GetAllStageChanges().Count == 0 || fileIndex < 0 || fileIndex >= statusService.GetAllStageChanges().Count)
                                    {
                                        break;
                                    }

                                    filePath = statusService.GetAllStageChanges()[fileIndex].GetFilePath();
                                    fileName = statusService.GetAllStageChanges()[fileIndex].GetFileName();
                                }

                                if (!currentDiff[0].Contains(fileName))
                                {
                                    break;
                                }

                                communicationService.SetFilePath(filePath);
                                SetHunkIndex(currentDiff[diffIndex]);
                                communicationService.SetHunkIndex(hunkIndex);
                                communicationService.SetDiffIndex(diffIndex);
                                communicationService.SetLineToBeStaged(currentDiff[diffIndex]);
                                Console.Clear();
                                TabsPanel tabs = new TabsPanel();

                                if (ReadButtons.WorkingInUnstagePanel == true)
                                {
                                    hunksService.StageHunk();
                                    var list = statusDiffService.GetCurrentDiff(fileName, "unstage");
                                   
                                    if (list.Count > 0)
                                    {
                                        currentDiff = statusDiffService.GetCurrentDiff(fileName, "unstage")[0].diffs;
                                        startIndex = 0;
                                        diffIndex = 0;
                                        y = dimensions.tabHeight + 2;
                                        tabs.Show();
                                        Refresh(currentDiff);
                                    }
                                }
                                else
                                {
                                    hunksService.UnstageHunk();
                                    var list = statusDiffService.GetCurrentDiff(fileName, "stage");

                                    if (list.Count > 0)
                                    {
                                        currentDiff = statusDiffService.GetCurrentDiff(fileName, "stage")[0].diffs;
                                        startIndex = 0;
                                        diffIndex = 0;
                                        y = dimensions.tabHeight + 2;
                                        tabs.Show();
                                        Refresh(currentDiff);
                                    }
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        {
                            ReadButtons.DiffMovements = false;
                            Console.Clear();
                            ReadButtons.RightOnce = false;
                            ReadButtons.Down = false;

                            if (statusService.GetAllUnstagedChanges().Count == 0)
                            {
                                ReadButtons.WorkingInUnstagePanel = false;
                                ReadButtons.WorkingInStagePanel = true;
                            }
                            else if (statusService.GetAllStageChanges().Count == 0)
                            {
                                ReadButtons.WorkingInUnstagePanel = true;
                                ReadButtons.WorkingInStagePanel = false;
                                communicationService.SetFileIndex(statusService.GetAllUnstagedChanges().Count - 1);
                            }

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

                if (ReadButtons.RightOnce == true)
                {
                    string textForBluexBox = currentDiff[0];
                    blueBox.SetBlueBox((1, y), textForBluexBox, Console.WindowWidth - 3);
                }
            }

            panels.DrawDiffPanel(currentDiff, x, diffIndex);
        }
        public int GetCurrentIndex()
        {
            return diffIndex;
        }
        
        private void SetHunkIndex(string targetLine)
        {
            int currentHunkIndex = -1;
            hunkIndex = -1;

            foreach (var line in currentDiff)
            {
                if (line.StartsWith("@@"))
                {
                    currentHunkIndex++;
                }

                if (line == targetLine)
                {
                    hunkIndex = currentHunkIndex;
                    break;
                }
            }

            if (hunkIndex == -1)
            {
                throw new InvalidOperationException("Target line not found in diff");
            }
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

            if (ReadButtons.RightOnce == true)
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
        private void GetNewLineDiff(List<string> currentDiff)
        {
            if (y == height - 1 && ReadButtons.Down == true || y == 3 && ReadButtons.Up == true)
            {
                y = dimensions.tabHeight + 2;
                GetAllDiffLines(currentDiff);
                
                if (ReadButtons.Up)
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
            string displayText = TextSettings.GetTextLength(currentDiff[diffIndex], Console.WindowWidth - 3);
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
