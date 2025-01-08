namespace Chess.Model.Piece
{
    public enum Color
    {
        White,

        Black
    }

    public static class ColorExtension
    {
        public static Color Toggle(this Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}