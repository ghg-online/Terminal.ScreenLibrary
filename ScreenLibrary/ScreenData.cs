using System;

namespace Terminal.ScreenLibrary
{
    public sealed class ScreenData : IScreenData
    {
        ScreenCell[,] Cells;

        public int Width => Cells.GetLength(0);
        public int Height => Cells.GetLength(1);
        public ScreenCell this[int x, int y]
        {
            get => Cells[x, y];
            set => Cells[x, y] = value;
        }

        public ScreenData(int width, int height)
        {
            if (width < 0)
                throw new ArgumentException("Negative number", nameof(width));
            if (height < 0)
                throw new ArgumentException("Negative number", nameof(height));
            Cells = new ScreenCell[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    Cells[x, y] = new ScreenCell();
        }

        /// <summary>
        /// Expand the screen height, adding lines to the upside.
        /// </summary>
        /// <param name="lines">The number of expanded lines</param>
        /// <exception cref="ArgumentException"></exception>"
        public void ExpandHeightUp(int lines, ScreenCell cell)
        {
            if (lines < 0)
                throw new ArgumentException("Negative number", nameof(lines));
            int width = Width;
            int height = Height;
            ScreenCell[,] newCells = new ScreenCell[width, height + lines];
            for (int y = 0; y < lines; y++)
                for (int x = 0; x < width; x++)
                    newCells[x, y] = cell;
            for (int y = lines; y < height + lines; y++)
                for (int x = 0; x < width; x++)
                    newCells[x, y] = Cells[x, y - lines];
            Cells = newCells;
        }

        /// <summary>
        /// Expand the screen height, adding lines to the downside.
        /// </summary>
        /// <param name="lines">The number of expanded lines</param>
        /// <exception cref="ArgumentException"></exception>
        public void ExpandHeightDown(int lines, ScreenCell cell)
        {
            if (lines < 0)
                throw new ArgumentException("Negative number", nameof(lines));
            int width = Width;
            int height = Height;
            ScreenCell[,] newCells = new ScreenCell[width, height + lines];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    newCells[x, y] = Cells[x, y];
            for (int y = height; y < height + lines; y++)
                for (int x = 0; x < width; x++)
                    newCells[x, y] = cell;
            Cells = newCells;
        }

        /// <summary>
        /// Expand the screen width, adding columns to the right side.
        /// </summary>
        /// <param name="columns">The number of expanded columns</param>
        /// <exception cref="ArgumentException"></exception>"
        public void ExpandWidthRight(int columns, ScreenCell cell)
        {
            if (columns < 0)
                throw new ArgumentException("Negative number", nameof(columns));
            int width = Width;
            int height = Height;
            ScreenCell[,] newCells = new ScreenCell[width + columns, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    newCells[x, y] = Cells[x, y];
            for (int y = 0; y < height; y++)
                for (int x = width; x < width + columns; x++)
                    newCells[x, y] = cell;
            Cells = newCells;
        }

        /// <summary>
        /// Expand the screen width, adding columns to the left side.
        /// </summary>
        /// <param name="columns">The number of expanded columns</param>
        /// <exception cref="ArgumentException"></exception>
        public void ExpandWidthLeft(int columns, ScreenCell cell)
        {
            if (columns < 0)
                throw new ArgumentException("Negative number", nameof(columns));
            int width = Width;
            int height = Height;
            ScreenCell[,] newCells = new ScreenCell[width + columns, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < columns; x++)
                    newCells[x, y] = cell;
            for (int y = 0; y < height; y++)
                for (int x = columns; x < width + columns; x++)
                    newCells[x, y] = Cells[x - columns, y];
            Cells = newCells;
        }
    }
}
