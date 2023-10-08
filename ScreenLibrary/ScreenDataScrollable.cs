using System;
using System.Drawing;

namespace Terminal.ScreenLibrary
{
    /// <inheritdoc/>
    public class ScreenDataScrollable : IScreenData
    {
        readonly IScreenData screenData;
        int height;
        int width;
        int xOffset; // (x on screenData) = (x on this) + xOffset
        int yOffset; // (y on screenData) = (y on this) + yOffset

        /// <summary>
        /// When the screen is scrolled by <see cref="Scroll(int, int)"/>, the new area will be filled with this color
        /// </summary>
        public Color DefaultBackground { set; get; }

        /// <summary>
        /// Initialize a new instance of <see cref="ScreenDataScrollable"/>
        /// </summary>
        /// <param name="screenData">
        /// The screen data to be scrolled
        /// </param>
        /// <param name="width">
        /// The width of the virtual screen
        /// </param>
        /// <param name="height">
        /// The height of the virtual screen
        /// </param>
        /// <param name="defaultBackground">
        /// When the screen is scrolled by <see cref="Scroll(int, int)"/>, the new area will be filled with this color
        /// </param>
        /// <param name="xOffset">
        /// The initial offset of x (default = 0)
        /// </param>
        /// <param name="yOffset">
        /// The initial offset of y (default = 0)
        /// </param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public ScreenDataScrollable(IScreenData screenData, int width, int height
            , Color defaultBackground, int xOffset = 0, int yOffset = 0)
        {
            if (width <= 0)
                throw new ArgumentException("Negative number", nameof(width));
            if (height <= 0)
                throw new ArgumentException("Negative number", nameof(height));
            this.screenData = screenData ?? throw new ArgumentNullException(nameof(screenData));
            this.height = height;
            this.width = width;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            DefaultBackground = defaultBackground;
            EnsureScreenDataBigEnough();
        }

        private void EnsureScreenDataBigEnough(ScreenCell cell)
        {

            if (0 + yOffset < 0)
            {
                screenData.ExpandHeightUp(-yOffset, cell);
                yOffset = 0;
            }
            if ((height - 1) + yOffset >= screenData.Height)
            {
                screenData.ExpandHeightDown((height - 1) + yOffset - (screenData.Height - 1), cell);
            }
            if (0 + xOffset < 0)
            {
                screenData.ExpandWidthLeft(-xOffset, cell);
                xOffset = 0;
            }
            if ((width - 1) + xOffset >= screenData.Width)
            {
                screenData.ExpandWidthRight((width - 1) + xOffset - (screenData.Width - 1), cell);
            }
        }

        private void EnsureScreenDataBigEnough()
        {
            EnsureScreenDataBigEnough(new ScreenCell()
            {
                Background = DefaultBackground,
                Foreground = Color.White,
                Character = ' ',
                Dirty = true,
            });
        }

        /// <inheritdoc/>
        public void SetDirty(int x, int y, bool flag)
        {
            screenData.SetDirty(x + xOffset, y + yOffset, flag);
        }

        /// <inheritdoc/>
        public void SetDirty(bool flag)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    screenData.SetDirty(x + xOffset, y + yOffset, flag);
        }

        /// <summary>
        /// Scroll the screen without lose data of the screen
        /// </summary>
        /// <remarks>
        /// You can imagine that the data of the screen is a big rectangle, and the screen is 
        /// a small rectangle. This method moves the small rectangle on the big rectangle, with
        /// a offset vector (x, y).
        /// </remarks>
        /// <param name="x">positive number means scroll left</param>
        /// <param name="y">positive number means scroll up</param>
        public void Scroll(int x, int y)
        {
            xOffset += x;
            yOffset += y;
            EnsureScreenDataBigEnough();
        }

        /// <summary>
        /// Scroll the screen without lose data of the screen
        /// </summary>
        /// <remarks>
        /// You can imagine that the data of the screen is a big rectangle, and the screen is 
        /// a small rectangle. This method moves the small rectangle on the big rectangle, with
        /// a offset vector (x, y).
        /// </remarks>
        /// <param name="x">positive number means scroll left</param>
        /// <param name="y">positive number means scroll up</param>
        /// <param name="cell">the cell to fill in new area if not exists</param>
        public void Scroll(int x, int y, ScreenCell cell)
        {
            xOffset += x;
            yOffset += y;
            EnsureScreenDataBigEnough(cell);
        }

        /// <inheritdoc/>
        public ScreenCell this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= width)
                    throw new IndexOutOfRangeException();
                if (y < 0 || y >= height)
                    throw new IndexOutOfRangeException();
                return screenData[x + xOffset, y + yOffset];
            }
            set
            {
                if (x < 0 || x >= width)
                    throw new IndexOutOfRangeException();
                if (y < 0 || y >= height)
                    throw new IndexOutOfRangeException();
                screenData[x + xOffset, y + yOffset] = value;
            }
        }

        /// <inheritdoc/>
        public int Height => height;

        /// <inheritdoc/>
        public int Width => width;

        /// <inheritdoc/>
        public void ExpandHeightDown(int lines, ScreenCell cell)
        {
            if (lines < 0)
                throw new ArgumentException("Negative number", nameof(lines));
            height += lines;
            EnsureScreenDataBigEnough(cell);
        }

        /// <inheritdoc/>
        public void ExpandHeightUp(int lines, ScreenCell cell)
        {
            if (lines < 0)
                throw new ArgumentException("Negative number", nameof(lines));
            height += lines;
            yOffset -= lines;
            EnsureScreenDataBigEnough(cell);
        }

        /// <inheritdoc/>
        public void ExpandWidthLeft(int columns, ScreenCell cell)
        {
            if (columns < 0)
                throw new ArgumentException("Negative number", nameof(columns));
            width += columns;
            xOffset -= columns;
            EnsureScreenDataBigEnough(cell);
        }

        /// <inheritdoc/>
        public void ExpandWidthRight(int columns, ScreenCell cell)
        {
            if (columns < 0)
                throw new ArgumentException("Negative number", nameof(columns));
            width += columns;
            EnsureScreenDataBigEnough(cell);
        }
    }
}
