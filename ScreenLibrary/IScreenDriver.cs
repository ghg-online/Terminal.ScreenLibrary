using System.Drawing;

namespace Terminal.ScreenLibrary
{
    /// <summary>
    /// All screen drivers must implement this interface.
    /// </summary>
    public interface IScreenDriver
    {
        /// <summary>
        /// Get the width and height of the screen.
        /// </summary>
        /// <remarks>
        /// This should be a constant value, because screen resizing is not supported yet.
        /// If this value is changed, undefined behavior may occur.
        /// </remarks>
        int Width { get; }

        /// <summary>
        /// Get the height of the screen.
        /// </summary>
        /// <remarks>
        /// This should be a constant value, because screen resizing is not supported yet.
        /// If this value is changed, undefined behavior may occur.
        /// </remarks>
        int Height { get; }

        /// <summary>
        /// Apply the changes to the screen.
        /// </summary>
        void Redraw();

        /// <summary>
        /// This should updates the screen buffer, but it is not needed
        /// to apply the changes when this method is called.
        /// It is promised that when changes need to be shown, Redraw()
        /// will be called.
        /// </summary>
        /// <param name="x">
        /// The x coordinate of the character to be updated.
        /// </param>
        /// <param name="y">
        /// The y coordinate of the character to be updated.
        /// </param>
        /// <param name="c">
        /// The character to be updated.
        /// </param>
        /// <param name="foreground">
        /// The foreground color of the character to be updated.
        /// </param>
        /// <param name="background">
        /// The background color of the character to be updated.
        /// </param>
        void Update(int x, int y, char c, Color foreground, Color background);
    }
}
