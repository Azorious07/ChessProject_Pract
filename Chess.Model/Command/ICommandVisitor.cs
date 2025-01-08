namespace Chess.Model.Command
{
    public interface ICommandVisitor
    {
        void Visit(EndTurnCommand command);

        void Visit(MoveCommand command);

        void Visit(RemoveCommand command);

        void Visit(SequenceCommand command);

        void Visit(SetLastUpdateCommand command);

        void Visit(SpawnCommand command);
    }

    public interface ICommandVisitor<T>
    {
        T Visit(EndTurnCommand command);

        T Visit(MoveCommand command);

        T Visit(RemoveCommand command);

        T Visit(SequenceCommand command);

        T Visit(SetLastUpdateCommand command);

        T Visit(SpawnCommand command);
    }
}