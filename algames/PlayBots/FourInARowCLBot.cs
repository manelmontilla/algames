using System;
using ALGAMES.MatrixBoardGames;
using static System.Console;
using System.Collections.Generic;
using System.IO;

namespace ALGAMES.PlayBots
{
    public class FourInARowCLBot
    {

        public FourInARowGame Game { get; set; } = new FourInARowGame();


        private int MovementsLeft
        {
            get
            {
                var res = this.b.Rules.GetFreePositions(this.Game.board, this.Game.NumberOfMovementsDone);
                return (res.Length);
            }
        }

        MatrixBoardGameMiniMax b = null;
        public FourInARowCLBot()
        {
            var rules = new NInARowBoardGameRules();
            b = new MatrixBoardGameMiniMax(rules);
        }

        public void Play()
        {
            this.Game.board.Reset();
            this.Game.NumberOfMovementsDone = 0;
            this.Game.NextMovePlayerToken = this.Game.Opponent_Token;
            ResumeGame();
        }

        public void ResumeGame()
        {
            WriteLine($"current board:\n");
            Game.board.PrintToConsole();
            bool exit = false;
            while (!exit)
            {
                if (this.Game.NextMovePlayerToken == Game.Bot_Token)
                {
                    var botMove = GetBotMove();
                    WriteLine($"Bot moves to {botMove.pos.GetStringRepr()}");
                    WriteLine($"current board:\n");
                    Game.board.PrintToConsole();
                    Game.NextMovePlayerToken = Game.Opponent_Token;
                    exit = EvaluateBotStatus(botMove.result);
                }

                else
                {
                    var movement = AskForOpponentMove();
                    Game.board[movement.row, movement.col] = Game.Opponent_Token;
                    Game.MovementsDone.Add(movement);
                    Game.NumberOfMovementsDone++;
                    WriteLine($"current board:\n");
                    Game.board.PrintToConsole();
                    var res = b.Rules.Evaluate(Game.board, Game.Opponent_Token, Game.NumberOfMovementsDone);
                    exit = EvaluateOpponentStatus(res);
                    Game.NextMovePlayerToken = Game.Bot_Token;
                }

            }
        }

        private (int row, int col) AskForOpponentMove()
        {
            bool correct = false;
            (int row, int col) pos = (-1, -1);
            while (!correct)
            {
                WriteLine("Enter your movement in format row,col");
                string movement = ReadLine();
                bool okey;
                pos = movement.MovementFromString(out okey);
                if (okey)
                {
                    correct = Game.board.IsValidMove(pos);
                }
                else
                {
                    if (IsSaveGameCommand(movement))
                        SaveToFile(Game);
                }
            }
            return (pos);
        }

        public void SaveToFile(FourInARowGame game)
        {
            //TODO make working on widows put filename in a constant
            var val = game.SerializeToJson();
            string fileName = $"game.json";
            File.WriteAllText(fileName, val);
            WriteLine($"Game saved to {fileName}");
        }

        private bool IsSaveGameCommand(string input)
        {
            bool ret = false;
            ret = input.IndexOf("save", StringComparison.OrdinalIgnoreCase) >= 0;
            return (ret);
        }

        private bool EvaluateBotStatus(int result)
        {
            bool exit = false;
            switch (result)
            {
                case 0:
                    if (this.MovementsLeft > 0)
                    {
                        WriteLine("it seems you are going to win well done!!");

                    }
                    else
                    {
                        WriteLine("You win!! well done");
                        exit = true;
                    }
                    break;
                case 1:

                    WriteLine("I win!!");
                    exit = true;

                    break;
                case 3:

                    WriteLine("We tied!!");
                    exit = true;

                    break;
                default:

                    break;
            }

            return (exit);
        }

        private bool EvaluateOpponentStatus(int result)
        {
            bool exit = false;
            switch (result)
            {
                case 0:
                    WriteLine("I win this time!!");
                    exit = true;
                    break;
                case 1:
                    WriteLine("You win well done!!");
                    exit = true;
                    break;
                case 3:
                    WriteLine("We tied!!");
                    exit = true;
                    break;
                default:
                    WriteLine($"I really don't know.. result:{result}");
                    break;
            }

            return (exit);
        }

        // Returns nextmove if the game is not finished, otherwise null.  
        // Next will be 0 if oponent wins, 1 if bot wins, 2 not resolved,3 if draw. 
        // if the game is finished then result will be -1 
        public (int result, (int, int) pos) GetBotMove()
        {
            WriteLine("Bot deciding next movement..");
            var (res, pos) = b.GetNextMove(this.Game.board, this.Game.NumberOfMovementsDone, Game.SearchDepth, Game.Bot_Token
            , Game.Opponent_Token);
            this.Game.NumberOfMovementsDone++;
            this.Game.board[pos.row, pos.column] = Game.Bot_Token;
            Game.MovementsDone.Add(pos);
            return (res, pos);
        }



    }
}
