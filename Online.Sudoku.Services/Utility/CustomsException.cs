using Online.Sudoku.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Utility
{
    public class AlreadyExistException : Exception
    {
        public override string Message => message;
        private string message;

        private Cell cell;
        public Cell Cell { get { return cell; } }

        public AlreadyExistException(Cell cell, string message) : base(message)
        {
            this.message = message;
            this.cell = cell;
        }
    }

    public class NoSolutionException : Exception
    {
        public override string Message => message;
        private string message;

        public NoSolutionException(string message) : base(message)
        {
            this.message = message;

        }
    }
}
