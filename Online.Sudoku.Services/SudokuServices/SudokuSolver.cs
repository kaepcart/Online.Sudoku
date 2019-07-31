using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;
using Online.Sudoku.Services.Interfaces;
using Online.Sudoku.Services.Utility;

namespace Online.Sudoku.Services.SudokuServices
{
    /// <summary>
    /// Проверка есть ли решение у данной игры
    /// </summary>
    public class SudokuSolver: ISudokuSolver
    {
        private static CspTerm[] GetSlice(CspTerm[][] sudoku, int Ra, int Rb, int Ca, int Cb)
        {
            CspTerm[] slice = new CspTerm[9];
            int i = 0;
            for (int row = Ra; row < Rb + 1; row++)
                for (int col = Ca; col < Cb + 1; col++)
                {
                    {
                        slice[i++] = sudoku[row][col];
                    }
                }
            return slice;
        }

        public void Solve(int?[,] field)
        {

            ConstraintSystem S = ConstraintSystem.CreateSolver();
            CspDomain Z = S.CreateIntegerInterval(1, 9);
            CspTerm[][] sudoku = S.CreateVariableArray(Z, "cell", 9, 9);
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (field[row, col] > 0)
                    {
                        S.AddConstraints(S.Equal(field[row, col]??0, sudoku[row][col]));
                    }
                }
                S.AddConstraints(S.Unequal(GetSlice(sudoku, row, row, 0, 8)));
            }
            for (int col = 0; col < 9; col++)
            {
                S.AddConstraints(S.Unequal(GetSlice(sudoku, 0, 8, col, col)));
            }
            for (int a = 0; a < 3; a++)
            {
                for (int b = 0; b < 3; b++)
                {
                    S.AddConstraints(S.Unequal(GetSlice(sudoku, a * 3, a * 3 + 2, b * 3, b * 3 + 2)));
                }
            }
            ConstraintSolverSolution soln = S.Solve();

            if (!soln.HasFoundSolution)
            {
                throw new NoSolutionException("Для поставленной цифры нет решение!");
            }

            
            object[] h = new object[9];
            for (int row = 0; row < 9; row++)
            {
                if ((row % 3) == 0) System.Console.WriteLine();
                for (int col = 0; col < 9; col++)
                {
                    soln.TryGetValue(sudoku[row][col], out h[col]);
                }
                System.Console.WriteLine("{0}{1}{2} {3}{4}{5} {6}{7}{8}", h[0], h[1], h[2], h[3], h[4], h[5], h[6], h[7], h[8]);
            }
        }

    }
}
