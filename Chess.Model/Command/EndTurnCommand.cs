namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;

    public class EndTurnCommand : ICommand
    {
        public static readonly EndTurnCommand Instance = new();

        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return new Just<ChessGame>(game.EndTurn());
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