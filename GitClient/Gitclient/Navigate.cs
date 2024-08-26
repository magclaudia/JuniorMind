namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughDiffsContent(int index, GetCertainList list, GetVariablesForFiles variablesForFiles, string fileFullName)
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
                            if (index == list.listStartAt[variablesForFiles.x] && totalLines - variablesForFiles.currentLine < Console.WindowHeight - 2 && totalLines > Console.WindowHeight - 2)
                            {
                                GetDiffsLine.GetLineIfDownMoves(list, variablesForFiles);
                            }

                            DiffHelper.Print(variablesForFiles, index, fileFullName, list);
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
                                DiffHelper.Print(variablesForFiles, index, fileFullName, list);
                            }

                            variablesForFiles.currentLine--;
                            DiffHelper.Print(variablesForFiles, index, fileFullName, list);
                        }
                        break;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

        public static void NavigateThroughCommits(IntPtr repo, GetVariablesForCommits variablesForCommits, CommitElements listOfCommits, GetCertainList list, int height, int width)
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
                        if (variablesForCommits.currentCommitIndex < listOfCommits.Id.Count && variablesForCommits.currentCommitIndex > 0)
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
                                ReplaceEachCommitOneByOne.PrintNewCommitIfReachLimit(repo, variablesForCommits, listOfCommits, list, blueFond);
                            }

                            VerifySize(repo, variablesForCommits, listOfCommits, height, width);

                            if (variablesForCommits.displayPanel == true)
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfPanel(repo, variablesForCommits, listOfCommits, list, reachLimit, blueFond);
                            }
                            else
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfNoPanel(repo, variablesForCommits, listOfCommits, list, reachLimit, blueFond);
                            }
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (variablesForCommits.currentCommitIndex < listOfCommits.Id.Count - 1)
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
                                if (variablesForCommits.cursorPosition == listOfCommits.Id.Count - (Console.WindowHeight - 3))
                                {
                                    variablesForCommits.cursorPosition = 2;
                                }

                                variablesForCommits.currentCommitIndex = variablesForCommits.cursorPosition;
                                variablesForCommits.cursorPosition++;
                                variablesForCommits.heightPosition = 1;
                                ReplaceEachCommitOneByOne.PrintNewCommitIfReachLimit(repo, variablesForCommits, listOfCommits, list, blueFond);
                            }

                            VerifySize(repo, variablesForCommits, listOfCommits, height, width);

                            if (variablesForCommits.displayPanel == true)
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfPanel(repo, variablesForCommits, listOfCommits, list, reachLimit, blueFond);
                            }
                            else
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfNoPanel(repo, variablesForCommits, listOfCommits, list, reachLimit, blueFond);
                            }
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        {
                            if (variablesForCommits.enter == true)
                            {
                                variablesForCommits.rigth = true;
                                int index = 1;
                                CommitDetail(repo, variablesForCommits, listOfCommits, variablesForCommits.clear);
                                GitOid oid = listOfCommits.IdGitOid[variablesForCommits.currentCommitIndex];
                                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                {
                                    Files.GetFilesAffectedByCommit(repo, commitPtr, index, variablesForCommits);
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            variablesForCommits.enter = true;
                            if (variablesForCommits.rigth == false)
                            {
                                variablesForCommits.displayPanel = true;

                                if (variablesForCommits.panelAlreadyDisplayed == false && variablesForCommits.displayPanel == true)
                                {
                                    variablesForCommits.panelAlreadyDisplayed = true;
                                    CommitDetail(repo, variablesForCommits, listOfCommits, variablesForCommits.clear);
                                }
                                else
                                {
                                    variablesForCommits.displayPanel = false;
                                    Console.Clear();
                                    DrawExternalBorder.DrawBox();
                                }

                                GetCommits.PrintCommits(repo, variablesForCommits, listOfCommits);
                            }
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }

        public static void CommitDetail(IntPtr repo, GetVariablesForCommits indexes, CommitElements listOfCommits, bool clear)
        {
            IntPtr commitPtr = IntPtr.Zero;
            int i = 1;
            if (clear == true)
            {
                Console.Clear();
                if (indexes.rigth == true)
                {
                    DrawPanelLeftSide.Info();
                }
                else
                {
                    DrawPanelRigthSide.Info();
                }
            }

            HeaderPanel.Header(indexes);
            Info.GetInfo(indexes, listOfCommits);
            Message.ReturnMessage(indexes.currentCommitIndex, listOfCommits, indexes);
            GitOid oid = listOfCommits.IdGitOid[indexes.currentCommitIndex];
            if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
            {
                Files.GetFilesAffectedByCommit(repo, commitPtr, i, indexes);
            }
        }

        private static void VerifySize(IntPtr repo, GetVariablesForCommits indexes, CommitElements listOfCommits, int height, int width)
        {
            if (Console.WindowHeight != height && Console.WindowWidth != width)
            {
                Console.Clear();
                DrawExternalBorder.DrawBox();
                GetCommits.PrintCommits(repo, indexes, listOfCommits);
            }
        }
    }
}