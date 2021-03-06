using NUnit.Framework;
using ALGAMES;
using ALGAMES.MatrixBoardGames;


namespace Tests
{
    public class TestInARowTests
    {


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestForAnEasyWin()
        {
            int[,] board = {
                             {0,1,-1,-1},
                             {0,1,0,-1},
                             {1,1,0,0},
                             {0,0,1,1},
                             {0,1,1,1}
                           };
            var rules = new NInARowBoardGameRules();
            TestContext.Out.Write($"TestForAEasyWin.\nInitial Board\n{board.ConvertToStringImproved()}");
            MatrixBoardGameMiniMax b = new MatrixBoardGameMiniMax(rules);
            var (result, pos) = b.GetNextMove(board, 17, 8, 0, 1);
            board[pos.row, pos.column] = 0;
            TestContext.Out.Write($"Board After move\n{board.ConvertToStringImproved()}");
            Assert.AreEqual(result, int.MaxValue);
        }

        [Test]
        public void TestAvoidLossing()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            int[,] board ={
                           {0,-1,1,-1},
                           {0,1,0,-1},
                           {1,1,0,0},
                           {0,1,0,1},
                           {0,0,1,1}
                 };
            var rules = new NInARowBoardGameRules();
            TestContext.Out.Write($"{testName}.\nInitial Board\n{board.ConvertToStringImproved()}");
            MatrixBoardGameMiniMax b = new MatrixBoardGameMiniMax(rules);
            var (result, pos) = b.GetNextMove(board, 17, 8, 0, 1);
            board[pos.Item1, pos.Item2] = 0;
            TestContext.Out.Write($"Board After move\n{board.ConvertToStringImproved()}");
            Assert.AreEqual(0, result);
        }

    }
}