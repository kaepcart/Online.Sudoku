using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSudoku.Data.Data
{
    public class TopUser
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public int CountMoves { get; set; }

        public TimeSpan Time { get; set; }

        public string ConectionId { get; set; }

    }
}
