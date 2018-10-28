using System;
using static System.Console;
using ALGAMES.PlayBots;

namespace _4inr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FourInARowCLBot bot = new FourInARowCLBot();
            if (args.Length > 0)
            {
                // start game from a file.
                var str = System.IO.File.ReadAllText("game.json");
                var game = GameUtils.DeSerializeFromJson(str);
                bot.Game = game;
                if (args.Length > 1)
                {

                    int backto = int.Parse(args[1]);
                    game.BackTo(backto);
                }
                bot.ResumeGame();
            }
            else
            {
                bot.Play();
            }
        }

    }
}
