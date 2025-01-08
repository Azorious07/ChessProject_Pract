namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;

    public class SequenceCommand : ICommand
    {
        public readonly ICommand FirstCommand;

        public readonly ICommand SecondCommand;

        public SequenceCommand(ICommand firstCommand, ICommand secondCommand)
        {
            Validation.NotNull(firstCommand, nameof(firstCommand));
            Validation.NotNull(secondCommand, nameof(secondCommand));

            this.FirstCommand = firstCommand;
            this.SecondCommand = secondCommand;
        }

        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return this.FirstCommand.Execute(game).Bind(g => this.SecondCommand.Execute(g));
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