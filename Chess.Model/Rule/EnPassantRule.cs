namespace Chess.Model.Rule
{
    using Chess.Model.Command;
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System;

    public class EnPassantRule : IChessPieceVisitor<bool>, ICommandVisitor<Func<Position, Pawn, IMaybe<ICommand>>>
    {
        public IMaybe<ICommand> GetCommand(ChessGame game, Position position, Pawn pawn)
        {
            return game.LastUpdate.Bind(e => e.Command.Accept(this)(position, pawn));
        }

        public bool Visit(Bishop _)
        {
            return false;
        }

        public bool Visit(King _)
        {
            return false;
        }

        public bool Visit(Knight _)
        {
            return false;
        }

        public bool Visit(Pawn _)
        {
            return true;
        }

        public bool Visit(Queen _)
        {
            return false;
        }

        public bool Visit(Rook _)
        {
            return false;
        }

        public Func<Position, Pawn, IMaybe<ICommand>> Visit(EndTurnCommand _)
        {
            return (_, _) => Nothing<ICommand>.Instance;
        }

        public Func<Position, Pawn, IMaybe<ICommand>> Visit(MoveCommand command)
        {
            return (position, pawn) =>
            {
                var enemyColor = command.Piece.Color;
                var enPassantRow = enemyColor == Color.White ? 3 : 4;
                var source = command.Source;
                var target = command.Target;

                                if
                (
                                        position.Row == enPassantRow &&
                                        command.Piece.Accept(this) &&
                                        position.Row == target.Row &&
                    Math.Abs(position.Column - target.Column) == 1 &&
                                        Math.Abs(source.Row - target.Row) == 2
                )
                {
                    var removeCommand = new RemoveCommand(target, command.Piece);
                    var newPosition = new Position((source.Row + target.Row) / 2, target.Column);
                    var moveCommand = new MoveCommand(position, newPosition, pawn);

                    return new Just<ICommand>
                    (
                        new SequenceCommand(removeCommand, moveCommand)
                    );
                }

                return Nothing<ICommand>.Instance;
            };
        }

        public Func<Position, Pawn, IMaybe<ICommand>> Visit(RemoveCommand _)
        {
            return (_, _) => Nothing<ICommand>.Instance;
        }

        public Func<Position, Pawn, IMaybe<ICommand>> Visit(SequenceCommand command)
        {
            return (position, pawn) =>
            {
                var firstResult = command.FirstCommand.Accept(this)(position, pawn);

                if (firstResult.HasValue)
                {
                    return firstResult;
                }

                return command.SecondCommand.Accept(this)(position, pawn);
            };
        }

        public Func<Position, Pawn, IMaybe<ICommand>> Visit(SetLastUpdateCommand _)
        {
            return (_, _) => Nothing<ICommand>.Instance;
        }

        public Func<Position, Pawn, IMaybe<ICommand>> Visit(SpawnCommand _)
        {
            return (_, _) => Nothing<ICommand>.Instance;
        }
    }
}