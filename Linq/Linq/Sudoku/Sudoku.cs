using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLinq
{
    public class Sudoku
    {
        public static bool IsValidSudoku(int[,] sudokuBoard)
        {
           return Enumerable.Range(0, 9).All(j => Line(sudokuBoard, j) && Column(sudokuBoard, j) && Block(sudokuBoard, j));
        }

        public static bool Line(int[,] sudokuBoard, int j)
        {
            return Enumerable.Range(0, 9).Select(i => sudokuBoard[j, i]).GroupBy(key => key).All(group => group.Count() == 1);
        }
           
        private static bool Column(int[,] sudokuBoard, int j)
        {
            return Enumerable.Range(0, 9).Select(i => sudokuBoard[i, j]).GroupBy(key => key).All(group => group.Count() == 1);
        }

        private static bool Block(int[,] sudokuBoard, int position)
        {
            var line = Enumerable.Range(position / 3 * 3, 3);
            var column = Enumerable.Range(position % 3 * 3, 3);
            var block = line.SelectMany(j => column, (i, j) => sudokuBoard[i, j]);
            return block.GroupBy(key => key).All(group => group.Count() == 1);
        }
    }
}
