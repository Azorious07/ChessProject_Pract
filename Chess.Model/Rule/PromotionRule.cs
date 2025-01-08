namespace Chess.Model.Rule
{
    using Chess.Model.Command;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System.Collections.Generic;

    public class PromotionRule
    {
                #pragma warning disable CA1822
        public IEnumerable<ICommand> GetCommands(Position position, Pawn pawn)
        {
            switch (position.Row)
            {
                case 0:
                case 7:
                    foreach (var form in PromotionRule.GetPieces(pawn.Color))
                    {
                        yield return new SequenceCommand
                        (
                            new RemoveCommand(position, pawn),
                            new SpawnCommand(position, form)
                        );
                    }
                    break;
                default:
                    yield break;
            }
        }
#pragma warning restore CA1822

        private static IEnumerable<ChessPiece> GetPieces(Color color)
        {
            yield return new Queen(color);
            yield return new Bishop(color);
            yield return new Knight(color);
            yield return new Rook(color);
        }
    }
}