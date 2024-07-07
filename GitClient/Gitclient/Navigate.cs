using System;
using System.Collections.Generic;
using System.Text;

namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughConsole(IntPtr repo, Indexes indexes, CommitElements listOfCommits, int height, int width)
        {
            var size = new DrawPanel.FilesBox();
            IntPtr commitPtr = IntPtr.Zero;
            bool clear = true;
            List<string> addList = new List<string>();
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

                    case ConsoleKey.Enter:
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

                            Commits.PrintCommits(repo, indexes, listOfCommits);
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }

        public static void CommitDetail(IntPtr repo, Indexes indexes, CommitElements listOfCommits, bool clear)
        {
            IntPtr commitPtr = IntPtr.Zero;
            int i = 1;
            if (clear == true)
            {
                Console.Clear();
                DrawPanel.MessagePanel();
            }

            HeaderPanel.Header();
            Message.ReturnMessage(indexes.currentCommitIndex, listOfCommits);
            GitOid oid = listOfCommits.IdGitOid[indexes.currentCommitIndex];
            if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
            {
                Files.GetFilesAffectedByCommit(repo, commitPtr, i);
            }
        }

        private static void VerifySize(IntPtr repo, Indexes indexes, CommitElements listOfCommits, int height, int width)
        {
            if (Console.WindowHeight != height && Console.WindowWidth != width)
            {
                Console.Clear();
                DrawExternalBorder.DrawBox();
                Commits.PrintCommits(repo, indexes, listOfCommits);
            }
        }
    }
}