using System.Collections.Generic;
using System.Drawing;

namespace Terminal.ScreenLibrary
{
    /// <summary>
    /// The data of a screen cell
    /// </summary>
    public struct ScreenCell
    {
        /// <summary>
        /// The character to display
        /// </summary>
        public char Character { get; set; }

        /// <summary>
        /// The foreground color
        /// </summary>
        public Color Foreground { get; set; }

        /// <summary>
        /// The background color
        /// </summary>
        public Color Background { get; set; }

        /// <summary>
        /// Whether the cell has been modified since the last update
        /// </summary>
        public bool Dirty { get; set; }

        /// <summary>
        /// Whether two screen cells are equal
        /// </summary>
        /// <param name="screenCell1">One screen cell</param>
        /// <param name="screenCell2">Another screen cell</param>
        /// <returns>True if equal</returns>
        public static bool operator ==(ScreenCell screenCell1, ScreenCell screenCell2)
            => screenCell1.Character == screenCell2.Character
            && screenCell1.Foreground == screenCell2.Foreground
            && screenCell1.Background == screenCell2.Background;

        /// <summary>
        /// Whether two screen cells are not equal
        /// </summary>
        /// <param name="screenCell1">One screen cell</param>
        /// <param name="screenCell2">Another screen cell</param>
        /// <returns>True if not equal</returns>
        public static bool operator !=(ScreenCell screenCell1, ScreenCell screenCell2)
            => screenCell1.Character != screenCell2.Character
            || screenCell1.Foreground != screenCell2.Foreground
            || screenCell1.Background != screenCell2.Background;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is ScreenCell cell &&
                   Character == cell.Character &&
                   EqualityComparer<Color>.Default.Equals(Foreground, cell.Foreground) &&
                   EqualityComparer<Color>.Default.Equals(Background, cell.Background);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1842600013;
            hashCode = hashCode * -1521134295 + Character.GetHashCode();
            hashCode = hashCode * -1521134295 + Foreground.GetHashCode();
            hashCode = hashCode * -1521134295 + Background.GetHashCode();
            return hashCode;
        }
    }
}
