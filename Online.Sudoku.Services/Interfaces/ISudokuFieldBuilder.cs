using Online.Sudoku.Services.SudokuServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Interfaces
{
    public interface ISudokuFieldBuilder
    {
        int Rang { get; }

        int EmptyCount { get; }

        SudokuFieldBuilder SetEmptyCount(int emptyCount);

        SudokuFieldBuilder SetRang(int rang);

        SudokuFieldBuilder InitField();

        SudokuFieldBuilder Mix();

        SudokuFieldBuilder DeleteNumbers();

        int?[,] Build();



    }
}
