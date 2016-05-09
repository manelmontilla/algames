using System;
namespace  ALGAMES
{
    public class TestBackTraking
    {
        private void TestForAEasyWin(System.IO.TextWriter writer)
        {
            int[,] board={
                           {0,1,0},
                           {1,1,0},
                           {1,-1,0}
                 };
        writer.WriteLine($"TestForAEasyWin.Initial Board\n {board.convertToString()}");
        var b=new TicTacToeBackTracking();
        int result;
        var move=b.GetNextMove(board,8,2,1,0, out result);
        board[move.Item1,move.Item2]=1;
        writer.WriteLine($"Board After move\n {board.convertToString()}");
        
        writer.WriteLine($"Test Result:{(result==1).ToString()}");
        
        
        }
        public void TestAvoidForASureLose(System.IO.TextWriter writer)
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
        public void TestInitialMovement(System.IO.TextWriter writer)
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