namespace Chess.View.Selector
{
    using Chess.Model.Piece;
    using System.Windows;
    using System.Windows.Controls;

    public class PieceSymbolSelector : DataTemplateSelector, IChessPieceVisitor<string>
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IChessPieceVisitable piece)
            {
                return Application.Current.FindResource(piece.Accept(this)) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }

        public string Visit(Bishop bishop)
        {
            return ToResourceString(bishop, "Bishop");
        }

        public string Visit(King king)
        {
            return ToResourceString(king, "King");
        }

        public string Visit(Knight knight)
        {
            return ToResourceString(knight, "Knight");
        }

        public string Visit(Pawn pawn)
        {
            return ToResourceString(pawn, "Pawn");
        }

        public string Visit(Queen queen)
        {
            return ToResourceString(queen, "Queen");
        }

        public string Visit(Rook rook)
        {
            return ToResourceString(rook, "Rook");
        }

        private static string ToResourceString(ChessPiece piece, string form)
        {
            return
                piece.Color == Color.White
                    ? "white" + form
                    : "black" + form;
        }
    }
}