using System;
using System.Collections.Generic;
namespace ALGAMES.PlayBots
{

    public class FourInARowGame
    {
        public int Rows { get; set; } = 6;
        public int Cols { get; set; } = 7;

        public int SearchDepth { get; set; } = 3;

        public int Bot_Token { get; set; } = 0;

        public int Opponent_Token { get; set; } = 1;

        public int[,] board { get; set; } = new int[6, 7];

        public int NextMovePlayerToken { get; set; } = -1;

        public int NumberOfMovementsDone { get; set; } = 0;

        public List<(int row, int col)> MovementsDone { get; set; } = new List<(int, int)>();
    }

}