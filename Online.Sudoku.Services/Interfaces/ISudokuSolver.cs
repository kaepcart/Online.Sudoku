using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Interfaces
{
    public interface ISudokuSolver
    {
        void Solve(int?[,] field);
    }
}
