using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class BlueBackground
    {
        public static void DisplayBlueBox(int blueBoxPosition, int index, List<string> listOfCommits)
        {
            var position = new DrawPanel.CommitsPanel();
            if (blueBoxPosition > position.height)
            {
                blueBoxPosition = position.height;
            }

            Console.SetCursorPosition(1, blueBoxPosition);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            var commitRow = listOfCommits[index].Split(" ");

            const int commitIdStandadDimension = 7;
            const int dateTimeStandardDimension = 10;
            const int authorStandardDimension = 20;
            
            Console.Write($"{commitRow[0],-commitIdStandadDimension} ");
            Console.Write($"{commitRow[1],-dateTimeStandardDimension} ");
            Console.Write($"{commitRow[2],-authorStandardDimension}");
            
            var message = "";
            var firstThreeColumns = $"{commitRow[0]} ".Length + $"{commitRow[1]} ".Length + $"{commitRow[2]} ".Length;
            var columnsStandardDimentions = $"{commitRow[0], -commitIdStandadDimension} ".Length
                + $"{commitRow[1], -dateTimeStandardDimension} ".Length 
                  + $"{commitRow[2], -authorStandardDimension} ".Length;
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
            Console.ResetColor();
        }
    }
}
