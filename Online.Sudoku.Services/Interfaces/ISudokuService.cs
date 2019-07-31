using Online.Sudoku.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Interfaces
{
    public interface ISudokuService
    {
        Matrix CreateSudoku();

        Matrix GetSudoku();

        Matrix SetNumberInCell(Cell cell, string conectionId);

        void Reset();

        void RemoveUser(string connectionId);

        IEnumerable<User> AddUser(string name, string connectionId);

    }

}
