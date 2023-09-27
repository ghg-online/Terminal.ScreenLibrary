using Terminal.ScreenLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Terminal.ScreenLibrary.Tests
{
    [TestClass()]
    public class ScreenTests
    {

        [TestMethod()]
        [DataRow(0, 0, "", 0, 0, ' ')]
        [DataRow(0, 0, "Hello, world!", 0, 0, 'H')]
        [DataRow(0, 0, "Hello, world!", 12, 0, '!')]
        [DataRow(0, 0, "Hello, world!", 13, 0, ' ')]
        [DataRow(0, 0, "\nHello, world!", 0, 1, 'H')]
        [DataRow(0, 0, "\nHello, world!", 12, 1, '!')]
        [DataRow(0, 0, "\nHello, world!", 13, 1, ' ')]
        [DataRow(0, 0, "01234567890123456789012345678901234567890123456789012345678901234567890123456789ABC", 79, 0, '9')]
        [DataRow(0, 0, "01234567890123456789012345678901234567890123456789012345678901234567890123456789ABC", 0, 1, 'A')]
        [DataRow(0, 0, "01234567890123456789012345678901234567890123456789012345678901234567890123456789ABC", 2, 1, 'C')]
        [DataRow(0, 0, "01234567890123456789012345678901234567890123456789012345678901234567890123456789ABC", 3, 1, ' ')]
        [DataRow(0, 0, "a\tb", 0, 0, 'a')]
        [DataRow(0, 0, "a\tb", 1, 0, ' ')]
        [DataRow(0, 0, "a\tb", 7, 0, ' ')]
        [DataRow(0, 0, "a\tb", 8, 0, 'b')]
        [DataRow(0, 23, "a\nb\nc", 0, 23, 'c')]
        [DataRow(0, 23, "a\nb\nc", 0, 22, 'b')]
        [DataRow(0, 23, "a\nb\nc", 0, 21, 'a')]
        [DataRow(0, 23, "a\nb\nc", 0, 20, ' ')]
        [DataRow(0, 23, "a\nb\nc", 1, 23, ' ')]
        [DataRow(0, 0, "H\ae\al\alo, \aworld!", 0, 0, 'H')]
        [DataRow(0, 0, "H\ae\al\alo, \aworld!", 12, 0, '!')]
        [DataRow(0, 0, "H\ae\al\alo, \aworld!", 13, 0, ' ')]
        [DataRow(0, 0, "A\bHello, A\bworld!", 0, 0, 'H')]
        [DataRow(0, 0, "A\bHello, A\bworld!", 12, 0, '!')]
        [DataRow(0, 0, "A\bHello, A\bworld!", 13, 0, ' ')]
        [DataRow(0, 0, "Hello, \rworld", 0, 0, 'w')]
        [DataRow(0, 0, "Hello, \rworld", 4, 0, 'd')]
        [DataRow(0, 0, "Hello, \rworld", 5, 0, ',')]
        [DataRow(0, 0, "Hello, \rworld", 6, 0, ' ')]
        [DataRow(0, 0, "Hello, \rworld", 7, 0, ' ')]
        public void HandleStringTest1(int initCursorX, int initCursorY
            , string str, int assertX, int assertY, char assertChar)
        {
            FakeDriver driver = new(); // 80x24
            IScreen screen = new Screen(driver, Color.White, Color.Black)
            {
                CursorX = initCursorX,
                CursorY = initCursorY
            };
            screen.HandleString(str);
            Assert.AreEqual(assertChar, driver[assertX, assertY].Character);
        }

        [TestMethod()]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , "", 0, 0, ' ', unchecked((int)0xffffffff), unchecked((int)0xffff0000))]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , " ", 0, 0, ' ', unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000))]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , "a\x1b[34mb\x1b[0mc", 0, 0, 'a', unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000))]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , "a\x1b[34mb\x1b[0mc", 1, 0, 'b', unchecked((int)0xff0000ff), unchecked((int)0xffff0000))]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , "a\x1b[34mb\x1b[0mc", 2, 0, 'c', unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000))]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , "a\x1b[34mb\x1b[0mc", 3, 0, ' ', unchecked((int)0xffffffff), unchecked((int)0xffff0000))]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , "a\x1b[44mb\x1b[0mc", 0, 0, 'a', unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000))]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , "a\x1b[44mb\x1b[0mc", 1, 0, 'b', unchecked((int)0xffaaaaaa), unchecked((int)0xff0000ff))]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , "a\x1b[44mb\x1b[0mc", 2, 0, 'c', unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000))]
        [DataRow(0, 0, unchecked((int)0xffaaaaaa), unchecked((int)0xffff0000)
            , "a\x1b[44mb\x1b[0mc", 3, 0, ' ', unchecked((int)0xffffffff), unchecked((int)0xffff0000))]
        public void HandleStringTest2(int initCursorX, int initCursorY
            , int initForegroundRgb, int initBackgroundRgb, string str
            , int assertX, int assertY, char assertChar
            , int assertForegroundRgb, int assertBackgroundRgb)
        {
            FakeDriver driver = new();// 80x24
            IScreen screen = new Screen(driver, Color.FromArgb(initForegroundRgb), Color.FromArgb(initBackgroundRgb))
            {
                CursorX = initCursorX,
                CursorY = initCursorY
            };
            screen.HandleString(str);
            ScreenCell assertCell = new()
            {
                Character = assertChar,
                Foreground = Color.FromArgb(assertForegroundRgb),
                Background = Color.FromArgb(assertBackgroundRgb),
            };
            Assert.AreEqual(assertCell.Character, driver[assertX, assertY].Character);
            Assert.AreEqual(assertCell.Foreground.A, driver[assertX, assertY].Foreground.A);
            Assert.AreEqual(assertCell.Foreground.R, driver[assertX, assertY].Foreground.R);
            Assert.AreEqual(assertCell.Foreground.G, driver[assertX, assertY].Foreground.G);
            Assert.AreEqual(assertCell.Foreground.B, driver[assertX, assertY].Foreground.B);
            Assert.AreEqual(assertCell.Background.A, driver[assertX, assertY].Background.A);
            Assert.AreEqual(assertCell.Background.R, driver[assertX, assertY].Background.R);
            Assert.AreEqual(assertCell.Background.G, driver[assertX, assertY].Background.G);
            Assert.AreEqual(assertCell.Background.B, driver[assertX, assertY].Background.B);
        }

        [TestMethod()]
        [DataRow(0, 0, "", 0, 0, true)]
        [DataRow(0, 0, "Hello, world!", 13, 0, true)]
        [DataRow(0, 0, "\nHello, world!", 13, 1, true)]
        [DataRow(0, 0, "01234567890123456789012345678901234567890123456789012345678901234567890123456789ABC", 3, 1, true)]
        [DataRow(0, 0, "a\tb", 9, 0, true)]
        [DataRow(0, 23, "a\nb\nc", 1, 23, true)]
        [DataRow(0, 0, "H\ae\al\alo, \aworld!", 13, 0, true)]
        [DataRow(0, 0, "A\bHello, A\bworld!", 13, 0, true)]
        [DataRow(0, 0, "Hello, \rworld", 5, 0, true)]
        public void HandleStringTest3(int initCursorX, int initCursorY
            , string str, int assertCursorX, int assertCursorY, bool assertCursorVisible)
        {
            FakeDriver driver = new(); // 80x24
            IScreen screen = new Screen(driver, Color.White, Color.Black)
            {
                CursorX = initCursorX,
                CursorY = initCursorY
            };
            screen.HandleString(str);
            Assert.AreEqual(assertCursorX, screen.CursorX);
            Assert.AreEqual(assertCursorY, screen.CursorY);
            Assert.AreEqual(assertCursorVisible, screen.CursorVisible);
        }

        [TestMethod()]
        [DataRow(true, 0, 0, "", 0, 0, true)]
        [DataRow(true, 0, 0, "Hello, world!", 13, 0, true)]
        [DataRow(true, 0, 0, "\nHello, world!", 13, 1, true)]
        [DataRow(true, 0, 0, "01234567890123456789012345678901234567890123456789012345678901234567890123456789ABC", 3, 1, true)]
        [DataRow(true, 0, 0, "01234567890123456789012345678901234567890123456789012345678901234567890123456789ABC", 3, 1, true)]
        [DataRow(true, 0, 0, "a\tb", 9, 0, true)]
        [DataRow(true, 0, 23, "a\nb\nc", 1, 23, true)]
        [DataRow(true, 0, 0, "H\ae\al\alo, \aworld!", 13, 0, true)]
        [DataRow(true, 0, 0, "A\bHello, A\bworld!", 13, 0, true)]
        [DataRow(true, 0, 0, "Hello, \rworld", 5, 0, true)]
        [DataRow(true, 0, 0, "\bHello, world!", 12, 1, true)]
        [DataRow(false, 0, 0, "\bHello, world!", 13, 0, true)]
        [DataRow(false, 0, 0, "\b\b\b\b\b\b\bHello, world!", 13, 0, true)]
        [DataRow(true, 0, 0, "01234567890123456789012345678901234567890123456789012345678901234567890123456789ABC\b\b\b\b", 79, 0, true)]
        [DataRow(false, 0, 0, "01234567890123456789012345678901234567890123456789012345678901234567890123456789ABC\b\b\b\b", 0, 1, true)]
        public void HandleStringTest4(bool initCrossLineBackspace, int initCursorX, int initCursorY
            , string str, int assertCursorX, int assertCursorY, bool assertCursorVisible)
        {
            FakeDriver driver = new(); // 80x24
            IScreen screen = new Screen(driver, Color.White, Color.Black)
            {
                CursorX = initCursorX,
                CursorY = initCursorY,
                CrossLineBackspace = initCrossLineBackspace,
            };
            screen.HandleString(str);
            Assert.AreEqual(assertCursorX, screen.CursorX);
            Assert.AreEqual(assertCursorY, screen.CursorY);
            Assert.AreEqual(assertCursorVisible, screen.CursorVisible);
        }

        [TestMethod()]
        public void RefreshTest()
        {
            FakeDriver driver = new();// 80x24
            IScreen screen = new Screen(driver, Color.White, Color.Black);
            Assert.AreEqual(' ', driver[0, 0].Character);
            Assert.AreEqual(' ', driver[0, 23].Character);
            Assert.AreEqual(' ', driver[79, 0].Character);
            Assert.AreEqual(' ', driver[79, 23].Character);
            screen.CursorMoveTo(0, 0);
            screen.HandleString("a", false);
            screen.CursorMoveTo(0, 23);
            screen.HandleString("a", false);
            screen.CursorMoveTo(79, 0);
            screen.HandleString("a", false);
            screen.CursorMoveTo(79, 23);
            screen.HandleString("a", false);
            screen.Scroll(-1, false);
            Assert.AreEqual(' ', driver[0, 0].Character);
            Assert.AreEqual(' ', driver[0, 23].Character);
            Assert.AreEqual(' ', driver[79, 0].Character);
            Assert.AreEqual(' ', driver[79, 23].Character);
            screen.Refresh();
            Assert.AreEqual('a', driver[0, 0].Character);
            Assert.AreEqual('a', driver[0, 23].Character);
            Assert.AreEqual('a', driver[79, 0].Character);
            Assert.AreEqual('a', driver[79, 23].Character);
        }

        [TestMethod()]
        public void ScrollTest()
        {
            FakeDriver driver = new();// 80x24
            IScreen screen = new Screen(driver, Color.White, Color.Black);
            screen.CursorMoveTo(0, 0);
            screen.HandleString("a", false);
            screen.CursorMoveTo(0, 23);
            screen.HandleString("a", false);
            screen.CursorMoveTo(79, 0);
            screen.HandleString("a", false);
            screen.CursorMoveTo(79, 23);
            screen.HandleString("a", false);
            screen.Scroll(-1);
            Assert.AreEqual('a', driver[0, 0].Character);
            Assert.AreEqual('a', driver[0, 23].Character);
            Assert.AreEqual('a', driver[79, 0].Character);
            Assert.AreEqual('a', driver[79, 23].Character);
            screen.Scroll(1);
            Assert.AreEqual('a', driver[0, 22].Character);
            Assert.AreEqual('a', driver[79, 22].Character);
            screen.Scroll(-1);
            Assert.AreEqual('a', driver[0, 0].Character);
            Assert.AreEqual('a', driver[0, 23].Character);
            Assert.AreEqual('a', driver[79, 0].Character);
            Assert.AreEqual('a', driver[79, 23].Character);
            screen.Scroll(-1);
            Assert.AreEqual('a', driver[0, 1].Character);
            Assert.AreEqual('a', driver[79, 1].Character);
        }

        [TestMethod()]
        public void CursorMoveTest()
        {
            FakeDriver driver = new();// 80x24
            IScreen screen = new Screen(driver, Color.White, Color.Black);
            screen.CursorMoveTo(0, 0);
            screen.HandleString("a");
            screen.CursorMoveTo(0, 23);
            screen.HandleString("a");
            screen.CursorMoveTo(79, 0);
            screen.HandleString("a");
            screen.CursorMoveTo(79, 23);
            screen.HandleString("a");
            screen.Scroll(-1);
            screen.CursorMoveTo(0, 0);
            screen.CursorMove(-1, 0);
            Assert.AreEqual('a', driver[0, 0].Character);
            Assert.AreEqual('a', driver[0, 23].Character);
            Assert.AreEqual('a', driver[79, 0].Character);
            Assert.AreEqual('a', driver[79, 23].Character);
            Assert.AreEqual((0, 0), (screen.CursorX, screen.CursorY));
            screen.CursorMove(79, 0);
            screen.CursorMove(1, 0);
            Assert.AreEqual((79, 0), (screen.CursorX, screen.CursorY));
            Assert.AreEqual((79, 0), (screen.CursorX, screen.CursorY));
            Assert.AreEqual('a', driver[0, 0].Character);
            Assert.AreEqual('a', driver[0, 23].Character);
            Assert.AreEqual('a', driver[79, 0].Character);
            Assert.AreEqual('a', driver[79, 23].Character);
            screen.CursorMove(0, 23);
            Assert.AreEqual((79, 23), (screen.CursorX, screen.CursorY));
            screen.CursorMove(0, 1);
            Assert.AreEqual((79, 23), (screen.CursorX, screen.CursorY));
            screen.CursorMove(1, 0);
            Assert.AreEqual((79, 23), (screen.CursorX, screen.CursorY));
            Assert.AreEqual('a', driver[0, 22].Character);
            Assert.AreEqual('a', driver[79, 22].Character);
        }
    }
}