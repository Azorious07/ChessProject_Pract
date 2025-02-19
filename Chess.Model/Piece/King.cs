﻿namespace Chess.Model.Piece
{
    public class King : ChessPiece
    {
        public King(Color color) : base(color)
        {
        }

        public override void Accept(IChessPieceVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IChessPieceVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}