using System;
using System.Collections.Generic;
namespace ALGAMES.MatrixBoardGames
{
    public class NInARowBoardGameRules : IMatrixBoardGameRules
    {
        const int WIN_ROW_LENGTH = 4;
        public (int row, int column)[] GetFreePositions(int[,] board, int NumberOfMovesDone)
        {
            // TODO: Improve. After  NumberOfMovesDone (board.GetLength(0)* board.GetLength(1))/2 
            // start visiting de rows at board.GetLength(1)-1.
            List<(int row, int col)> freePos = new List<(int, int)>();
            Queue<int> ColsToReviewForFreePlaces = new Queue<int>();
            if (board.GetLength(0) < 1)
            {
                return (freePos.ToArray());
            }

            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[board.GetLength(0) - 1, j] < 0)
                {
                    freePos.Add((board.GetLength(0) - 1, j));

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
                        freePos.Add((i, col));
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

        /// <summary>
        ///  Evaluate returns the estimated value of the current board.
        ///  Returns int.MinValue if the current board makes the winId to loose. 
        ///  Returns int.MaxValue if the current board makes the winID to win.
        ///  Returns 0 if the current board is a draw.
        ///  Returns 1 if the current board is not in a terminal status.
        ///  </summary>
        /// <param name="board"></param>
        /// <param name="WinId"></param>
        /// <param name="NumberOfMovsDone"></param>
        /// <returns></returns>
        public int Evaluate(int[,] board, int WinId, int NumberOfMovsDone)
        {
            var nonFreePos = GetNonFreePos(board, NumberOfMovsDone);
            int result = -1;
            bool valWins = false;
            int val = -1;
            foreach (var pos in nonFreePos)
            {
                var adjacent = GetAdjacentPositions(board, pos.Item1, pos.Item2);
                val = board[pos.Item1, pos.Item2];
                // 4 iterations at max.
                foreach (var adj in adjacent)
                {
                    if (board[adj.row, adj.col] == val)
                    {
                        var (drow, dcol) = PathDistance(pos, adj);
                        valWins = findPath(board, adj, WIN_ROW_LENGTH - 2, current =>
                          {
                              (int row, int col) res = (current.Item1 + drow, current.Item2 + dcol);
                              if (checkInBounds(res, board))
                                  return (res);
                              return ((null, 0));
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
                    result = 0;
                else
                    result = 1;

            }
            else
            {
                result = (val == WinId) ? int.MaxValue : int.MinValue;
            }
            return (result);
        }
        public List<(int row, int col)> GetNonFreePos(int[,] board, int NumberOfMovsDone)
        {
            // TODO: Improve bad design!!.
            List<(int row, int col)> nonFreePos = new List<(int row, int col)>();
            var freePos = GetFreePositions(board, NumberOfMovsDone);
            foreach (var pos in freePos)
            {

                int i = pos.row + 1;
                while (i < board.GetLength(0))
                {
                    nonFreePos.Add((i, pos.column));
                    i++;
                }
            }

            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[0, j] >= 0)
                {
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        nonFreePos.Add((i, j));
                    }
                }
            }

            return (nonFreePos);
        }


        public (int rowDis, int colDis) PathDistance((int row, int col) src, (int row, int col) des)
        {
            return ((des.row - src.row), (des.col - src.col));
        }

        public bool findPath(int[,] board, (int row, int col) current, int valLeft, Func<(int row, int col), (int? row, int col)> NextInPath)
        {
            if (valLeft == 0)
                return (true);
            var next = NextInPath(current);
            if (next.Item1 == null)
                return (false);
            if (board[current.Item1, current.Item2] != board[(int)next.Item1, next.Item2])
                return (false);
            valLeft--;
            return findPath(board, ((int)next.Item1, next.Item2), valLeft, NextInPath);
        }

        public bool checkInBounds((int row, int col) pos, int[,] board)
        {
            bool inBounds = pos.row >= 0 && pos.row < board.GetLength(0) && pos.col >= 0 && pos.col < board.GetLength(1);
            return (inBounds);
        }

        public List<(int row, int col)> GetAdjacentPositions(int[,] board, int i, int j)
        {
            List<(int row, int col)> adjacent = new List<(int row, int col)>();
            adjacent.Add((i - 1, j));
            adjacent.Add((i - 1, j - 1));
            adjacent.Add((i - 1, j + 1));
            adjacent.Add((i + 1, j));
            adjacent.Add((i + 1, j + 1));
            adjacent.Add((i + 1, j - 1));
            adjacent.Add((i, j - 1));
            adjacent.Add((i, j + 1));
            List<(int, int)> res = new List<(int, int)>();
            foreach (var adj in adjacent)
            {
                if (checkInBounds(adj, board))
                {
                    res.Add(adj);
                }
            }

            return (res);
        }

    }

}