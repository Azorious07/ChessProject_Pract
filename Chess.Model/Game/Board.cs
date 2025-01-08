namespace Chess.Model.Game
{
    using Chess.Model.Data;
    using Chess.Model.Piece;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public class Board : IEnumerable<PlacedPiece>
    {
        private readonly IImmutableDictionary<Position, ChessPiece> pieces;

        public Board(IImmutableDictionary<Position, ChessPiece> pieces)
        {
            Validation.NotNull(pieces, nameof(pieces));
            this.pieces = pieces;
        }

        public IMaybe<Board> Add(Position position, ChessPiece piece)
        {
            try
            {
                return new Just<Board>(new Board(this.pieces.Add(position, piece)));
            }
            catch
            {
                return Nothing<Board>.Instance;
            }
        }

        public IMaybe<Board> Remove(Position position)
        {
            var newPieces = this.pieces.Remove(position);

            return
                this.pieces == newPieces
                    ? Nothing<Board>.Instance
                    : new Just<Board>(new Board(newPieces));
        }

        public bool IsOccupied(Position position)
        {
            return this.GetPiece(position).HasValue;
        }

        public bool IsOccupied(Position position, Color color)
        {
            return this.GetPiece(position, color).HasValue;
        }

        public IMaybe<PlacedPiece> GetPiece(Position position)
        {
            if (this.pieces.TryGetValue(position, out var piece))
            {
                return new Just<PlacedPiece>(new PlacedPiece(position, piece));
            }

            return Nothing<PlacedPiece>.Instance;
        }

        public IMaybe<PlacedPiece> GetPiece(Position position, Color color)
        {
            return this.GetPiece(position).Guard(p => p.Color == color);
        }

        public IMaybe<PlacedPiece> GetPiece(int row, int column)
        {
            return this.GetPiece(new Position(row, column));
        }

        public IMaybe<PlacedPiece> GetPiece(int row, int column, Color color)
        {
            return this.GetPiece(new Position(row, column), color);
        }

        public IEnumerable<PlacedPiece> GetPieces(Color color)
        {
            foreach (var pair in this.pieces)
            {
                if (pair.Value.Color == color)
                {
                    yield return new PlacedPiece(pair.Key, pair.Value);
                }
            }
        }

        public IEnumerator<PlacedPiece> GetEnumerator()
        {
            foreach (var pair in this.pieces)
            {
                yield return new PlacedPiece(pair.Key, pair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}