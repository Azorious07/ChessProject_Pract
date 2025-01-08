namespace Chess.Model.Rule
{
    using Chess.Model.Command;
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using Chess.Model.Visitor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CastlingRule : IChessPieceVisitor<bool>, ICommandVisitor<Func<Position, bool>>
    {
        private readonly ThreatAnalyzer threatAnalyzer;

        public CastlingRule(ThreatAnalyzer threatAnalyzer)
        {
            Validation.NotNull(threatAnalyzer, nameof(threatAnalyzer));
            this.threatAnalyzer = threatAnalyzer;
        }

        public IEnumerable<ICommand> GetCommands(ChessGame game, Position position, King king)
        {
            var expectedPosition = king.Color == Color.White
                        ? new Position(0, 4)
                        : new Position(7, 4);

            if (!position.Equals(expectedPosition))
            {
                return Enumerable.Empty<ICommand>();
            }

                        var threats = new Lazy<List<Position>>
            (
                () =>
                {
                    var enemies = game.Board.GetPieces(king.Color.Toggle());
                    var stream = enemies.SelectMany(p => this.threatAnalyzer.GetThreats(game.Board, p));
                    return stream.ToList();
                }
            );

            return this.GetCommands(game, position, king, threats);
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
            return false;
        }

        public bool Visit(Queen _)
        {
            return false;
        }

        public bool Visit(Rook _)
        {
            return true;
        }

        public Func<Position, bool> Visit(EndTurnCommand _)
        {
            return _ => false;
        }

        public Func<Position, bool> Visit(MoveCommand command)
        {
            return position => command.Source.Equals(position);
        }

        public Func<Position, bool> Visit(RemoveCommand _)
        {
            return _ => false;
        }

        public Func<Position, bool> Visit(SequenceCommand command)
        {
            return position =>
                command.FirstCommand.Accept(this)(position) ||
                command.SecondCommand.Accept(this)(position);
        }

        public Func<Position, bool> Visit(SetLastUpdateCommand _)
        {
            return _ => false;
        }

        public Func<Position, bool> Visit(SpawnCommand _)
        {
            return _ => false;
        }

        private IEnumerable<ICommand> GetCommands(ChessGame game, Position position, King king, Lazy<List<Position>> threats)
        {
            var maybeLeftRook = game.Board.GetPiece(position.Row, 0, king.Color);
            var maybeRightRook = game.Board.GetPiece(position.Row, 7, king.Color);
            var leftRook = maybeLeftRook.Guard(r => r.Accept(this));
            var rightRook = maybeRightRook.Guard(r => r.Accept(this));

            foreach (var rook in leftRook.Yield().Union(rightRook.Yield()))
            {
                var direction = position.Column > rook.Position.Column ? -1 : 1;
                var oneNext = new Position(position.Row, position.Column + direction);
                var twoNext = new Position(position.Row, oneNext.Column + direction);

                                if
                (
                                        !game.Board.IsOccupied(oneNext) &&
                    !game.Board.IsOccupied(twoNext) &&
                                        !threats.Value.Contains(oneNext) &&
                    !threats.Value.Contains(twoNext) &&
                                        !threats.Value.Contains(position) &&
                                                            !game.History.Any(u =>
                    {
                        var eval = u.Command.Accept(this);
                        return eval(position) || eval(rook.Position);
                    })
                )
                {
                    yield return new SequenceCommand
                    (
                        new MoveCommand(position, twoNext, king),
                        new MoveCommand(rook, oneNext)
                    );
                }
            }
        }
    }
}