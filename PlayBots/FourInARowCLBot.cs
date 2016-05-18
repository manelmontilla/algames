using System;
using ALGAMES.MatrixBoardGames;
using static System.Console;
namespace  ALGAMES.PlayBots
{
    public class FourInARowCLBot
    {
        const int ROWS=6;
        const int  COLS=7;
       
        const int SEARCHDEPTH=5;
       
        const int BOT_TOKEN=0;
        
        const int OPONENT_TOKEN=1;
         
        int[,] board=new int[ROWS,COLS];
        
        int nextMovePlayerToken=-1;
        int numberOfMovementsDone=0;
        
        private int MovementsLeft
        {
            get 
            {
                var res=this.b.Rules.GetFreePositions(this.board,numberOfMovementsDone);
                return(res.Length);
            }
        }
        
        MatrixBoardGameBackTracking b=null;
        public FourInARowCLBot()
        {
            var rules= new NInARowBoardGameRules();
            b=new MatrixBoardGameBackTracking(rules);
        }
        
        public void Play()
        {
            board.Reset();
            numberOfMovementsDone=0;
            this.nextMovePlayerToken=BOT_TOKEN;
            bool exit=false;
            while(!exit)
            {
                if(this.nextMovePlayerToken==BOT_TOKEN)
                {
                    int result;
                    var botMove=GetBotMove(out result);
                    WriteLine($"Bot moves to {botMove.GetStringRepr()}");
                    WriteLine($"current board:\n{board.convertToString()}");
                    nextMovePlayerToken=OPONENT_TOKEN;
                    exit=EvaluateBotStatus(result);
                 }
            
                else
                {
                    Tuple<int,int> movement=AskForOponentMove();
                    board[movement.Item1,movement.Item2]=OPONENT_TOKEN;
                    numberOfMovementsDone++;
                    WriteLine($"current board:\n{board.convertToString()}");
                    var res=b.Rules.Evaluate(board,BOT_TOKEN,numberOfMovementsDone);
                    exit=EvaluateOponentStatus(res);
                    nextMovePlayerToken=BOT_TOKEN;
                }
                
            }
        }

        private Tuple<int, int> AskForOponentMove()
        {
            bool correct=false;
            Tuple<int, int> pos=null;
            while(!correct)
            {
                WriteLine("Enter you movement in format row,col");
                string movement=ReadLine();
                bool okey;
                pos=movement.MovementFromString(out okey);     
                if(okey)
                {
                    correct=board.IsValidMove(pos);    
                }
            }
            return(pos);
        }
        
        
        private bool EvaluateBotStatus(int result)
        {
            bool exit=false;
            switch (result)
            {
                case 0:
                    if(this.MovementsLeft>0)
                    {
                        WriteLine("it seems you are going to win well done!!");
                        
                    }
                    else
                    {
                         WriteLine("You win!! well done");
                         exit=true;
                    }
                    break;
                 case 1:
                    if(this.MovementsLeft>0)
                    {
                        WriteLine("I'm going to win!! but well played!!");
                        
                    }
                    else
                    {
                         WriteLine("I win!!");
                         exit=true;
                    }
                    break;
                 case 3:
                    if(this.MovementsLeft>0)
                    {
                        WriteLine(" We are going to Tie!!");
                        
                    }
                    else
                    {
                         WriteLine("We tied!!");
                         exit=true;
                    }
                    break;
                default:
                    
                break;
            }
            
           return(exit);
        }
        private bool EvaluateOponentStatus(int result)
        {
            bool exit=false;
            switch (result)
            {
                case 0:
                    
                         WriteLine("You win!! well done");
                         exit=true;
                    
                    break;
                 case 1:
                   
                         WriteLine("I win!!");
                         exit=true;
                   
                    break;
                 case 3:
                   
                         WriteLine("We tied!!");
                         exit=true;
                   
                    break;
                default:
                    
                break;
            }
            
           return(exit);
        }
        //resturns nextmove if the game is not finished, otherwise null.  
        //next will be 0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw 
        //if the game is finished then next will be -1 
        public Tuple<int,int> GetBotMove(out int result)
        {
            WriteLine("Bot deciding next movement..");
            var move=b.GetNextMove(this.board,this.numberOfMovementsDone,SEARCHDEPTH,BOT_TOKEN
            ,OPONENT_TOKEN, out result);
             this.numberOfMovementsDone++;
             this.board[move.Item1,move.Item2]=BOT_TOKEN;
            return(move);
            
        
        }
        
        
    
    }
}
