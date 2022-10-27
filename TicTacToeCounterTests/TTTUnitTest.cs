using TicTacToeStateCounter;

namespace TicTacToeCounterTests
{
    [TestClass]
    public class TTTUnitTest
    {
        #region Status
        #region X wins
        [TestMethod]
        public void TestStatus3x3XWinsTopRow()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "xxxoo....");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.X_Won, s);
        }
        [TestMethod]
        public void TestStatus3x3XWinsMidRow()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "oo.xxx...");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.X_Won, s);
        }
        [TestMethod]
        public void TestStatus3x3XWinsBotRow()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "oo....xxx");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.X_Won, s);
        }
        [TestMethod]
        public void TestStatus3x3XWinsDiag1()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "xoxoxo..x");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.X_Won, s);
        }
        [TestMethod]
        public void TestStatus3x3XWinsDiag2()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "xoxoxox..");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.X_Won, s);
        }
        #endregion X wins
        #region O Wins
        [TestMethod]
        public void TestStatus3x3OWinsTopRow()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "oooxxx...");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.O_Won, s);
        }
        [TestMethod]
        public void TestStatus3x3OWinsMidRow()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "xx.ooox..");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.O_Won, s);
        }
        [TestMethod]
        public void TestStatus3x3OWinsBotRow()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "xx.x..ooo");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.O_Won, s);
        }
        [TestMethod]
        public void TestStatus3x3OWinsDiag1()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "oxoxoxx.o");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.O_Won, s);
        }
        [TestMethod]
        public void TestStatus3x3OWinsDiag2()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "oxoxoxo.x");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.O_Won, s);
        }
        #endregion O Wins
        #region Ties
        [TestMethod]
        public void TestStatus3x3Tie()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "oxxxooxox");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.Tie, s);
        }
        #endregion Ties
        #region Unfinished

        [TestMethod]
        public void TestStatus3x3Unfinished()
        {
            TTT t = new(3);
            var s = TTT.GetStatus(t, "oxoxox..x");
            Assert.IsNotNull(s);
            Assert.AreEqual(TTT.Status.Unfinished, s);
        }
        #endregion Unfinished
        #endregion Status
        #region Transforms
        [TestMethod]
        public void TestRotation3x3()
        {
            TTT t = new(3);
            var s = TTT.Rotate(t, "012345678");
            Assert.IsNotNull(s);
            Assert.AreEqual("258147036", s);
        }
        [TestMethod]
        public void TestRotation4x4()
        {
            TTT t = new(4);
            var s = TTT.Rotate(t, "0123456789abcdef");
            Assert.IsNotNull(s);
            Assert.AreEqual("37bf26ae159d048c", s);
        }
        [TestMethod]
        public void TestMirror3x3()
        {
            TTT t = new(3);
            var s = TTT.MirrorImage(t, "012345678");
            Assert.IsNotNull(s);
            Assert.AreEqual("210543876", s);
        }
        [TestMethod]
        public void TestMirror4x4()
        {
            TTT t = new(4);
            var s = TTT.MirrorImage(t, "0123456789abcdef");
            Assert.IsNotNull(s);
            Assert.AreEqual("32107654ba98fedc", s);
        }
        #endregion Test Transforms
        #region Ply
        [TestMethod]
        public void TestPly()
        {
            TTT t = new(3);
            var ply = TTT.Ply(t, "x.o.x.o..");
            Assert.AreEqual(4, ply);
            ply = TTT.Ply(t, ".........");
            Assert.AreEqual(0, ply);
            ply = TTT.Ply(t, "oxoxx..xo");
            Assert.AreEqual(7, ply);
            ply = TTT.Ply(t, "xooxooxxx");
            Assert.AreEqual(9, ply);
        }
        #endregion Ply
    }
}
