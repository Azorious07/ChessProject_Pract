namespace Chess.ViewModel.Game
{
    using Chess.Model.Command;
    using Chess.Model.Game;
    using Chess.ViewModel.Piece;
    using Chess.ViewModel.Visitor;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class BoardVM
    {
        private readonly TargetSetter targetSetter;

        private readonly FieldVM[,] fields;

        private readonly Dictionary<FieldVM, List<Update>> targets;

        public BoardVM(Board board)
        {
            var pieces = board.Select(p => new PlacedPieceVM(p));
            var fieldArray = new FieldVM[8, 8];
            var fieldVMs =
               from row in Enumerable.Range(0, 8)
               from column in Enumerable.Range(0, 8)
               select new FieldVM(row, column);

            foreach (var field in fieldVMs)
            {
                fieldArray[field.Row, field.Column] = field;
            }

            this.fields = fieldArray;
            this.Pieces = new ObservableCollection<PlacedPieceVM>(pieces);
            this.targets = new Dictionary<FieldVM, List<Update>>();
            this.targetSetter = new TargetSetter();
        }

        public FieldVM Source { get; set; }

        public ObservableCollection<PlacedPieceVM> Pieces { get; }

        public IEnumerable<FieldVM> Fields
        {
            get
            {
                var rowCount = this.fields.GetLength(0);
                var columnCount = this.fields.GetLength(1);

                return
                    from row in Enumerable.Range(0, rowCount)
                    from column in Enumerable.Range(0, columnCount)
                    select this.fields[row, column];
            }
        }

        public void CleanUp()
        {
            foreach (var piece in this.Pieces.Where(p => p.Removed).ToList())
            {
                this.Pieces.Remove(piece);
            }
        }

        public FieldVM GetField(Position position)
        {
            return this.fields[position.Row, position.Column];
        }

        public void ClearUpdates()
        {
            if (this.Source != null)
            {
                this.Source.IsTarget = false;
                this.Source = null;
            }

            foreach (var target in this.targets.Keys)
            {
                target.IsTarget = false;
            }

            this.targets.Clear();
        }

        public void SetTargets(IEnumerable<Update> updates)
        {
            foreach (var update in updates)
            {
                update.Command.Accept(this.targetSetter)(update, this);
            }
        }

        public IList<Update> GetUpdates(FieldVM field)
        {
            if (this.targets.TryGetValue(field, out var updates))
            {
                return updates;
            }

            return Array.Empty<Update>();
        }

        public void SetSource(Position position)
        {
            var field = this.fields[position.Row, position.Column];
            field.IsTarget = true;
            this.Source = field;
        }

        public void AddUpdate(Position position, Update update)
        {
            var field = this.fields[position.Row, position.Column];
            field.IsTarget = true;

            if (this.targets.TryGetValue(field, out var updates))
            {
                updates.Add(update);
            }
            else
            {
                this.targets.Add(field, new List<Update> { update });
            }
        }

        public void Execute(MoveCommand command)
        {
            var piece = this.Pieces.FirstOrDefault
            (
                p => !p.Removed && p.Position.Equals(command.Source)
            );

            if (piece != null)
            {
                piece.Position = new PositionVM(command.Target);
            }
        }

        public void Execute(RemoveCommand command)
        {
            var piece = this.Pieces.FirstOrDefault
            (
                p => !p.Removed && p.Position.Equals(command.Position)
            );

            if (piece != null)
            {
                piece.Removed = true;
            }
        }

        public void Execute(SpawnCommand command)
        {
            this.Pieces.Add(new PlacedPieceVM(command.Position, command.Piece));
        }
    }
}