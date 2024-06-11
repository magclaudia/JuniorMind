using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class Commits
    {
        public static void PrintCommits(IntPtr repo, List<GitOid> listofIds, IntPtr commitPtr, bool displayPanel, int heightPosition, int cursorPosionBiggerThenHeight, int upOrDownOneStep, int index, int rightCursor, int cursorPosition, List<string> listOfCommits)
        {
            CommitNumber.ReturnCommitNumber(listOfCommits, upOrDownOneStep);
            var position = new DrawPanel.CommitsPanel();
            if (displayPanel == false) 
            {
                if (heightPosition > position.height)
                {
                    index = heightPosition - position.height;
                }

                DisplayCommitsOnEntireConsole(repo, listofIds, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition, listOfCommits);
            }
            else
            {
                index = index - (Console.WindowHeight - 2);
                DisplayCommitsWithPanel(repo, listofIds, commitPtr, displayPanel, heightPosition, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition, listOfCommits);
            }
        }

        private static void DisplayCommitsOnEntireConsole(IntPtr repo, List<GitOid> listofIds, IntPtr commitPtr, bool displayPanel, int commitNumber, int cursorPosionBiggerThenHeight, int upOrDownOneStep, int index, int rightCursor, int cursorPosition, List<string> listOfCommits)
        {
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
            Cursor.UpdateCursorPosition(displayPanel, commitNumber, listOfCommits, upOrDownOneStep);
            Console.ResetColor();
            Navigate.NavigateThroughConsole(repo, listofIds, commitPtr, commitNumber, listOfCommits, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition);
        }

        private static void DisplayCommitsWithPanel(IntPtr repo, List<GitOid> listofIds, IntPtr commitPtr, bool displayPanel, int heightPosition, int cursorPosionBiggerThenHeight, int upOrDownOneStep, int index, int rightCursor, int cursorPosition, List<string> listOfCommits)
        {
            while (cursorPosition < Console.WindowHeight - 2 && index < listOfCommits.Count && index >= 0)
            {
                Console.SetCursorPosition(1, cursorPosition + 1);
                var commitRow = listOfCommits[index].Split(" ");

                const int commitIdStandadDimension = 7;
                const int dateTimeStandardDimension = 10;
                const int authorStandardDimension = 20;
                
                int rowLength = listOfCommits[index].Length;
                int panelWidth = Console.WindowWidth / 2  + 7 - 2;
                int firstTreeColumnsLength = 0;

                Console.Write($"{commitRow[0],-commitIdStandadDimension} ",
                    Console.ForegroundColor = ConsoleColor.Magenta);
                Console.Write($"{commitRow[1],-dateTimeStandardDimension} ",
                    Console.ForegroundColor = ConsoleColor.Cyan);

                int actualRowLength = 0;
                string author = "";

                if (commitRow[1].Length == 8)
                {
                    actualRowLength = commitIdStandadDimension + 8 + 2;
                    author = listOfCommits[index].Substring(actualRowLength, 2);
                }
                else
                {
                    actualRowLength = commitIdStandadDimension + dateTimeStandardDimension + 2;
                    author = listOfCommits[index].Substring(actualRowLength, 2);
                }

                int startIndex = $"{commitRow[0]} ".Length + $"{commitRow[1]} ".Length +
                   $"{commitRow[2]} ".Length;

                if (actualRowLength + authorStandardDimension >= Console.WindowWidth / 2  + 7 - 2)
                {
                    Console.Write($"{author}",
                    Console.ForegroundColor = ConsoleColor.Green);
                    Console.ResetColor();
                    Console.Write("..  ");
                    firstTreeColumnsLength = $"{commitRow[0],-commitIdStandadDimension} ".Length + $"{commitRow[1],-dateTimeStandardDimension} ".Length +
                        author.Length + "..  ".Length;
                }
                else
                {
                    Console.Write($"{commitRow[2],-authorStandardDimension}",
                    Console.ForegroundColor = ConsoleColor.Green);
                    
                    firstTreeColumnsLength = $"{commitRow[0], - commitIdStandadDimension} ".Length + $"{commitRow[1], -dateTimeStandardDimension} ".Length +
                   $"{commitRow[2], -authorStandardDimension} ".Length;
                }

                Console.ResetColor();
                string message;

                var completeMessage = listOfCommits[index].Length - startIndex - 2;

                if (completeMessage + firstTreeColumnsLength > Console.WindowWidth / 2 + 7 - 2)
                {
                    message = listOfCommits[index].Substring(startIndex,
                        panelWidth - firstTreeColumnsLength);
                }
                else
                {
                    message = listOfCommits[index].Substring(startIndex, completeMessage);
                }

                Console.Write($"{message}");

                index++;
                cursorPosition++;
            }

            rightCursor = index;
            Cursor.UpdateCursorPosition(displayPanel, heightPosition, listOfCommits, upOrDownOneStep);
            Console.ResetColor();
            Navigate.NavigateThroughConsole(repo, listofIds, commitPtr, heightPosition, listOfCommits, cursorPosionBiggerThenHeight, upOrDownOneStep, index, rightCursor, cursorPosition);
        }
    }
}
