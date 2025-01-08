namespace Chess.ViewModel.Visitor
{
    using Chess.Model.Command;
    using Chess.Model.Game;
    using Chess.ViewModel.Game;
    using System;

    public class TargetSetter : ICommandVisitor<Func<Update, BoardVM, bool>>
    {
        public Func<Update, BoardVM, bool> Visit(SequenceCommand command)
        {
            return (update, board) =>
                command.FirstCommand.Accept(this)(update, board) ||
                command.SecondCommand.Accept(this)(update, board);
        }

        public Func<Update, BoardVM, bool> Visit(EndTurnCommand _)
        {
            return (_, _) => false;
        }

        public Func<Update, BoardVM, bool> Visit(MoveCommand command)
        {
            return (update, board) =>
            {
                board.AddUpdate(command.Target, update);
                return true;
            };
        }

        public Func<Update, BoardVM, bool> Visit(RemoveCommand _)
        {
            return (_, _) => false;
        }

        public Func<Update, BoardVM, bool> Visit(SetLastUpdateCommand _)
        {
            return (_, _) => false;
        }

        public Func<Update, BoardVM, bool> Visit(SpawnCommand _)
        {
            return (_, _) => false;
        }
    }
}