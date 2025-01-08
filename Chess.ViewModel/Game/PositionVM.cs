namespace Chess.ViewModel.Game
{
    using Chess.Model.Game;
    using System;

    public class PositionVM : IEquatable<Position>
    {
        public readonly Position Position;

        public PositionVM(Position position)
        {
            this.Position = position ?? throw new ArgumentNullException(nameof(position));
        }

        public int Row => this.Position.Row;

        public int Column => this.Position.Column;

        public bool Equals(Position other)
        {
            return this.Position.Equals(other);
        }
    }
}