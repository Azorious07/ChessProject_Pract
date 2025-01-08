namespace Chess.Model.Piece
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using System;

    public class PlacedPiece : ChessPiece
    {
        public readonly Position Position;

        public readonly ChessPiece Piece;

        public PlacedPiece(Position position, ChessPiece piece) : base(piece.Color)
        {
            Validation.NotNull(position, nameof(position));
            Validation.NotNull(piece, nameof(piece));

            this.Position = position;
            this.Piece = piece;
        }

        public IMaybe<PlacedPiece> Move(Direction direction)
        {
            return this.Position.Offset(direction).Map
            (
                p => new PlacedPiece(p, this.Piece)
            );
        }

        public PlacedPiece MoveTo(Position newPosition)
        {
            return new PlacedPiece(newPosition, this.Piece);
        }

        public override void Accept(IChessPieceVisitor visitor)
        {
            this.Piece.Accept(visitor);
        }

        public override T Accept<T>(IChessPieceVisitor<T> visitor)
        {
            return this.Piece.Accept(visitor);
        }

        public override bool Equals(ChessPiece obj)
        {
            return
                obj is PlacedPiece other &&
                this.Position.Equals(other.Position) &&
                this.Piece.Equals(other.Piece);
        }

        public override bool Equals(object obj)
        {
            return
                obj is PlacedPiece other &&
                this.Position.Equals(other.Position) &&
                this.Piece.Equals(other.Piece);
        }

        public override int GetHashCode()
        {
            var hashCodeBuilder = new HashCode();
            hashCodeBuilder.Add(this.Position);
            hashCodeBuilder.Add(this.Piece);
            return hashCodeBuilder.ToHashCode();
        }
    }
}