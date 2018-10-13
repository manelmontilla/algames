using System;
using System.Collections.Generic;
namespace ALGAMES.MatrixBoardGames
{
    public class NInARowBoardGameRules : IMatrixBoardGameRules
    {
        const int WIN_ROW_LENGTH = 4;
        public Tuple<int, int>[] GetFreePositions(int[,] board, int NumberOfMovesDone)
        {
            // TODO: Improve. After  NumberOfMovesDone (board.GetLength(0)* board.GetLength(1))/2 
            // start visiting de rows at board.GetLength(1)-1.
            List<Tuple<int, int>> freePos = new List<Tuple<int, int>>();
            Queue<int> ColsToReviewForFreePlaces = new Queue<int>();
            if (board.GetLength(0) < 1)
            {
                return (freePos.ToArray());
            }

            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[board.GetLength(0) - 1, j] < 0)
                {
                    freePos.Add(new Tuple<int, int>(board.GetLength(0) - 1, j));

                }
                else
                {
                    ColsToReviewForFreePlaces.Enqueue(j);
                }

            }

            int i = board.GetLength(0) - 2;
            while (i >= 0 && ColsToReviewForFreePlaces.Count > 0)
            {
                var auxQueue = new Queue<int>();
                while (ColsToReviewForFreePlaces.Count > 0)
                {
                    int col = ColsToReviewForFreePlaces.Dequeue();
                    if (board[i, col] < 0)
                    {
                        freePos.Add(new Tuple<int, int>(i, col));
                    }
                    else
                    {
                        auxQueue.Enqueue(col);
                    }
                }

                ColsToReviewForFreePlaces = auxQueue;
                i--;
            }

            return (freePos.ToArray());
        }

        public int Evaluate(int[,] board, int WinId, int NumberOfMovsDone)
        {

            var nonFreePos = GetNonFreePos(board, NumberOfMovsDone);
            int result = -1;
            bool valWins = false;
            int val = -1;
            foreach (var pos in nonFreePos)
            {
                var adjacents = GetAdjacentPositions(board, pos.Item1, pos.Item2);
                val = board[pos.Item1, pos.Item2];
                // 4 iterations at max.
                foreach (var adj in adjacents)
                {
                    if (board[adj.Item1, adj.Item2] == val)
                    {
                        int drow, crow;
                        PathDistance(pos, adj, out drow, out crow);
                        valWins = findPath(board, adj, WIN_ROW_LENGTH - 2, current =>
                          {
                              Tuple<int, int> res = new Tuple<int, int>(current.Item1 + drow, current.Item2 + crow);
                              if (checkInBounds(res, board))
                                  return (res);
                              return (null);
                          });
                        if (valWins)
                            break;
                    }
                }
                if (valWins)
                    break;
            }
            if (!valWins)
            {
                if (nonFreePos.Count == board.GetLength(0) * board.GetLength(1))
                    result = 3;
                else
                    result = 2;

            }
            else
            {
                result = (val == WinId) ? 1 : 0;
            }
            return (result);
        }
        public List<Tuple<int, int>> GetNonFreePos(int[,] board, int NumberOfMovsDone)
        {
            // TODO: Improve bad design!!.
            List<Tuple<int, int>> nonFreePos = new List<Tuple<int, int>>();
            var freePos = GetFreePositions(board, NumberOfMovsDone);
            foreach (var pos in freePos)
            {

                int i = pos.Item1 + 1;

                while (i < board.GetLength(0))
                {
                    nonFreePos.Add(new Tuple<int, int>(i, pos.Item2));
                    i++;
                }
            }

            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[0, j] >= 0)
                {
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        nonFreePos.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return (nonFreePos);
        }

        public bool checkInBounds(Tuple<int, int> pos, int[,] board)
        {
            bool inBounds = pos.Item1 >= 0 && pos.Item1 < board.GetLength(0) && pos.Item2 >= 0 && pos.Item2 < board.GetLength(1);
            return (inBounds);
        }

        public void PathDistance(Tuple<int, int> src, Tuple<int, int> des, out int RowDis, out int ColDis)
        {
            RowDis = des.Item1 - src.Item1;
            ColDis = des.Item2 - src.Item2;
        }

        public bool findPath(int[,] board, Tuple<int, int> current, int NOfValsLeft, Func<Tuple<int, int>, Tuple<int, int>> NextInPath)
        {
            if (NOfValsLeft == 0)
                return (true);
            var next = NextInPath(current);
            if (next == null)
                return (false);
            if (board[current.Item1, current.Item2] != board[next.Item1, next.Item2])
                return (false);
            NOfValsLeft--;
            return findPath(board, next, NOfValsLeft, NextInPath);
        }

        public List<Tuple<int, int>> GetAdjacentPositions(int[,] board, int i, int j)
        {
            List<Tuple<int, int>> adjacents = new List<Tuple<int, int>>();
            adjacents.Add(new Tuple<int, int>(i - 1, j));
            adjacents.Add(new Tuple<int, int>(i - 1, j - 1));
            adjacents.Add(new Tuple<int, int>(i - 1, j + 1));
            adjacents.Add(new Tuple<int, int>(i + 1, j));
            adjacents.Add(new Tuple<int, int>(i + 1, j + 1));
            adjacents.Add(new Tuple<int, int>(i + 1, j - 1));
            adjacents.Add(new Tuple<int, int>(i, j - 1));
            adjacents.Add(new Tuple<int, int>(i, j + 1));
            List<Tuple<int, int>> res = new List<Tuple<int, int>>();
            foreach (var adj in adjacents)
            {
                if (adj.Item1 >= 0 && adj.Item1 < board.GetLength(0) && adj.Item2 >= 0 && adj.Item2 < board.GetLength(1))
                {
                    res.Add(adj);
                }
            }

            return (res);
        }

    }

}