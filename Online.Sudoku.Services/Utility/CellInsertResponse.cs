using Online.Sudoku.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Utility
{
    public class CellResponse
    {
        public MoveStatuses status;

        public string Status
        {
            get
            {
                var st = "";
                switch (status)
                {
                    case MoveStatuses.Error:
                        st = "Error";
                        break;
                    case MoveStatuses.Won:
                        st = "Win";
                        break;
                    case MoveStatuses.Success:
                        st = "Success";
                        break;
                    case MoveStatuses.Lost:
                        st = "Lost";
                        break;
                    case MoveStatuses.NoSolution:
                        st = "NoSolution";
                        break;
                };

                return st;
            }
        }

        public Cell Cell { get; set; }

        public string Error { get; set; }

        public TopUserViewModel Results { get; set; }

        public CellResponse(AlreadyExistException ex)
        {
            status = MoveStatuses.Error;

            Cell = ex.Cell;

            Error = ex.Message;
        }

        public CellResponse(Exception ex)
        {
            status = MoveStatuses.Error;

            Error = ex.Message;
        }

        public CellResponse(NoSolutionException ex)
        {
            this.status = MoveStatuses.NoSolution;

            Error = ex.Message;
        }

        public CellResponse(Cell сell)
        {
            status = MoveStatuses.Success;
            Cell = сell;
        }

        public CellResponse(MoveStatuses status, Cell сell,TopUserViewModel results)
        {
            Cell = сell;
            this.status = status;
            Results = results;
        }

        public CellResponse(MoveStatuses status, Cell сell)
        {
            Cell = сell;
            this.status = status;
        }

    }

    public enum MoveStatuses
    {
        Error,
        NoSolution,
        Won,
        Success,
        Lost

    }
}
