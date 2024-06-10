using System;
using System.Collections.Generic;
using System.Text;

namespace GitClient
{
    public class Navigate
    {
        public static void NavigateThroughConsole(IntPtr repo, List<GitOid> listOfIds, IntPtr commitPtr, int heightPosition, List<string> listOfCommits, int cursorPosionBiggerThenHeight, int upOrDownOneStep, int i, int j, int cursorPosition)
        {
            bool displayPanel = false;
            var size = new DrawPanel.FilesBox();
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                cursorPosition = 0;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (i <= listOfCommits.Count && i > 0)
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
                                cursorPosionBiggerThenHeight--;
                                upOrDownOneStep = cursorPosionBiggerThenHeight;
                                if (cursorPosionBiggerThenHeight < 0) cursorPosionBiggerThenHeight = 0;
                            }

                            Console.Clear();
                            DrawExternalBorder.DrawBox();
                            i = cursorPosionBiggerThenHeight;
                            Commits.PrintCommits(repo, listOfIds, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (upOrDownOneStep < listOfCommits.Count - 1)
                        {
                            if (i < 0)
                            {
                                i = 0;
                            }

                            upOrDownOneStep++;
                            if (heightPosition < Console.WindowHeight - 2)
                            {
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                i = cursorPosionBiggerThenHeight;
                                heightPosition++;
                                Commits.PrintCommits(repo, listOfIds, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                            }
                            else
                            {
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                cursorPosionBiggerThenHeight++;
                                i = cursorPosionBiggerThenHeight;
                                Commits.PrintCommits(repo, listOfIds, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                            }
                        }
                        break;

                    case ConsoleKey.Enter:
                        {
                            displayPanel = true;
                            int index = 1;
                            var split = listOfCommits[upOrDownOneStep].Split(' ');
                            Console.Clear();
                            DrawPanel.MessagePanel();
                            HeaderPanel.Header();
                            Message.ReturnMessage(listOfCommits, upOrDownOneStep, split[0]);
                            Console.SetCursorPosition(size.edgeOneX + 1, size.edgeOneY + 1);
                            var commitOid = listOfIds[upOrDownOneStep];
                            if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref commitOid) == 0)
                            {
                                Files.GetFilesAffectedByCommit(repo, commitPtr, index);
                            }

                            Commits.PrintCommits(repo, listOfIds, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}
