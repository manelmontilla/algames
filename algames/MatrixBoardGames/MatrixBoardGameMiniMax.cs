using System;
using System.Collections.Generic;
using System.Linq;
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
        public (int next, (int row, int column) pos) GetNextMove(int[,] CurrentBoard,
        int NumberOfMovsDone, int SearchDepth,
        int WinId, int LoseId)
        {
            var searchList = this.Rules.GetFreePositions(CurrentBoard, NumberOfMovsDone);
            if (searchList.Length == 0)
            {
                return (-1, (0, 0));
            }
            var loBound = CurrentBoard.GetLength(0);
            var hiBound = CurrentBoard.GetLength(1);
            (int column, int row) move = (0, 0);
            int i = 0;
            // 0 opponent wins, 1 bot wins, 2 not resolved, 3 draw.
            List<(int, int)>[] candidates = new List<(int, int)>[4]{
               new List<(int, int)>(),
               new List<(int, int)>(),
               new List<(int, int)>(),
               new List<(int, int)>(),
            };
            while (i < searchList.Length)
            {
                var pos = searchList[i];
                var nextBoard = new int[loBound, hiBound];
                Array.Copy(CurrentBoard, nextBoard, CurrentBoard.Length);
                nextBoard[pos.Item1, pos.Item2] = WinId;
                var res = Rules.Evaluate(nextBoard, WinId, NumberOfMovsDone + 1);
                candidates[res].Add(pos);
                // res == 1 means the bot wins the game with this movement.
                // so we can return immediately.
                if (res == 1)
                {
                    return (1, (pos));
                }
                i++;
            }

            // Any movements not making program lose or draw ?.
            if (candidates[2].Count > 0)
            {
                // TODO: Improve the selection criteria using some heuristics. 
                SearchDepth--;
                if (SearchDepth < 1)
                {
                    move = candidates[2][0];
                    // 0 opponent wins, 1 if bot wins, 2 not resolved, 3 draw.
                    return (2, (move));
                }

                var CandidatesSim = Simulate(CurrentBoard, candidates[2], NumberOfMovsDone + 1, SearchDepth, WinId, LoseId);
                return ChooseMovement(CandidatesSim.ToArray());
            }
            // Movements making bot draw. result = 3.
            else if (candidates[3].Count > 0)
            {
                // Pick one
                return (3, candidates[3][0]);

            }
            else
            {
                // Program loses the match :(
                move = candidates[0][0];
                return (0, move);
            }
        }

        /// <summary>
        ///  Returns the best movement given a set of movements and the calculated outcome.
        /// </summary>
        /// <param name="simulateResults">A N*3 matrix containing per each a row a position to move
        ///  in the first two columns and a the outcome of that position in the third column.
        /// </param>
        /// <returns>The chosen movement.</returns>
        public (int result, (int, int)) ChooseMovement((int result, (int, int))[] simulateResults)
        {
            if (simulateResults.Length < 1)
                throw new ArgumentException("simulateResults needs at least one result");
            (int result, (int, int)) current = simulateResults[0];
            // if the first movement makes the bot win choose that one.
            if (current.result == 1)
                return current;
            // Return 0 if opponent wins, 1 if bot wins, 2 not resolved, 3 draw.
            bool exit = false;
            int i = 1;
            while (i < simulateResults.Length && !exit)
            {
                if (this.isBetter(current.result, simulateResults[i].result))
                {
                    current = simulateResults[i];
                }
                exit = (current.result == 1);
                i++;
            }
            return (current);
        }

        public bool isBetter(int current, int candidate)
        {
            // 0 if opponent wins, 1 if bot wins, 2 not resolved, 3 draw.
            int[][] compareTable = new int[4][];
            compareTable[0] = new int[] { 1, 2, 3 };
            compareTable[1] = new int[] { 1 };
            compareTable[2] = new int[] { 1 };
            compareTable[3] = new int[] { 1, 2 };

            return compareTable[current].Where(x => x == candidate).Count() > 0;

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
        public List<(int res, (int, int))> Simulate(int[,] CurrentBoard, List<(int row, int col)> candidates,
       int NumberOfMovsDone, int SearchDepth, int WinId, int LoseId)
        {
            List<(int res, (int, int))> res = new List<(int, (int, int))>();
            var loBound = CurrentBoard.GetLength(0);
            var hiBound = CurrentBoard.GetLength(1);
            foreach (var candidate in candidates)
            {
                var nextBoard = new int[loBound, hiBound];
                Array.Copy(CurrentBoard, nextBoard, CurrentBoard.Length);
                nextBoard[candidate.row, candidate.col] = WinId;
                var (moveResult, _) = GetNextMove(nextBoard, NumberOfMovsDone + 1, SearchDepth, LoseId, WinId);
                if (moveResult == 1)
                    moveResult = 0;
                else if (moveResult == 0)
                {
                    moveResult = 1;
                }
                res.Add((moveResult, (candidate)));
            }
            return (res);
        }
    }
}