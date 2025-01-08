namespace Chess.Model.Rule
{
    using Chess.Model.Command;
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using Chess.Model.Visitor;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    public class StandardRulebook : IRulebook
    {
        private readonly CheckRule checkRule;

        private readonly EndRule endRule;

        private readonly MovementRule movementRule;

        public StandardRulebook()
        {
            var threatAnalyzer = new ThreatAnalyzer();
            var castlingRule = new CastlingRule(threatAnalyzer);
            var enPassantRule = new EnPassantRule();
            var promotionRule = new PromotionRule();

            this.checkRule = new CheckRule(threatAnalyzer);
            this.movementRule = new MovementRule(castlingRule, enPassantRule, promotionRule, threatAnalyzer);
            this.endRule = new EndRule(this.checkRule, this.movementRule);
        }

        public ChessGame CreateGame()
        {
            IEnumerable<PlacedPiece> makeBaseLine(int row, Color color)
            {
                yield return new PlacedPiece(new Position(row, 0), new Rook(color));
                yield return new PlacedPiece(new Position(row, 1), new Knight(color));
                yield return new PlacedPiece(new Position(row, 2), new Bishop(color));
                yield return new PlacedPiece(new Position(row, 3), new Queen(color));
                yield return new PlacedPiece(new Position(row, 4), new King(color));
                yield return new PlacedPiece(new Position(row, 5), new Bishop(color));
                yield return new PlacedPiece(new Position(row, 6), new Knight(color));
                yield return new PlacedPiece(new Position(row, 7), new Rook(color));
            }

            IEnumerable<PlacedPiece> makePawns(int row, Color color) =>
                Enumerable.Range(0, 8).Select(
                    i => new PlacedPiece(new Position(row, i), new Pawn(color))
                );

            IImmutableDictionary<Position, ChessPiece> makePieces(int pawnRow, int baseRow, Color color)
            {
                var pawns = makePawns(pawnRow, color);
                var baseLine = makeBaseLine(baseRow, color);
                var pieces = baseLine.Union(pawns);
                var empty = ImmutableSortedDictionary.Create<Position, ChessPiece>(PositionComparer.DefaultComparer);
                return pieces.Aggregate(empty, (s, p) => s.Add(p.Position, p.Piece));
            }

            var whitePlayer = new Player(Color.White);
            var whitePieces = makePieces(1, 0, Color.White);
            var blackPlayer = new Player(Color.Black);
            var blackPieces = makePieces(6, 7, Color.Black);
            var board = new Board(whitePieces.AddRange(blackPieces));

            return new ChessGame(board, whitePlayer, blackPlayer);
        }

        public Status GetStatus(ChessGame game)
        {
            return this.endRule.GetStatus(game);
        }

        public IEnumerable<Update> GetUpdates(ChessGame game, Position position)
        {
            var piece = game.Board.GetPiece(position, game.ActivePlayer.Color);
            var updates = piece.Map(
                p =>
                {
                    var moves = this.movementRule.GetCommands(game, p);
                    var turnEnds = moves.Select(c => new SequenceCommand(c, EndTurnCommand.Instance));
                    var records = turnEnds.Select
                    (
                        c => new SequenceCommand(c, new SetLastUpdateCommand(new Update(game, c)))
                    );
                    var futures = records.Select(c => c.Execute(game).Map(g => new Update(g, c)));
                    return futures.FilterMaybes().Where
                    (
                        e => !this.checkRule.Check(e.Game, e.Game.PassivePlayer)
                    );
                }
            );

            return updates.GetOrElse(Enumerable.Empty<Update>());
        }
    }
}