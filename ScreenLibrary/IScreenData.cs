using System;

namespace Terminal.ScreenLibrary
{
    /// <summary>
    /// This class is used to access and change data of a virtual screen.
    /// </summary>
    public interface IScreenData
    {
        /// <summary>
        /// Get and change the value of a given cell.
        /// </summary>
        /// <param name="x">
        /// The x coordinate of the cell.
        /// </param>
        /// <param name="y">
        /// The y coordinate of the cell.
        /// </param>
        ScreenCell this[int x, int y] { get; set; }

        /// <summary>
        /// The height of this virtual screen.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// THe width of this virtual screen.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Expand the screen height, adding lines to the downside.
        /// </summary>
        /// <param name="lines">The number of expanded lines</param>
        /// <param name="cell">The cell to fill the new lines with.</param>
        /// <exception cref="ArgumentException"></exception>
        void ExpandHeightDown(int lines, ScreenCell cell);

        /// <summary>
        /// Expand the screen height, adding lines to the upside.
        /// </summary>
        /// <param name="lines">The number of expanded lines</param>
        /// <param name="cell">The cell to fill the new lines with.</param>
        /// <exception cref="ArgumentException"></exception>"
        void ExpandHeightUp(int lines, ScreenCell cell);

        /// <summary>
        /// Expand the screen width, adding columns to the left side.
        /// </summary>
        /// <param name="columns">The number of expanded columns</param>
        /// <param name="cell">The cell to fill the new lines with.</param>
        /// <exception cref="ArgumentException"></exception>
        void ExpandWidthLeft(int columns, ScreenCell cell);

        /// <summary>
        /// Expand the screen width, adding columns to the right side.
        /// </summary>
        /// <param name="columns">The number of expanded columns</param>
        /// <param name="cell">The cell to fill the new lines with.</param>
        /// <exception cref="ArgumentException"></exception>"
        void ExpandWidthRight(int columns, ScreenCell cell);
    }
}