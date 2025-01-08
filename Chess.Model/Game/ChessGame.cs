namespace Chess.Model.Game
{
    using Chess.Model.Data;
    using System.Collections.Generic;
    using System.Linq;

    public class ChessGame
    {
        public readonly Board Board;

        public readonly Player ActivePlayer;

        public readonly Player PassivePlayer;

        public readonly IMaybe<Update> LastUpdate;

        public ChessGame(Board board, Player activePlayer, Player passivePlayer)
            : this(board, activePlayer, passivePlayer, Nothing<Update>.Instance)
        {
        }

        public ChessGame(Board board, Player activePlayer, Player passivePlayer, IMaybe<Update> lastUpdate)
        {
            Validation.NotNull(board, nameof(board));
            Validation.NotNull(activePlayer, nameof(activePlayer));
            Validation.NotNull(passivePlayer, nameof(passivePlayer));
            Validation.NotNull(lastUpdate, nameof(lastUpdate));

            this.Board = board;
            this.ActivePlayer = activePlayer;
            this.PassivePlayer = passivePlayer;
            this.LastUpdate = lastUpdate;
        }

        public IEnumerable<Update> History
        {
            get
            {
                return this.LastUpdate.GetOrElse
                (
                    u => Enumerable.Prepend(u.Game.History, u),
                    Enumerable.Empty<Update>()
                );
            }
        }

        public ChessGame SetLastUpdate(IMaybe<Update> update)
        {
            Validation.NotNull(update, nameof(update));

            return new ChessGame
            (
                this.Board,
                this.ActivePlayer,
                this.PassivePlayer,
                update
            );
        }

        public ChessGame SetBoard(Board board)
        {
            Validation.NotNull(board, nameof(board));

            return new ChessGame
            (
                board,
                this.ActivePlayer,
                this.PassivePlayer,
                this.LastUpdate
            );
        }

        public ChessGame EndTurn()
        {
            return new ChessGame
            (
                this.Board,
                this.PassivePlayer,
                this.ActivePlayer,
                this.LastUpdate
            );
        }
    }
}