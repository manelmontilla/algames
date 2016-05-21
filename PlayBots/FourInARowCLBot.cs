using System;
using ALGAMES.MatrixBoardGames;
using static System.Console;
using System.Collections.Generic;
using System.IO;

namespace  ALGAMES.PlayBots
{
    public class FourInARowCLBot
    {
       
        public FourInArowGame Game{get;set;}=new FourInArowGame ();
        
        
        private int MovementsLeft
        {
            get 
            {
                var res=this.b.Rules.GetFreePositions(this.Game.board,this.Game.numberOfMovementsDone);
                return(res.Length);
            }
        }
        
        MatrixBoardGameMiniMax b=null;
        public FourInARowCLBot()
        {
            var rules= new NInARowBoardGameRules();
            b=new MatrixBoardGameMiniMax(rules);
        }
        
        public void Play()
        {
            this.Game.board.Reset();
            this.Game.numberOfMovementsDone=0;
            this.Game.nextMovePlayerToken=this.Game.Oponent_Token;
            ResumeGame();
        }
        
        public void ResumeGame()
        {
            WriteLine($"current board:\n");
            Game.board.PrintToConsole();
            bool exit=false;
            while(!exit)
            {
                if(this.Game.nextMovePlayerToken==Game.Bot_Token)
                {
                    int result;
                    var botMove=GetBotMove(out result);
                    WriteLine($"Bot moves to {botMove.GetStringRepr()}");
                    WriteLine($"current board:\n");
                    Game.board.PrintToConsole();
                    Game.nextMovePlayerToken=Game.Oponent_Token;
                    exit=EvaluateBotStatus(result);
                 }
            
                else
                {
                    Tuple<int,int> movement=AskForOponentMove();
                    Game.board[movement.Item1,movement.Item2]=Game.Oponent_Token;
                    Game.movementsDone.Add(movement);
                    Game.numberOfMovementsDone++;
                    WriteLine($"current board:\n");
                    Game.board.PrintToConsole();
                    var res=b.Rules.Evaluate(Game.board,Game.Oponent_Token,Game.numberOfMovementsDone);
                    exit=EvaluateOponentStatus(res);
                    Game.nextMovePlayerToken=Game.Bot_Token;
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
                    correct=Game.board.IsValidMove(pos);    
                }
                else
                {
                    if(IsSaveGameCommand(movement))
                        SaveToFile(Game);
                }
            }
            return(pos);
        }

        public void SaveToFile(FourInArowGame game)
        {
            //TODO make working on widows put filename in a constant
            var val=game.SerializeToJson();
            string fileName=$"game.json";
            File.WriteAllText(fileName,val);
            WriteLine($"Game saved to {fileName}");
        }

        private bool IsSaveGameCommand(string input)
        {
            bool ret=false;
            ret=input.IndexOf("save",StringComparison.OrdinalIgnoreCase)>=0;
            return(ret);
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
        private bool EvaluateOponentStatus(int result)
        {
            bool exit=false;
            switch (result)
            {
                case 0:
                    
                         WriteLine("I win this time!!");
                         exit=true;
                    
                    break;
                 case 1:
                   
                         WriteLine(" You win well done!!");
                         exit=true;
                   
                    break;
                 case 3:
                   
                         WriteLine("We tied!!");
                         exit=true;
                   
                    break;
                default:
                    WriteLine($"I really don't know.. result:{result}");
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
            var move=b.GetNextMove(this.Game.board,this.Game.numberOfMovementsDone,Game.SearchDepth,Game.Bot_Token
            ,Game.Oponent_Token, out result);
             this.Game.numberOfMovementsDone++;
             this.Game.board[move.Item1,move.Item2]=Game.Bot_Token;
             Game.movementsDone.Add(move);
            return(move);
            
        
        }
        
        
    
    }
}
