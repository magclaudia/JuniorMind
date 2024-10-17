using System.Collections.Generic;
using System.IO;

namespace GitClient
{
    public class Navigate
    {
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
        private static DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();


        public static void NavigateThroughCommits(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, GetVariablesForTabs tab)
        {
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
                                    HandleFilesUpMoves(variablesForCommits, variablesForFiles, list, commitElement);
                                }
                                else if (variablesForCommits.pressRight == 2)
                                {
                                    if (variablesForFiles.index == 0 && variablesForFiles.up == true)
                                    {
                                        variablesForFiles.end = true;
                                        variablesForFiles.up = false;
                                        break;
                                    }

                                    HandleDiffUpMoves(variablesForCommits, variablesForFiles, list, commitElement);
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
                                if (variablesForFiles.stageChanges == true && variablesForFiles.fileIndex == 0 && variablesForFiles.fileRow == dimensions.stagedStart)
                                {
                                    variablesForFiles.up = true;
                                }

                                if (variablesForFiles.up == false && variablesForFiles.fileIndex == 0 || variablesForCommits.right == true)
                                {
                                    break;
                                }

                                HandleStatusFilesUp(variablesForCommits, variablesForFiles, list, commitElement, tab);
                            }
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        {
                            if (variablesForCommits.logTab == true)
                            {
                                if (variablesForCommits.stopWorkingOnCommits == true && variablesForCommits.pressRight == 1)
                                {
                                    HandleFilesDownMoves(variablesForCommits, variablesForFiles, list, commitElement);
                                }
                                else if (variablesForCommits.pressRight == 2)
                                {
                                    if (variablesForFiles.index == list.listOfAllDiffs[variablesForFiles.indexDiff].Count - 1 && variablesForFiles.currentLine != 1)
                                    {
                                        break;
                                    }

                                    HandleDiffDownMoves(variablesForCommits, variablesForFiles, list, commitElement);
                                }
                                else
                                {

                                    HandleCommitsDownMoves(variablesForCommits, variablesForFiles, commitElement, list, blueFond);
                                }
                            }
                            else
                            {
                                if (variablesForCommits.right == true)
                                {
                                    break;
                                }

                                if (list.stagedChangesFiles.Count > 0 && variablesForFiles.fileIndex == list.stagedChangesFiles.Count - 1)
                                {
                                    break;
                                }

                                HandleStatusFilesDownMoves(variablesForCommits, variablesForFiles, list, commitElement, tab);
                            }
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        {
                            if (variablesForCommits.logTab == true)
                            {
                                if (variablesForCommits.pressRight == 2)
                                {
                                    break;
                                }

                                HandleRightArrowLog(variablesForCommits, variablesForFiles, commitElement, list);
                            }
                            else
                            {
                                if (variablesForCommits.pressRight > 1)
                                {
                                    break;
                                }

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
                                        DrawExternalBorder.DrawBox();
                                    }

                                    GetCommits.PrintCommits(variablesForCommits, variablesForFiles, commitElement, list);
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
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
                                    Console.Clear();
                                    DrawExternalBorder.DrawBox();
                                }

                                GetCommits.PrintCommits(variablesForCommits, variablesForFiles, commitElement, list);
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
                                    Console.Clear();
                                    variablesForFiles.nextFile = false;
                                    HandleRightArrowLog(variablesForCommits, variablesForFiles, commitElement, list);
                                }
                            }
                            else
                            {
                                if (variablesForCommits.right == true)
                                {
                                    variablesForFiles.down = false;
                                    variablesForCommits.right = false;
                                    variablesForCommits.esc = true;
                                    variablesForCommits.pressRight = 0;
                                    Tabs.ChooseStatusTab(commitElement, variablesForCommits, variablesForFiles, list);
                                }
                            }
                        }
                        break;

                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            variablesForFiles.fileRow = 5;
                            variablesForFiles.fileIndex = 0;
                            variablesForFiles.indexDiff = 0;
                            variablesForFiles.down = false;
                            variablesForCommits.right = false;

                            if (variablesForCommits.logTab == true)
                            {
                                variablesForCommits.pressRight = 1;
                                variablesForCommits.logTab = false;
                                string path = string.Empty;
                                DrawTabs.DrawOnlyTabs();
                                Tabs.GetRepoPath(path);
                                Tabs.GetTabsNames();
                                DrawStatus.DrawPanelsForStatus();
                                StatusTab.GetStatusChangesNames();
                                GetAllFiles.PrintStatusFilesIfAlreadyReceived(variablesForFiles, list, 5, 0);
                                string fileFullName = "";

                                if (list.unstagedChangesFiles.Count > 0)
                                {
                                    fileFullName = list.unstagedChangesFiles[variablesForFiles.fileIndex];
                                    variablesForFiles.fileRow = dimensions.unstagedStart;
                                }
                                else
                                {
                                    fileFullName = list.stagedChangesFiles[variablesForFiles.fileIndex];
                                    variablesForFiles.fileRow = dimensions.stagedStart;
                                }

                                DiffHelper.FilesBackground(fileFullName, variablesForFiles, variablesForCommits);
                                variablesForFiles.down = false;
                                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
                            }

                            Tabs.ChooseStatusTab(commitElement, variablesForCommits, variablesForFiles, list);
                        }
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            variablesForCommits.logTab = true;
                            tab.finishUpMoves = false;
                            Tabs.ChooseLogTab(commitElement, variablesForCommits, variablesForFiles, list);
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }


        public static void GetCommitDetails(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, bool clear)
        {
            IntPtr commitPtr = IntPtr.Zero;
            int i = 1;
            if (clear == true)
            {
                Console.Clear();
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
                Files.GetFilesAffectedByCommit(commitElement.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElement);
            }
        }

        private static void HandleStatusFilesUp(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement, GetVariablesForTabs tab)
        {
            var filelist = new List<string>();
            int y = 0;

            if (variablesForFiles.fileIndex <= list.unstagedChangesFiles.Count - 1)
            {
                filelist = list.unstagedChangesFiles;
                y = 5;
            }
            else
            {
                filelist = list.stagedChangesFiles;
            }

            if (variablesForFiles.fileRow > y)
            {
                Console.SetCursorPosition(1, variablesForFiles.fileRow);
                Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                Console.SetCursorPosition(1, variablesForFiles.fileRow);
                GetAllFiles.ChooseColorForFiles(variablesForFiles, list, variablesForFiles.fileRow, variablesForFiles.fileIndex);
                
                if (variablesForFiles.fileRow == dimensions.stagedStart && variablesForFiles.fileIndex == 0 && variablesForFiles.up == true)
                {
                    variablesForFiles.unstageChanges = true;
                    variablesForFiles.stageChanges = false;
                    variablesForFiles.fileRow = dimensions.unstagedStart;
                    filelist = list.unstagedChangesFiles;
                    variablesForFiles.indexDiff = 1;
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

                variablesForFiles.indexDiff--;

                if (variablesForFiles.indexDiff >= 0)
                {
                    variablesForFiles.down = false;
                    variablesForFiles.up = false;
                    GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                    DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
                }
            }
            else if (variablesForFiles.fileRow == y)
            {
                int i = variablesForFiles.fileRow;
                int j = i - 3;

                while (j < dimensions.changesPanelHeight - 1)
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

        private static void HandleStatusFilesDownMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement, GetVariablesForTabs tab)
        {
            var filelist = new List<string>();

            int y = 0;

            if (variablesForFiles.unstageChanges == true)
            {
                filelist = list.unstagedChangesFiles;
                y = dimensions.changesPanelHeight;
            }
            else
            {
                filelist = list.stagedChangesFiles;
                //variablesForFiles.fileRow = dimensions.stagedStart;
                y = (Console.WindowHeight - 1);
            }

            if (variablesForFiles.fileIndex <= filelist.Count - 1)
            {
                if (variablesForFiles.fileRow <= y)
                {
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, variablesForFiles.fileRow);
                    GetAllFiles.ChooseColorForFiles(variablesForFiles, list, variablesForFiles.fileRow, variablesForFiles.fileIndex);
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
                }
                else
                {

                    int i = variablesForFiles.fileRow;
                    int j = i - 3;

                    while (j < dimensions.changesPanelHeight - 1)
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

        private static void HandleDiffUpMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
            variablesForFiles.up = true;
            variablesForFiles.down = false;

            if (list.listOfAllDiffs[variablesForFiles.indexDiff].Count > Console.WindowHeight - 2 && list.startingIndexesLog[variablesForFiles.indexDiff].Contains(variablesForFiles.index))
            {
                GetDiffs.CleaningEntireDiffPanel(variablesForCommits);
                if (variablesForFiles.x > 0)
                {
                    variablesForFiles.x--;
                }

                variablesForFiles.index = list.startingIndexesLog[variablesForFiles.indexDiff][variablesForFiles.x];
                variablesForFiles.up = false;
                variablesForFiles.down = false;
                variablesForFiles.row = 0;
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

        private static void HandleDiffDownMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
            if (variablesForCommits.pressRight == 2)
            {
                variablesForFiles.diffMoves = true;
            }

            if (variablesForFiles.index == list.listOfAllDiffs[variablesForFiles.indexDiff].Count)
            {
                variablesForFiles.numberOfNavigations++;
            }

            if (variablesForFiles.diffMoves == true && variablesForFiles.currentLine < list.listOfAllDiffs[variablesForFiles.indexDiff].Count)
            {
                variablesForFiles.up = false;
                if (variablesForFiles.row == Console.WindowHeight - 2)
                {
                    variablesForFiles.index = variablesForFiles.index - (Console.WindowHeight - 2);
                }

                if (variablesForFiles.row == Console.WindowHeight - 3)
                {
                    variablesForFiles.index++;
                    list.startingIndexesLog[variablesForFiles.indexDiff].Add(variablesForFiles.index);
                    variablesForFiles.x++;
                    GetDiffs.CleaningEntireDiffPanel(variablesForCommits);
                    variablesForFiles.down = false;
                }

                if (variablesForFiles.row == list.listOfAllDiffs[variablesForFiles.indexDiff].Count)
                {
                    variablesForFiles.row = 0;
                }

                variablesForFiles.down = true;
                variablesForFiles.currentLine++;


                if (variablesForFiles.row == Console.WindowHeight - 3)
                {
                    variablesForFiles.row = 0;
                    variablesForFiles.down = false;
                }

                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElements, list);
            }
        }

        private static void HandleRightArrowLog(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list)
        {
            if (variablesForCommits.enter == true)
            {
                variablesForCommits.pressRight++;
                if (variablesForCommits.pressRight == 1)
                {
                    variablesForCommits.right = true;
                    variablesForFiles.fileIndex = 0;
                    variablesForFiles.row = 0;
                    variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;
                    variablesForFiles.currentLine = 1;
                    variablesForFiles.x = 0;
                    variablesForFiles.index = 0;
                    variablesForFiles.down = false;
                    GetCommitDetails(variablesForCommits, variablesForFiles, commitElement, list, variablesForCommits.clear);
                    variablesForCommits.stopWorkingOnCommits = true;
                }
                else
                {
                    Console.Clear();
                    int i = 1;
                    DrawExternalBorder.DrawBox();
                    LibGit2Wrapper.GitOid oid = commitElement.IdGitOid[variablesForCommits.currentCommitIndex];
                    variablesForFiles.down = false;
                    variablesForFiles.fileIndex = 0;
                    variablesForFiles.row = 0;
                    variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;
                    variablesForFiles.currentLine = 1;
                    variablesForFiles.diffMoves = false;
                    variablesForFiles.index = 0;
                    variablesForFiles.row = 0;

                    IntPtr commitPtr = IntPtr.Zero;

                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElement.repo, ref oid) == 0)
                    {
                        Files.GetFilesAffectedByCommit(commitElement.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElement);
                    }

                    variablesForCommits.pressRight = 1;
                }
            }
        }

        private static void HandleRightArrowStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list)
        {
            variablesForFiles.index = 0;
            variablesForFiles.down = false;
            variablesForCommits.right = true;
            variablesForFiles.row = 3;
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

        private static void HandleFilesUpMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement)
        {
            int y = variablesForFiles.fileRow;
            var panel = new DrawPanelRigthSide.FilesBox();
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
            }


            if (variablesForFiles.fileIndex > 0)
            {
                GetAllFiles.ChooseColorForFiles(variablesForFiles, list, y, variablesForFiles.fileIndex);
                Console.SetCursorPosition(1, y - 1);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(list.listOfFiles[variablesForFiles.fileIndex - 1]);
                Console.ResetColor();

                Console.SetCursorPosition(1, y - 1);
                variablesForFiles.fileRow--;
                variablesForFiles.fileIndex--;

                GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                IntPtr commitPtr = IntPtr.Zero;
                int i = 1;
                variablesForFiles.row = 0;
                variablesForFiles.nextFile = true;

                LibGit2Wrapper.GitOid oid = commitElement.IdGitOid[variablesForCommits.currentCommitIndex];
                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElement.repo, ref oid) == 0)
                {
                    Files.GetFilesAffectedByCommit(commitElement.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElement);
                }
            }
        }

        private static void HandleFilesDownMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
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
                }
                else
                {
                    int y = variablesForFiles.fileRow;
                    GetAllFiles.ChooseColorForFiles(variablesForFiles, list, y, variablesForFiles.fileIndex);
                    Console.SetCursorPosition(1, y + 1);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(list.listOfFiles[variablesForFiles.fileIndex + 1]);
                    Console.ResetColor();

                    Console.SetCursorPosition(1, y + 1);
                    variablesForFiles.fileRow++;
                    variablesForFiles.fileIndex++;
                    GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                    IntPtr commitPtr = IntPtr.Zero;
                    int i = 1;
                    variablesForFiles.row = 0;
                    variablesForFiles.nextFile = true;

                    LibGit2Wrapper.GitOid oid = commitElements.IdGitOid[variablesForCommits.currentCommitIndex];
                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElements.repo, ref oid) == 0)
                    {
                        Files.GetFilesAffectedByCommit(commitElements.repo, commitPtr, i, variablesForCommits, variablesForFiles, list, commitElements);
                    }
                }
            }
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


        private static void HandleCommitsUpMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, int blueFond)
        {
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
                if (variablesForCommits.heightPosition == Console.WindowHeight - 2 || variablesForCommits.heightPosition == 0)
                {
                    variablesForCommits.heightPosition = 1;
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
            if (variablesForCommits.currentCommitIndex < commitElement.Id.Count - 1)
            {
                if (variablesForCommits.currentCommitIndex == 0)
                {
                    variablesForCommits.heightPosition = 1;
                }

                variablesForCommits.down = true;
                variablesForCommits.up = false;
                bool reachLimit = false;
                if (variablesForCommits.currentCommitIndex > variablesForCommits.height && variablesForCommits.cursorPosition == 0 || variablesForCommits.cursorPosition < variablesForCommits.currentCommitIndex - Console.WindowHeight - 2 && variablesForCommits.heightPosition == Console.WindowHeight - 2)
                {
                    variablesForCommits.cursorPosition = variablesForCommits.currentCommitIndex - (Console.WindowHeight - 2) + 2;
                    variablesForCommits.currentCommitIndex = variablesForCommits.cursorPosition;
                }

                if (variablesForCommits.heightPosition == Console.WindowHeight - 2)
                {
                    if (variablesForCommits.cursorPosition == commitElement.Id.Count - (Console.WindowHeight - 3))
                    {
                        variablesForCommits.cursorPosition = 2;
                    }

                    variablesForCommits.currentCommitIndex = variablesForCommits.cursorPosition;
                    variablesForCommits.cursorPosition++;
                    variablesForCommits.heightPosition = 1;
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
            if (Console.WindowHeight != variablesForCommits.height && Console.WindowWidth != variablesForCommits.width)
            {
                variablesForCommits.height = Console.WindowHeight;
                variablesForCommits.width = Console.WindowWidth;
                Console.Clear();
                DrawExternalBorder.DrawBox();
                GetCommits.PrintCommits(variablesForCommits, variablesForFiles, listOfCommits, list);
            }
        }
    }
}