namespace Chess.Model.Piece
{
    public interface IChessPieceVisitor
    {
        void Visit(Bishop bishop);

        void Visit(King king);

        void Visit(Knight knight);

        void Visit(Pawn pawn);

        void Visit(Queen queen);

        void Visit(Rook rook);
    }

    public interface IChessPieceVisitor<T>
    {
        T Visit(Bishop bishop);

        T Visit(King king);

        T Visit(Knight knight);

        T Visit(Pawn pawn);

        T Visit(Queen queen);

        T Visit(Rook rook);
    }
}