using System.Collections.Generic;

namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughDiffsContent(int index, GetCertainList list, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, CommitElements commitElements, string fileFullName)
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            variablesForFiles.up = false;
                            if (index == list.listOfDiff.Count - 1 && variablesForFiles.currentLine == list.startingIndexes[variablesForFiles.fileIndex] - list.startingIndexes[variablesForFiles.fileIndex - 1])
                            {
                                break;
                            }

                            if (index + 1 == list.listStartAt[variablesForFiles.x + 1] && index > 0 && index + 1 == list.startingIndexes[variablesForFiles.fileIndex])
                            {
                                GetDiffs.CleanCodePanel();
                                variablesForFiles.row = 0;
                                variablesForFiles.numberOfNavigations = 0;
                                variablesForFiles.currentLine = 0;
                                string currentFileName = list.listOfFiles[variablesForFiles.fileIndex];
                                DiffHelper.FilesBackground(currentFileName, variablesForFiles);
                                if (variablesForFiles.fileIndex <= list.listOfFiles.Count - 1)
                                {
                                    variablesForFiles.fileIndex++;
                                }

                                index++;
                                variablesForFiles.down = false;
                                variablesForFiles.x++;

                            }
                            else if (index + 1 < list.startingIndexes[variablesForFiles.fileIndex] && variablesForFiles.row == Console.WindowHeight - 3)
                            {
                                GetDiffs.CleanCodePanel();
                                variablesForFiles.row = 0;
                                index++;
                                variablesForFiles.numberOfNavigations = 0;
                                variablesForFiles.down = false;
                                variablesForFiles.x++;
                            }

                            if (variablesForFiles.row <= Console.WindowHeight - 2 || index == list.startingIndexes[variablesForFiles.fileIndex] - 1)
                            {
                                variablesForFiles.currentLine++;
                            }

                            int totalLines = list.startingIndexes[variablesForFiles.fileIndex] - list.startingIndexes[variablesForFiles.fileIndex - 1];
                           
                            if (index == list.listStartAt[variablesForFiles.x] && list.startingIndexes[variablesForFiles.fileIndex] - list.listStartAt[variablesForFiles.x] < Console.WindowHeight - 2 && totalLines - variablesForFiles.currentLine < Console.WindowHeight - 2 && totalLines > Console.WindowHeight - 2 && variablesForFiles.fileIndex != list.listOfFiles.Count)
                            {
                                GetDiffsLine.GetLineIfDownMoves(list, variablesForFiles);
                            }

                            DiffHelper.Print(variablesForCommits, commitElements, index, fileFullName);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            variablesForFiles.up = true;
                            if (index == 0 && variablesForFiles.up == true)
                            {
                                variablesForFiles.end = true;
                                break;
                            }

                            if (list.listStartAt.Contains(index))
                            {
                                GetDiffs.CleanCodePanel();
                                variablesForFiles.row = 0;
                                variablesForFiles.numberOfNavigations = 0;

                                if (list.startingIndexes.Contains(index))
                                {
                                    variablesForFiles.fileIndex = variablesForFiles.fileIndex - 2;
                                    variablesForFiles.fileRow = variablesForFiles.fileRow - 2;
                                    string currentFileName = list.listOfFiles[variablesForFiles.fileIndex];
                                    DiffHelper.FilesBackground(currentFileName, variablesForFiles);
                                }

                                variablesForFiles.down = false;
                                variablesForFiles.x--;
                                index = list.listStartAt[variablesForFiles.x];
                                int totalLines = list.startingIndexes[variablesForFiles.fileIndex] - list.startingIndexes[variablesForFiles.fileIndex - 1];

                                if (totalLines > Console.WindowHeight - 2)
                                {
                                    GetDiffsLine.GetLineIfUpMoves(index, list, variablesForFiles);
                                }

                                variablesForFiles.up = false;
                                DiffHelper.Print(variablesForCommits, commitElements, index, fileFullName);
                            }

                            variablesForFiles.currentLine--;
                            DiffHelper.Print(variablesForCommits, commitElements,  index, fileFullName);
                        }
                        break;

                        case ConsoleKey.LeftArrow:
                        {
                            IntPtr commitPtr = IntPtr.Zero;
                            variablesForCommits.right = true;
                            int i = 1;
                            CommitDetail(variablesForCommits, commitElements, variablesForCommits.clear);
                            GitOid oid = commitElements.IdGitOid[variablesForCommits.currentCommitIndex];
                            if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElements.repo, ref oid) == 0)
                            {
                                Files.GetFilesAffectedByCommit(commitElements.repo, commitPtr, i, variablesForCommits, commitElements);
                            }
                        }
                        break;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

        public static void NavigateThroughCommits(GetVariablesForCommits variablesForCommits, CommitElements commitElement, int height, int width)
        {
            var size = new DrawPanelRigthSide.FilesBox();
            IntPtr commitPtr = IntPtr.Zero;
            List<string> addList = new List<string>();
            List<string> filesNames = new List<string>();
            ConsoleKeyInfo keyInfo;
            int blueFond = 0;
            do
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (variablesForCommits.currentCommitIndex < commitElement.Id.Count && variablesForCommits.currentCommitIndex > 0)
                        {
                            variablesForCommits.up = true;
                            variablesForCommits.down = false;
                            if (variablesForCommits.heightPosition == 1 && variablesForCommits.currentCommitIndex == 0)
                            {
                                break;
                            }

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
                                ReplaceEachCommitOneByOne.PrintNewCommitIfReachLimit(variablesForCommits, commitElement, addList, blueFond);
                            }

                            VerifySize(variablesForCommits, commitElement, height, width);

                            if (variablesForCommits.displayPanel == true)
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfPanel(variablesForCommits, commitElement, addList, reachLimit, blueFond);
                            }
                            else
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfNoPanel(variablesForCommits, commitElement, addList, reachLimit, blueFond);
                            }
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (variablesForCommits.currentCommitIndex < commitElement.Id.Count - 1)
                        {
                            variablesForCommits.down = true;
                            variablesForCommits.up = false;
                            if (variablesForCommits.startIndex < 0)
                            {
                                variablesForCommits.startIndex = 0;
                            }

                            bool reachLimit = false;
                            if (variablesForCommits.currentCommitIndex > Console.WindowHeight - 2 && variablesForCommits.cursorPosition == 0 || variablesForCommits.cursorPosition < variablesForCommits.currentCommitIndex - Console.WindowHeight - 2 && variablesForCommits.heightPosition == Console.WindowHeight - 2)
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
                                ReplaceEachCommitOneByOne.PrintNewCommitIfReachLimit(variablesForCommits, commitElement, addList, blueFond);
                            }

                            VerifySize(variablesForCommits, commitElement, height, width);

                            if (variablesForCommits.displayPanel == true)
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfPanel(variablesForCommits, commitElement, addList, reachLimit, blueFond);
                            }
                            else
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfNoPanel(variablesForCommits, commitElement, addList, reachLimit, blueFond);
                            }
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        {
                            if (variablesForCommits.enter == true)
                            {
                                variablesForCommits.right = true;
                                int index = 1;
                                CommitDetail(variablesForCommits, commitElement, variablesForCommits.clear);
                                GitOid oid = commitElement.IdGitOid[variablesForCommits.currentCommitIndex];
                                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElement.repo, ref oid) == 0)
                                {
                                    Files.GetFilesAffectedByCommit(commitElement.repo, commitPtr, index, variablesForCommits, commitElement);
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            variablesForCommits.enter = true;
                            if (variablesForCommits.right == false)
                            {
                                variablesForCommits.displayPanel = true;

                                if (variablesForCommits.panelAlreadyDisplayed == false && variablesForCommits.displayPanel == true)
                                {
                                    variablesForCommits.panelAlreadyDisplayed = true;
                                    CommitDetail(variablesForCommits, commitElement, variablesForCommits.clear);
                                }
                                else
                                {
                                    variablesForCommits.displayPanel = false;
                                    Console.Clear();
                                    DrawExternalBorder.DrawBox();
                                }

                                GetCommits.PrintCommits(variablesForCommits, commitElement);
                            }
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }

        public static void CommitDetail(GetVariablesForCommits variablesForCommits, CommitElements commitElement, bool clear)
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
            GitOid oid = commitElement.IdGitOid[variablesForCommits.currentCommitIndex];
            if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElement.repo, ref oid) == 0)
            {
                Files.GetFilesAffectedByCommit(commitElement.repo, commitPtr, i, variablesForCommits, commitElement);
            }
        }

        private static void VerifySize(GetVariablesForCommits variablesForCommits, CommitElements listOfCommits, int height, int width)
        {
            if (Console.WindowHeight != height && Console.WindowWidth != width)
            {
                Console.Clear();
                DrawExternalBorder.DrawBox();
                GetCommits.PrintCommits(variablesForCommits, listOfCommits);
            }
        }
    }
}