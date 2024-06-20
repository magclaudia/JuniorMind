using System;
using System.Collections.Generic;
using System.Text;
using static GitClient.ListOfCommits;

namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughConsole(IntPtr repo, ListOfCommits.Indexes indexes, ListOfCommits.CommitElements listOfCommits)
        {
            var size = new DrawPanel.FilesBox();
            IntPtr commitPtr = IntPtr.Zero;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                indexes.cursorPosition = 0;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (indexes.upOrDownOneStep < listOfCommits.Id.Count)
                        {
                            if (indexes.heightPosition == 1 && indexes.upOrDownOneStep == 0)
                            {
                                break;
                            }

                            indexes.heightPosition--;
                            indexes.upOrDownOneStep--;

                            if (indexes.heightPosition < 1)
                            {
                                indexes.heightPosition = 1;
                                indexes.cursorPositionBiggerThenHeight--;
                                if (indexes.cursorPositionBiggerThenHeight < 0)
                                {
                                    indexes.cursorPositionBiggerThenHeight = 0;
                                }
                            }

                            Console.Clear();
                            DrawExternalBorder.DrawBox();
                            indexes.index = indexes.cursorPositionBiggerThenHeight;
                            if (indexes.displayPanel == true)
                            {
                                int i = 1;
                                Console.Clear();
                                DrawPanel.MessagePanel();
                                HeaderPanel.Header();
                                Message.ReturnMessage(indexes.upOrDownOneStep, listOfCommits);
                                GitOid oid = listOfCommits.IdGitOid[indexes.upOrDownOneStep];
                                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                {
                                    Files.GetFilesAffectedByCommit(repo, commitPtr, i);
                                }
                            }

                            Commits.PrintCommits(repo, indexes, listOfCommits);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (indexes.upOrDownOneStep < listOfCommits.Id.Count - 1)
                        {
                            if (indexes.index < 0)
                            {
                                indexes.index = 0;
                            }

                            indexes.upOrDownOneStep++;
                            if (indexes.heightPosition < Console.WindowHeight - 2)
                            {
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                indexes.index = indexes.cursorPositionBiggerThenHeight;
                                indexes.heightPosition++;

                                if (indexes.displayPanel == true)
                                {
                                    int i = 1;
                                    Console.Clear();
                                    DrawPanel.MessagePanel();
                                    HeaderPanel.Header();
                                    Message.ReturnMessage(indexes.upOrDownOneStep, listOfCommits);
                                    GitOid oid = listOfCommits.IdGitOid[indexes.upOrDownOneStep];
                                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                    {
                                        Files.GetFilesAffectedByCommit(repo, commitPtr, i);
                                    }
                                }
                                
                                Commits.PrintCommits(repo, indexes, listOfCommits);
                            }
                            else
                            {
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                indexes.cursorPositionBiggerThenHeight++;
                                indexes.index = indexes.cursorPositionBiggerThenHeight;
                                if (indexes.displayPanel == true)
                                {
                                    int i = 1;
                                    Console.Clear();
                                    DrawPanel.MessagePanel();
                                    HeaderPanel.Header();
                                    Message.ReturnMessage(indexes.upOrDownOneStep, listOfCommits);
                                    GitOid oid = listOfCommits.IdGitOid[indexes.upOrDownOneStep];
                                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                    {
                                        Files.GetFilesAffectedByCommit(repo, commitPtr, i);
                                    }
                                }

                                Commits.PrintCommits(repo, indexes, listOfCommits);
                            }
                        }
                        break;

                    case ConsoleKey.Enter:
                        {
                            indexes.displayPanel = true;
                            if (indexes.panelAlreadyDisplayed == false && indexes.displayPanel == true)
                            {
                                indexes.panelAlreadyDisplayed = true;
                                int i = 1;
                                indexes.index = indexes.cursorPositionBiggerThenHeight;
                                Console.Clear();
                                DrawPanel.MessagePanel();
                                HeaderPanel.Header();
                                Message.ReturnMessage(indexes.upOrDownOneStep, listOfCommits);
                                GitOid oid = listOfCommits.IdGitOid[indexes.upOrDownOneStep];
                                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                {
                                    Files.GetFilesAffectedByCommit(repo, commitPtr, i);
                                }

                                Commits.PrintCommits(repo, indexes, listOfCommits);
                            }
                            else
                            {
                                indexes.index = indexes.cursorPositionBiggerThenHeight;
                                indexes.displayPanel = false;
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                Commits.PrintCommits(repo, indexes, listOfCommits);
                            }
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}