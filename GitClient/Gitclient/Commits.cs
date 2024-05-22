using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gitclient
{
    public class Commits
    {
        public static void PrintColumns(int heightPosition, int cursorPosionBiggerThenHeight, int upOrDownOneStep, int index, int rightCursor, int cursorPosition, List<string> listOfCommits)
        {
            CommitNumber.ReturnCommitNumber(listOfCommits, upOrDownOneStep);

            while (cursorPosition < Console.WindowHeight - 2 && index < listOfCommits.Count && index >= 0)
            {
                Console.SetCursorPosition(1, cursorPosition + 1);
                var commitRow = listOfCommits[index].Split(" ");

                const int commitIdStandadDimension = 7;
                const int dateTimeStandardDimension = 10;
                const int authorStandardDimension = 20;

                Console.Write($"{commitRow[0],-commitIdStandadDimension} ",
                    Console.ForegroundColor = ConsoleColor.Magenta);
                Console.Write($"{commitRow[1],-dateTimeStandardDimension} ",
                    Console.ForegroundColor = ConsoleColor.Cyan);
                Console.Write($"{commitRow[2],-authorStandardDimension}",
                    Console.ForegroundColor = ConsoleColor.Green);
                Console.ResetColor();

                var message = "";
                
                var firstThreeColumns = $"{commitRow[0]} ".Length + $"{commitRow[1]} ".Length +
                    $"{commitRow[2]} ".Length;
                
                var columnsStandardDimentions = $"{commitRow[0],-commitIdStandadDimension} ".Length +
                    $"{commitRow[1],-dateTimeStandardDimension} ".Length +
                         $"{commitRow[2],-authorStandardDimension} ".Length;
                
                var completeMessage = listOfCommits[index].Length - firstThreeColumns;

                if (completeMessage + columnsStandardDimentions > Console.WindowWidth - 2)
                {
                    message = listOfCommits[index].Substring(firstThreeColumns,
                        Console.WindowWidth - 2 - columnsStandardDimentions);
                }
                else
                {
                    message = listOfCommits[index].Substring(firstThreeColumns, completeMessage);
                }

                Console.Write($"{message}");

                index++;
                cursorPosition++;
            }

            rightCursor = index;
            Cursor.UpdateCursorPosition(heightPosition, listOfCommits, upOrDownOneStep);
            Console.ResetColor();
            Navigate.NavigateThroughConsole(heightPosition, listOfCommits,  cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition);
        }
    }
}
