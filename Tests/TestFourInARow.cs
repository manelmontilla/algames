using System;
using ALGAMES.MatrixBoardGames;
namespace  ALGAMES
{
    public class TestFourInARow
    {
        public void TestForAEasyWin(System.IO.TextWriter writer)
        {
            int[,] board={
                           {0,1,-1,-1},
                           {0,1,0,-1},
                           {1,1,0,0},
                           {0,0,1,1},
                           {0,1,1,1}
                 };
        var rules= new NInARowBoardGameRules();
        writer.WriteLine($"TestForAEasyWin.\nInitial Board\n{board.convertToString()}");
        MatrixBoardGameBackTracking b=new MatrixBoardGameBackTracking(rules);
        
        int result;
        var move=b.GetNextMove(board,17,8,0,1, out result);
        board[move.Item1,move.Item2]=0;
        writer.WriteLine($"Board After move\n{board.convertToString()}");
        
        writer.WriteLine($"Test Result:{(result).ToString()}");
        
        
        }
        private void TestAvoidForASureLose(System.IO.TextWriter writer)
        {
            int[,] board={
                           {1,0,1},
                           {-1,0,0},
                           {-1,1,-1}
                 };
        writer.WriteLine($"TestForASureLose.Initial Board\n {board.convertToString()}");
        writer.WriteLine($"Boot goes with 1");
        var b=new TicTacToeBackTracking();
        int result;
        var move=b.GetNextMove(board,6,2,1,0, out result);
        board[move.Item1,move.Item2]=1;
        writer.WriteLine($"Board After move\n {board.convertToString()}");
        
        writer.WriteLine($"Test Result:(draw) {result==3}");
        
        
        }
        private void TestInitialMovement(System.IO.TextWriter writer)
        {
            int[,] board={
                           {-1,-1,-1},
                           {-1,-1,-1},
                           {-1,-1,-1}
                 };
        writer.WriteLine($"TestInitialMovement.Initial Board\n {board.convertToString()}");
        writer.WriteLine($"Boot goes with 1");
        var b=new TicTacToeBackTracking();
        int result;
        var move=b.GetNextMove(board,0,2,1,0, out result);
        board[move.Item1,move.Item2]=1;
        writer.WriteLine($"Board After move\n {board.convertToString()}");
        
        writer.WriteLine($"Test Result:(not defined) {result}");
        
        
        }
        
       
    }
}