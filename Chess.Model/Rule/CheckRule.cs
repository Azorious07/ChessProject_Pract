namespace Chess.Model.Rule
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using Chess.Model.Visitor;
    using System.Linq;

    public class CheckRule : IChessPieceVisitor<bool>
    {
        private readonly ThreatAnalyzer threatAnalyzer;

        public CheckRule(ThreatAnalyzer threatAnalyzer)
        {
            Validation.NotNull(threatAnalyzer, nameof(threatAnalyzer));
            this.threatAnalyzer = threatAnalyzer;
        }

        public bool Check(ChessGame game, Player player)
        {
            var color = player.Color;
            var enemyColor = color.Toggle();
            var enemies = game.Board.GetPieces(enemyColor);
            var threats = enemies.SelectMany(p => this.threatAnalyzer.GetThreats(game.Board, p));
            var king = game.Board.GetPieces(color).Find(p => p.Accept(this));
            var isChecked = king.Map(p => threats.Any(t => t.Equals(p.Position)));

            return isChecked.GetOrElse(c => c, false);
        }

        public bool Visit(Bishop _)
        {
            return false;
        }

        public bool Visit(King _)
        {
            return true;
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
            return false;
        }
    }
}