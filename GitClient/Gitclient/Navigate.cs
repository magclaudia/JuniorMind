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
                            DrawExternalBox.DrawBox();
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
                                DrawExternalBox.DrawBox();
                                i = cursorPosionBiggerThenHeight;
                                heightPosition++;
                                Commits.PrintColumns(heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                            }
                            else
                            {
                                Console.Clear();
                                DrawExternalBox.DrawBox();
                                cursorPosionBiggerThenHeight++;
                                i = cursorPosionBiggerThenHeight;
                                Commits.PrintColumns(heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, i, j, cursorPosition, listOfCommits);
                            }
                        }
                        break;

                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}
