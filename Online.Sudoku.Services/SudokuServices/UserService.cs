using Online.Sudoku.Services.Interfaces;
using Online.Sudoku.Services.Models;
using OnlineSudoku.Data.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.SudokuServices
{
    public class UserService : IUserService
    {
        #region ctor

        private readonly SudokuContext db;

        public UserService()
        {
            db = new SudokuContext();
        }

        #endregion

        public async Task<IEnumerable<TopUserViewModel>> GetTop(string conectionId)
        {
            var result = await db.TopUsers.Select(u => new TopUserViewModel
            {
                UserName = u.UserName,
                Count = u.CountMoves,
                Id = u.Id,
                ConectionId = u.ConectionId,
                Time = u.Time,
                IsCurrent = u.ConectionId == conectionId

            }).OrderBy(x => x.Time).Take(50).ToListAsync();

            return result;
        }

        public async Task AddInTop(TopUserViewModel viewModel)
        {
            var user = new TopUser
            {
                ConectionId = viewModel.ConectionId,
                CountMoves = viewModel.Count,
                Time = viewModel.Time,
                UserName = viewModel.UserName
            };

            db.TopUsers.Add(user);
            await db.SaveChangesAsync();
        }

    }
}
