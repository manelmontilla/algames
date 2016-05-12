using System;
using static System.Console;


namespace ALGAMES
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args[0]=="Test")
                {
                    Test(args[1]);
                    return;
                }
            else
                {
                    
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
