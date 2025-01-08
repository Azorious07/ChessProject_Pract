namespace Chess.Model.Game
{
    using Chess.Model.Piece;

    public class Player
    {
        public readonly Color Color;

        public Player(Color color)
        {
            this.Color = color;
        }
    }
}