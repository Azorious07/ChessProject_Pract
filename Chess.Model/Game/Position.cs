namespace Chess.Model.Game
{
    using Chess.Model.Data;
    using System;

    public class Position : IEquatable<Position>
    {
        public readonly int Row;

        public readonly int Column;

        public Position(int row, int column)
        {
            Validation.InRange(row, 0, 7, nameof(row));
            Validation.InRange(column, 0, 7, nameof(column));

            this.Row = row;
            this.Column = column;
        }

        public IMaybe<Position> Offset(Direction offset)
        {
            return this.Offset(offset.RowDelta, offset.ColumnDelta);
        }

        public IMaybe<Position> Offset(int rowDelta, int columnDelta)
        {
            var newRow = this.Row + rowDelta;
            var newColumn = this.Column + columnDelta;

            if (Validation.IsInRange(newRow, 0, 7) &&
                Validation.IsInRange(newColumn, 0, 7))
            {
                return new Just<Position>(new Position(newRow, newColumn));
            }

            return Nothing<Position>.Instance;
        }

        public bool Equals(Position other)
        {
            return
                this.Row == other.Row &&
                this.Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return
                obj is Position other &&
                this.Row == other.Row &&
                this.Column == other.Column;
        }

        public override int GetHashCode()
        {
            var hashCodeBuilder = new HashCode();
            hashCodeBuilder.Add(this.Row);
            hashCodeBuilder.Add(this.Column);
            return hashCodeBuilder.ToHashCode();
        }
    }
}