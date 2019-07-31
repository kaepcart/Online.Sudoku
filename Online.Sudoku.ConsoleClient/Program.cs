using Online.Sudoku.Services.Models;
using Online.Sudoku.Services.SudokuServices;
using Online.Sudoku.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online.Sudoku.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuSolver ss = new SudokuSolver();
            SudokuFieldBuilder sudokuFieldBuilder = new SudokuFieldBuilder();
            SudokuValidator sudokuValidator = new SudokuValidator();

            var rang = 9;
            var mass = sudokuFieldBuilder.InitField().Mix().Build().Clone() as int?[,];
            var gameMass = sudokuFieldBuilder.SetEmptyCount(35).DeleteNumbers().Build().Clone() as int?[,];

            Console.WindowWidth = 200;

            for (int i = 0; i < rang; i++)
            {
                for (int j = 0; j < rang; j++)
                {
                    Console.Write((mass[i, j] ?? 0) + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();

            ss.Solve(gameMass);

            Console.WriteLine();

        game:
            for (int i = 0; i < rang; i++)
            {
                for (int j = 0; j < rang; j++)
                {
                    Console.Write((gameMass[i, j]??0) + " ");
                }

                Console.WriteLine();
            }

            Console.Write("Row:");
            var row = Convert.ToInt32(Console.ReadLine());

            Console.Write("Column:");
            var column = Convert.ToInt32(Console.ReadLine());

            Console.Write("Value:");
            var value = Convert.ToInt32(Console.ReadLine());

            try
            {
                sudokuValidator.Validate(gameMass, new Cell { Column = column, Row = row, Value = value });

                gameMass[row, column] = value;
            }
            catch (AlreadyExistException ex)
            {
                Console.WriteLine(ex.Message + " " + ex.Cell.ToString());
            }

            goto game;
        }
    }
}
