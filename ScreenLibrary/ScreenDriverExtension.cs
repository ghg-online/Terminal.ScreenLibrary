namespace Terminal.ScreenLibrary
{
    public static class ScreenDriverExtension
    {
        public static void Update(this IScreenDriver screenDriver, int x, int y, ScreenCell screenCell)
        {
            screenDriver.Update(x, y, screenCell.Character, screenCell.Foreground, screenCell.Background);
        }
    }
}
