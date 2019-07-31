using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Online.Sudoku.Services.Interfaces;
using Online.Sudoku.Services.Models;
using Online.Sudoku.Services.SudokuServices;
using Online.Sudoku.Services.Utility;

namespace Online.Sudoku.Hubs
{
    public class SudokuHub : Hub
    {

        #region ctor


        private readonly ISudokuService sudokuService;
        private readonly IUserService userService;

        public SudokuHub()
        {
            sudokuService = new SudokuService();
            userService = new UserService();
        }

        #endregion

        public void InsertCell(Cell cell)
        {
            CellResponse mr=null;

            try
            {
                var result = sudokuService.SetNumberInCell(cell,Context.ConnectionId);

                if (result.IsWin)
                {
                  
                    mr = new CellResponse(MoveStatuses.Won, cell, result.WinUser);
                    sudokuService.Reset();

                }
                else
                {
                    mr = new CellResponse(cell);
                }

               
            }
            catch (AlreadyExistException ex)
            {
                mr=new CellResponse(ex);
            }
            catch (NoSolutionException ex)
            {
                mr=new CellResponse(MoveStatuses.NoSolution,ex);
            }
            catch (Exception ex)
            {
                mr = new CellResponse(ex);
            }

            if (mr.status == MoveStatuses.Error)
            {

                Clients.Client(Context.ConnectionId).InsertNumber(mr);
            }
            else if (mr.status == MoveStatuses.Won)
            {
                Clients.Client(Context.ConnectionId).InsertNumber(mr);
                Clients.AllExcept(Context.ConnectionId).InsertNumber(new CellResponse(MoveStatuses.Lost, cell));
            }
            else
            {

                Clients.All.InsertNumber(mr);

            }

        }

        public void Connected(string name)
        {
            var connectionId = Context.ConnectionId;
            var users=sudokuService.AddUser(name, connectionId);

            if (users != null)
            {
                Clients.All.AddUser(users);

            }

        }

        public void JoinGame()
        {
            var matrix = sudokuService.GetSudoku();
            Clients.Caller.GetMatrix(matrix);
        }

        public void NewGame()
        {

           var  matrix = sudokuService.CreateSudoku();
            Clients.Caller.GetMatrix(matrix);
        }

        public void EndGame()
        {
            sudokuService.RemoveUser(Context.ConnectionId);
        }

        public async Task UsersTop()
        {
           var top= await userService.GetTop(Context.ConnectionId);
           Clients.Caller.GetUserTop(top);
        }

        public async Task AddUserInTop(TopUserViewModel user)
        {
            user.ConectionId = Context.ConnectionId;

            await userService.AddInTop(user);

            Clients.Caller.UserAdded();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            sudokuService.RemoveUser(Context.ConnectionId);
            Clients.All.RemoveUser(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}