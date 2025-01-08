namespace Chess.Model.Piece
{
    using System;

    public abstract class ChessPiece : IChessPieceVisitable, IEquatable<ChessPiece>
    {
        public readonly Color Color;

        public ChessPiece(Color color)
        {
            this.Color = color;
        }

        public abstract void Accept(IChessPieceVisitor visitor);

        public abstract T Accept<T>(IChessPieceVisitor<T> visitor);

        public virtual bool Equals(ChessPiece other)
        {
            return
                this.Color == other.Color &&
                this.GetType() == other.GetType();
        }

        public override bool Equals(object obj)
        {
            return
                obj is ChessPiece other &&
                this.Color == other.Color &&
                this.GetType() == other.GetType();
        }

        public override int GetHashCode()
        {
            var hashCodeBuilder = new HashCode();
            hashCodeBuilder.Add(this.GetType());
            hashCodeBuilder.Add(this.Color);
            return hashCodeBuilder.ToHashCode();
        }
    }
}