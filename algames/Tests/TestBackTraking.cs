using System;
using ALGAMES.MatrixBoardGames;
namespace ALGAMES
{
    public class TestBackTraking
    {

        public void TestAvoidForASureLose(System.IO.TextWriter writer)
        {
            int[,] board ={
                           {1,0,1},
                           {-1,0,0},
                           {-1,1,-1}
                 };
            writer.WriteLine($"TestForASureLose.Initial Board\n {board.ConvertToString()}");
            writer.WriteLine($"Boot goes with 1");
            var b = new TicTacToeBackTracking();
            int result;
            var move = b.GetNextMove(board, 6, 2, 1, 0, out result);
            board[move.Item1, move.Item2] = 1;
            writer.WriteLine($"Board After move\n {board.ConvertToString()}");

            writer.WriteLine($"Test Result:(draw) {result == 3}");


        }
        public void TestInitialMovement(System.IO.TextWriter writer)
        {
            int[,] board ={
                           {-1,-1,-1},
                           {-1,-1,-1},
                           {-1,-1,-1}
                 };
            writer.WriteLine($"TestInitialMovement.Initial Board\n {board.ConvertToString()}");
            writer.WriteLine($"Boot goes with 1");
            var b = new TicTacToeBackTracking();
            int result;
            var move = b.GetNextMove(board, 0, 2, 1, 0, out result);
            board[move.Item1, move.Item2] = 1;
            writer.WriteLine($"Board After move\n {board.ConvertToString()}");

            writer.WriteLine($"Test Result:(not defined) {result}");


        }


    }
}