using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ALGAMES.PlayBots
{
    public static class GameUtils
    {

        public static int RowLength(this int[,] arr)
        {
            return (arr.GetLength(0));
        }
        public static int ColLength(this int[,] arr)
        {
            return (arr.GetLength(1));
        }
        public static void SetValues(this int[,] arr, Func<int, int> SetFunc)
        {
            for (int i = 0; i < arr.RowLength(); i++)
            {
                for (int j = 0; j < arr.ColLength(); j++)
                {
                    arr[i, j] = SetFunc(arr[i, j]);
                }
            }
        }
        public static void Reset(this int[,] arr)
        {
            arr.SetValues(val => -1);
        }

        public static bool IsValidMove(this int[,] arr, (int row, int col) pos)
        {
            bool isValid = pos.row >= 0 && pos.col >= 0 && pos.row < arr.RowLength() && pos.col < arr.ColLength();
            isValid = isValid && arr[pos.row, pos.col] < 0;
            if (pos.Item1 < arr.RowLength() - 1)
            {
                isValid = isValid && arr[pos.row + 1, pos.col] >= 0;
            }
            return (isValid);
        }

        public static string GetStringRepr(this (int, int) pos)
        {
            return ($"({pos.Item1},{pos.Item2})");
        }

        public static Dictionary<int, string> TranslateTable = new Dictionary<int, string>{
             {0,"0"},
             {1,"1"}
          };

        public static (int row, int col) MovementFromString(this string Input, out bool Ok)
        {
            Ok = false;
            var parts = Input.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                return (-1, -1);
            }
            int row;
            Ok = int.TryParse(parts[0], out row);
            if (!Ok)
                return (-1, -1);
            int col;
            Ok = int.TryParse(parts[1], out col);
            if (!Ok)
                return (-1, -1);
            return (row, col);

        }
        public static void PrintToConsole(this int[,] arr)
        {
            StringBuilder sb = new StringBuilder("\n");
            sb.Append("\n");
            sb.Append(" ");
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                sb.Append(" " + j);
            }
            sb.Append("\n");
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        sb.Append($"{i}|");
                    }

                    var val = arr[i, j];
                    string strVal = val < 0 ? " " : TranslateTable[val];
                    sb.Append(strVal);
                    sb.Append("|");
                }

                sb.Append("\n");
            }
            sb.Append("\n");
            Console.Write(sb.ToString());

        }

        public static string SerializeToJson(this FourInARowGame game)
        {
            Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            ser.Serialize(sw, game);
            return (sw.ToString());
        }
        public static FourInARowGame DeserializeFromJson(string str)
        {
            FourInARowGame game;
            game = Newtonsoft.Json.JsonConvert.DeserializeObject<FourInARowGame>(str);
            return (game);
        }

        public static void BackTo(this FourInARowGame game, int movement)
        {
            for (int i = game.NumberOfMovementsDone; i >= movement; i--)
            {
                game.BackOneMove();
            }
        }

        public static void BackOneMove(this FourInARowGame game)
        {
            game.NumberOfMovementsDone--;
            var move = game.MovementsDone.Last();
            game.board[move.row, move.col] = -1;
            game.MovementsDone.Remove(move);
            game.NextMovePlayerToken = game.NextMovePlayerToken == game.Bot_Token ? game.Opponent_Token : game.Bot_Token;
        }
    }

}