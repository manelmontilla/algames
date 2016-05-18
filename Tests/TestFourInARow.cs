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
        writer.WriteLine($"TestForAEasyWin.\nInitial Board\n{board.convertToStringImproved()}");
        MatrixBoardGameBackTracking b=new MatrixBoardGameBackTracking(rules);
        
        int result;
        //next will be 0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw 
        var move=b.GetNextMove(board,17,8,0,1, out result);
        board[move.Item1,move.Item2]=0;
        writer.WriteLine($"Board After move\n{board.convertToStringImproved()}");
        
        writer.WriteLine($"Test Result:{(result==1).ToString()}");
        
        
        }
      
        
       
    }
}