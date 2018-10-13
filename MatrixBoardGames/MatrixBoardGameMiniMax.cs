using System;
using System.Collections.Generic;
namespace ALGAMES.MatrixBoardGames
{
    public class MatrixBoardGameMiniMax
    {

        public IMatrixBoardGameRules Rules { get; private set; }

        public MatrixBoardGameMiniMax(IMatrixBoardGameRules Rules)
        {
            this.Rules = Rules;
        }

        /// <summary>
        ///  Returns the next position the program wants to move. 
        ///  If the game is finished returns null.
        /// </summary>
        /// <param name="CurrentBoard">The current board with all the movements made byt the players.</param>
        /// <param name="NumberOfMovsDone"></param>
        /// <param name="SearchDepth">The maximum depth the program should expand the decission tree.</param>
        /// <param name="WinId">ID of the player to maximize the output.</param>
        /// <param name="LoseId">ID of the player to minimize the output.</param>
        /// <param name="next">
        /// 0 if the LoseID player wins.
        /// 1 if the winID player wins.
        /// 2 if the game is not in a finished state.
        /// 3 if draw.
        /// -1 if the game is already finished.
        /// </param>
        /// <returns>The position the program wants to move for the next movement.</returns>
        public Tuple<int, int> GetNextMove(int[,] CurrentBoard,
        int NumberOfMovsDone, int SearchDepth,
        int WinId, int LoseId, out int next)
        {

            var searchList = this.Rules.GetFreePositions(CurrentBoard, NumberOfMovsDone);
            if (searchList.Length == 0)
            {
                next = -1;
                return null;
            }
            var loBound = CurrentBoard.GetLength(0);
            var hiBound = CurrentBoard.GetLength(1);
            next = -1;
            Tuple<int, int> move = null;
            bool stop = false;
            int i = 0;
            // 0 opponent wins, 1 bot wins, 2 not resolved, 3 draw.
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
                var res = Rules.Evaluate(nextBoard, WinId, NumberOfMovsDone + 1);
                candidates[res].Add(pos);
                if (res == 1)
                {

                    stop = true;
                    move = pos;
                    next = 1;
                }
                i++;
            }
            // Program wins with this movement !!.
            if (stop)
                return (move);
            
            // Any movements not making program lose or draw ?
            if (candidates[2].Count > 0)
            {
                // TODO: Improve the selection criteria using some heuristics. 
                SearchDepth--;
                if (SearchDepth < 1)
                {
                    move = candidates[2][0];
                    // 0 opponent wins, 1 if bot wins, 2 not resolved, 3 draw.
                    next = 2;
                    return (move);
                }

                var CandidatesSim = Simulate(CurrentBoard, candidates[2], NumberOfMovsDone + 1, SearchDepth, WinId, LoseId);
                var res = ChooseMovement(CandidatesSim.ToArray());
                move = new Tuple<int, int>(res.Item1, res.Item2);
                next = res.Item3;
            }
            // Movements making bot draw
            else if (candidates[3].Count > 0)
            {
                // Pick one
                move = candidates[3][0];
                next = 3;
            }
            else
            {
                // Program loses the match :(
                move = candidates[0][0];
                next = 3;
            }

            return (move);
        }

        /// <summary>
        ///  Returns the best movement given a set of movements and the calculated outcome.
        /// </summary>
        /// <param name="simulateResults">A N*3 matrix containing per each a row a position to move
        ///  in the first two columns and a the outcome of that position in the third column.
        /// </param>
        /// <returns>The chosen movement.</returns>
        public Tuple<int, int, int> ChooseMovement(Tuple<int, int, int>[] simulateResults)
        {
            if (simulateResults.Length < 1)
                throw new ArgumentException("simulateResults must have at least one result");
            Tuple<int, int, int> res = new Tuple<int, int, int>(simulateResults[0].Item1, simulateResults[0].Item2, simulateResults[0].Item3);
            if (simulateResults[0].Item3 == 1)
                return res;
            // Return 0 if opponent wins, 1 if bot wins, 2 not resolved, 3 draw.
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
                    // go for win!!
                    
                    res = simulateResults[i];
                }
                else if (result == 3 && res.Item3 == 2)
                {
                    // go for a win!!
                    res = simulateResults[i];
                }
                i++;
            }
            return (res);
        }

        /// <summary>
        /// Simulates a the game of up to the specified Depth for a set of non terminal game positions.
        /// </summary>
        /// <param name="CurrentBoard"></param>
        /// <param name="candidates"></param>
        /// <param name="NumberOfMovsDone"></param>
        /// <param name="SearchDepth"></param>
        /// <param name="WinId"></param>
        /// <param name="LoseId"></param>
        /// <returns></returns>
        public List<Tuple<int, int, int>> Simulate(int[,] CurrentBoard, List<Tuple<int, int>> candidates,
       int NumberOfMovsDone, int SearchDepth, int WinId, int LoseId)
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
                var move = GetNextMove(nextBoard, NumberOfMovsDone + 1, SearchDepth, LoseId, WinId, out moveResult);
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