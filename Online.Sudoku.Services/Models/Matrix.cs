using Online.Sudoku.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Models
{
   public class Matrix
    {
        
        public bool IsWin { get; set; }

        public string Error { get; set; }

        public GameStatuses status;

        public string Status
        {
            get
            {
                var st = "";
                switch (status)
                {
                    case GameStatuses.Error:
                        st = "Error";
                        break;
                    case GameStatuses.New:
                        st = "New";
                        break;
                    case GameStatuses.Join:
                        st = "Join";
                        break;
                };

                return st;
            }
        }

        public int?[,] Field { get; set; }

        public TopUserViewModel WinUser { get; set; }

    }

    public enum GameStatuses
    {
        New,
        Error,
        Join
    }


}
