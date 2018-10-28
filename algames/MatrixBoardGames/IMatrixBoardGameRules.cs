using System;

namespace ALGAMES.MatrixBoardGames
{
    public interface IMatrixBoardGameRules
    {
        (int row, int column)[] GetFreePositions(int[,] board, int NumberOfMovesDone);
        int Evaluate(int[,] board, int WinId, int NumberOfMovsDone);

    }
}