namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;

    public interface ICommand : ICommandVisitable
    {
        IMaybe<ChessGame> Execute(ChessGame game);
    }
}