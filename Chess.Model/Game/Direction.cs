namespace Chess.Model.Game
{
    using System.Collections.Immutable;

    public class Direction
    {
        public static readonly ImmutableArray<Direction> Orthogonals = ImmutableArray.Create
        (
            new Direction(1, 0),
            new Direction(0, 1),
            new Direction(-1, 0),
            new Direction(0, -1)
        );

        public static readonly ImmutableArray<Direction> Diagonals = ImmutableArray.Create
        (
            new Direction(1, -1),
            new Direction(1, 1),
            new Direction(-1, 1),
            new Direction(-1, -1)
        ); 
        
        public readonly int RowDelta;

        public readonly int ColumnDelta;

        public Direction(int rowDelta, int columnDelta)
        {
            this.RowDelta = rowDelta;
            this.ColumnDelta = columnDelta;
        }
    }
}