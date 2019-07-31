using Online.Sudoku.Services.Models;
using OnlineSudoku.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.Utility
{
    /// <summary>
    /// Одиночка для работы с единым полем 
    /// </summary>
    public sealed class GamesSingleton
    {
        public GamesSingleton()
        {
            Users = new List<User>();
            Moves = new List<Move>();
        }

        public int?[,] Matrix;

        public bool IsWin { get; set; }

        public List<User> Users { get; set; }

        public List<TopUser> TopUsers { get; set; }

        public List<Move> Moves { get; set; }

        private static volatile GamesSingleton singletonInstance;

        private static readonly Object syncRoot = new Object();

        public static GamesSingleton Instance
        {
            get
            {
                if (singletonInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (singletonInstance == null)
                        {
                            singletonInstance = new GamesSingleton();

                        }
                    }
                }
                return singletonInstance;
            }
        }
    }


   
}
