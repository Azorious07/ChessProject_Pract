namespace Chess.Model.Game
{
    using System.Collections.Generic;

    public class PositionComparer : IComparer<Position>
    {
        public static readonly IComparer<Position> DefaultComparer = new PositionComparer();

        public int Compare(Position x, Position y)
        {
            var rowComparison = x.Row.CompareTo(y.Row);

            return
                rowComparison == 0
                    ? x.Column.CompareTo(y.Column)
                    : rowComparison;
        }
    }
}