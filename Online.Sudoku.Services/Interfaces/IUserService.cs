using Online.Sudoku.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<TopUserViewModel>> GetTop(string conectionId);

        Task AddInTop(TopUserViewModel viewModel);
    }
}
