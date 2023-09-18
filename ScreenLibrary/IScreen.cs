using System.Drawing;

namespace Terminal.ScreenLibrary
{
    /// <summary>
    /// The screen class
    /// </summary>
    /// <remarks>
    /// It can handle color related escape sequences,
    /// and move cursor smartly.
    /// </remarks>
    public interface IScreen
    {
        /// <summary>
        /// The x coordinate of the cursor.
        /// </summary>
        int CursorX { get; set; }

        /// <summary>
        /// The y coordinate of the cursor.
        /// </summary>
        int CursorY { get; set; }

        /// <summary>
        /// The default background color.
        /// </summary>
        Color DefaultBackgroundColor { get; set; }

        /// <summary>
        /// The default foreground color.
        /// </summary>
        Color DefaultForegroundColor { get; set; }

        /// <summary>
        /// The height of the screen.
        /// </summary>
        /// <remarks>
        /// The value is get from the screen driver,
        /// so it's not changeable.
        /// </remarks>
        int Height { get; }

        /// <summary>
        /// The width of the screen.
        /// </summary>
        /// <remarks>
        /// The value is get from the screen driver,
        /// so it's not changeable.
        /// </remarks>
        int Width { get; }

        /// <summary>
        /// If this is true, \n will equal \r\n.
        /// </summary>
        /// <remarks>
        /// The default value is true.
        /// </remarks>
        bool NewLineMode { get; set; }

        /// <summary>
        /// The size of a tab.
        /// </summary>
        /// <remarks>
        /// The default value is 8.
        /// </remarks>
        int TabSize { get; set; }

        /// <summary>
        /// Backspaces one column (but not past the beginning of the line)
        /// </summary>
        void CursorBackward();

        /// <summary>
        /// Gives a carriage return
        /// </summary>
        /// <remarks>
        /// No linefeed is done.
        /// </remarks>
        void CursorCarriageReturn();

        /// <summary>
        /// Forward the cursor by one step
        /// </summary>
        /// <remarks>
        /// If the cursor is already at the final position of a line,
        /// then the cursor will be moved to the beginning of the next line.
        /// </remarks>
        void CursorForward();

        /// <summary>
        /// Give a linefeed, and if NewLineMode is set true (default behavior)
        /// , also a carriage return
        /// </summary>
        void CursorLineFeed();

        /// <summary>
        /// Move the cursor by the given delta
        /// </summary>
        /// <param name="deltaX">The expected delta of X coordinate</param>
        /// <param name="deltaY">The expected delta of Y coordinate</param>
        /// <remarks>
        /// <para>If X is out of range after move, cursor will stay at edge, and Y won't change.</para>
        /// <para>However, if Y is out of range after move, the screen will scroll to make cursor within the screen.</para>
        /// </remarks>
        void CursorMove(int deltaX, int deltaY);

        /// <summary>
        /// Move the cursor to the given position
        /// </summary>
        /// <param name="targetX">The expected X coordinate after move</param>
        /// <param name="targetY">The expected Y coordinate after move</param>
        /// <remarks>
        /// <para>If X is out of range after move, cursor will stay at edge, and Y won't change.</para>
        /// <para>However, if Y is out of range after move, the screen will scroll to make cursor within the screen.</para>
        /// </remarks>
        void CursorMoveTo(int targetX, int targetY);

        /// <summary>
        /// Same effect as \t
        /// </summary>
        void CursorTab();

        /// <summary>
        /// Handle a given character
        /// </summary>
        /// <param name="c">The character to handle</param>
        /// <param name="refresh">
        /// If this is set true (by default), the change will be displaced to the screen
        /// as soon as it is made.
        /// </param>
        void HandleCharacter(char c, bool refresh = true);

        /// <summary>
        /// Handle a given string
        /// </summary>
        /// <param name="text">The string to handle</param>
        /// <param name="refresh">
        /// If this is set true (by default), the change will be displaced to the screen
        /// as soon as it is made.
        /// </param>
        void HandleString(string text, bool refresh = true);

        /// <summary>
        /// Refresh the entire screen
        /// </summary>
        void Refresh();

        /// <summary>
        /// Reset the color to default
        /// </summary>
        void ResetColor();

        /// <summary>
        /// Scroll the screen by given lines
        /// </summary>
        /// <param name="lines">Positive number will make the content move up</param>
        /// <param name="refresh">
        /// If this is set true (by default), the change will be displaced to the screen
        /// as soon as it is made.
        /// </param>
        /// <remarks>
        /// Positive number will make the content move up.
        /// </remarks>
        void Scroll(int lines, bool refresh = true);

        /// <summary>
        /// Set the background color
        /// </summary>
        /// <remarks>
        /// This won't change the color of existing characters,
        /// nor will it change <see cref="DefaultBackgroundColor"/>.
        /// The color can be reset by calling <see cref="ResetColor"/>
        /// </remarks>
        void SetBackgroundColor(Color color);

        /// <summary>
        /// Set the foreground color
        /// </summary>
        /// <remarks>
        /// This won't change the color of existing characters,
        /// nor will it change <see cref="DefaultForegroundColor"/>.
        /// The color can be reset by calling <see cref="ResetColor"/>.
        /// </remarks>
        void SetForegroundColor(Color color);
    }
}