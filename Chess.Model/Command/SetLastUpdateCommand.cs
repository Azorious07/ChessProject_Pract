namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;

    public class SetLastUpdateCommand : ICommand
    {
        public readonly IMaybe<Update> Update;

        public SetLastUpdateCommand(Update update) : this(new Just<Update>(update))
        {
        }

        public SetLastUpdateCommand(IMaybe<Update> update)
        {
            Validation.NotNull(update, nameof(update));
            this.Update = update;
        }

        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return new Just<ChessGame>(game.SetLastUpdate(this.Update));
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