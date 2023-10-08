using System;

namespace Terminal.ScreenLibrary
{
    /// <inheritdoc/>
    public sealed class ScreenData : IScreenData
    {
        ScreenCell[,] Cells;

        /// <inheritdoc/>
        public int Width => Cells.GetLength(0);

        /// <inheritdoc/>
        public int Height => Cells.GetLength(1);

        /// <inheritdoc/>
        public ScreenCell this[int x, int y]
        {
            get => Cells[x, y];
            set
            {
                if (Cells[x, y] != value)
                {
                    Cells[x, y] = value;
                    Cells[x, y].Dirty = true;
                }
            }
        }

        /// <inheritdoc/>
        public void SetDirty(int x, int y, bool flag)
        {
            Cells[x, y].Dirty = flag;
        }

        /// <inheritdoc/>
        public void SetDirty(bool flag)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    SetDirty(x, y, flag);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
