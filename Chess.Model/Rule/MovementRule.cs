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

    public class MovementRule : IChessPieceVisitor<Func<ChessGame, Position, IEnumerable<ICommand>>>
    {
        private readonly CastlingRule castlingRule;

        private readonly EnPassantRule enPassantRule;

        private readonly PromotionRule promotionRule;

        private readonly ThreatAnalyzer threatAnalyzer;

        public MovementRule
        (
            CastlingRule castlingRule,
            EnPassantRule enPassantRule,
            PromotionRule promotionRule,
            ThreatAnalyzer threatAnalyzer
        )
        {
            Validation.NotNull(castlingRule, nameof(castlingRule));
            Validation.NotNull(enPassantRule, nameof(enPassantRule));
            Validation.NotNull(promotionRule, nameof(promotionRule));
            Validation.NotNull(threatAnalyzer, nameof(threatAnalyzer));

            this.castlingRule = castlingRule;
            this.enPassantRule = enPassantRule;
            this.promotionRule = promotionRule;
            this.threatAnalyzer = threatAnalyzer;
        }

        public IEnumerable<ICommand> GetCommands(ChessGame game, PlacedPiece piece)
        {
            return this.GetCommands(game, piece.Position, piece.Piece);
        }

        public IEnumerable<ICommand> GetCommands(ChessGame game, Position position, ChessPiece piece)
        {
            return piece.Accept(this)(game, position);
        }

        public IEnumerable<ICommand> GetCommands(ChessGame game, Position position)
        {
            return game.Board.GetPiece(position).GetOrElse
            (
                p => p.Accept(this)(game, position),
                Enumerable.Empty<ICommand>()
            );
        }

        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Bishop bishop)
        {
            return this.GetThreatCommands(bishop);
        }

        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(King king)
        {
            return (game, position) =>
            {
                var threatMoves = this.GetThreatCommands(game.Board, position, king);
                var castlingMoves = this.castlingRule.GetCommands(game, position, king);

                return threatMoves.Union(castlingMoves);
            };
        }

        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Knight knight)
        {
            return this.GetThreatCommands(knight);
        }

        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Pawn pawn)
        {
            return (game, position) =>
            {
                                var enemyColor = pawn.Color.Toggle();
                var threats = this.threatAnalyzer.Visit(pawn)(game.Board, position);
                var hits = threats.Select(p => game.Board.GetPiece(p, enemyColor)).FilterMaybes();

                                var rowBase = pawn.Color == Color.White ? 1 : 6;
                var rowDelta = pawn.Color == Color.White ? 1 : -1;

                var oneForward = position.Offset(rowDelta, 0);
                var oneFreeForward = oneForward.Guard(p => !game.Board.IsOccupied(p));

                var twoForwardValid = oneFreeForward.Guard(p => p.Row == rowBase + rowDelta);
                var twoForward = twoForwardValid.Bind(p => p.Offset(rowDelta, 0));
                var twoFreeForward = twoForward.Guard(p => !game.Board.IsOccupied(p));

                var forwardPositions = oneFreeForward.Yield().Union(twoFreeForward.Yield());

                                var enPassantCommand = this.enPassantRule.GetCommand(game, position, pawn);

                                var hitCommands = hits.SelectMany
                (
                    enemy =>
                    {
                        var removeCommand = new RemoveCommand(enemy);
                        var moveCommand = new MoveCommand(position, enemy.Position, pawn);
                        var sequenceCommand = new SequenceCommand(removeCommand, moveCommand);
                        var promotionCommands = this.promotionRule.GetCommands(enemy.Position, pawn);

                        if (promotionCommands.Any())
                        {
                            return promotionCommands.Select
                            (
                                c => new SequenceCommand(sequenceCommand, c)
                            );
                        }

                        return sequenceCommand.Yield();
                    }
                );

                var moveCommands = forwardPositions.SelectMany<Position, ICommand>
                (
                    target =>
                    {
                        var moveCommand = new MoveCommand(position, target, pawn);
                        var promotionCommands = this.promotionRule.GetCommands(target, pawn);

                        if (promotionCommands.Any())
                        {
                            return promotionCommands.Select
                            (
                                c => new SequenceCommand(moveCommand, c)
                            );
                        }

                        return moveCommand.Yield();
                    }
                );

                return hitCommands.Union(moveCommands).Union(enPassantCommand.Yield());
            };
        }

        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Queen queen)
        {
            return this.GetThreatCommands(queen);
        }

        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Rook rook)
        {
            return this.GetThreatCommands(rook);
        }

        private Func<ChessGame, Position, IEnumerable<ICommand>> GetThreatCommands(ChessPiece piece)
        {
            return (game, position) => this.GetThreatCommands(game.Board, position, piece);
        }

        private IEnumerable<ICommand> GetThreatCommands(Board board, Position position, ChessPiece piece)
        {
            var targets = piece.Accept(this.threatAnalyzer)(board, position);

            foreach (var target in targets)
            {
                var moveCommand = new MoveCommand(position, target, piece);
                var enemy = board.GetPiece(target, piece.Color.Toggle());

                yield return enemy.GetOrElse<ICommand>(
                    e => new SequenceCommand(new RemoveCommand(e), moveCommand),
                    moveCommand
                );
            }
        }
    }
}