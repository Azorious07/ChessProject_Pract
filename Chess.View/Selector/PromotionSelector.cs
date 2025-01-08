namespace Chess.View.Selector
{
    using Chess.Model.Command;
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System.Collections.Generic;

    public class PromotionSelector : ICommandVisitor<IMaybe<ChessPiece>>
    {
        public Dictionary<ChessPiece, Update> Find(IEnumerable<Update> events)
        {
            var result = new Dictionary<ChessPiece, Update>();

            foreach (var e in events)
            {
                e.Command.Accept(this).Do
                (
                    p => result.Add(p, e)
                );
            }

            return result;
        }

        public IMaybe<ChessPiece> Visit(EndTurnCommand command)
        {
            return Nothing<ChessPiece>.Instance;
        }

        public IMaybe<ChessPiece> Visit(MoveCommand command)
        {
            return Nothing<ChessPiece>.Instance;
        }

        public IMaybe<ChessPiece> Visit(RemoveCommand command)
        {
            return Nothing<ChessPiece>.Instance;
        }

        public IMaybe<ChessPiece> Visit(SequenceCommand command)
        {
            var firstResult = command.FirstCommand.Accept(this);

            if (firstResult.HasValue)
            {
                return firstResult;
            }

            return command.SecondCommand.Accept(this);
        }

        public IMaybe<ChessPiece> Visit(SetLastUpdateCommand command)
        {
            return Nothing<ChessPiece>.Instance;
        }

        public IMaybe<ChessPiece> Visit(SpawnCommand command)
        {
            return new Just<ChessPiece>(command.Piece);
        }
    }
}