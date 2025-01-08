namespace Chess.Model.Rule
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using System.Collections.Generic;

    public interface IRulebook
    {
        ChessGame CreateGame();

        Status GetStatus(ChessGame game);

        IEnumerable<Update> GetUpdates(ChessGame game, Position position);
    }
}