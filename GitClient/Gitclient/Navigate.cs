namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughDiffsContent(int index, GetCertainList list, VariablesForFiles indexes, string fileFullName)
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            if (indexes.up == true && index == 0)
                            {
                                indexes.row = 0;
                            }

                            if (index == list.listOfDiff.Count - 1 && indexes.currentLine == list.startingIndexes[indexes.fileIndex] - list.startingIndexes[indexes.fileIndex - 1])
                            {
                                break;
                            }

                            if (indexes.end == true && indexes.numberOfNavigations == 1)
                            {
                                if (index < list.startingIndexes[indexes.fileIndex] && indexes.currentLine > Console.WindowHeight - 2)
                                {
                                    index = list.listStartAt[list.listStartAt.Count - 1];
                                    indexes.currentLine--;
                                }
                                else
                                {
                                    index = list.startingIndexes[indexes.fileIndex - 1];
                                    if (index == 0)
                                    {
                                        ChooseStartingIndexForNextFileContain(list, index, indexes);
                                    }
                                    else if (!list.listStartAt.Contains(index))
                                    {
                                        ChooseStartingIndexForNextFileContain(list, index, indexes);
                                    }
                                }

                                indexes.row = 0;
                                indexes.down = true;
                                indexes.end = false;
                            }

                            if (index == list.startingIndexes[indexes.fileIndex] - 1)
                            {
                                GetDiffs.CleanCodePanel();
                                indexes.row = 0;
                                index++;
                                ChooseStartingIndexForNextFileContain(list, index, indexes);
                                indexes.numberOfNavigations = 0;
                                indexes.currentLine = 0;
                                string currentFileName = list.listOfFiles[indexes.fileIndex];
                                DiffHelper.FilesBackground(currentFileName, indexes);
                                if (indexes.fileIndex <= list.listOfFiles.Count - 1)
                                {
                                    indexes.fileIndex++;
                                }

                                indexes.down = false;

                            }
                            else if (index < list.startingIndexes[indexes.fileIndex] && indexes.row == Console.WindowHeight - 2)
                            {
                                GetDiffs.CleanCodePanel();
                                indexes.row = 0;
                                index++;
                                ChooseStartingIndexForNextFileContain(list, index, indexes);
                                indexes.numberOfNavigations = 0;
                                indexes.down = false;
                            }

                            indexes.currentLine++;
                            DiffHelper.Print(indexes, index, fileFullName, list);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        {
                            if (index == 0 && indexes.up == true)
                            {
                                indexes.end = true;
                                break;
                            }

                            if (list.filePath[indexes.fileIndex - 1] == list.listOfDiff[index] && index > 0 || indexes.currentLine == 0)
                            {
                                indexes.nextFile = true;
                            }
                            else
                            {
                                indexes.nextFile = false;
                            }

                            indexes.up = true;

                            if (indexes.nextFile == true)
                            {
                                indexes.diffForEachFileIndex = list.listStartAt[indexes.max] - 1;
                                indexes.max--;
                                index = list.listStartAt[indexes.max];
                                GetDiffs.CleanCodePanel();
                                indexes.row = 0;
                                indexes.currentLine = 0;
                                indexes.numberOfNavigations = 0;
                                indexes.down = false;
                                indexes.end = false;
                            }
                            else
                            {
                                if (indexes.row == 0)
                                {
                                    indexes.diffForEachFileIndex = list.listStartAt[indexes.max];
                                    indexes.max--;
                                    index = list.listStartAt[indexes.max];
                                    GetDiffs.CleanCodePanel();
                                    indexes.row = 0;
                                    indexes.currentLine = 0;
                                    indexes.numberOfNavigations = 0;
                                    indexes.down = false;
                                    indexes.up = false;
                                    indexes.end = false;
                                }
                            }

                            DiffHelper.Print(indexes, index, fileFullName, list);
                        }
                        break;
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }

        public static void NavigateThroughCommits(IntPtr repo, VariablesForCommits indexes, CommitElements listOfCommits, int height, int width)
        {
            var size = new DrawPanelRigthSide.FilesBox();
            IntPtr commitPtr = IntPtr.Zero;
            bool clear = true;
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
                        if (indexes.currentCommitIndex < listOfCommits.Id.Count && indexes.currentCommitIndex > 0)
                        {
                            indexes.up = true;
                            indexes.down = false;
                            if (indexes.heightPosition == 1 && indexes.currentCommitIndex == 0)
                            {
                                break;
                            }

                            indexes.heightPosition--;
                            indexes.currentCommitIndex--;
                            if (indexes.cursorPosition > 0)
                            {
                                indexes.cursorPosition--;
                            }

                            bool reachLimit = false;
                            if (indexes.heightPosition == Console.WindowHeight - 2 || indexes.heightPosition == 0)
                            {
                                indexes.heightPosition = 1;
                                ReplaceEachCommitOneByOne.PrintNewCommitIfReachLimit(repo, indexes, listOfCommits, addList, blueFond);
                            }

                            VerifySize(repo, indexes, listOfCommits, height, width);

                            if (indexes.displayPanel == true)
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfPanel(repo, indexes, listOfCommits, addList, reachLimit, blueFond);
                            }
                            else
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfNoPanel(repo, indexes, listOfCommits, addList, reachLimit, blueFond);
                            }
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (indexes.currentCommitIndex < listOfCommits.Id.Count - 1)
                        {
                            indexes.down = true;
                            indexes.up = false;
                            if (indexes.startIndex < 0)
                            {
                                indexes.startIndex = 0;
                            }

                            bool reachLimit = false;
                            if (indexes.currentCommitIndex > Console.WindowHeight - 2 && indexes.cursorPosition == 0 || indexes.cursorPosition < indexes.currentCommitIndex - Console.WindowHeight - 2 && indexes.heightPosition == Console.WindowHeight - 2)
                            {
                                indexes.cursorPosition = indexes.currentCommitIndex - (Console.WindowHeight - 2) + 2;
                                indexes.currentCommitIndex = indexes.cursorPosition;
                            }

                            if (indexes.heightPosition == Console.WindowHeight - 2)
                            {
                                if (indexes.cursorPosition == listOfCommits.Id.Count - (Console.WindowHeight - 3))
                                {
                                    indexes.cursorPosition = 2;
                                }

                                indexes.currentCommitIndex = indexes.cursorPosition;
                                indexes.cursorPosition++;
                                indexes.heightPosition = 1;
                                ReplaceEachCommitOneByOne.PrintNewCommitIfReachLimit(repo, indexes, listOfCommits, addList, blueFond);
                            }

                            VerifySize(repo, indexes, listOfCommits, height, width);

                            if (indexes.displayPanel == true)
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfPanel(repo, indexes, listOfCommits, addList, reachLimit, blueFond);
                            }
                            else
                            {
                                ReplaceEachCommitOneByOne.PrintNewCommitIfNoPanel(repo, indexes, listOfCommits, addList, reachLimit, blueFond);
                            }
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        {
                            if (indexes.enter == true)
                            {
                                indexes.rigth = true;
                                int index = 1;
                                CommitDetail(repo, indexes, listOfCommits, clear);
                                GitOid oid = listOfCommits.IdGitOid[indexes.currentCommitIndex];
                                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                {
                                    Files.GetFilesAffectedByCommit(repo, commitPtr, index, indexes);
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        {
                            indexes.enter = true;
                            if (indexes.rigth == false)
                            {
                                indexes.displayPanel = true;

                                if (indexes.panelAlreadyDisplayed == false && indexes.displayPanel == true)
                                {
                                    indexes.panelAlreadyDisplayed = true;
                                    CommitDetail(repo, indexes, listOfCommits, clear);
                                }
                                else
                                {
                                    indexes.displayPanel = false;
                                    Console.Clear();
                                    DrawExternalBorder.DrawBox();
                                }

                                GetCommits.PrintCommits(repo, indexes, listOfCommits);
                            }
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }

        public static void CommitDetail(IntPtr repo, VariablesForCommits indexes, CommitElements listOfCommits, bool clear)
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

        private static void VerifySize(IntPtr repo, VariablesForCommits indexes, CommitElements listOfCommits, int height, int width)
        {
            if (Console.WindowHeight != height && Console.WindowWidth != width)
            {
                Console.Clear();
                DrawExternalBorder.DrawBox();
                GetCommits.PrintCommits(repo, indexes, listOfCommits);
            }
        }

        private static void ChooseStartingIndexForNextFileContain(GetCertainList list, int index, VariablesForFiles indexes)
        {
            if (indexes.up == false)
            {
                list.listStartAt.Add(index);
            }
        }
    }
}