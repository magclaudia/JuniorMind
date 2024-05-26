using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gitclient
{
    public class Navigate
    {
        public static void NavigateThroughConsole(int heightPosition, List<string> listOfCommits, int cursorPosionBiggerThenHeight, int upOrDownOneStep, int i, int j, int cursorPosition)
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                cursorPosition = 0;
                bool returnFullLine = true;
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
                            Commits.PrintColumns(heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
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
                                Commits.PrintColumns(heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                            }
                            else
                            {
                                Console.Clear();
                                DrawExternalBorder.DrawBox();
                                cursorPosionBiggerThenHeight++;
                                i = cursorPosionBiggerThenHeight;
                                Commits.PrintColumns(heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                            }
                        }
                        break;

                    case ConsoleKey.Enter:
                        {
                            int numberOfFiles = 0;
                            Console.Clear();
                            DrawPanel.Panel();
                            HeaderPanel.Header(numberOfFiles);
                            Console.SetCursorPosition(Console.WindowWidth / 2 + 1, 1);
                            Message.ReturnMessage(listOfCommits, upOrDownOneStep);
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}
