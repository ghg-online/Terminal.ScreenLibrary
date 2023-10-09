using System.Drawing;

namespace Terminal.ScreenLibrary.Tests
{
    public class FakeDriver : IScreenDriver
    {
        readonly private ScreenData screenData;

        ScreenCell[,] Cells;

        private bool UpdateCursorAtOnce = true;
        private int cursorX;
        private int cursorY;
        private bool cursorVisible;
        public int CursorX { get; private set; }
        public int CursorY { get; private set; }
        public bool CursorVisible { get; private set; }
        public int UpdateCounter { get; private set; }

        public FakeDriver()
        {
            screenData = new ScreenData(80, 24);
            Cells = new ScreenCell[80, 24];
            UpdateCounter = 0;
        }

        public ScreenCell this[int x, int y] => Cells[x, y];

        public void ResetUpdateCounter()
            => UpdateCounter = 0;

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
            if (false == UpdateCursorAtOnce)
            {
                CursorX = cursorX;
                CursorY = cursorY;
                CursorVisible = cursorVisible;
            }
        }

        public void Update(int x, int y, char c, Color foreground, Color background)
        {
            if (x < 0 || x >= screenData.Width)
                throw new System.ArgumentOutOfRangeException(nameof(x), x, "x must be between 0 and " + screenData.Width);
            if (y < 0 || y >= screenData.Height)
                throw new System.ArgumentOutOfRangeException(nameof(y), y, "y must be between 0 and " + screenData.Height);
            screenData[x, y] = new ScreenCell()
            {
                Character = c,
                Foreground = foreground,
                Background = background
            };
            UpdateCounter++;
        }

        public void UpdateCursor(int x, int y, bool show)
        {
            if (UpdateCursorAtOnce)
            {
                CursorX = x;
                CursorY = y;
                CursorVisible = show;
            }
            else
            {
                cursorX = x;
                cursorY = y;
                cursorVisible = show;
            }
        }
    }
}
