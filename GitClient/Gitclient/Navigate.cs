using System.Collections.Generic;
using System.IO;

namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughCommits(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, GetVariablesForTabs tab)
        {
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            IntPtr commitPtr = IntPtr.Zero;
            ConsoleKeyInfo keyInfo;
            int blueFond = 0;

            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            if (variablesForCommits.logTab == true)
                            {
                                if (variablesForCommits.stopWorkingOnCommits == true && variablesForCommits.pressRight == 1)
                                {
                                    HandleFilesUpMovesLog(variablesForCommits, variablesForFiles, list, commitElement);
                                }
                                else if (variablesForCommits.pressRight == 2)
                                {
                                    if (variablesForFiles.index == 0 && variablesForFiles.up == true)
                                    {
                                        variablesForFiles.end = true;
                                        variablesForFiles.up = false;
                                        break;
                                    }

                                    HandleDiffUpMovesLog(variablesForCommits, variablesForFiles, list, commitElement);
                                }
                                else
                                {
                                   
                                    if (variablesForCommits.heightPosition == 1 && variablesForCommits.currentCommitIndex == 0)
                                    {
                                        break;
                                    }

                                    HandleCommitsUpMoves(variablesForCommits, variablesForFiles, commitElement, list, blueFond);
                                }
                            }
                            else
                            {
                                if (variablesForFiles.statusDiffOpen == false)
                                {
                                    if (variablesForFiles.fileIndex == 0 && variablesForFiles.fileRow == dimensions.stagedStart && list.unstagedChangesFiles.Count == 0)
                                    {
                                        break;
                                    }

                                    if (variablesForFiles.stageChanges == true && variablesForFiles.fileIndex == 0 && variablesForFiles.fileRow == dimensions.stagedStart)
                                    {
                                        variablesForFiles.up = true;
                                    }

                                    if (variablesForFiles.up == false && variablesForFiles.fileIndex == 0 || variablesForCommits.right == true)
                                    {
                                        break;
                                    }

                                    HandleFilesUpStatus(variablesForCommits, variablesForFiles, list, commitElement, tab);
                                }
                                else
                                {
                                    if (variablesForFiles.up == true && variablesForFiles.row == dimensions.tabHeight + 2)
                                    {
                                        variablesForFiles.up = false;
                                        break;
                                    }

                                    HandleDiffUpMovesStatus(variablesForCommits, variablesForFiles, list, commitElement, tab);
                                }
                            }
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        {
                            if (variablesForCommits.logTab == true)
                            {
                                if (variablesForCommits.stopWorkingOnCommits == true && variablesForCommits.pressRight == 1)
                                {
                                    HandleFilesDownMovesLog(variablesForCommits, variablesForFiles, list, commitElement);
                                }
                                else if (variablesForCommits.pressRight == 2)
                                {
                                    if (variablesForFiles.index == list.listOfAllDiffs[variablesForFiles.indexDiff].Count - 1 && variablesForFiles.currentLine != 1)
                                    {
                                        break;
                                    }

                                    HandleDiffDownMovesLog(variablesForCommits, variablesForFiles, list, commitElement);
                                }
                                else
                                {
                                    HandleCommitsDownMoves(variablesForCommits, variablesForFiles, commitElement, list, blueFond);
                                }
                            }
                            else
                            {
                                if (variablesForFiles.statusDiffOpen == false)
                                {
                                    if (variablesForCommits.right == true || list.unstagedChangesFiles.Count == 0 && list.stagedChangesFiles.Count == 0 || list.unstagedChangesFiles.Count == 1 && list.stagedChangesFiles.Count == 0)
                                    {
                                        break;
                                    }

                                    if (list.stagedChangesFiles.Count == 1 && variablesForFiles.fileRow == dimensions.stagedStart)
                                    {
                                        break;
                                    }

                                    if (list.stagedChangesFiles.Count > 0 && variablesForFiles.fileIndex == list.stagedChangesFiles.Count - 1 && variablesForFiles.stageChanges == true)
                                    {
                                        break;
                                    }

                                    if (variablesForFiles.fileIndex == list.unstagedChangesFiles.Count - 1 && list.stagedChangesFiles.Count == 0)
                                    {
                                        break;
                                    }

                                    HandleFilesDownMovesStatus(variablesForCommits, variablesForFiles, list, commitElement, tab);
                                }
                                else
                                {
                                    List<string> listDiffs = new List<string>();
                                    
                                    if (variablesForFiles.unstageChanges == true)
                                    {
                                        listDiffs = list.unstagedChangesDiff[variablesForFiles.indexDiff];
                                    }
                                    else
                                    {
                                        listDiffs = list.stagedChangesDiff[variablesForFiles.indexDiff];
                                    }

                                    if (variablesForFiles.index == listDiffs.Count - 1)
                                    {
                                        break;
                                    }

                                    HandleDiffDownMovesStatus(variablesForCommits, variablesForFiles, list, commitElement, tab);
                                }
                            }
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        {
                            if (variablesForCommits.logTab == true)
                            {
                                if (variablesForCommits.pressRight == 2 || variablesForCommits.enter == false)
                                {
                                    break;
                                }

                                HandleRightArrowLog(variablesForCommits, variablesForFiles, commitElement, list);
                            }
                            else
                            {
                                if (variablesForCommits.pressRight > 1 || list.unstagedChangesFiles.Count == 0 && list.stagedChangesFiles.Count == 0)
                                {
                                    break;
                                }

                                variablesForFiles.statusDiffOpen = true;
                                variablesForCommits.pressRight++;
                                HandleRightArrowStatus(variablesForCommits, variablesForFiles, commitElement, list);
                            }

                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            if (variablesForFiles.diffMoves == false && variablesForCommits.logTab == true)
                            {
                                variablesForCommits.stopWorkingOnCommits = false;
                                variablesForCommits.enter = true;
                                variablesForCommits.panelAlreadyDisplayed = false;
                                variablesForCommits.esc = false;
                                
                                if (variablesForCommits.right == true)
                                {
                                    variablesForCommits.pressRight = 0;
                                    variablesForCommits.right = false;
                                    variablesForCommits.displayPanel = true;
                                    variablesForFiles.nextFile = false;

                                    if (variablesForCommits.panelAlreadyDisplayed == false && variablesForCommits.displayPanel == true)
                                    {
                                        variablesForCommits.panelAlreadyDisplayed = true;
                                        list.ClearAllLists();
                                        GetCommitDetails(variablesForCommits, variablesForFiles, commitElement, list, variablesForCommits.clear);
                                    }
                                    else
                                    {
                                        variablesForCommits.displayPanel = false;
                                        Console.Clear();
                                        DrawLogPanel.DrawLargePanel();
                                    }

                                    GetCommits.PrintCommits(variablesForCommits, variablesForFiles, commitElement, list);
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            if (variablesForCommits.logTab == true)
                            {
                                variablesForCommits.numberOfEnterPresses++;
                                variablesForCommits.enter = true;

                                if (variablesForCommits.numberOfEnterPresses > 1)
                                {
                                    variablesForCommits.enter = false;
                                    variablesForCommits.numberOfEnterPresses = 0;
                                }

                                if (variablesForCommits.right == false)
                                {
                                    variablesForCommits.displayPanel = true;

                                    if (variablesForCommits.panelAlreadyDisplayed == false && variablesForCommits.displayPanel == true)
                                    {
                                        variablesForCommits.panelAlreadyDisplayed = true;
                                        GetCommitDetails(variablesForCommits, variablesForFiles, commitElement, list, variablesForCommits.clear);
                                    }
                                    else
                                    {
                                        variablesForCommits.displayPanel = false;
                                        int i = dimensions.tabHeight + 1;

                                        while (i < Console.WindowHeight)
                                        {
                                            Console.SetCursorPosition(0, i);
                                            Console.Write(new string(' ', dimensions.width + 1));
                                            i++;
                                        }

                                        DrawLogPanel.DrawLargePanel();
                                    }

                                    GetCommits.PrintCommits(variablesForCommits, variablesForFiles, commitElement, list);
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        {
                            if (variablesForCommits.logTab == true)
                            {
                                if (variablesForCommits.pressRight == 2)
                                {
                                    variablesForCommits.esc = true;
                                    variablesForCommits.pressRight = 0;
                                    variablesForFiles.diffMoves = false;
                                    variablesForFiles.up = false;
                                    variablesForFiles.numberOfNavigations = 0;
                                    variablesForFiles.nextFile = false;
                                    HandleRightArrowLog(variablesForCommits, variablesForFiles, commitElement, list);
                                }
                            }
                            else
                            {
                                if (variablesForCommits.right == true)
                                {
                                    variablesForFiles.statusDiffStartNavigate = true;
                                    variablesForFiles.statusDiffOpen = false;
                                    variablesForFiles.down = false;
                                    variablesForFiles.up = false;
                                    variablesForCommits.right = false;
                                    variablesForCommits.esc = true;
                                    variablesForFiles.end = false;
                                    variablesForCommits.pressRight = 1;
                                    variablesForFiles.index = 0;
                                    Tabs.ChooseStatusTab(commitElement, variablesForCommits, variablesForFiles, list);
                                }
                                else
                                {
                                    CloseApplication();
                                }
                            }
                        }
                        break;

                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            variablesForFiles.fileRow = 5;
                            variablesForFiles.fileIndex = 0;
                            variablesForFiles.indexDiff = -1;
                            variablesForFiles.index = 0;
                            variablesForFiles.down = false;
                            variablesForFiles.statusDiffOpen = false;
                            variablesForCommits.right = false;
                           
                            if (variablesForCommits.logTab == true)
                            {
                                list.ClearAllLists();
                                variablesForCommits.pressRight = 1;
                                variablesForCommits.logTab = false;
                                string path = string.Empty;
                                GetDiffs.CleaningEntireDiffPanel(variablesForCommits);
                                DrawStatus.DrawPanelsForStatus();
                                Tabs.SetInitialState(commitElement, variablesForCommits, variablesForFiles, list);
                            }
                        }
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            variablesForCommits.logTab = true;
                            tab.finishUpMoves = false;
                            int i = dimensions.tabHeight + 1;
                            Tabs.ChooseLogTab(commitElement, variablesForCommits, variablesForFiles, list);
                            GetListOfCommits.GetAllCommits(commitElement.repo);
                        }
                        break;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

       
        public static void CleaningFilePanel(DrawPanelRigthSide.FilesBox panel)
        {
            for (int i = 2; i <= panel.height - 2; i++)
            {
                Console.SetCursorPosition(1, panel.edgeOneY + i);
                Console.Write(new string(' ', panel.width + 8));
            }

            Console.SetCursorPosition(1, panel.edgeOneY + 2);
        }

        public static void GetCommitDetails(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, bool clear)
        {
            IntPtr commitPtr = IntPtr.Zero;
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            GetVariablesForTabs tabs = new GetVariablesForTabs();   
            int i = 1;

            if (clear == true)
            {
                ClearConsole.Clear();

                if (variablesForCommits.right == true)
                {
                    DrawPanelLeftSide.Info();
                }
                else
                {
                    DrawPanelRigthSide.Info();
                }
            }

            HeaderPanel.Header(variablesForCommits);
            Info.GetInfo(variablesForCommits, commitElement);
            Message.ReturnMessage(variablesForCommits.currentCommitIndex, commitElement, variablesForCommits);
            LibGit2Wrapper.GitOid oid = commitElement.IdGitOid[variablesForCommits.currentCommitIndex];

            if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElement.repo, ref oid) == 0)
            {
                GetLogFiles.GetFilesAffectedByCommit(commitElement.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElement);
            }
        }

        private static void CloseApplication()
        {
            Console.Clear();
            Environment.Exit(0);
        }

        private static void HandleDiffUpMovesStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement, GetVariablesForTabs tab)
        {
            List<List<string>> diff = new List<List<string>>();
            List<List<int>> indexes = new List<List<int>>();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            variablesForFiles.up = true;
            variablesForFiles.down = false;

            if (variablesForFiles.unstageChanges == true)
            {
                diff = list.unstagedChangesDiff;
                indexes = list.startingIndexesUnstaged;
            }
            else
            {
                diff = list.stagedChangesDiff;
                indexes = list.startingIndexesStaged;
            }

            if (diff[variablesForFiles.indexDiff].Count > (Console.WindowHeight - 2) - 3 && indexes[variablesForFiles.indexDiff].Contains(variablesForFiles.index))
            {
                int i = dimensions.tabHeight + 1;

                while (i < Console.WindowHeight)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write(new string(' ', dimensions.width + 1));
                    i++;
                }

                DrawLogPanel.DrawLargePanel();

                if (variablesForFiles.lastLine == 0)
                {
                    variablesForFiles.startAt = indexes[variablesForFiles.indexDiff].Count - 1;
                    variablesForFiles.lastLine++;
                }

                if (variablesForFiles.startAt > 0)
                {
                    variablesForFiles.startAt--;
                }

                variablesForFiles.index = indexes[variablesForFiles.indexDiff][variablesForFiles.startAt];
                variablesForFiles.up = false;
                variablesForFiles.down = false;
                variablesForFiles.row = dimensions.tabHeight + 1;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);

            }

            variablesForFiles.numberOfNavigations = 1;
            if (variablesForFiles.index > 0)
            {
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
            }
        }

        private static void HandleDiffDownMovesStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement, GetVariablesForTabs tab)
        {
            List<List<string>> diff = new List<List<string>>();
            List<List<int>> indexes = new List<List<int>>();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            variablesForFiles.down = true;
            variablesForFiles.up = false;

            if (variablesForFiles.unstageChanges == true)
            {
                diff = list.unstagedChangesDiff;
                indexes = list.startingIndexesUnstaged;
                variablesForFiles.lastLine = 0;
            }
            else
            {
                diff = list.stagedChangesDiff;
                indexes = list.startingIndexesStaged;
                variablesForFiles.lastLine = 0;
            }

            if (variablesForFiles.statusDiffStartNavigate == true)
            {
                variablesForFiles.row = dimensions.tabHeight + 1;
                variablesForFiles.index = 0;
                variablesForFiles.statusDiffStartNavigate = false;
            }

            if (variablesForFiles.row == Console.WindowHeight - 3)
            {
                variablesForFiles.row = dimensions.tabHeight + 1;
                variablesForFiles.index++;

                if (!indexes[variablesForFiles.indexDiff].Contains(variablesForFiles.index))
                {
                    indexes[variablesForFiles.indexDiff].Add(variablesForFiles.index);
                }

                int i = dimensions.tabHeight + 1;
               
                while (i < Console.WindowHeight)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write(new string(' ', dimensions.width + 1));
                    i++;
                }
                
                DrawLogPanel.DrawLargePanel();
                variablesForFiles.down = false;
            }

            if (variablesForFiles.row == Console.WindowHeight - 2 && variablesForFiles.index < diff[variablesForFiles.indexDiff].Count - 1)
            {
                variablesForFiles.row = dimensions.tabHeight + 1;
            }

            DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
        }

        private static void HandleFilesUpStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement, GetVariablesForTabs tab)
        {
            var filelist = new List<string>();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            int y = 0;
            int x = 1;

            if (variablesForFiles.unstageChanges == true && variablesForFiles.fileIndex <= list.unstagedChangesFiles.Count - 1)
            {
                filelist = list.unstagedChangesFiles;
                y = dimensions.unstagedStart;
            }
            else
            {
                filelist = list.stagedChangesFiles;
            }

            if (variablesForFiles.fileRow > y && filelist.Count > 0)
            {
                Console.SetCursorPosition(1, variablesForFiles.fileRow);
                Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                Console.SetCursorPosition(1, variablesForFiles.fileRow);
                GetAllFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, variablesForFiles.fileRow, variablesForFiles.fileIndex);
                
                if (variablesForFiles.fileRow == dimensions.stagedStart && variablesForFiles.fileIndex == 0 && variablesForFiles.up == true)
                {
                    variablesForFiles.unstageChanges = true;
                    variablesForFiles.stageChanges = false;
                    filelist = list.unstagedChangesFiles;
                    
                    if (list.unstagedChangesFiles.Count > dimensions.unstagedEnd - dimensions.unstagedStart)
                    {
                        variablesForFiles.fileRow = dimensions.unstagedStart;
                    }
                    else
                    {
                        variablesForFiles.fileRow = list.unstagedChangesFiles.Count - 1 + dimensions.unstagedStart;
                    }

                    variablesForFiles.indexDiff = list.unstagedChangesFiles.Count;
                    variablesForFiles.fileIndex = list.unstagedChangesFiles.Count - 1;
                }
                else
                {
                    variablesForFiles.fileIndex--;
                    variablesForFiles.fileRow--;
                }

                Console.SetCursorPosition(1, variablesForFiles.fileRow);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(filelist[variablesForFiles.fileIndex]);
                Console.ResetColor();
                
                variablesForFiles.up = true;
                Cursor.UpdateCursorPositionForFilesList(variablesForFiles, variablesForCommits, list, size);
                variablesForFiles.up = false;
                variablesForFiles.indexDiff--;

                if (variablesForFiles.indexDiff >= 0)
                {
                    variablesForFiles.down = false;
                    variablesForFiles.up = false;
                    GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                    DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
                }
            }
            else if (variablesForFiles.fileRow == y  && filelist.Count > 0)
            {
                int i = variablesForFiles.fileRow;
                int j = 0;

                while (j <= dimensions.unstagedEnd - dimensions.unstagedStart)
                {
                    Console.SetCursorPosition(1, i);
                    Console.Write(new string(' ', dimensions.changesPanelWidth - 2));
                    j++;
                    i++;
                }

                variablesForFiles.fileIndex = 0;
                Console.SetCursorPosition(1, variablesForFiles.fileRow);
                GetAllFiles.PrintRemaingingFiles(variablesForFiles, variablesForCommits, list);
                Console.SetCursorPosition(1, y);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(filelist[variablesForFiles.fileIndex]);
                Console.ResetColor();
                variablesForFiles.indexDiff = 0;
                variablesForFiles.down = false;
                GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                tab.finishUpMoves = true;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
            }
        }

        private static void HandleFilesDownMovesStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement, GetVariablesForTabs tab)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            var filelist = new List<string>();
            int y = 0;
            int x = 1;

            if (variablesForFiles.unstageChanges == true)
            {
                filelist = list.unstagedChangesFiles;
                y = dimensions.changesPanelHeight;
            }
            else
            {
                filelist = list.stagedChangesFiles;
                y = Console.WindowHeight - 1;
            }

            if (variablesForFiles.fileIndex <= filelist.Count - 1)
            {
                if (variablesForFiles.fileRow < y)
                {
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                    GetAllFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, variablesForFiles.fileRow, variablesForFiles.fileIndex);
                    variablesForFiles.fileIndex++;
                    variablesForFiles.fileRow++;

                    if (variablesForFiles.fileIndex > filelist.Count - 1 && variablesForFiles.unstageChanges == true)
                    {
                        variablesForFiles.unstageChanges = false;
                        variablesForFiles.stageChanges = true;
                        filelist = list.stagedChangesFiles;
                        variablesForFiles.fileRow = dimensions.stagedStart;
                        variablesForFiles.fileIndex = 0;
                        variablesForFiles.indexDiff = -1;
                    }

                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filelist[variablesForFiles.fileIndex]);
                    Console.ResetColor();
                    variablesForFiles.down = true;
                    Cursor.UpdateCursorPositionForFilesList(variablesForFiles, variablesForCommits, list, size);
                    variablesForFiles.down = false;
                }
                else
                {

                    int i = dimensions.tabHeight + 3;
                    int j = 0;

                    while (j <= dimensions.unstagedEnd - dimensions.unstagedStart)
                    {
                        Console.SetCursorPosition(1, i);
                        Console.Write(new string(' ', dimensions.changesPanelWidth - 2));
                        j++;
                        i++;
                    }

                    variablesForFiles.fileIndex++;
                    GetAllFiles.PrintRemaingingFiles(variablesForFiles, variablesForCommits, list);
                   
                    if (variablesForFiles.unstageChanges == true)
                    {
                        variablesForFiles.fileRow = dimensions.unstagedStart;
                    }
                    else
                    {
                        variablesForFiles.fileRow = dimensions.stagedStart;
                    }

                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filelist[variablesForFiles.fileIndex]);
                    Console.ResetColor();
                }

                variablesForFiles.indexDiff++;
                variablesForFiles.down = false;
                GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
            }
        }

        private static void HandleDiffUpMovesLog(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
            variablesForFiles.up = true;
            variablesForFiles.down = false;
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (list.listOfAllDiffs[variablesForFiles.indexDiff].Count > Console.WindowHeight - 2 && list.startingIndexesLog[variablesForFiles.indexDiff].Contains(variablesForFiles.index))
            {
                GetDiffs.CleaningEntireDiffPanel(variablesForCommits);
                DrawLogPanel.DrawLargePanel();
                
                if (variablesForFiles.indexForLog > 0)
                {
                    variablesForFiles.indexForLog--;
                }

                variablesForFiles.index = list.startingIndexesLog[variablesForFiles.indexDiff][variablesForFiles.indexForLog];
                variablesForFiles.up = false;
                variablesForFiles.down = false;
                variablesForFiles.row = dimensions.tabHeight + 1;
                variablesForFiles.currentLine = variablesForFiles.index + 1;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);

            }

            variablesForFiles.numberOfNavigations = 1;
            if (variablesForFiles.index > 0)
            {
                variablesForFiles.currentLine--;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
            }
        }

        private static void HandleDiffDownMovesLog(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (variablesForFiles.stop == list.listOfAllDiffs[variablesForFiles.indexDiff].Count)
            {
                variablesForFiles.row = dimensions.tabHeight + 1;
                variablesForFiles.stop = 0;
            }
           

            if (variablesForCommits.pressRight == 2)
            {
                variablesForFiles.diffMoves = true;
            }

            if (variablesForFiles.index == list.listOfAllDiffs[variablesForFiles.indexDiff].Count)
            {
                variablesForFiles.numberOfNavigations++;
            }

            variablesForFiles.down = true;

            if (variablesForFiles.diffMoves == true && variablesForFiles.currentLine < list.listOfAllDiffs[variablesForFiles.indexDiff].Count)
            {
                variablesForFiles.up = false;
                //if (variablesForFiles.row == Console.WindowHeight - 2)
                //{
                //    variablesForFiles.index = variablesForFiles.index - (Console.WindowHeight - 2);
                //}

                if (variablesForFiles.row == Console.WindowHeight - 3)
                {
                    variablesForFiles.index++;
                    list.startingIndexesLog[variablesForFiles.indexDiff].Add(variablesForFiles.index);
                    variablesForFiles.indexForLog++;
                    variablesForFiles.row = dimensions.tabHeight + 1;
                    GetDiffs.CleaningEntireDiffPanel(variablesForCommits);
                    variablesForFiles.down = false;
                    DrawLogPanel.DrawLargePanel();
                }
                else if (variablesForFiles.row == Console.WindowHeight - 2)
                {
                    variablesForFiles.row = dimensions.tabHeight + 1;
                }

                variablesForFiles.currentLine++;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
            }
        }

        private static void HandleRightArrowLog(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            GetVariablesForTabs tabs = new GetVariablesForTabs();

            if (variablesForCommits.enter == true)
            {
                variablesForCommits.pressRight++;
                if (variablesForCommits.pressRight == 1)
                {
                    variablesForCommits.right = true;
                    variablesForFiles.fileIndex = 0;
                    variablesForFiles.row = dimensions.tabHeight + 2;
                    variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;
                    variablesForFiles.currentLine = 1;
                    variablesForFiles.indexForLog = 0;
                    variablesForFiles.index = 0;
                    variablesForFiles.down = false;
                    GetCommitDetails(variablesForCommits, variablesForFiles, commitElement, list, variablesForCommits.clear);
                    variablesForCommits.stopWorkingOnCommits = true;
                }
                else
                {
                    int j = dimensions.tabHeight + 1;

                    while (j < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(0, j);
                        Console.Write(new string(' ', dimensions.width + 1));
                        j++;
                    }

                    int i = 1;
                    DrawLogPanel.DrawLargePanel();
                    LibGit2Wrapper.GitOid oid = commitElement.IdGitOid[variablesForCommits.currentCommitIndex];
                    variablesForFiles.down = false;
                    variablesForFiles.fileIndex = 0;
                    variablesForFiles.row = dimensions.tabHeight + 1;
                    variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;
                    variablesForFiles.currentLine = 1;
                    variablesForFiles.diffMoves = false;
                    variablesForFiles.index = 0;

                    IntPtr commitPtr = IntPtr.Zero;

                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElement.repo, ref oid) == 0)
                    {
                        GetLogFiles.GetFilesAffectedByCommit(commitElement.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElement);
                    }

                    variablesForCommits.pressRight = 1;
                }
            }
        }

        private static void HandleRightArrowStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            variablesForFiles.index = 0;
            variablesForFiles.down = false;
            variablesForCommits.right = true;
            variablesForFiles.row = dimensions.tabHeight + 1;
            int i = variablesForFiles.row;

            while (i <= Console.WindowHeight - 1)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
                i++;
            }

            DrawLogPanel.DrawLargePanel();
            DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
        }

        private static void HandleFilesUpMovesLog(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement)
        {
            int x = 0;
            int y = 0;

            if (variablesForCommits.pressRight == 0)
            {
                y = variablesForFiles.fileRow;
                x = Console.WindowWidth / 2 + 2;
            }
            else
            {
                y = variablesForFiles.fileRow;
                x = 1;
            }

            var panel = new DrawPanelRigthSide.FilesBox();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            variablesForFiles.down = false;

            if (variablesForFiles.fileRow == panel.edgeOneY + 2 && variablesForFiles.fileIndex > 0)
            {
                CleaningFilePanel(panel);
                variablesForFiles.fileIndex = 0;
                GetAllFiles.PrintRemaingingFiles(variablesForFiles, variablesForCommits, list);
                Console.SetCursorPosition(1, variablesForFiles.fileRow);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(list.listOfFiles[variablesForFiles.fileIndex]);
                Console.ResetColor();
                Console.SetCursorPosition(1, variablesForFiles.fileRow);
                
                GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                IntPtr commitPtr = IntPtr.Zero;
                int i = 1;
                variablesForFiles.row = dimensions.tabHeight + 1;
                variablesForFiles.nextFile = true;

                LibGit2Wrapper.GitOid oid = commitElement.IdGitOid[variablesForCommits.currentCommitIndex];
                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElement.repo, ref oid) == 0)
                {
                    GetLogFiles.GetFilesAffectedByCommit(commitElement.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElement);
                }

            }

            if (variablesForFiles.fileIndex > 0)
            {
                GetAllFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, variablesForFiles.fileIndex);
                Console.SetCursorPosition(1, y - 1);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(list.listOfFiles[variablesForFiles.fileIndex - 1]);
                Console.ResetColor();
                variablesForFiles.up = true;
                //Cursor.UpdateCursorPositionForFilesList(variablesForFiles, list, size);
                variablesForFiles.up = false;
                Console.SetCursorPosition(1, y - 1);
                variablesForFiles.fileRow--;
                variablesForFiles.fileIndex--;

                GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                IntPtr commitPtr = IntPtr.Zero;
                int i = 1;
                variablesForFiles.row = dimensions.tabHeight + 1;
                variablesForFiles.nextFile = true;

                LibGit2Wrapper.GitOid oid = commitElement.IdGitOid[variablesForCommits.currentCommitIndex];
                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElement.repo, ref oid) == 0)
                {
                    GetLogFiles.GetFilesAffectedByCommit(commitElement.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElement);
                }
            }
        }

        private static void HandleFilesDownMovesLog(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();

            if (variablesForFiles.fileIndex < list.listOfFiles.Count - 1)
            {
                if (variablesForFiles.fileRow == Console.WindowHeight - 2)
                {
                    DrawPanelRigthSide.FilesBox panel = new DrawPanelRigthSide.FilesBox();
                    CleaningFilePanel(panel);
                    variablesForFiles.fileRow = variablesForFiles.fileRow - panel.height + 4;
                    variablesForFiles.fileIndex++;
                    GetAllFiles.PrintRemaingingFiles(variablesForFiles, variablesForCommits, list);
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(list.listOfFiles[variablesForFiles.fileIndex]);
                    Console.ResetColor();
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);


                    GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                    int i = 1;
                    IntPtr commitPtr = IntPtr.Zero;
                    variablesForFiles.row = dimensions.tabHeight + 1;
                    variablesForFiles.nextFile = true;
                    variablesForFiles.down = false;
                    LibGit2Wrapper.GitOid oid = commitElements.IdGitOid[variablesForCommits.currentCommitIndex];
                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElements.repo, ref oid) == 0)
                    {
                        GetLogFiles.GetFilesAffectedByCommit(commitElements.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElements);
                    }

                }
                else
                {
                    int y = variablesForFiles.fileRow;
                    int x = 1;
                    GetAllFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, variablesForFiles.fileIndex);
                    Console.SetCursorPosition(1, y + 1);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(list.listOfFiles[variablesForFiles.fileIndex + 1]);
                    Console.ResetColor();
                    variablesForFiles.down = true;
                    variablesForFiles.up = false;
                    Cursor.UpdateCursorPositionForFilesList(variablesForFiles, variablesForCommits, list, size);
                    Console.SetCursorPosition(1, y + 1);
                    variablesForFiles.fileRow++;
                    variablesForFiles.fileIndex++;
                    GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                    IntPtr commitPtr = IntPtr.Zero;
                    int i = 1;
                    variablesForFiles.row = dimensions.tabHeight + 1;
                    variablesForFiles.nextFile = true;
                    variablesForFiles.down = false;
                    LibGit2Wrapper.GitOid oid = commitElements.IdGitOid[variablesForCommits.currentCommitIndex];
                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElements.repo, ref oid) == 0)
                    {
                        GetLogFiles.GetFilesAffectedByCommit(commitElements.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElements);
                    }
                }
            }
        }

        private static void HandleCommitsUpMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, int blueFond)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (variablesForCommits.currentCommitIndex < commitElement.Id.Count && variablesForCommits.currentCommitIndex > 0)
            {
                variablesForCommits.up = true;
                variablesForCommits.down = false;
                variablesForCommits.heightPosition--;
                variablesForCommits.currentCommitIndex--;
                
                if (variablesForCommits.cursorPosition > 0)
                {
                    variablesForCommits.cursorPosition--;
                }

                bool reachLimit = false;

                if (variablesForCommits.heightPosition == Console.WindowHeight - 2 || variablesForCommits.heightPosition == dimensions.tabHeight + 1)
                {
                    variablesForCommits.heightPosition = dimensions.tabHeight + 2;
                    ReplaceEachCommitOneByOne.PrintNewCommitIfReachLimit(variablesForCommits, variablesForFiles, commitElement, list, blueFond);
                }

                VerifySize(variablesForCommits, variablesForFiles, commitElement, list);

                if (variablesForCommits.displayPanel == true)
                {
                    ReplaceEachCommitOneByOne.PrintNewCommitIfPanel(variablesForCommits, variablesForFiles, commitElement, list, reachLimit, blueFond);
                }
                else
                {
                    ReplaceEachCommitOneByOne.PrintNewCommitIfNoPanel(variablesForCommits, variablesForFiles, commitElement, list, reachLimit, blueFond);
                }
            }
        }

        private static void HandleCommitsDownMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, int blueFond)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (variablesForCommits.currentCommitIndex < commitElement.Id.Count - 1)
            {
                if (variablesForCommits.currentCommitIndex == 0)
                {
                    variablesForCommits.heightPosition = dimensions.tabHeight + 2;
                }

                variablesForCommits.down = true;
                variablesForCommits.up = false;
                bool reachLimit = false;

                if (variablesForCommits.currentCommitIndex > variablesForCommits.height - 4 && variablesForCommits.cursorPosition == 0 || variablesForCommits.cursorPosition < (variablesForCommits.currentCommitIndex - Console.WindowHeight) - 4 && variablesForCommits.heightPosition == Console.WindowHeight - 2)
                {
                    variablesForCommits.cursorPosition = variablesForCommits.currentCommitIndex - (Console.WindowHeight - 4);
                    variablesForCommits.currentCommitIndex = variablesForCommits.cursorPosition;
                }

                if (variablesForCommits.heightPosition == Console.WindowHeight - 2)
                {
                    if (variablesForCommits.cursorPosition == commitElement.Id.Count - (Console.WindowHeight - dimensions.tabHeight - 3))
                    {
                        variablesForCommits.cursorPosition = 2;
                    }

                    variablesForCommits.currentCommitIndex = variablesForCommits.cursorPosition;
                    variablesForCommits.cursorPosition++;
                    variablesForCommits.heightPosition = dimensions.tabHeight + 2;
                    ReplaceEachCommitOneByOne.PrintNewCommitIfReachLimit(variablesForCommits, variablesForFiles, commitElement, list, blueFond);
                }

                VerifySize(variablesForCommits, variablesForFiles, commitElement, list);

                if (variablesForCommits.displayPanel == true)
                {
                    ReplaceEachCommitOneByOne.PrintNewCommitIfPanel(variablesForCommits, variablesForFiles, commitElement, list, reachLimit, blueFond);
                }
                else
                {
                    ReplaceEachCommitOneByOne.PrintNewCommitIfNoPanel(variablesForCommits, variablesForFiles, commitElement, list, reachLimit, blueFond);
                }
            }
        }

        private static void VerifySize(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements listOfCommits, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (Console.WindowHeight != (variablesForCommits.height - dimensions.tabHeight) - 1 && Console.WindowWidth != variablesForCommits.width)
            {
                variablesForCommits.height = (variablesForCommits.height - dimensions.tabHeight) - 1;
                variablesForCommits.width = Console.WindowWidth - 2;

                Console.Clear();
                DrawLogPanel.DrawLargePanel();
                GetCommits.PrintCommits(variablesForCommits, variablesForFiles, listOfCommits, list);
            }
        }
    }
}