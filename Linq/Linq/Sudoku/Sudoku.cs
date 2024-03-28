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
            var concatenatedElements = Lines(sudokuBoard).Concat(Columns(sudokuBoard)).Concat(Blocks(sudokuBoard));
            return concatenatedElements.All(elements => elements.Distinct().Count() == 9);
        }

        public static IEnumerable<IEnumerable<int>> Lines(int[,] sudokuBoard)
        {
            return Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(j => sudokuBoard[i, j]));
        }
           
        private static IEnumerable<IEnumerable<int>> Columns(int[,] sudokuBoard)
        {
            return Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(j => sudokuBoard[i, j]));
        }

        private static IEnumerable<IEnumerable<int>> Blocks(int[,] sudokuBoard)
        {
            return Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9)
            .Select(j => sudokuBoard[i / 3 * 3 + j / 3, i % 3 * 3 + j % 3]));
        }
    }
}
