using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SudokuLinq
{
    public class SudokuFacts
    {
        [Fact]
        public void IsValidSudoku()
        {
            var sudokuBoard = new int [,] {  { 9, 1, 8,  5, 7, 2,  6, 4, 3 },
                                             { 7, 5, 3,  6, 9, 4,  1, 8, 2 },
                                             { 2, 6, 4,  1, 8, 3,  7, 9, 5 },
                                            
                                             { 1, 9, 6,  4, 2, 8,  5, 3, 7 },
                                             { 3, 8, 2,  7, 5, 6,  9, 1, 4 },
                                             { 5, 4, 7,  9, 3, 1,  8, 2, 6 },
                                            
                                             { 4, 7, 9,  2, 1, 5,  3, 6, 8 },
                                             { 8, 2, 5,  3, 6, 9,  4, 7, 1 },
                                             { 6, 3, 1,  8, 4, 7,  2, 5, 9 } 
                                          };
            
            var result = Sudoku.IsValidSudoku(sudokuBoard);
            Assert.True(result);
        }

        [Fact]
        public void InvalidLine()
        {
            var sudokuBoard = new int[,] {   { 9, 1, 8,  5, 7, 2,  6, 4, 3 },
                                             { 7, 5, 3,  6, 9, 4,  1, 8, 7 },
                                             { 2, 6, 4,  1, 8, 3,  7, 9, 5 },

                                             { 1, 9, 6,  4, 2, 8,  5, 3, 7 },
                                             { 3, 8, 2,  7, 5, 6,  9, 1, 4 },
                                             { 5, 4, 7,  9, 3, 1,  8, 2, 6 },

                                             { 4, 7, 9,  2, 1, 5,  3, 6, 8 },
                                             { 8, 2, 5,  3, 6, 9,  4, 7, 1 },
                                             { 6, 3, 1,  8, 4, 7,  2, 5, 9 }
                                          };

            var result = Sudoku.IsValidSudoku(sudokuBoard);
            Assert.False(result);
        }

        [Fact]
        public void InvalidColumn()
        {
            var sudokuBoard = new int[,] {   { 9, 1, 8,  5, 7, 2,  6, 4, 3 },
                                             { 7, 5, 3,  6, 9, 4,  1, 8, 2 },
                                             { 2, 6, 4,  1, 8, 3,  7, 9, 5 },

                                             { 1, 9, 6,  4, 2, 8,  5, 3, 7 },
                                             { 3, 8, 2,  7, 5, 6,  9, 1, 4 },
                                             { 5, 4, 7,  9, 3, 1,  8, 2, 6 },

                                             { 4, 7, 9,  2, 1, 5,  3, 6, 8 },
                                             { 8, 2, 5,  3, 6, 9,  4, 7, 1 },
                                             { 6, 1, 1,  8, 4, 7,  2, 5, 9 }
                                          };

            var result = Sudoku.IsValidSudoku(sudokuBoard);
            Assert.False(result);
        }

        [Fact]
        public void InvalidBlock()
        {
            var sudokuBoard = new int[,] {   { 9, 1, 8,  5, 7, 2,  6, 4, 3 },
                                             { 7, 5, 3,  6, 9, 4,  1, 8, 2 },
                                             { 2, 6, 4,  1, 8, 3,  7, 9, 5 },

                                             { 1, 9, 6,  4, 2, 8,  5, 3, 7 },
                                             { 3, 8, 2,  7, 5, 6,  9, 1, 4 },
                                             { 5, 4, 7,  9, 3, 1,  8, 2, 6 },

                                             { 1, 7, 9,  2, 1, 5,  3, 6, 8 },
                                             { 8, 2, 5,  3, 6, 9,  4, 7, 1 },
                                             { 6, 3, 1,  8, 4, 7,  2, 5, 9 }
                                          };

            var result = Sudoku.IsValidSudoku(sudokuBoard);
            Assert.False(result);
        }

        [Fact]
        public void InputNumberIsNotInTheCorrectInterval()
        {
            var sudokuBoard = new int[,] {   { -9, 1, 8,  5, 7, 2,  6, 4, 3 },
                                             { 7, 5, 3,  6, 9, 4,  1, 8, 2 },
                                             { 2, 6, 4,  1, 8, 3,  7, 9, 5 },

                                             { 1, 9, 6,  4, 2, 8,  5, 3, 7 },
                                             { 3, 8, 2,  7, 5, 6,  9, 1, 4 },
                                             { 5, 4, 7,  9, 3, 1,  8, 2, 6 },

                                             { 1, 7, 9,  2, 1, 5,  3, 6, 8 },
                                             { 8, 2, 5,  3, 6, 9,  4, 7, 1 },
                                             { 6, 3, 1,  8, 4, 7,  2, 5, 9 }
                                          };

            var result = Sudoku.IsValidSudoku(sudokuBoard);
            Assert.False(result);
        }
    }
}
