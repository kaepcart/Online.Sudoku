using Online.Sudoku.Services.Interfaces;
using Online.Sudoku.Services.Models;
using Online.Sudoku.Services.Utility;
using OnlineSudoku.Data.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.SudokuServices
{
    /// <summary>
    /// Сервис для работы с игровым полем
    /// </summary>
    public class SudokuService : ISudokuService
    {
        #region ctor


        private readonly ISudokuFieldBuilder sudokuFieldBuilder;
        private readonly ISudokuValidator sudokuValidator;
        private readonly ISudokuSolver sudokuSolver;

        private static readonly Object syncRoot = new Object();


        public SudokuService()
        {
            sudokuFieldBuilder = new SudokuFieldBuilder();
            sudokuValidator = new SudokuValidator();
            sudokuSolver = new SudokuSolver();

        }

        #endregion

        #region Work with field

        /// <summary>
        /// Создание нового поле
        /// </summary>
        /// <returns></returns>
        public Matrix CreateSudoku()
        {
            var model = new Matrix();

            if (GamesSingleton.Instance.Matrix != null)
            {
                model.status = GameStatuses.Error;
                model.Error = "Игра уже начата, Вам следует присоединися к игре";
                return model;
            }

            model.Field = sudokuFieldBuilder.InitField().Mix().DeleteNumbers().Build();

            GamesSingleton.Instance.Matrix = model.Field;

            return model;
        }

        /// <summary>
        /// Получить текущее игровое поле
        /// </summary>
        /// <returns></returns>
        public Matrix GetSudoku()
        {
            var model = new Matrix();

            if (GamesSingleton.Instance.Matrix == null)
            {
                model.status = GameStatuses.Error;
                model.Error = "Игра не начата, начните новую иигру";
                return model;
            }

            model.Field = GamesSingleton.Instance.Matrix;

            return model;
        }

        /// <summary>
        /// Вставка новой цифры в клетку
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="conectionId"></param>
        /// <returns></returns>
        public Matrix SetNumberInCell(Cell cell, string conectionId)
        {
            int?[,] sudocu;
            var matrixModel = new Matrix();
            string user = "";
            var movesCount = 0;
            var gameTime = new TimeSpan();

            lock (syncRoot)
            {
                if (GamesSingleton.Instance.IsWin)
                {
                    return matrixModel;
                }

                sudocu = InsertCell(cell);

                GamesSingleton.Instance.Moves.Add(new Move
                {
                    Cell = cell,
                    Date = DateTime.Now,
                    ConnectionId = conectionId

                });

                matrixModel.IsWin = sudokuValidator.IsWin(sudocu);

                GamesSingleton.Instance.IsWin = matrixModel.IsWin;

                if (matrixModel.IsWin)
                {
                    user = GamesSingleton.Instance.Users.FirstOrDefault(x => x.ConnectionId == conectionId)?.Name;
                    movesCount = GamesSingleton.Instance.Moves.Where(x => x.ConnectionId == conectionId).Count();
                    gameTime = GamesSingleton.Instance.Moves.Max(x => x.Date) - GamesSingleton.Instance.Moves.Min(x => x.Date);
                }

                GamesSingleton.Instance.Matrix = sudocu;
            }

            if (matrixModel.IsWin)
            {
                matrixModel.WinUser = GetWinUser(conectionId, user, movesCount, gameTime);
            }

            matrixModel.Field = sudocu;

            return matrixModel;
        }

        /// <summary>
        /// Добавиление победителя в ТОП
        /// </summary>
        /// <param name="conectionId">id сесии соединения</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="movesCount">Количество ходов пользователя</param>
        /// <param name="gameTime">Игровое время</param>
        private TopUserViewModel GetWinUser(string conectionId, string userName, int movesCount, TimeSpan gameTime)
        {
            var userViewModel = new TopUserViewModel
            {
                UserName = userName,
                Count = movesCount,
                Time = gameTime,
                ConectionId = conectionId
            };

            return userViewModel;
        }

        /// <summary>
        /// Валидация и вставка цифры в клетку
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int?[,] InsertCell(Cell cell)
        {
            int?[,] sudocu = GamesSingleton.Instance.Matrix.Clone() as int?[,];

            sudokuValidator.Validate(sudocu, cell);

            sudocu[cell.Row, cell.Column] = cell.Value;

            sudokuSolver.Solve(sudocu);

            return sudocu;
        }

        /// <summary>
        /// Сброс игры
        /// </summary>
        public void Reset()
        {
            lock (syncRoot)
            {
                GamesSingleton.Instance.Matrix = null;
                GamesSingleton.Instance.Users = new List<User>();
                GamesSingleton.Instance.Moves = new List<Move>();
                GamesSingleton.Instance.IsWin = false;
            }

        }

        #endregion

        #region Work woth users


        public IEnumerable<User> AddUser(string name, string connectionId)
        {
            User user = null;
            List<User> users = null;
            lock (syncRoot)
            {
                if (!GamesSingleton.Instance.Users.Any(x => x.ConnectionId == connectionId))
                {
                    user = new User
                    {
                        ConnectionId = connectionId,
                        Name = name
                    };

                    GamesSingleton.Instance.Users.Add(user);
                    users = GamesSingleton.Instance.Users;
                };

            }

            return users;
        }

        public void RemoveUser(string connectionId)
        {
            lock (syncRoot)
            {
                var item = GamesSingleton.Instance.Users.FirstOrDefault(x => x.ConnectionId == connectionId);
                if (item != null)
                {
                    GamesSingleton.Instance.Users.Remove(item);


                    if (!GamesSingleton.Instance.Users.Any())
                    {
                        Reset();
                    }
                }

            }
        }

       
        #endregion
    }
}
