using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gitclient
{
    public class Navigate
    {
        public static void NavigateThroughConsole(IntPtr repo, IntPtr commitPtr, int heightPosition, List<string> listOfCommits,  int cursorPosionBiggerThenHeight, int upOrDownOneStep, int i, int j, int cursorPosition)
        {
            bool displayPanel = false;
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
                            Commits.PrintCommits(repo, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
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
                                Commits.PrintCommits(repo, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                            }
                            else
                            {
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                cursorPosionBiggerThenHeight++;
                                i = cursorPosionBiggerThenHeight;
                                Commits.PrintCommits(repo, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                            }
                        }
                        break;

                    case ConsoleKey.Enter:
                        {
                            int numberOfFiles = 0;
                            displayPanel = true;
                            var split = listOfCommits[upOrDownOneStep].Split(' ');
                            Console.Clear();
                            DrawPanel.Panel();
                            HeaderPanel.Header(numberOfFiles);
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 11, 1);
                            Message.ReturnMessage(listOfCommits, upOrDownOneStep, split[1]);
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 11, Console.WindowHeight / 2 + 2);
                            Files.GetFilesAffectedByCommit(repo, commitPtr);
                            Commits.PrintCommits(repo, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}
