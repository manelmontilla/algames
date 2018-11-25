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
        /// <param name="CurrentBoard">The current board with all the movements made by the players.</param>
        /// <param name="NumberOfMovsDone">Ther number of movements already done, taking into account both players.</param>
        /// <param name="SearchDepth">The maximum depth the program should expand the decission tree.</param>
        /// <param name="WinId">ID of the player to maximize the output.</param>
        /// <param name="LoseId">ID of the player to minimize the output.</param>
        /// <returns>
        /// <para>
        /// (next): 
        ///  int.MinValue if the LoseID player wins.
        ///  in.MaxValue if the winID player wins.
        ///  1 if the game is not in a finished state.
        ///  0 if draw.
        /// </para>
        /// <para>
        /// (row,col):
        ///     The position the program wants to move for the next movement.
        /// </para>
        /// </returns>
        public (int next, (int row, int column) pos) GetNextMove(int[,] CurrentBoard,
        int NumberOfMovsDone, int SearchDepth,
        int WinId, int LoseId, bool minimize = false)
        {
            var searchList = this.Rules.GetFreePositions(CurrentBoard, NumberOfMovsDone);
            if (searchList.Length == 0)
            {
                throw new ApplicationException("Error trying to get next movement the game ids finished.");
            }
            var loBound = CurrentBoard.GetLength(0);
            var hiBound = CurrentBoard.GetLength(1);
            int i = 0;
            // int.MinValue opponent wins, int.MaxValue bot wins, 1 not resolved, 0 draw.
            List<(int next, (int row, int column) pos)> candidates = new List<(int, (int, int))>();
            List<(int next, (int row, int column) pos)> openCandidates = new List<(int, (int, int))>();
            while (i < searchList.Length)
            {
                var pos = searchList[i];
                var nextBoard = new int[loBound, hiBound];
                Array.Copy(CurrentBoard, nextBoard, CurrentBoard.Length);
                nextBoard[pos.Item1, pos.Item2] = !minimize ? WinId : LoseId;
                var res = Rules.Evaluate(nextBoard, WinId, NumberOfMovsDone + 1);
                switch (res)
                {
                    case int.MaxValue:
                        if (!minimize)
                        {
                            return (res, (pos));
                        }
                        candidates.Add((res, pos));
                        break;
                    case int.MinValue:
                        if (minimize)
                        {
                            return (res, (pos));
                        }
                        candidates.Add((res, pos));
                        break;
                    case 0:
                        candidates.Add((res, pos));
                        break;
                    default:
                        openCandidates.Add((res, pos));
                        break;
                }
                i++;
            }

            SearchDepth--;
            // Expand nodes when possible.
            if (SearchDepth > 1)
            {

                foreach (var candidate in openCandidates)
                {
                    var nextBoard = new int[loBound, hiBound];
                    Array.Copy(CurrentBoard, nextBoard, CurrentBoard.Length);
                    nextBoard[candidate.pos.row, candidate.pos.column] = WinId;
                    var (moveResult, _) = GetNextMove(nextBoard, NumberOfMovsDone + 1, SearchDepth, WinId, LoseId, !minimize);
                    candidates.Add(((int)moveResult, (candidate.pos)));
                }

            }
            else
            {
                candidates.AddRange(openCandidates);
            }

            // Choose the best movement taking into account if we are minimizing or maximazing.
            return ChooseMovement(candidates.ToArray(), minimize);
        }

        /// <summary>
        ///  Returns the best movement given a set of movements and the calculated outcome.
        /// </summary>
        /// <param name="simulateResults">A N*3 matrix containing per each a row a position to move
        ///  in the second and third columns and the outcome of that position in the first column.
        /// </param>
        /// <returns>The c movement.</returns>
        public (int result, (int, int)) ChooseMovement((int result, (int, int))[] simulateResults, bool minimize)
        {
            if (simulateResults.Length < 1)
                throw new ArgumentException("simulateResults needs at least one result");
            Func<int, int, bool> comparer = (int prev, int next) =>
                 {
                     return next > prev;
                 };
            if (minimize)
            {
                comparer = (int prev, int next) =>
                {
                    return next < prev;
                };
            }
            (int result, (int, int)) current = simulateResults[0];
            // if the first movement makes the bot win choose that one.
            if (current.result == int.MaxValue && !minimize)
                return current;
            if (current.result == int.MinValue && minimize)
                return current;
            bool exit = false;
            int i = 1;
            while (i < simulateResults.Length && !exit)
            {
                if (comparer(current.result, simulateResults[i].result))
                {
                    current = simulateResults[i];
                }
                exit = (current.result == int.MaxValue && !minimize) || (current.result == int.MinValue && minimize);
                i++;
            }
            return (current);
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
       int NumberOfMovsDone, int SearchDepth, int WinId, int LoseId, bool minimize)
        {
            List<(int res, (int, int))> res = new List<(int, (int, int))>();
            var loBound = CurrentBoard.GetLength(0);
            var hiBound = CurrentBoard.GetLength(1);
            foreach (var candidate in candidates)
            {
                var nextBoard = new int[loBound, hiBound];
                Array.Copy(CurrentBoard, nextBoard, CurrentBoard.Length);
                nextBoard[candidate.row, candidate.col] = WinId;
                var (moveResult, _) = GetNextMove(nextBoard, NumberOfMovsDone + 1, SearchDepth, LoseId, WinId, minimize);
                res.Add(((int)moveResult, (candidate)));
            }
            return (res);
        }
    }
}