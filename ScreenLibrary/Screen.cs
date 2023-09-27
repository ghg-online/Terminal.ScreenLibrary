using System.Drawing;

namespace Terminal.ScreenLibrary
{
    /// <inheritdoc/>
    public partial class Screen : IScreen
    {
        readonly IScreenDriver screenDriver;

        Color defaultForegroundColor;
        Color defaultBackgroundColor;
        /// <inheritdoc/>
        public Color DefaultForegroundColor
        {
            get => defaultForegroundColor;
            set
            {
                lock (this)
                {
                    defaultForegroundColor = value;
                }
            }
        }

        /// <inheritdoc/>
        public Color DefaultBackgroundColor
        {
            get => defaultBackgroundColor;
            set
            {
                lock (this)
                {
                    defaultForegroundColor = value;
                    screenData.DefaultBackground = value;
                }
            }
        }

        readonly IScreenData buffer;
        readonly ScreenDataScrollable screenData;
        Cursor cursor;
        EscapeSequenceHandler escapeSequenceHandler;

        /// <inheritdoc/>
        public int Width => screenData.Width;

        /// <inheritdoc/>
        public int Height => screenData.Height;

        /// <inheritdoc/>
        public int CursorX
        {
            get => cursor.cursorX;
            set => CursorMoveTo(value, cursor.cursorY);
        }

        /// <inheritdoc/>
        public int CursorY
        {
            get => cursor.cursorY;
            set => CursorMoveTo(cursor.cursorX, value);
        }

        /// <inheritdoc/>
        public int TabSize
        {
            get => cursor.tabSize;
            set { lock (this) cursor.tabSize = value; }
        }

        /// <inheritdoc/>
        public bool NewLineMode
        {
            get => cursor.newLineMode;
            set { lock (this) cursor.newLineMode = value; }
        }

        /// <inheritdoc/>
        public bool CursorVisible
        {
            get => cursor.cursorVisible;
            set
            {
                cursor.cursorVisible = value;
                UpdateCursor();
            }
        }

        /// <inheritdoc/>
        public bool CrossLineBackspace
        {
            get => cursor.crossLineBackspace;
            set => cursor.crossLineBackspace = value;
        }

        /// <summary>
        /// Initialize a screen with the given driver, default foreground color and default background color.
        /// </summary>
        /// <param name="screenDriver">
        /// All screen operations are delegated to this driver.
        /// </param>
        /// <param name="defaultForegroundColor">
        /// The default foreground color of the screen.
        /// </param>
        /// <param name="defaultBackgroundColor">
        /// The default background color of the screen.
        /// </param>
        public Screen(IScreenDriver screenDriver
            , Color defaultForegroundColor, Color defaultBackgroundColor)
        {
            this.screenDriver = screenDriver;
            this.defaultForegroundColor = defaultForegroundColor;
            this.defaultBackgroundColor = defaultBackgroundColor;
            int width = screenDriver.Width;
            int height = screenDriver.Height;
            buffer = new ScreenData(width, 0);
            screenData = new ScreenDataScrollable(buffer, width, height, defaultBackgroundColor);
            cursor = new Cursor()
            {
                width = width,
                height = height,
                cursorX = 0,
                cursorY = 0,
                cursorBackgroundColor = defaultBackgroundColor,
                cursorForegroundColor = defaultForegroundColor,
                tabSize = 8,
                newLineMode = true,
                cursorVisible = true,
                crossLineBackspace = true,
            };
            escapeSequenceHandler = null;
            Refresh();
        }

        /// <inheritdoc/>
        public void Refresh()
        {
            lock (this)
            {
                int width = screenData.Width;
                int height = screenData.Height;
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        screenDriver.Update(x, y, screenData[x, y]);
                UpdateCursor();
                screenDriver.Redraw();
            }
        }

        /// <inheritdoc/>
        public void HandleString(string text, bool refresh = true)
        {
            lock (this)
            {
                foreach (var c in text)
                    HandleCharacter(c, false);
                if (refresh)
                    Refresh();
            }

        }

        /// <inheritdoc/>
        public void HandleCharacter(char c, bool refresh = true)
        {
            lock (this)
            {
                /* 
                 * See https://www.systutorials.com/docs/linux/man/4-console_codes/
                 * 
                 * BEL (0x07, haG) beeps;
                 * BS (0x08, haH) backspaces one column (but not past the beginning of the line);
                 * HT (0x09, haI) goes to the next tab stop or to the end of the line if there is no earlier tab stop;
                 * LF (0x0A, haJ), VT (0x0B, haK) and FF (0x0C, haL) all give a linefeed, and if LF/NL (new-line mode) is set also a carriage return;
                 * CR (0x0D, haM) gives a carriage return;
                 * SO (0x0E, haN) activates the G1 character set;
                 * SI (0x0F, haO) activates the G0 character set;
                 * CAN (0x18, haX) and SUB (0x1A, haZ) abort escape sequences;
                 * ESC (0x1B, ha[) starts an escape sequence;
                 * DEL (0x7F) is ignored;
                 * CSI (0x9B) is equivalent to ESC [.
                 * 
                 */

                // escapeSequenceHandler is not null if and only if we are in an escape sequence
                switch (c)
                {
                    case '\x00': // NUL: ignore
                        break;

                    case '\x07': // BEL: beep, ignore
                        break;

                    case '\x08': // BS: backspace
                        CursorBackward();
                        break;

                    case '\x09': // HT: tab
                        CursorTab();
                        break;

                    case '\x0a':
                    case '\x0b':
                    case '\x0c': // LF, VT, FF: line feed
                        CursorLineFeed();
                        break;

                    case '\x0d': // CR: carriage return
                        CursorCarriageReturn();
                        break;

                    case '\x0e': // SO: activate G1 character set
                    case '\x0f': // SI: activate G0 character set
                        break;   // Do nothing since utf-16 is used in C# string

                    case '\x18':
                    case '\x1a': // CAN, SUB: abort escape sequences
                        escapeSequenceHandler = null;
                        break;

                    case '\x1b': // ESC: start escape sequence
                        escapeSequenceHandler = new EscapeSequenceHandler(c, this);
                        break;

                    case '\x7f': // DEL: ignore
                        break;

                    case '\x9b': // CSI: equivalent to ESC [
                        escapeSequenceHandler = new EscapeSequenceHandler(c, this);
                        break;

                    default:
                        if (escapeSequenceHandler == null)
                        {
                            screenData[cursor.cursorX, cursor.cursorY] = new ScreenCell()
                            {
                                Character = c,
                                Foreground = cursor.cursorForegroundColor,
                                Background = cursor.cursorBackgroundColor,
                            };
                            CursorForward();
                        }
                        else
                        {
                            if (escapeSequenceHandler.Continue(c))
                                escapeSequenceHandler = null;
                        }
                        break;
                }
                if (refresh)
                    Refresh();
            }
        }

        // positive lines: scroll down
        // negative lines: scroll up
        /// <inheritdoc/>
        public void Scroll(int lines, bool refresh = true)
        {
            if (lines != 0)
            {
                lock (this)
                {
                    screenData.Scroll(0, lines);
                    if (refresh)
                        Refresh();
                }
            }
        }

        /// <inheritdoc/>
        public void ResetColor()
        {
            lock (this)
            {
                cursor.cursorForegroundColor = defaultForegroundColor;
                cursor.cursorBackgroundColor = defaultBackgroundColor;
            }
        }

        /// <inheritdoc/>
        public void SetForegroundColor(Color color)
        {
            lock (this)
            {
                cursor.cursorForegroundColor = color;
            }
        }

        /// <inheritdoc/>
        public void SetBackgroundColor(Color color)
        {
            lock (this)
            {
                cursor.cursorBackgroundColor = color;
            }
        }

        /// <inheritdoc/>
        public void CursorBackward()
        {
            lock (this)
            {
                cursor.Backward(out int scroll);
                Scroll(scroll);
                UpdateCursor();
            }
        }

        /// <inheritdoc/>
        public void CursorCarriageReturn()
        {
            lock (this)
            {
                cursor.CarriageReturn(out int scroll);
                Scroll(scroll);
                UpdateCursor();
            }
        }

        /// <inheritdoc/>
        public void CursorForward()
        {
            lock (this)
            {
                cursor.Forward(out int scroll);
                Scroll(scroll);
                UpdateCursor();
            }
        }

        /// <inheritdoc/>
        public void CursorLineFeed()
        {
            lock (this)
            {
                cursor.LineFeed(out int scroll);
                Scroll(scroll);
                UpdateCursor();
            }
        }

        /// <inheritdoc/>
        public void CursorMove(int deltaX, int deltaY)
        {
            lock (this)
            {
                cursor.Move(deltaX, deltaY, out int scroll);
                Scroll(scroll);
                UpdateCursor();
            }
        }

        /// <inheritdoc/>
        public void CursorMoveTo(int targetX, int targetY)
        {
            lock (this)
            {
                cursor.MoveTo(targetX, targetY, out int scroll);
                Scroll(scroll);
                UpdateCursor();
            }
        }

        /// <inheritdoc/>
        public void CursorTab()
        {
            lock (this)
            {
                cursor.Tab(out int scroll);
                Scroll(scroll);
                UpdateCursor();
            }
        }

        private void UpdateCursor()
        {
            screenDriver.UpdateCursor(cursor.cursorX, cursor.cursorY, cursor.cursorVisible);
        }
    }
}
