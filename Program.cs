using System;
using static System.Console;
using ALGAMES.PlayBots;

namespace ALGAMES
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length>0 && args[0]=="Test")
                {
                    Test(args[1]);
                    return;
                }
            else
                {
                    FourInARowCLBot bot=new FourInARowCLBot();
                    if(args.Length>0)
                    {
                        //star game from a file
                        var str=System.IO.File.ReadAllText("game.json");
                        var game=GameUtils.DeSerializeFromJson(str);
                        
                        bot.Game=game;
                        if(args.Length>1)
                        {
                            
                            int backto=int.Parse(args[1]);
                            game.BackTo(backto);
                        }
                        bot.ResumeGame();
                    }
                    else
                    {
                        bot.Play();    
                    }
                    
                    
                }  
        }
            
        private static void Test(string Test)
        {
            try 
            {
             
             TestRunner t=new TestRunner($"ALGAMES.Test{Test}");
             t.Run(Console.Out);
             }
            catch(Exception e)
            {
               
               Console.WriteLine($"\nUnhandled error:\n{e.Message}\n{e.ToString()}\nStackTrace\n{e.StackTrace}");
               
               if(e.InnerException!=null)
               {
                   e=e.InnerException;
                    Console.WriteLine($"\nUnhandled error:\n{e.Message}\n{e.ToString()}");
               }
            }
        }
        private static void TestEvaluate()
        {
                TicTacToeBackTracking b=new TicTacToeBackTracking();
            //botvalueid=0
            int[,] board={
                          {0,1,0},
                          {1,1,0},
                          {1,0,-1}
                 };
            var res=b.Evaluate(board,1,0);
            //return 0 if oponent wins, 1 if bot wins, -1 not resolved,2 if draw  
            switch (res)
            {
                case 0:
                WriteLine("oponnent wins");
                break;
                case 1:
                   WriteLine("bot wins");
                break; 
                case -1:
                   WriteLine("not resolved");
                break; 
                default:
                     WriteLine("draw");
                break;
            }
        }
        
        
        
    }
}
