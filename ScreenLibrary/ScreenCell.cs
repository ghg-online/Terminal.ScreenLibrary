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
    }
}
