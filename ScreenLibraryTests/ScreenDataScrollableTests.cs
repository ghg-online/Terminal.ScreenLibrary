using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Terminal.ScreenLibrary.Tests
{
    [TestClass()]
    public class ScreenDataScrollableTests
    {

        // the following code is a copy of ScreenDataTests.cs
        // however, first line of every method has been modified to use ScreenDataScrollable

        [TestMethod()]
        [DataRow(1, 1)]
        [DataRow(1, 100000)]
        [DataRow(100000, 1)]
        [DataRow(100, 1000)]
        public void ExpandHeightUpTest(int width, int height)
        {
            IScreenData screenData = new ScreenDataScrollable(new ScreenData(width, height), width, height, new Color());
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    screenData[x, y] = new ScreenCell()
                    {
                        Character = '啊',
                        Background = Color.Black,
                        Foreground = Color.White,
                    };
            Assert.ThrowsException<ArgumentException>(() => screenData.ExpandHeightUp(-1, new ScreenCell()));
            screenData.ExpandHeightUp(0, new ScreenCell());
            screenData.ExpandHeightUp(1, new ScreenCell());
            Assert.AreEqual(screenData.Width, width);
            Assert.AreEqual(screenData.Height, height + 1);
            Assert.AreEqual(screenData[width - 1, 0], new ScreenCell());
            Assert.AreEqual(screenData[width - 1, 1], new ScreenCell()
            {
                Character = '啊',
                Background = Color.Black,
                Foreground = Color.White,
            });
        }

        [TestMethod()]
        [DataRow(1, 1)]
        [DataRow(1, 100000)]
        [DataRow(100000, 1)]
        [DataRow(100, 1000)]
        public void ExpandHeightDownTest(int width, int height)
        {
            IScreenData screenData = new ScreenDataScrollable(new ScreenData(width, height), width, height, new Color());
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    screenData[x, y] = new ScreenCell()
                    {
                        Character = '啊',
                        Background = Color.Black,
                        Foreground = Color.White,
                    };
            Assert.ThrowsException<ArgumentException>(() => screenData.ExpandHeightDown(-1, new ScreenCell()));
            screenData.ExpandHeightDown(0, new ScreenCell());
            screenData.ExpandHeightDown(1, new ScreenCell());
            Assert.AreEqual(screenData.Width, width);
            Assert.AreEqual(screenData.Height, height + 1);
            Assert.AreEqual(screenData[width - 1, height], new ScreenCell());
            Assert.AreEqual(screenData[width - 1, height - 1], new ScreenCell()
            {
                Character = '啊',
                Background = Color.Black,
                Foreground = Color.White,
            });
        }

        [TestMethod()]
        [DataRow(1, 1)]
        [DataRow(1, 100000)]
        [DataRow(100000, 1)]
        [DataRow(100, 1000)]
        public void ExpandWidthRightTest(int width, int height)
        {
            IScreenData screenData = new ScreenDataScrollable(new ScreenData(width, height), width, height, new Color());
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    screenData[x, y] = new ScreenCell()
                    {
                        Character = '啊',
                        Background = Color.Black,
                        Foreground = Color.White,
                    };
            Assert.ThrowsException<ArgumentException>(() => screenData.ExpandWidthRight(-1, new ScreenCell()));
            screenData.ExpandWidthRight(0, new ScreenCell());
            screenData.ExpandWidthRight(1, new ScreenCell());
            Assert.AreEqual(screenData.Width, width + 1);
            Assert.AreEqual(screenData.Height, height);
            Assert.AreEqual(screenData[width, height - 1], new ScreenCell());
            Assert.AreEqual(screenData[width - 1, height - 1], new ScreenCell()
            {
                Character = '啊',
                Background = Color.Black,
                Foreground = Color.White,
            });
        }

        [TestMethod()]
        [DataRow(1, 1)]
        [DataRow(1, 100000)]
        [DataRow(100000, 1)]
        [DataRow(100, 1000)]
        public void ExpandWidthLeftTest(int width, int height)
        {
            IScreenData screenData = new ScreenDataScrollable(new ScreenData(width, height), width, height, new Color());
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    screenData[x, y] = new ScreenCell()
                    {
                        Character = '啊',
                        Background = Color.Black,
                        Foreground = Color.White,
                    };
            Assert.ThrowsException<ArgumentException>(() => screenData.ExpandWidthLeft(-1, new ScreenCell()));
            screenData.ExpandWidthLeft(0, new ScreenCell());
            screenData.ExpandWidthLeft(1, new ScreenCell());
            Assert.AreEqual(screenData.Width, width + 1);
            Assert.AreEqual(screenData.Height, height);
            Assert.AreEqual(screenData[0, height - 1], new ScreenCell());
            Assert.AreEqual(screenData[1, height - 1], new ScreenCell()
            {
                Character = '啊',
                Background = Color.Black,
                Foreground = Color.White,
            });
        }

        [TestMethod()]
        public void ScrollTest()
        {
            ScreenCell empty = new()
            {
                Character = ' ',
                Background = Color.Red,
                Foreground = Color.White,
            };
            ScreenCell fulfilled = new()
            {
                Character = '啊',
                Background = Color.Black,
                Foreground = Color.White,
            };
            var data = new ScreenData(40, 10);
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 40; x++)
                    data[x, y] = new ScreenCell()
                    {
                        Character = '啊',
                        Background = Color.Black,
                        Foreground = Color.White,
                    };
            var screen = new ScreenDataScrollable(data, 20, 5, empty.Background, 20, 5);
            Assert.AreEqual(screen[0, 0], fulfilled);
            Assert.AreEqual(screen[19, 4], fulfilled);
            Assert.ThrowsException<IndexOutOfRangeException>(() => screen[-1, 0]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => screen[0, -1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => screen[20, 0]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => screen[0, 5]);

            screen.Scroll(1, 0);
            Assert.AreEqual(screen[19, 0], empty);
            Assert.AreEqual(screen[18, 0], fulfilled);
            Assert.AreEqual(screen[19, 4], empty);
            Assert.AreEqual(screen[18, 4], fulfilled);
            screen.Scroll(-1, 0);

            screen.Scroll(0, 1);
            Assert.AreEqual(screen[0, 4], empty);
            Assert.AreEqual(screen[0, 3], fulfilled);
            Assert.AreEqual(screen[19, 4], empty);
            Assert.AreEqual(screen[19, 3], fulfilled);
            screen.Scroll(0, -1);

            screen.Scroll(-21, 0);
            Assert.AreEqual(screen[0, 0], empty);
            Assert.AreEqual(screen[1, 0], fulfilled);
            Assert.AreEqual(screen[0, 4], empty);
            Assert.AreEqual(screen[1, 4], fulfilled);
            screen.Scroll(21, 0);

            screen.Scroll(0, -6);
            Assert.AreEqual(screen[0, 0], empty);
            Assert.AreEqual(screen[0, 1], fulfilled);
            Assert.AreEqual(screen[19, 0], empty);
            Assert.AreEqual(screen[19, 1], fulfilled);
            screen.Scroll(0, 6);
        }
    }
}