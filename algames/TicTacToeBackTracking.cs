using System;
using System.Collections.Generic;
namespace ALGAMES
{
    public class TicTacToeBackTracking
    {
        const int WINROWLEGTH=3;
        
        //resturns nextmove if the game is not finished, otherwise null.  
        //next will be 0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw  
        public Tuple<int,int> GetNextMove(int[,] CurrentBoard,
        int NumberOfMovsDone,int SearchrDepth,int WinId,int LoseId, out int next){
            
          var NumOfFreePosistions=CurrentBoard.GetLength(0)*CurrentBoard.GetLength(1)-NumberOfMovsDone;
          var searchList=GetFreePos(CurrentBoard,NumOfFreePosistions).ToArray();
          
          var loBound=CurrentBoard.GetLength(0);
          var hiBound=CurrentBoard.GetLength(1);
          next=-1;
          Tuple<int,int> move=null;
          bool stop=false;
          int i=0;
          //0->oponent wins,1,bot wins,2 not resolved,3 draw 
          List<Tuple<int,int>>[] candidates=new List<Tuple<int,int>>[4];
          candidates[0]=new List<Tuple<int,int>>();
          candidates[1]=new List<Tuple<int,int>>();
          candidates[2]=new List<Tuple<int,int>>();
          candidates[3]=new List<Tuple<int,int>>();
          while(i<searchList.Length && !stop)
          {
             var pos=searchList[i];
             var nextBoard=new int[loBound,hiBound];
             Array.Copy(CurrentBoard,nextBoard,CurrentBoard.Length);
             nextBoard[pos.Item1,pos.Item2]=WinId;
             var res=Evaluate(nextBoard,NumberOfMovsDone+1,WinId);
             candidates[res].Add(pos);
             if(res==1){
                 
                 stop=true;
                 move=pos;
                 next=1;
             }
             i++;
           }
          //bot wins with this move!!
          if(stop)
            return(move);
          //movements not making bot lose or draw?
          if(candidates[2].Count>0)
          {
             //TODO:select a candidate considering posible oponnet next moves 
              SearchrDepth--;
              if(SearchrDepth<1)
                {
                    move=candidates[2][0];
                    //0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw
                    next=2; 
                    return(move);
                }
             
              var CandidatesSim=Simulate(CurrentBoard,candidates[2],NumberOfMovsDone+1,SearchrDepth,WinId,LoseId);
              var res=ChooseMove(CandidatesSim.ToArray());
              move=new Tuple<int,int>(res.Item1,res.Item2);
              next=res.Item3;
          }
          //movements making bot draw
          else if(candidates[3].Count>0)
          {
              //pick one
              move=candidates[3][0];
              next=3;
          }
          else
          {
              //bot lose the match :(
              move=candidates[0][0];
              next=3;
          } 
           
           return(move);
        }
        public Tuple<int,int,int> ChooseMove(Tuple<int,int,int>[] simulateResults)
        {
            if(simulateResults.Length<1)
                throw new ArgumentException("simulateResults must havew at least one result");
            Tuple<int,int,int> res=new Tuple<int,int,int>(simulateResults[0].Item1,simulateResults[0].Item2,simulateResults[0].Item3);
            if(simulateResults[0].Item3==1)
                    return res;
            //return 0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw
            bool exit=false;
            int i=1;
            while(i<simulateResults.Length && !exit)
            {
               var result= simulateResults[i].Item3;
               if(result==1)
                 {
                     res=simulateResults[i];
                     exit=true;
                 }
               else if(result==2 && res.Item3==0)
               {
                   res=simulateResults[i];
               }
               else if(result==2 && res.Item3==3)
               {
                   //go for win!!
                    res=simulateResults[i];
               }
               else if(result==3 && res.Item3==2)
               {
                   //go for a win!!
                    res=simulateResults[i];
               }
               i++;
            } 
            return(res);
        }
         public List<Tuple<int,int,int>> Simulate(int[,] CurrentBoard,List<Tuple<int,int>> candidates,
        int NumberOfMovsDone,int SearchrDepth,int WinId,int LoseId){
          List<Tuple<int,int,int>> res= new List<Tuple<int,int,int>>();
          var loBound=CurrentBoard.GetLength(0);
          var hiBound=CurrentBoard.GetLength(1);
          foreach (var candidate in candidates)
              {
                   var nextBoard=new int[loBound,hiBound];
                   Array.Copy(CurrentBoard,nextBoard,CurrentBoard.Length);
                   nextBoard[candidate.Item1,candidate.Item2]=WinId;
                   int moveResult;
                   var move=GetNextMove(nextBoard,NumberOfMovsDone+1,SearchrDepth,LoseId,WinId,out moveResult);
                   if(moveResult==1)
                        moveResult=0;
                   else if(moveResult==0)
                   {
                         moveResult=1;
                   }
                   
                   Tuple<int,int,int> currenRes=new Tuple<int,int,int>(candidate.Item1,candidate.Item2,moveResult);
                   res.Add(currenRes); 
              }
          return(res);
        }
        
        //return 0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw  
        public int Evaluate(int[,] board,int NumberOFNonFreePositions,int WinId)
        {
            var nonFreePos=GetNonFreePos(board,NumberOFNonFreePositions);
            int result=-1;
            bool valWins=false;
            int val=-1;
            foreach( var pos in nonFreePos)
            {
                var adjacents=GetAdjacentPositions(board,pos.Item1,pos.Item2);
                val=board[pos.Item1,pos.Item2];
                //max of four iterations
                foreach(var adj in adjacents)
                {
                    if(board[adj.Item1,adj.Item2]==val)
                    {
                        int drow,crow;
                        PathDistance(pos,adj,out drow,out crow);     
                        valWins=findPath(board,adj,WINROWLEGTH-2, current=>{
                         Tuple<int,int>res =new Tuple<int,int>(current.Item1+drow,current.Item2+crow);   
                         if(checkInBounds(res,board))
                            return(res);
                         return(null);
                        });
                        if(valWins)
                            break;
                    }
               }
               if(valWins)
                  break;
            }
            if(!valWins)
            { 
                  if(NumberOFNonFreePositions==board.GetLength(0)*board.GetLength(1))
                        result=3;
                   else
                        result=2;
                   
            }
            else
            {
                result=(val==WinId)?1:0;
            }
            return(result);
        }
        
        public bool checkInBounds(Tuple<int,int> pos,int[,] board)
        {
            bool inBounds=pos.Item1>=0 && pos.Item1<board.GetLength(0) && pos.Item2>=0 && pos.Item2<board.GetLength(1);
            return(inBounds);
        }
        public void PathDistance(Tuple<int,int> src, Tuple<int,int> des,out int RowDis,out int ColDis)
        {
            RowDis=des.Item1-src.Item1;
            ColDis=des.Item2-src.Item2;
        }
        public bool findPath(int[,] board,Tuple<int,int> current, int NOfValsLeft,Func<Tuple<int,int>,Tuple<int,int>> NextInPath)
        {
             if(NOfValsLeft==0)
                return(true);
             var next=NextInPath(current);
             if(next==null)
                    return(false);
              if(board[current.Item1,current.Item2]!=board[next.Item1,next.Item2])
                    return(false);
               NOfValsLeft--;
               return findPath(board,next,NOfValsLeft,NextInPath);
             
        }
        public List<Tuple<int,int>> GetAdjacentPositions(int[,] board,int i,int j)
        {
            List<Tuple<int,int>> adjacents=new List<Tuple<int,int>>();
            adjacents.Add(new Tuple<int,int>(i-1,j));
            adjacents.Add(new Tuple<int,int>(i-1,j-1));
            adjacents.Add(new Tuple<int,int>(i-1,j+1));
            adjacents.Add(new Tuple<int,int>(i+1,j));
            adjacents.Add(new Tuple<int,int>(i+1,j+1));
            adjacents.Add(new Tuple<int,int>(i+1,j-1));
            adjacents.Add(new Tuple<int,int>(i,j-1));
            adjacents.Add(new Tuple<int,int>(i,j+1));  
            List<Tuple<int,int>> res=new List<Tuple<int,int>>();
            foreach(var adj in adjacents)
            {
                if(adj.Item1>=0 && adj.Item1<board.GetLength(0) && adj.Item2>=0 && adj.Item2<board.GetLength(1))
                {
                    res.Add(adj);
                }
            }
            
            return(res); 
        }
        //free positions have a negative value
        public List<Tuple<int, int>> GetNonFreePos(int[,] board,int NonFreePositions)
        {
            List<Tuple<int, int>> nonFreePos=new List<Tuple<int, int>>();
            var count=0;
            for(int i=0;i<board.GetLength(0) && count<NonFreePositions;i++)
            {
                for(int j=0;j<board.GetLength(1)&& count<NonFreePositions;j++)
                {
                    if(board[i,j]>=0)
                    {
                         nonFreePos.Add(new Tuple<int,int>(i,j));
                         count++;  
                    };
                  }
                }
            
               return(nonFreePos);
            }
       
        public List<Tuple<int, int>> GetFreePos(int[,] board,int MaxFreePos)
        {
            List<Tuple<int, int>> freePos=new List<Tuple<int, int>>();
            var count=0;
            for(int i=0;i<board.GetLength(0) && count<MaxFreePos;i++)
            {
                for(int j=0;j<board.GetLength(1)&& count<MaxFreePos;j++)
                {
                    if(board[i,j]<0)
                    {
                         freePos.Add(new Tuple<int,int>(i,j));
                         count++;  
                    };
                  }
                }
            
               return(freePos);
        }
    }
}