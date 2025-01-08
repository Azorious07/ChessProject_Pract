namespace Chess.Model.Piece
{
    public interface IChessPieceVisitable
    {
        void Accept(IChessPieceVisitor visitor);

        T Accept<T>(IChessPieceVisitor<T> visitor);
    }
}