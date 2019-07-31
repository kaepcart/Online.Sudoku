using Online.Sudoku.Services.Interfaces;
using Online.Sudoku.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.Services.SudokuServices
{
    /// <summary>
    /// Построитель поля
    /// </summary>
    public class SudokuFieldBuilder: ISudokuFieldBuilder
    {
        #region ctor
 
        private int?[,] field;
        private int[] digits;
        private int cellsCount;
        private readonly Random rnd;

        private int rang;
        public int Rang { get { return rang; } }

        private int emptyCount;
        public int EmptyCount { get { return emptyCount; } }

        public SudokuFieldBuilder()
        {
            this.rang = 9;
            this.emptyCount = 54;
            rnd = new Random();
        }

        #endregion

        #region SetSettings

        /// <summary>
        /// Конфигурация количество пустых клеток
        /// </summary>
        /// <param name="emptyCount"></param>
        /// <returns></returns>
        public SudokuFieldBuilder SetEmptyCount(int emptyCount)
        {
            this.emptyCount = emptyCount;

            return this;

        }

        /// <summary>
        /// Указание ранга матрицы для игры
        /// </summary>
        /// <param name="rang"></param>
        /// <returns></returns>
        public SudokuFieldBuilder SetRang(int rang)
        {
            var count = Math.Sqrt(rang);
            if (count != Math.Truncate((double)count))
            {
                throw new ArgumentException("Не допустимый ранг матрицы");
            }

            this.rang = rang;

            return this;

        }

        #endregion

        /// <summary>
        /// Создания матрицы для игры
        /// </summary>
        /// <returns></returns>
        public SudokuFieldBuilder InitField()
        {

            cellsCount = Convert.ToInt32(Math.Sqrt(rang));

            digits = new int[rang];

            digits = Enumerable.Range(1, rang).ToArray();

            field = new int?[rang, rang];

            var begin = 0;
            for (int n = 0; n < cellsCount; n++)
            {
                for (int m = 0; m < cellsCount; m++)
                {
                    int current = begin;

                    for (int i = cellsCount * m; i < cellsCount * m + cellsCount; i++)
                    {
                        for (int j = cellsCount * n; j < cellsCount * n + cellsCount; j++, current++)
                        {
                            if (current >= rang)
                            {
                                current = 0;
                            }

                            field[i, j] = digits[current];

                        }

                    }
                    begin++;
                }
            }

            return this;
        }

        /// <summary>
        /// Перемешивания цифр
        /// </summary>
        /// <returns></returns>
        public SudokuFieldBuilder Mix()
        {
            IList<Action> mixeFunctions = GetOperation();

            var countOperation = rnd.Next(10, 40);

            for (int i = 0; i < countOperation; i++)
            {
                var operation = rnd.Next(0, mixeFunctions.Count());

                mixeFunctions[operation]();
            }

            return this;
        }

        /// <summary>
        /// Удаление цифр
        /// </summary>
        /// <returns></returns>
        public SudokuFieldBuilder DeleteNumbers()
        {

            List<Cell> removedNumber = new List<Cell>();

            for (int i = 0; i < emptyCount;)
            {

                var col = rnd.Next(0, rang);
                var row = rnd.Next(0, rang);

                if (!removedNumber.Any(x => x.Column == col && x.Row == row))
                {
                    field[col, row] = null;
                    removedNumber.Add(new Cell { Column = col, Row = row });
                    i++;
                }
            }

            return this;
        }

        public int?[,] Build()
        {
            return field;
        }

        #region Transform

        public SudokuFieldBuilder Transpose()
        {

            int?[,] result = new int?[rang, rang];

            for (int i = 0; i < rang; i++)
            {
                for (int j = 0; j < rang; j++)
                {
                    result[j, i] = field[i, j];
                }
            }

            field = result;

            return this;
        }

        public SudokuFieldBuilder ShiftRows(int area, int row1, int row2)
        {
            ShiftValidation(area1: area, num1: row1, num2: row2);

            var curentRow1 = (area - 1) * cellsCount + row1 - 1;
            var curentRow2 = (area - 1) * cellsCount + row2 - 1;

            for (int i = 0; i < rang; i++)
            {
                var curenValue = field[curentRow1, i];
                field[curentRow1, i] = field[curentRow2, i];
                field[curentRow2, i] = curenValue;
            }


            return this;
        }

        public SudokuFieldBuilder ShiftColumns(int area, int column1, int column2)
        {
            ShiftValidation(area1: area, num1: column1, num2: column2);

            var curentCulomn1 = (area - 1) * cellsCount + column1 - 1;
            var curentColumn2 = (area - 1) * cellsCount + column2 - 1;

            for (int i = 0; i < rang; i++)
            {
                var curenValue = field[i, curentCulomn1];
                field[i, curentCulomn1] = field[i, curentColumn2];
                field[i, curentColumn2] = curenValue;
            }

            return this;
        }

        public SudokuFieldBuilder ShiftRowAreas(int area1, int area2)
        {
            ShiftValidation(area1: area1, area2: area2);

            for (int i1 = (area1 - 1) * cellsCount, i2 = (area2 - 1) * cellsCount; i1 < cellsCount * area1; i1++, i2++)
            {
                for (int j = 0; j < rang; j++)
                {
                    var curenValue = field[i1, j];
                    field[i1, j] = field[i2, j];
                    field[i2, j] = curenValue;

                }
            }

            return this;
        }

        public SudokuFieldBuilder ShiftColumnAreas(int area1, int area2)
        {
            ShiftValidation(area1: area1, area2: area2);

            for (int i1 = (area1 - 1) * cellsCount, i2 = (area2 - 1) * cellsCount; i1 < cellsCount * area1; i1++, i2++)
            {
                for (int j = 0; j < rang; j++)
                {
                    var curenValue = field[j, i1];
                    field[j, i1] = field[j, i2];
                    field[j, i2] = curenValue;

                }
            }

            return this;
        }

        #endregion

        /// <summary>
        /// Создание коллекции процедур перемешивание
        /// </summary>
        /// <returns></returns>
        private IList<Action> GetOperation()
        {
            return new List<Action>()
            {
                ()=>
                {
                 var area=   rnd.Next(1,cellsCount);
                 var row1=   rnd.Next(1,cellsCount);
                 var row2=   rnd.Next(1,cellsCount);
                 ShiftRows(area,row1,row2);

                },
                ()=>
                {
                 var area=   rnd.Next(1,cellsCount);
                 var column1=   rnd.Next(1,cellsCount);
                 var column2=   rnd.Next(1,cellsCount);
                 ShiftColumns(area,column1,column2);
                },
                ()=>
                {
                 var area1=   rnd.Next(1,cellsCount);
                 var area2=   rnd.Next(1,cellsCount);
                 ShiftRowAreas(area1,area2);
                },
                ()=>
                {
                 var area1=   rnd.Next(1,cellsCount);
                 var area2=   rnd.Next(1,cellsCount);
                 ShiftColumnAreas(area1,area2);
                },
                ()=>
                {
                 Transpose();
                },

            };
        }

        /// <summary>
        /// Валидация входных параметров для перемешивания цифр
        /// </summary>
        /// <param name="area1">Область 1</param>
        /// <param name="area2">Область 2</param>
        /// <param name="num1">Столбец/строка 1</param>
        /// <param name="num2">Столбец/строка 2</param>
        private void ShiftValidation(int area1, int? area2 = null, int? num1 = null, int? num2 = null)
        {
            if (area1 < 0 || area1 > cellsCount)
            {
                throw new ArgumentException("Область выходит за пределы матрицы");
            }

            if (area2 < 0 || area2 > cellsCount)
            {
                throw new ArgumentException("Вторая область выходит за пределы матрицы");
            }

            if (num1 != null && (num1 < 0 || num1 > cellsCount))
            {
                throw new ArgumentException("Первый аргумент выходит за пределы матрицы");
            }

            if (num2 != null && (num2 < 0 || num2 > cellsCount))
            {
                throw new ArgumentException("Второй аргумент выходит за пределы матрицы");
            }
        }

    }
}
