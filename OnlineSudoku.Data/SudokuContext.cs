using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSudoku.Data.Data
{
    public class SudokuContext :DbContext
    {
        public SudokuContext () : base("SudokuContext")
        {

        }

        public virtual DbSet<TopUser> TopUsers { get; set; }
    }
}
