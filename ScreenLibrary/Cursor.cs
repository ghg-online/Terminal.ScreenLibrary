using System.Drawing;

namespace Terminal.ScreenLibrary
{
    // Some comment in this file is copied from Screen.cs
    // and is a copy of https://www.systutorials.com/docs/linux/man/4-console_codes/
    internal struct Cursor
    {
        public int width;
        public int height;
        public int cursorX;
        public int cursorY;
        public Color cursorForegroundColor;
        public Color cursorBackgroundColor;
        public int tabSize;
        public bool newLineMode;
        public bool cursorVisible;

        // for variable scroll:
        // positive lines: scroll down
        // negative lines: scroll up

        public void Forward(out int scroll)
        {
            scroll = 0;
            if (cursorX < width - 1)
                cursorX++;
            else
            {
                cursorX = 0;
                if (cursorY < height - 1)
                    cursorY++;
                else
                    scroll = 1;
            }
        }

        /// <summary>
        /// backspaces one column (but not past the beginning of the line)
        /// </summary>
        public void Backward(out int scroll)
        {
            scroll = 0;
            if (cursorX > 0)
                cursorX--;
        }

        public void CarriageReturn(out int scroll)
        {
            scroll = 0;
            cursorX = 0;
        }

        public void LineFeed(out int scroll)
        {
            scroll = 0;
            if (cursorY < height - 1)
                cursorY++;
            else
                scroll = 1;
            if (newLineMode)
                cursorX = 0;
        }

        /// <summary>
        /// goes to the next tab stop or to the end of the line if there is no earlier tab stop
        /// </summary>
        public void Tab(out int scroll)
        {
            scroll = 0;
            int targetX = cursorX + tabSize - cursorX % tabSize;
            if (targetX < width)
                cursorX = targetX;
        }

        public void Move(int deltaX, int deltaY, out int scroll)
        {
            int targetX = cursorX += deltaX;
            int targetY = cursorY += deltaY;
            MoveTo(targetX, targetY, out scroll);
        }

        public void MoveTo(int targetX, int targetY, out int scroll)
        {
            // handle X movement
            if (targetX < 0)
                cursorX = 0;
            else if (targetX >= width)
                cursorX = width - 1;
            else
                cursorX = targetX;

            // handle Y movement
            if (targetY < 0)
            {
                scroll = targetY - 0;
                cursorY = 0;
            }
            else if (targetY >= height)
            {
                scroll = targetY - (height - 1);
                cursorY = height - 1;
            }
            else
            {
                scroll = 0;
                cursorY = targetY;
            }
        }
    }
}
