namespace Chess.Model.Game
{
    using Chess.Model.Command;
    using Chess.Model.Data;

    public class Update
    {
        public readonly ChessGame Game;

        public readonly ICommand Command;

        public Update(ChessGame game, ICommand command)
        {
            Validation.NotNull(game, nameof(game));
            Validation.NotNull(command, nameof(command));

            this.Game = game;
            this.Command = command;
        }
    }
}