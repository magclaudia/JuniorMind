using System.Collections.Generic;

namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughDiffsContent(GetCertainList list, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, CommitElements commitElements, string fileFullName)
        {
            ConsoleKeyInfo keyInfo;
            variablesForFiles.height = Console.WindowHeight;

            do
            {
                keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {

                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {

                        }
                        break;

                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

        public static void NavigateThroughCommits(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list)
        {
            var size = new DrawPanelRigthSide.FilesBox();
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
                        break;

                    case ConsoleKey.DownArrow:
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
                        break;

                    case ConsoleKey.RightArrow:
                        {
                            if (variablesForCommits.pressRight == 2)
                            {
                                break;
                            }

                            HandleRightArrow(variablesForCommits, variablesForFiles, commitElement, list);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            if (variablesForFiles.diffMoves == false)
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
                            variablesForCommits.esc = true;
                            variablesForCommits.pressRight = 0;
                            variablesForFiles.diffMoves = false;
                            variablesForFiles.x = 0;
                            variablesForFiles.numberOfNavigations = 0;
                            list.startingIndexes.Clear();
                            Console.Clear();
                            variablesForFiles.nextFile = false;
                            HandleRightArrow(variablesForCommits, variablesForFiles, commitElement, list);
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

        private static void HandleDiffUpMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
            variablesForFiles.up = true;
            variablesForFiles.down = false;

            if (list.listOfAllDiffs[variablesForFiles.indexDiff].Count > Console.WindowHeight - 2 && list.startingIndexes[variablesForFiles.indexDiff].Contains(variablesForFiles.index))
            {
                GetDiffs.CleaningEntireDiffPanel(variablesForCommits);
                if (variablesForFiles.x > 0)
                {
                    variablesForFiles.x--;
                }

                variablesForFiles.index = list.startingIndexes[variablesForFiles.indexDiff][variablesForFiles.x];
                variablesForFiles.up = false;
                variablesForFiles.down = false;
                variablesForFiles.row = 0;
                variablesForFiles.currentLine = variablesForFiles.index + 1;
                DiffHelper.Print(variablesForCommits, commitElements);

            }

            variablesForFiles.numberOfNavigations = 1;
            if (variablesForFiles.index > 0)
            {
                variablesForFiles.currentLine--;
                DiffHelper.Print(variablesForCommits, commitElements);
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
                    list.startingIndexes[variablesForFiles.indexDiff].Add(variablesForFiles.index);
                    variablesForFiles.x++;
                    variablesForFiles.row = 0;
                    GetDiffs.CleaningEntireDiffPanel(variablesForCommits);
                    variablesForFiles.down = false;
                }

                variablesForFiles.currentLine++;
                DiffHelper.Print(variablesForCommits, commitElements);
            }
        }

        private static void HandleRightArrow(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, CommitElements commitElement, GetCertainList list)
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

        private static void HandleFilesUpMoves(GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElement)
        {
            int y = variablesForFiles.fileRow;
            var panel = new DrawPanelRigthSide.FilesBox();
            if (variablesForFiles.fileRow == panel.edgeOneY + 2 && variablesForFiles.fileIndex > 0)
            {
                CleaningFilePanel(panel);
                variablesForFiles.fileIndex = 0;
                GetAllFiles.PrintRemaingingFiles(variablesForFiles, list);
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
                    GetAllFiles.PrintRemaingingFiles(variablesForFiles, list);
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