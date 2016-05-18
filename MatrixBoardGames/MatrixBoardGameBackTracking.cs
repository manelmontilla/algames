using System;
using System.Collections.Generic;
namespace ALGAMES.MatrixBoardGames
{
    public class MatrixBoardGameBackTracking
    {
        
         public IMatrixBoardGameRules Rules{get;private set;} 
         
         public MatrixBoardGameBackTracking(IMatrixBoardGameRules Rules)
         {
             this.Rules=Rules;
         }
         
         
        //resturns nextmove if the game is not finished, otherwise null.  
        //next will be 0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw 
        //if the game is finished then next will be -1 
        public Tuple<int, int> GetNextMove(int[,] CurrentBoard,
        int NumberOfMovsDone,int SearchDepth,  
        int WinId, int LoseId, out int next)
        {

           var searchList = this.Rules.GetFreePositions(CurrentBoard, NumberOfMovsDone);
           if(searchList.Length==0)
           {
               next=-1;
               return null;
           }
           var loBound = CurrentBoard.GetLength(0);
            var hiBound = CurrentBoard.GetLength(1);
            next = -1;
            Tuple<int, int> move = null;
            bool stop = false;
            int i = 0;
            //0->oponent wins,1 bot wins,2 not resolved,3 draw 
            List<Tuple<int, int>>[] candidates = new List<Tuple<int, int>>[4];
            candidates[0] = new List<Tuple<int, int>>();
            candidates[1] = new List<Tuple<int, int>>();
            candidates[2] = new List<Tuple<int, int>>();
            candidates[3] = new List<Tuple<int, int>>();
            while (i < searchList.Length && !stop)
            {
                var pos = searchList[i];
                var nextBoard = new int[loBound, hiBound];
                Array.Copy(CurrentBoard, nextBoard, CurrentBoard.Length);
                nextBoard[pos.Item1, pos.Item2] = WinId;
                var res = Rules.Evaluate(nextBoard,WinId,NumberOfMovsDone+1);
                candidates[res].Add(pos);
                if (res == 1)
                {

                    stop = true;
                    move = pos;
                    next = 1;
                }
                i++;
            }
            //bot wins with this move!!
            if (stop)
                return (move);
            //movements not making bot lose or draw?
            if (candidates[2].Count > 0)
            {
                //TODO:select a candidate considering posible oponnet next moves 
                SearchDepth--;
                if (SearchDepth< 1)
                {
                    move = candidates[2][0];
                    //0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw
                    next = 2;
                    return (move);
                }

                var CandidatesSim = Simulate(CurrentBoard, candidates[2], NumberOfMovsDone + 1, SearchDepth, WinId, LoseId);
                var res = ChooseMove(CandidatesSim.ToArray());
                move = new Tuple<int, int>(res.Item1, res.Item2);
                next = res.Item3;
            }
            //movements making bot draw
            else if (candidates[3].Count > 0)
            {
                //pick one
                move = candidates[3][0];
                next = 3;
            }
            else
            {
                //bot lose the match :(
                move = candidates[0][0];
                next = 3;
            }

            return (move);
        }
        public Tuple<int, int, int> ChooseMove(Tuple<int, int, int>[] simulateResults)
        {
            if (simulateResults.Length < 1)
                throw new ArgumentException("simulateResults must have at least one result");
            Tuple<int, int, int> res = new Tuple<int, int, int>(simulateResults[0].Item1, simulateResults[0].Item2, simulateResults[0].Item3);
            if (simulateResults[0].Item3 == 1)
                return res;
            //return 0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw
            bool exit = false;
            int i = 1;
            while (i < simulateResults.Length && !exit)
            {
                var result = simulateResults[i].Item3;
                if (result == 1)
                {
                    res = simulateResults[i];
                    exit = true;
                }
                else if (result == 2 && res.Item3 == 0)
                {
                    res = simulateResults[i];
                }
                else if (result == 2 && res.Item3 == 3)
                {
                    //go for win!!
                    res = simulateResults[i];
                }
                else if (result == 3 && res.Item3 == 2)
                {
                    //go for a win!!
                    res = simulateResults[i];
                }
                i++;
            }
            return (res);
        }
        public List<Tuple<int, int, int>> Simulate(int[,] CurrentBoard, List<Tuple<int, int>> candidates,
       int NumberOfMovsDone, int SearchrDepth, int WinId, int LoseId)
        {
            List<Tuple<int, int, int>> res = new List<Tuple<int, int, int>>();
            var loBound = CurrentBoard.GetLength(0);
            var hiBound = CurrentBoard.GetLength(1);
            foreach (var candidate in candidates)
            {
                var nextBoard = new int[loBound, hiBound];
                Array.Copy(CurrentBoard, nextBoard, CurrentBoard.Length);
                nextBoard[candidate.Item1, candidate.Item2] = WinId;
                int moveResult;
                var move = GetNextMove(nextBoard, NumberOfMovsDone + 1, SearchrDepth, LoseId, WinId, out moveResult);
                if (moveResult == 1)
                    moveResult = 0;
                else if (moveResult == 0)
                {
                    moveResult = 1;
                }

                Tuple<int, int, int> currenRes = new Tuple<int, int, int>(candidate.Item1, candidate.Item2, moveResult);
                res.Add(currenRes);
            }
            return (res);
        }
    }
}