using System;
using System.Collections.Generic;
using System.Text;

namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughConsole(IntPtr repo, bool panelAlreadyDisplayed, bool displayPanel, int heightPosition, ListOfCommits.CommitElements listOfCommits, int cursorPositionBiggerThenHeight, int upOrDownOneStep, int index, int j, int cursorPosition)
        {
            var size = new DrawPanel.FilesBox();
            IntPtr commitPtr = IntPtr.Zero;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                cursorPosition = 0;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (upOrDownOneStep < listOfCommits.Id.Count)
                        {
                            if (heightPosition == 1 && upOrDownOneStep == 0)
                            {
                                break;
                            }

                            heightPosition--;
                            upOrDownOneStep--;

                            if (heightPosition < 1)
                            {
                                heightPosition = 1;
                                cursorPositionBiggerThenHeight--;
                                if (cursorPositionBiggerThenHeight < 0)
                                {
                                    cursorPositionBiggerThenHeight = 0;
                                }
                            }

                            Console.Clear();
                            DrawExternalBorder.DrawBox();
                            index = cursorPositionBiggerThenHeight;
                            if (displayPanel == true)
                            {
                                int i = 1;
                                Console.Clear();
                                DrawPanel.MessagePanel();
                                HeaderPanel.Header();
                                Message.ReturnMessage(upOrDownOneStep, listOfCommits);
                                GitOid oid = listOfCommits.IdGitOid[upOrDownOneStep];
                                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                {
                                    Files.GetFilesAffectedByCommit(repo, commitPtr, i);
                                }
                            }

                            Commits.PrintCommits(repo, panelAlreadyDisplayed, displayPanel, heightPosition, cursorPositionBiggerThenHeight, upOrDownOneStep, index, j, cursorPosition, listOfCommits);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (upOrDownOneStep < listOfCommits.Id.Count - 1)
                        {
                            if (index < 0)
                            {
                                index = 0;
                            }

                            upOrDownOneStep++;
                            if (heightPosition < Console.WindowHeight - 2)
                            {
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                index = cursorPositionBiggerThenHeight;
                                heightPosition++;

                                if (displayPanel == true)
                                {
                                    int i = 1;
                                    Console.Clear();
                                    DrawPanel.MessagePanel();
                                    HeaderPanel.Header();
                                    Message.ReturnMessage(upOrDownOneStep, listOfCommits);
                                    GitOid oid = listOfCommits.IdGitOid[upOrDownOneStep];
                                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                    {
                                        Files.GetFilesAffectedByCommit(repo, commitPtr, i);
                                    }
                                }
                                
                                Commits.PrintCommits(repo, panelAlreadyDisplayed, displayPanel, heightPosition, cursorPositionBiggerThenHeight, upOrDownOneStep, index, j, cursorPosition, listOfCommits);
                            }
                            else
                            {
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                cursorPositionBiggerThenHeight++;
                                index = cursorPositionBiggerThenHeight;
                                if (displayPanel == true)
                                {
                                    int i = 1;
                                    Console.Clear();
                                    DrawPanel.MessagePanel();
                                    HeaderPanel.Header();
                                    Message.ReturnMessage(upOrDownOneStep, listOfCommits);
                                    GitOid oid = listOfCommits.IdGitOid[upOrDownOneStep];
                                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                    {
                                        Files.GetFilesAffectedByCommit(repo, commitPtr, i);
                                    }
                                }

                                Commits.PrintCommits(repo, panelAlreadyDisplayed, displayPanel, heightPosition, cursorPositionBiggerThenHeight, upOrDownOneStep, index, j, cursorPosition, listOfCommits);
                            }
                        }
                        break;

                    case ConsoleKey.Enter:
                        {
                            displayPanel = true;
                            if (panelAlreadyDisplayed == false && displayPanel == true)
                            {
                                panelAlreadyDisplayed = true;
                                int i = 1;
                                index = cursorPositionBiggerThenHeight;
                                Console.Clear();
                                DrawPanel.MessagePanel();
                                HeaderPanel.Header();
                                Message.ReturnMessage(upOrDownOneStep, listOfCommits);
                                GitOid oid = listOfCommits.IdGitOid[upOrDownOneStep];
                                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref oid) == 0)
                                {
                                    Files.GetFilesAffectedByCommit(repo, commitPtr, i);
                                }

                                Commits.PrintCommits(repo, panelAlreadyDisplayed, displayPanel, heightPosition, cursorPositionBiggerThenHeight, upOrDownOneStep, index, j, cursorPosition, listOfCommits);
                            }
                            else
                            {
                                index = cursorPositionBiggerThenHeight;
                                displayPanel = false;
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                Commits.PrintCommits(repo, panelAlreadyDisplayed, displayPanel, heightPosition, cursorPositionBiggerThenHeight, upOrDownOneStep, index, j, cursorPosition, listOfCommits);
                            }
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}