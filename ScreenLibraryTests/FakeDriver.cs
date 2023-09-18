using System.Drawing;

namespace Terminal.ScreenLibrary.Tests
{
    public class FakeDriver : IScreenDriver
    {
        readonly private ScreenData screenData;

        ScreenCell[,] Cells;

        public FakeDriver()
        {
            screenData = new ScreenData(80, 24);
            Cells = new ScreenCell[80, 24];
        }

        public ScreenCell this[int x, int y] => Cells[x, y];

        public int Width => Cells.GetLength(0);
        public int Height => Cells.GetLength(1);

        public void Redraw()
        {
            Cells = new ScreenCell[screenData.Width, screenData.Height];
            for (int y = 0; y < screenData.Height; y++)
            {
                for (int x = 0; x < screenData.Width; x++)
                {
                    var screenChar = screenData[x, y];
                    Cells[x, y] = screenChar;
                }
            }
        }

        public void Update(int x, int y, char c, Color foreground, Color background)
        {
            if (x < 0 || x >= screenData.Width)
                throw new System.ArgumentOutOfRangeException("x", x, "x must be between 0 and " + screenData.Width);
            if (y < 0 || y >= screenData.Height)
                throw new System.ArgumentOutOfRangeException("y", y, "y must be between 0 and " + screenData.Height);
            screenData[x, y] = new ScreenCell()
            {
                Character = c,
                Foreground = foreground,
                Background = background
            };
        }
    }
}
