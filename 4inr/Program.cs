using System;
using static System.Console;
using CommandLine;
using ALGAMES.PlayBots;

namespace _4inr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FourInARowCLBot bot = new FourInARowCLBot();
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                if (o.Resume == null)
                {
                    bot.Play();
                    return;
                }
                var filePath = o.Resume;
                var str = System.IO.File.ReadAllText(filePath);
                var game = GameUtils.DeserializeFromJson(str);
                bot.Game = game;
                if (o.BackTo != null)
                {
                    var back = (int)o.BackTo;
                    game.BackTo(back);
                }
                bot.ResumeGame();
                return;
            });
        }
    }
}
