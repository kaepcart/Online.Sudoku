using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Models
{
    /// <summary>
    /// Модель для сохранения ходов
    /// </summary>
    public class Move
    {
        public string ConnectionId { get; set; }

        public Cell Cell { get; set; }

        public DateTime Date { get; set; }

    }
}
