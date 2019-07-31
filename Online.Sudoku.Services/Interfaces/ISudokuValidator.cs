using Online.Sudoku.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Interfaces
{
    public interface ISudokuValidator
    {
        bool IsWin(int?[,] field);

        void Validate(int?[,] field, Cell cell);

    }
}
