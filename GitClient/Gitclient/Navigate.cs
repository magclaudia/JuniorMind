using LibGit2Sharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughCommits(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElements, GetCertainList list)
        {
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            IntPtr commitPtr = IntPtr.Zero;
            List<string> filesList = new List<string>();
            int blueFond = 0;
            int index = 0;
            int row = 0;

            if (variablesForFiles.unstageChanges == true)
            {
                filesList = list.unstagedChangesFiles;
                index = variablesForFiles.unstagedIndex;
                row = variablesForFiles.fileRowUnstaged;
            }
            else
            {
                filesList = list.stagedChangesFiles;
                index = variablesForFiles.stagedIndex;
                row = variablesForFiles.fileRowStaged;
            }

            ConsoleKeyInfo keyInfo;

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
                                    if (variablesForFiles.fileIndex == 0)
                                    {
                                        break;
                                    }

                                    HandleFilesUpMovesLog(variablesForCommits, variablesForFiles, list, commitElements);
                                }
                                else if (variablesForCommits.pressRight == 2)
                                {
                                    if (variablesForFiles.index == 0 && variablesForFiles.up == true)
                                    {
                                        variablesForFiles.end = true;
                                        variablesForFiles.up = false;
                                        break;
                                    }

                                    HandleDiffUpMovesLog(variablesForCommits, variablesForFiles, list, commitElements);
                                }
                                else
                                {
                                   
                                    if (variablesForCommits.heightPosition == 1 && variablesForCommits.currentCommitIndex == 0)
                                    {
                                        break;
                                    }

                                    HandleCommitsUpMoves(variablesForCommits, variablesForFiles, commitElements, list, blueFond);
                                }
                            }
                            else
                            {
                                if (variablesForFiles.statusDiffOpen == false)
                                {
                                    if (index == 0 && row == dimensions.stagedStart && list.unstagedChangesFiles.Count == 0)
                                    {
                                        break;
                                    }

                                    if (variablesForFiles.stageChanges == true && index == 0 && row == dimensions.stagedStart)
                                    {
                                        variablesForFiles.up = true;
                                    }

                                    if (variablesForFiles.up == false && index == 0 && variablesForFiles.unstageChanges == true || variablesForCommits.right == true)
                                    {
                                        break;
                                    }

                                    if (variablesForFiles.unstageChanges == true && index == 0 && variablesForFiles.up == true)
                                    {
                                        break;
                                    }

                                    HandleFilesUpStatus(variablesForCommits, variablesForFiles, list, commitElements);
                                }
                                else
                                {
                                    if (variablesForFiles.up == true && variablesForFiles.row == dimensions.tabHeight + 2)
                                    {
                                        variablesForFiles.up = false;
                                        break;
                                    }

                                    HandleDiffUpMovesStatus(variablesForCommits, variablesForFiles, list, commitElements);
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
                                    HandleFilesDownMovesLog(variablesForCommits, variablesForFiles, list, commitElements);
                                }
                                else if (variablesForCommits.pressRight == 2)
                                {
                                    if (variablesForFiles.index == list.listOfAllDiffs[variablesForFiles.indexDiff].Count - 1 && variablesForFiles.currentLine != 1)
                                    {
                                        break;
                                    }

                                    HandleDiffDownMovesLog(variablesForCommits, variablesForFiles, list, commitElements);
                                }
                                else
                                {
                                    HandleCommitsDownMoves(variablesForCommits, variablesForFiles, commitElements, list, blueFond);
                                }
                            }
                            else
                            {
                                if (variablesForFiles.statusDiffOpen == false)
                                {

                                    if (variablesForCommits.right == true || filesList.Count == 0 && filesList.Count == 0 || filesList.Count == 1 && filesList.Count == 0)
                                    {
                                        break;
                                    }

                                    if (index == list.stagedChangesFiles.Count - 1 && variablesForFiles.stageChanges == true || list.unstagedChangesFiles.Count == 1 && variablesForFiles.unstageChanges && list.stagedChangesFiles.Count == 0)
                                    {
                                        break;
                                    }

                                    if (index == list.unstagedChangesFiles.Count - 1 && list.stagedChangesFiles.Count == 0)
                                    {
                                        break;
                                    }

                                    HandleFilesDownMovesStatus(variablesForCommits, variablesForFiles, list, commitElements);
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

                                    if (variablesForFiles.enterPress == 0)
                                    {
                                        HandleDiffDownMovesStatus(variablesForCommits, variablesForFiles, list, commitElements);
                                    }
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

                                HandleRightArrowLog(variablesForCommits, variablesForFiles, commitElements, list);
                            }
                            else
                            {
                                if (variablesForCommits.pressRight > 1 || list.unstagedChangesFiles.Count == 0 && list.stagedChangesFiles.Count == 0)
                                {
                                    break;
                                }

                                variablesForFiles.statusDiffOpen = true;
                                variablesForCommits.pressRight++;
                                HandleRightArrowStatus(variablesForCommits, variablesForFiles, commitElements, list);
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
                                variablesForFiles.indexDiff = -1;
                                variablesForFiles.fileIndex = 0;
                                variablesForFiles.fileRow = Console.WindowHeight / 2 + 4;

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
                                        GetCommitDetails(variablesForCommits, variablesForFiles, commitElements, list, variablesForCommits.clear);
                                    }
                                    else
                                    {
                                        variablesForCommits.displayPanel = false;
                                        Console.Clear();
                                        DrawLogPanel.DrawLargePanel();
                                    }

                                    GetCommits.PrintCommits(variablesForCommits, variablesForFiles, commitElements, list);
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
                                        GetCommitDetails(variablesForCommits, variablesForFiles, commitElements, list, variablesForCommits.clear);
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

                                    GetCommits.PrintCommits(variablesForCommits, variablesForFiles, commitElements, list);
                                }
                            }
                            else
                            {
                                if (variablesForFiles.unstageChanges == true)
                                {
                                    if (variablesForCommits.right == true)
                                    {
                                        variablesForFiles.enterPress++;
                                        if (variablesForFiles.enterPress == 1)
                                        {
                                            HandleHunkTransferFromUnstagedToStaged(commitElements, list, variablesForCommits, variablesForFiles);
                                        }
                                    }
                                    else
                                    {
                                        if (list.stagedChangesFiles.Contains(list.unstagedChangesFiles[variablesForFiles.unstagedIndex]))
                                        {
                                            break;
                                        }

                                        HandleFileTransferFromUnstagedToStaged(commitElements, list, variablesForCommits, variablesForFiles);
                                    }
                                }
                                else
                                {
                                    if (variablesForCommits.right == true)
                                    {
                                        variablesForFiles.enterPress++;

                                        if (variablesForFiles.enterPress == 1)
                                        {
                                            HandleHunkTransferFromStagedToUnstaged(commitElements, list, variablesForCommits, variablesForFiles);
                                        }
                                    }
                                    else
                                    {
                                        if (list.unstagedChangesFiles.Contains(list.stagedChangesFiles[variablesForFiles.stagedIndex]))
                                        {
                                            break;
                                        }

                                        HandleFileTransferFromStagedToUnstaged(commitElements, list, variablesForCommits, variablesForFiles);
                                    }
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
                                    HandleRightArrowLog(variablesForCommits, variablesForFiles, commitElements, list);
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
                                    variablesForFiles.enterPress = 0;
                                    Tabs.SetInitialState(commitElements, variablesForCommits, variablesForFiles, list);
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
                           
                            if (list.stagedChangesFiles.Count > 0)
                            {
                                variablesForFiles.statusFilesSufferModifications = true;
                            }
                            
                            if (list.unstagedChangesFiles.Count > 0)
                            {
                                variablesForFiles.unstageChanges = true;
                            }

                            if (variablesForCommits.logTab == true)
                            {
                                variablesForCommits.pressRight = 1;
                                variablesForCommits.logTab = false;
                                string path = string.Empty;
                                list.listOfFiles.Clear();
                                list.listOfAllDiffs.Clear();
                                Tabs.SetInitialState(commitElements, variablesForCommits, variablesForFiles, list);
                            }
                        }
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            variablesForCommits.logTab = true;
                            variablesForFiles.finishUpMoves = false;
                            int i = dimensions.tabHeight + 1;
                            Tabs.ChooseLogTab(commitElements, variablesForCommits, variablesForFiles, list);
                            GetListOfCommits.GetAllCommits(commitElements.repo, list);
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
                Console.Write(new string(' ', panel.width + 9));
            }

            Console.SetCursorPosition(1, panel.edgeOneY + 2);
        }

        public static void GetCommitDetails(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list, bool clear)
        {
            IntPtr commitPtr = IntPtr.Zero;
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
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

        private static void HandleHunkTransferFromUnstagedToStaged(CommitElements commitElements, GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            List<string> diff = new List<string>();
            List<string> diffToBeAddedToStaged = new List<string>();
            string fileToBeTransfer = list.unstagedChangesFiles[variablesForFiles.unstagedIndex];
            diff = list.unstagedChangesDiff[variablesForFiles.unstagedIndex];
            string path = diff[0];
           
            if (diff[variablesForFiles.index].StartsWith('@') && variablesForFiles.enterPress == 1)
            {
                int count = variablesForFiles.index;

                if (!list.stagedChangesFiles.Contains(fileToBeTransfer))
                {
                    list.stagedChangesFiles.Add(fileToBeTransfer);
                    
                    if (list.stagedChangesFiles.Count > 1)
                    {
                        variablesForFiles.stagedIndex++;
                    }

                    list.stagedChangesDiff.Add(new List<string>());
                    diffToBeAddedToStaged = list.stagedChangesDiff[variablesForFiles.stagedIndex];
                    diffToBeAddedToStaged.Add(path);
                }

                //indexForStaged = variablesForFiles.stagedIndex;
                diffToBeAddedToStaged.Add(diff[count]);
                count++;

                while (!diff[count].StartsWith('@'))
                {
                    diffToBeAddedToStaged.Add(diff[count]);
                    
                    if (diff[count].StartsWith('@') || count == diff.Count - 1)
                    {
                        break;
                    }

                    count++;
                }

                for (int i = 0; i < diffToBeAddedToStaged.Count; i++)
                {
                    if (diff.Contains(diffToBeAddedToStaged[i]))
                    {
                        diff.RemoveAt(variablesForFiles.index);
                    }
                }

                if (diff.Count > 1)
                {
                    list.unstagedChangesDiff[variablesForFiles.unstagedIndex] = diff;
                    if (!list.stagedChangesDiff.Contains(diffToBeAddedToStaged))
                    {
                        list.stagedChangesDiff[list.stagedChangesDiff.Count - 1].AddRange(diffToBeAddedToStaged);
                    }
                }
                else
                {
                    list.unstagedChangesFiles.Remove(fileToBeTransfer);
                    list.unstagedChangesDiff.RemoveAt(variablesForFiles.unstagedIndex);
                    
                    if (!list.stagedChangesFiles.Contains(fileToBeTransfer))
                    {
                        list.stagedChangesFiles.Add(fileToBeTransfer);
                        variablesForFiles.stagedIndex++;
                    }

                    if (!list.stagedChangesDiff.Contains(diffToBeAddedToStaged))
                    {
                        list.stagedChangesDiff[list.stagedChangesDiff.Count - 1].AddRange(diffToBeAddedToStaged);
                    }

                    if (list.unstagedChangesFiles.Count == 0)
                    {
                        variablesForFiles.unstageChanges = false;
                        variablesForFiles.stageChanges = true;
                    }
                }

                variablesForFiles.down = false;
                variablesForFiles.statusFilesSufferModifications = true;
            }
        }

        private static void HandleHunkTransferFromStagedToUnstaged(CommitElements commitElements, GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            List<string> diff = new List<string>();
            List<string> diffToBeAddedToUnstaged = new List<string>();
            string fileToBeTransfer = list.stagedChangesFiles[variablesForFiles.stagedIndex];
            diff = list.stagedChangesDiff[variablesForFiles.stagedIndex];
            string path = diff[0];
            int index = variablesForFiles.unstagedIndex;

            if (diff[variablesForFiles.index].StartsWith('@') && variablesForFiles.enterPress == 1)
            {
                int count = variablesForFiles.index;
                int indexForUnstaged = 0;

                if (!list.unstagedChangesFiles.Contains(fileToBeTransfer))
                {
                    list.unstagedChangesFiles.Add(fileToBeTransfer);

                    if (list.unstagedChangesFiles.Count > 1)
                    {
                        index++;
                    }

                    list.unstagedChangesDiff.Add(new List<string>());
                    diffToBeAddedToUnstaged = list.unstagedChangesDiff[index];
                    diffToBeAddedToUnstaged.Add(path);
                }

                indexForUnstaged = index;
                diffToBeAddedToUnstaged.Add(diff[count]);
                count++;

                while (!diff[count].StartsWith('@'))
                {
                    diffToBeAddedToUnstaged.Add(diff[count]);

                    if (diff[count].StartsWith('@') || count == diff.Count - 1)
                    {
                        break;
                    }

                    count++;
                }

                for (int i = 0; i < diffToBeAddedToUnstaged.Count; i++)
                {
                    if (diff.Contains(diffToBeAddedToUnstaged[i]))
                    {
                        diff.RemoveAt(variablesForFiles.index);
                    }
                }

                if (diff.Count > 1)
                {
                    list.stagedChangesDiff[variablesForFiles.stagedIndex] = diff;
                    //list.stagedChangesDiff.Add(diff);

                    if (!list.unstagedChangesDiff.Contains(diffToBeAddedToUnstaged))
                    {
                        list.unstagedChangesDiff[list.unstagedChangesDiff.Count - 1].AddRange(diffToBeAddedToUnstaged);
                    }
                }
                else
                {
                    list.stagedChangesFiles.Remove(fileToBeTransfer);
                    list.stagedChangesDiff.RemoveAt(variablesForFiles.stagedIndex);

                    if (!list.unstagedChangesFiles.Contains(fileToBeTransfer))
                    {
                        list.unstagedChangesFiles.Add(fileToBeTransfer);
                        index++;
                    }

                    if (!list.unstagedChangesDiff.Contains(diffToBeAddedToUnstaged))
                    {
                        list.unstagedChangesDiff[list.unstagedChangesDiff.Count - 1].AddRange(diffToBeAddedToUnstaged);
                    }


                    if (list.stagedChangesFiles.Count == 0)
                    {
                        variablesForFiles.stageChanges = false;
                        variablesForFiles.unstageChanges = true;
                    }
                }

                variablesForFiles.down = false;
                variablesForFiles.statusFilesSufferModifications = true;
            }
        }

        private static void HandleFileTransferFromStagedToUnstaged(CommitElements commitElements, GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            List<string> diffToBeAdded = new List<string>();

            string fileToBeTransfer = list.stagedChangesFiles[variablesForFiles.stagedIndex];
            diffToBeAdded = list.stagedChangesDiff[variablesForFiles.stagedIndex];
            list.stagedChangesFiles.Remove(fileToBeTransfer);
            list.stagedChangesDiff.RemoveAt(variablesForFiles.stagedIndex);
            list.unstagedChangesFiles.Add(fileToBeTransfer);
            list.unstagedChangesDiff.Add(diffToBeAdded);
            variablesForFiles.down = false;
            variablesForFiles.statusFilesSufferModifications = true;
            variablesForFiles.stageChanges = false;
            variablesForFiles.unstageChanges = true;
            int startFrom = 0;
            
            if (list.unstagedChangesFiles.Count <= dimensions.unstagedEnd - dimensions.unstagedStart + 1)
            {
                variablesForFiles.unstagedIndex = 0;
                variablesForFiles.indexDiff = 0;
                variablesForFiles.fileRowUnstaged = dimensions.unstagedStart;
                startFrom = 0;
            }
            else
            {
                variablesForFiles.unstagedIndex = list.unstagedChangesFiles.IndexOf(fileToBeTransfer);
                variablesForFiles.indexDiff = variablesForFiles.unstagedIndex;
                startFrom = variablesForFiles.fileRowUnstaged - (dimensions.unstagedEnd - dimensions.unstagedStart + 1);
            }

            if (variablesForFiles.stagedIndex == list.stagedChangesFiles.Count && variablesForFiles.stagedIndex > 0)
            {
                variablesForFiles.stagedIndex--;
                variablesForFiles.indexDiff--;
                variablesForFiles.fileRowStaged--;
            }

            int b = variablesForFiles.unstagedIndex - startFrom;
            list.unstagedFilesStartAt.Clear();
            list.unstagedFilesStartAt.Add(b);
            Tabs.SetInitialState(commitElements, variablesForCommits, variablesForFiles, list);
        }

        private static void HandleFileTransferFromUnstagedToStaged(CommitElements commitElements, GetCertainList list, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            List<string> diffToBeAdded = new List<string>();
            
            string fileToBeTransfer = list.unstagedChangesFiles[variablesForFiles.unstagedIndex];
            diffToBeAdded = list.unstagedChangesDiff[variablesForFiles.unstagedIndex];
            list.unstagedChangesFiles.Remove(fileToBeTransfer);
            list.unstagedChangesDiff.RemoveAt(variablesForFiles.unstagedIndex);
            list.stagedChangesFiles.Add(fileToBeTransfer);
            list.stagedChangesDiff.Add(diffToBeAdded);
            variablesForFiles.down = false;
            variablesForFiles.statusFilesSufferModifications = true;
            
            int startFrom =  variablesForFiles.fileRowUnstaged - (dimensions.unstagedEnd - dimensions.unstagedStart + 1);
            int b = variablesForFiles.unstagedIndex - startFrom;
            list.stagedFilesStartAt.Clear();

            if (list.stagedChangesFiles.Count > dimensions.stagedEnd - dimensions.stagedStart && list.stagedChangesFiles.Count > 0)
            {
                list.stagedFilesStartAt.Add(b);
            }
            else
            {
                list.stagedFilesStartAt.Add(0);
            }

            if (list.unstagedChangesFiles.Count == 0)
            {
                list.stagedFilesStartAt.Clear();
                list.stagedFilesStartAt.Add(0);
            }

            int i = 0;
            while (i < list.stagedChangesFiles.Count)
            {
                list.stagedDiffListStartAt.Add(new List<int>());
                list.stagedDiffListStartAt[i].Add(0);
                i++;
            }

            Tabs.SetInitialState(commitElements, variablesForCommits, variablesForFiles, list);
        }
        private static void CloseApplication()
        {
            Console.Clear();
            Environment.Exit(0);
        }

        private static void HandleDiffUpMovesStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement)
        {
            List<List<string>> diff = new List<List<string>>();
            List<List<int>> indexes = new List<List<int>>();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            variablesForFiles.up = true;
            variablesForFiles.down = false;

            if (variablesForFiles.unstageChanges == true)
            {
                diff = list.unstagedChangesDiff;
                indexes = list.unstagedDiffListStartAt;
            }
            else
            {
                diff = list.stagedChangesDiff;
                indexes = list.stagedDiffListStartAt;
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
                    variablesForFiles.diffStartAt = indexes[variablesForFiles.indexDiff].Count - 1;
                    variablesForFiles.lastLine++;
                }

                if (variablesForFiles.diffStartAt > 0)
                {
                    variablesForFiles.diffStartAt--;
                }

                variablesForFiles.index = indexes[variablesForFiles.indexDiff][variablesForFiles.diffStartAt];
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

        private static void HandleDiffDownMovesStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement)
        {
            List<List<string>> diff = new List<List<string>>();
            List<List<int>> indexes = new List<List<int>>();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            variablesForFiles.down = true;
            variablesForFiles.up = false;

            if (variablesForFiles.unstageChanges == true)
            {
                diff = list.unstagedChangesDiff;
                indexes = list.unstagedDiffListStartAt;
                variablesForFiles.lastLine = 0;
            }
            else
            {
                diff = list.stagedChangesDiff;
                list.stagedDiffListStartAt = list.unstagedDiffListStartAt;
                indexes = list.stagedDiffListStartAt;
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

        private static void HandleFilesUpStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement)
        {
            List<string> filesList = new List<string>();
            List<int> indexes = new List<int>();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            variablesForFiles.down = false;
            int y = 0;
            int x = 1;
            int index = 0;
            int height = 0;
            int startingFrom = 0;
            int row = 0;

            if (variablesForFiles.unstageChanges == true)
            {
                filesList = list.unstagedChangesFiles;
                height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
                startingFrom = dimensions.unstagedStart;
                y = dimensions.unstagedEnd;
                index = variablesForFiles.unstagedIndex;
                row = variablesForFiles.fileRowUnstaged;
                variablesForFiles.indexDiff = variablesForFiles.unstagedIndex;
            }
            else
            {
                filesList = list.stagedChangesFiles;
                height = dimensions.stagedEnd - dimensions.stagedStart;
                startingFrom = dimensions.stagedStart;
                y = dimensions.stagedEnd;
                index = variablesForFiles.stagedIndex;
                row = variablesForFiles.fileRowStaged;
                variablesForFiles.indexDiff = variablesForFiles.stagedIndex;
            }


            if (variablesForFiles.stageChanges == true && variablesForFiles.stagedIndex == 0 && list.unstagedChangesFiles.Count > 0)
            {
                Console.SetCursorPosition(1, variablesForFiles.fileRowStaged);
                Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                Console.SetCursorPosition(1, variablesForFiles.fileRowStaged);
                GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, variablesForFiles.fileRowStaged, variablesForFiles.indexForStaged, list.stagedChangesFiles);
                
                if (list.unstagedChangesFiles.Count < dimensions.unstagedEnd - dimensions.unstagedStart + 1)
                {
                    row = dimensions.unstagedStart + list.unstagedChangesFiles.Count - 1;
                }
                else
                {
                    row = dimensions.unstagedEnd;
                }

                Console.SetCursorPosition(1, row);
                Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                Console.SetCursorPosition(1, row);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(list.unstagedChangesFiles[list.unstagedChangesFiles.Count - 1]);
                Console.ResetColor();
                variablesForFiles.indexDiff = list.unstagedChangesFiles.Count - 1;
                variablesForFiles.unstagedIndex = list.unstagedChangesFiles.Count - 1;
                variablesForFiles.fileRowUnstaged = row;
                variablesForFiles.unstageChanges = true;
                variablesForFiles.stageChanges = false;
                GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                variablesForFiles.up = false;
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
            }

            if (variablesForFiles.unstageChanges == true)
            {
                if (row > dimensions.unstagedStart)
                {
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, row, index, filesList);
                    index--;
                    row--;
                    variablesForFiles.indexDiff--;
                    Console.SetCursorPosition(1, row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filesList[index]);
                    Console.ResetColor();
                }
                else if (row == dimensions.unstagedStart && index <= filesList.Count - 1)
                {
                    int i = 0;
                    while (i < height)
                    {
                        Console.SetCursorPosition(1, startingFrom + i);
                        Console.Write(new string(' ', dimensions.changesPanelWidth - 1));
                        i++;
                    }

                    variablesForFiles.unstagedIndex--;
                    variablesForFiles.indexDiff--;
                    index = variablesForFiles.unstagedIndex;
                    FilesStatus.ScrollThrouthFilesList(list, variablesForCommits, variablesForFiles);
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filesList[index]);
                    Console.ResetColor();
                }

                variablesForFiles.unstagedIndex = index;
                variablesForFiles.fileRowUnstaged = row;
            }

            if (variablesForFiles.stageChanges == true)
            {
                if (row == dimensions.stagedStart)
                {
                    int i = 0;
                    while (i < height)
                    {
                        Console.SetCursorPosition(1, startingFrom + i);
                        Console.Write(new string(' ', dimensions.changesPanelWidth - 1));
                        i++;
                    }

                    variablesForFiles.stagedIndex--;
                    variablesForFiles.indexDiff--;
                    index = variablesForFiles.stagedIndex;
                    variablesForFiles.fileRowStaged = row;
                    FilesStatus.ScrollThrouthFilesList(list, variablesForCommits, variablesForFiles);
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filesList[index]);
                    Console.ResetColor();
                }
                else if (row > dimensions.stagedStart)
                {
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, row, index, filesList);
                    index--;
                    row--;
                    variablesForFiles.indexDiff--;
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filesList[index]);
                    Console.ResetColor();
                }

                variablesForFiles.stagedIndex = index;
                variablesForFiles.fileRowStaged = row;
            }

            GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
            DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
        }

        private static void HandleFilesDownMovesStatus(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            DrawPanelRigthSide.FilesBox size = new DrawPanelRigthSide.FilesBox();
            var filesList = new List<string>();
            int y = 0;
            int x = 1;
            int index = 0;
            int height = 0;
            int startingFrom = 0;
            int row = 0;

            if (variablesForFiles.unstageChanges == true)
            {
                filesList = list.unstagedChangesFiles;
                height = dimensions.unstagedEnd - dimensions.unstagedStart + 1;
                startingFrom = dimensions.unstagedStart;
                y = dimensions.unstagedEnd;
                index = variablesForFiles.unstagedIndex;
                row = variablesForFiles.fileRowUnstaged;
                variablesForFiles.indexDiff = variablesForFiles.unstagedIndex;
            }
            else
            {
                filesList = list.stagedChangesFiles;
                height = dimensions.stagedEnd - dimensions.stagedStart;
                startingFrom = dimensions.stagedStart;
                y = dimensions.stagedEnd;
                index = variablesForFiles.stagedIndex;
                row = variablesForFiles.fileRowStaged;
                variablesForFiles.indexDiff = variablesForFiles.stagedIndex;
            }

            if (variablesForFiles.unstageChanges == true && index == list.unstagedChangesFiles.Count - 1 && list.stagedChangesFiles.Count > 0)
            {
                variablesForFiles.stageChanges = true;
                variablesForFiles.unstageChanges = false;
                variablesForFiles.fileRowStaged = dimensions.stagedStart;
                variablesForFiles.stagedIndex = 0;
                variablesForFiles.indexDiff = 0;
            }

            //if (list.unstagedChangesFiles.Count == 1 && list.stagedChangesFiles.Count == 0 || variablesForFiles.unstagedIndex == list.unstagedChangesFiles.Count - 1)
            //{
            //    Navigate.NavigateThroughCommits(variablesForCommits, variablesForFiles, commitElement, list);
            //}

            if (variablesForFiles.unstageChanges == true)
            {
                if (row < dimensions.unstagedEnd)
                {
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, row, index, filesList);
                    row++;
                    index++;
                    variablesForFiles.indexDiff++;
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filesList[index]);
                    Console.ResetColor();
                    variablesForFiles.down = true;
                    //Cursor.UpdateCursorPositionForFilesList(variablesForFiles, variablesForCommits, list, size);
                    variablesForFiles.down = false;
                }
                else if (row >= dimensions.unstagedEnd && index < filesList.Count - 1)
                {
                    int i = 0;
                    while (i < height)
                    {
                        Console.SetCursorPosition(1, startingFrom + i);
                        Console.Write(new string(' ', dimensions.changesPanelWidth - 1));
                        i++;
                    }

                    variablesForFiles.unstagedIndex++;
                    index = variablesForFiles.unstagedIndex;
                    variablesForFiles.fileRowUnstaged = row;
                    variablesForFiles.indexDiff++;
                    FilesStatus.ScrollThrouthFilesList(list, variablesForCommits, variablesForFiles);
                    row = variablesForFiles.fileRowUnstaged;
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filesList[index]);
                    Console.ResetColor();
                }

                variablesForFiles.unstagedIndex = index;
                variablesForFiles.fileRowUnstaged = row;
                variablesForFiles.down = false;
                GetDiffs.CleaningHalfOfDiffPanel(variablesForCommits);
                DiffHelper.Print(variablesForCommits, variablesForFiles, commitElement, list);
            }
            

            if (variablesForFiles.stageChanges == true)
            {
                if (variablesForFiles.unstagedIndex == list.unstagedChangesFiles.Count - 1 && variablesForFiles.fileRowUnstaged == row)
                {
                    Console.SetCursorPosition(1, variablesForFiles.fileRowUnstaged);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, variablesForFiles.fileRowUnstaged);
                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, variablesForFiles.fileRowUnstaged, list.unstagedChangesFiles.Count - 1, list.unstagedChangesFiles);
                    Console.SetCursorPosition(1, variablesForFiles.fileRowStaged);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, variablesForFiles.fileRowStaged);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(list.stagedChangesFiles[variablesForFiles.stagedIndex]);
                    Console.ResetColor();
                    variablesForFiles.indexDiff = 0;
                    variablesForFiles.stagedIndex = 0;
                    index = variablesForFiles.stagedIndex;
                    row = variablesForFiles.fileRowStaged;
                }
                else if (row < dimensions.stagedEnd - 1)
                {
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, row, index, filesList);
                    row++;
                    index++;
                    variablesForFiles.indexDiff++;
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filesList[index]);
                    Console.ResetColor();
                }
                else
                {
                    int i = 0;
                    while (i < height)
                    {
                        Console.SetCursorPosition(1, startingFrom + i);
                        Console.Write(new string(' ', dimensions.changesPanelWidth - 1));
                        i++;
                    }

                    index++;
                    variablesForFiles.indexDiff++;
                    variablesForFiles.stagedIndex = index;
                    variablesForFiles.fileRowStaged = row;
                    variablesForFiles.down = true;
                    FilesStatus.ScrollThrouthFilesList(list, variablesForCommits, variablesForFiles);
                    Console.SetCursorPosition(1, row);
                    Console.Write(new string(' ', Console.WindowWidth / 2 - 3));
                    Console.SetCursorPosition(1, row);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(filesList[index]);
                    Console.ResetColor();
                }

                variablesForFiles.stagedIndex = index;
                variablesForFiles.fileRowStaged = row;
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
                    variablesForFiles.fileLogStartAt++;
                    FilesPrintLogFiles.PrintFilesForLog(list, variablesForCommits, variablesForFiles);
                    
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
                    GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, variablesForFiles.fileIndex, list.listOfFiles);
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

            DrawPanelRigthSide.FilesBox panel = new DrawPanelRigthSide.FilesBox();
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            variablesForFiles.down = false;

            if (/*variablesForFiles.fileIndex > 0 && */variablesForFiles.fileRow != panel.edgeOneY + 2)
            {
                GetFiles.ChooseColorForEachFiles(variablesForFiles, variablesForCommits, list, x, y, variablesForFiles.fileIndex, list.listOfFiles);
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
            else
            {
                variablesForFiles.fileLogStartAt--;
                CleaningFilePanel(panel);
                variablesForFiles.fileIndex = list.logFilesStartAt[variablesForFiles.fileLogStartAt];
                FilesPrintLogFiles.PrintFilesForLog(list, variablesForCommits, variablesForFiles);
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
        }

        private static void HandleRightArrowLog(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            if (variablesForCommits.enter == true)
            {
                variablesForCommits.pressRight++;

                if (variablesForCommits.pressRight == 1)
                {
                    variablesForCommits.right = true;
                    variablesForFiles.row = dimensions.tabHeight + 1;
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
                    variablesForFiles.row = dimensions.tabHeight + 1;
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