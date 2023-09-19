namespace Terminal.ScreenLibrary
{
    /// <summary>
    /// This class contains extension methods for the IScreenDriver interface.
    /// </summary>
    public static class ScreenDriverExtension
    {
        /// <summary>
        /// This method is only another way to call
        /// <see cref="IScreenDriver.Update(int, int, char, System.Drawing.Color, System.Drawing.Color)"/>
        /// </summary>
        public static void Update(this IScreenDriver screenDriver, int x, int y, ScreenCell screenCell)
        {
            screenDriver.Update(x, y, screenCell.Character, screenCell.Foreground, screenCell.Background);
        }
    }
}
