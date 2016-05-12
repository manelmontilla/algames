using System;
using System.Collections;
namespace ALGAMES.MatrixBoardGames
{
    public interface IMatrixBoardGameRules
    {
        Tuple<int,int>[] GetFreePositions(int[,] board, int NumberOfMovesDone);
        int Evaluate (int[,] board, int WinId,Tuple<int,int>[] frePoss);
    }
}