using System;
using System.Collections.Generic;
namespace  ALGAMES.PlayBots
{
        
    public class FourInArowGame
    {
        public int Rows {get;set;}=6;
        public int  Cols {get;set;}=7;
       
         public int SearchDepth {get;set;}=7;
       
        public int Bot_Token {get;set;}=0;
        
        public int Oponent_Token {get;set;}=1;
         
        public int[,] board {get;set;}=new int[6,7];
        
        public int nextMovePlayerToken {get;set;}=-1;
        public int numberOfMovementsDone {get;set;}=0;
        public  List<Tuple<int,int>> movementsDone {get;set;}=new List<Tuple<int,int>>();
    }
    
}