using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Models
{
    public class Cell
    {
        public int Column { get; set; }

        public int Row { get; set; }

        public int? Value { get; set; }

        public override string ToString()
        {
            return "Row:" + Row + "; Column:" + Column + "; Value:" + Value;

        }
    }
}
