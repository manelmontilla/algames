using System;
using static System.Console;
namespace  ALGAMES.PlayBots
{
    
    public class CLPlayBot {
        
        int[,] currentBoard= {
                           {-1,1,-1},
                           {-1,-1,-1},
                           {-1,-1,-1}
                        };
        public void Play()
        {
            WriteLine("Playing tick-tack-toe. Bot plays with O, you play with X\n");
            var r=new Random().Next(1,2);    
            if(r==2)
               WriteLine("Bot starts moving!!");
             
        
        }
        
        public Tuple<int,int> AskForAMove()
        {
            Tuple<int,int> move=null;
            bool exit=false;
            while(!exit)
            {
                WriteLine("Plase enter your move coordinates separated by a blank space;")
                string imput=ReadLine();
            }
        }
        
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
        
    }
    
    

}