namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;

    public class RemoveCommand : ICommand
    {
        public readonly Position Position;

        public readonly ChessPiece Piece;

        public RemoveCommand(PlacedPiece piece) : this(piece.Position, piece.Piece)
        {
        }

        public RemoveCommand(Position position, ChessPiece piece)
        {
            Validation.NotNull(position, nameof(position));
            Validation.NotNull(piece, nameof(piece));

            this.Position = position;
            this.Piece = piece;
        }

        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return game.Board.Remove(this.Position).Map
            (
                newBoard => game.SetBoard(newBoard)
            );
        }

        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public T Accept<T>(ICommandVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}