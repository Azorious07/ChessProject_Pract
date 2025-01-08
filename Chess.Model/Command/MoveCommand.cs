namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;

    public class MoveCommand : ICommand
    {
        public readonly Position Source;

        public readonly Position Target;

        public readonly ChessPiece Piece;

        public MoveCommand(PlacedPiece piece, Position target) : this(piece.Position, target, piece.Piece)
        {
        }

        public MoveCommand(Position source, Position target, ChessPiece piece)
        {
            Validation.NotNull(source, nameof(source));
            Validation.NotNull(target, nameof(target));
            Validation.NotNull(piece, nameof(piece));

            this.Source = source;
            this.Target = target;
            this.Piece = piece;
        }

        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return game.Board.Remove(this.Source).Bind(b => b.Add(this.Target, this.Piece)).Map
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