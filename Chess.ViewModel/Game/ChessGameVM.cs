namespace Chess.ViewModel.Game
{
    using Chess.Model.Command;
    using Chess.Model.Game;
    using Chess.Model.Rule;
    using Chess.ViewModel.Command;
    using Chess.ViewModel.Visitor;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class ChessGameVM : ICommandVisitor, INotifyPropertyChanged
    {
        private readonly IRulebook rulebook;

        private readonly Func<IList<Update>, Update> updateSelector;

        private readonly GenericCommand undoCommand;

        private ChessGame game;

        private BoardVM board;

        public ChessGameVM(Func<IList<Update>, Update> updateSelector)
        {
            this.rulebook = new StandardRulebook();
            this.Game = this.rulebook.CreateGame();
            this.board = new BoardVM(this.Game.Board);
            this.updateSelector = updateSelector;

            this.undoCommand = new GenericCommand
            (
                () => this.Game.LastUpdate.HasValue,
                () => this.Game.LastUpdate.Do
                (
                    e =>
                    {
                        this.Game = e.Game;
                        this.Board.ClearUpdates();
                    }
                )
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BoardVM Board
        {
            get
            {
                return this.board;
            }

            private set
            {
                if (this.board != value)
                {
                    this.board = value ?? throw new ArgumentNullException(nameof(this.Board));
                    this.OnPropertyChanged(nameof(this.Board));
                }
            }
        }

        public Status Status => this.rulebook.GetStatus(this.Game);

        public GenericCommand NewCommand
        {
            get
            {
                return new GenericCommand
                (
                    () => true,
                    () =>
                    {
                        this.Game = this.rulebook.CreateGame();
                        this.Board = new BoardVM(this.Game.Board);
                        this.OnPropertyChanged(nameof(this.Status));
                    }
                );
            }
        }

        public GenericCommand UndoCommand => this.undoCommand;

        private ChessGame Game
        {
            get
            {
                return this.game;
            }

            set
            {
                if (this.game != value)
                {
                    this.game = value ?? throw new ArgumentNullException(nameof(this.Game));
                    this.UndoCommand?.FireCanExecuteChanged();
                }
            }
        }

        public void Select(int row, int column)
        {
            var position = new Position(row, column);
            var field = this.Board.GetField(position);

            if (this.Board.Source == field)
            {
                this.Board.ClearUpdates();
                return;
            }

            var updates = this.Board.GetUpdates(field);
            var selectedUpdate = this.updateSelector(updates);
            this.Board.ClearUpdates();

            if (selectedUpdate != null)
            {
                this.Game = selectedUpdate.Game;
                selectedUpdate.Command.Accept(this);
            }
            else if (this.game.Board.IsOccupied(position, this.game.ActivePlayer.Color))
            {
                var newUpdates = this.rulebook.GetUpdates(this.Game, position);
                this.Board.SetSource(position);
                this.Board.SetTargets(newUpdates);
            }
        }

        public void Visit(SequenceCommand command)
        {
            command.FirstCommand.Accept(this);
            command.SecondCommand.Accept(this);
        }

        public void Visit(EndTurnCommand command)
        {
            this.OnPropertyChanged(nameof(this.Status));
        }

        public void Visit(MoveCommand command)
        {
            this.Board.Execute(command);
        }

        public void Visit(RemoveCommand command)
        {
            this.Board.Execute(command);
        }

        public void Visit(SetLastUpdateCommand command)
        {
                    }

        public void Visit(SpawnCommand command)
        {
            this.Board.Execute(command);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}