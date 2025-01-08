namespace Chess.Model.Rule
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System.Linq;

    public class EndRule
    {
        private readonly CheckRule checkRule;

        private readonly MovementRule movementRule;

        public EndRule(CheckRule checkRule, MovementRule movementRule)
        {
            Validation.NotNull(checkRule, nameof(checkRule));
            Validation.NotNull(movementRule, nameof(movementRule));

            this.checkRule = checkRule;
            this.movementRule = movementRule;
        }

        public Status GetStatus(ChessGame game)
        {
            var color = game.ActivePlayer.Color;
            var pieces = game.Board.GetPieces(color);
            var isChecked = this.checkRule.Check(game, game.ActivePlayer);
            var possibleMoves = pieces.SelectMany(p => this.movementRule.GetCommands(game, p));
            var futures = possibleMoves.Select(c => c.Execute(game)).FilterMaybes();
            var hasMoves = futures.Any(g => !this.checkRule.Check(g, game.ActivePlayer));

            if (isChecked && !hasMoves)
            {
                return
                    color == Color.White
                        ? Status.BlackWin
                        : Status.WhiteWin;
            }
            else if (!isChecked && !hasMoves)
            {
                return Status.Draw;
            }

            return
                color == Color.White
                    ? Status.WhiteTurn
                    : Status.BlackTurn;
        }
    }
}