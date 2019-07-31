using Online.Sudoku.Services.Interfaces;
using Online.Sudoku.Services.Models;
using Online.Sudoku.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.SudokuServices
{
    /// <summary>
    /// Проверка на валидность текущей игры
    /// </summary>
    public class SudokuValidator: ISudokuValidator
    {
        /// <summary>
        /// Проверка на конец игры
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool IsWin(int?[,] field)
        {
            var len = field.GetLength(0);
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (field[i, j] == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Валидация поле после вставки
        /// </summary>
        /// <param name="field">Поле</param>
        /// <param name="cell">Вставленная клетка</param>
        public void Validate(int?[,] field, Cell cell)
        {

            if (field[cell.Row, cell.Column] != null)
            {
                cell.Value = field[cell.Row, cell.Column];
                throw new AlreadyExistException(cell, "Поле уже заполнено!");
            }

            LineCheck(field, cell);

            AreaCheck(field, cell);

        }

        /// <summary>
        /// Валидация строк столбцов
        /// </summary>
        /// <param name="field">Поле</param>
        /// <param name="cell">Вставленная клетка</param>
        private static void LineCheck(int?[,] field, Cell cell)
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                if (field[i, cell.Column] == cell.Value)
                {
                    var exceptionCell = new Cell
                    {
                        Column = cell.Column,
                        Row = i,
                        Value = field[i, cell.Column]
                    };
                    throw new AlreadyExistException(exceptionCell, "Данное число уже есть в столбце!");
                }

                if (field[cell.Row, i] == cell.Value)
                {
                    var exceptionCell = new Cell
                    {
                        Column = i,
                        Row = cell.Row,
                        Value = field[cell.Row, i]
                    };
                    throw new AlreadyExistException(exceptionCell, "Данное число уже есть в строке!");
                }

            }
        }

        /// <summary>
        /// Валидация области
        /// </summary>
        /// <param name="field">Поле</param>
        /// <param name="cell">Вставленная клетка</param>
        private static void AreaCheck(int?[,] field, Cell cell)
        {
            var cellsCount = Convert.ToInt32(Math.Sqrt(field.GetLength(0)));
            var rang = field.GetLength(0);

            var beginColumn = (int)(cell.Column / cellsCount) * cellsCount;

            var beginRow = (int)(cell.Row / cellsCount) * cellsCount;


            for (int i = beginRow; i < beginRow + cellsCount; i++)
            {
                for (int j = beginColumn; j < beginColumn + cellsCount; j++)
                {


                    if (field[i, j] == cell.Value)
                    {
                        var exceptionCell = new Cell
                        {
                            Column = j,
                            Row = i
                        };
                        throw new AlreadyExistException(exceptionCell, "Данное число уже есть в области!");
                    }

                }

            }
        }
    }
}
