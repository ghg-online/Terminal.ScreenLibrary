using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Terminal.ScreenLibrary.Tests
{
    [TestClass()]
    public class ScreenDataTests
    {

        // ScreenDataScrollableTests.cs contains a copy of this code
        // If you change this code, you must change that code too

        [TestMethod()]
        [DataRow(1, 1)]
        [DataRow(1, 100000)]
        [DataRow(100000, 1)]
        [DataRow(100, 1000)]
        public void ExpandHeightUpTest(int width, int height)
        {
            IScreenData screenData = new ScreenData(width, height);
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
            IScreenData screenData = new ScreenData(width, height);
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
            IScreenData screenData = new ScreenData(width, height);
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
            IScreenData screenData = new ScreenData(width, height);
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
    }
}