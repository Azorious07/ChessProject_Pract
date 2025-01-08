namespace Chess.ViewModel.Piece
{
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using Chess.ViewModel.Game;
    using System;
    using System.ComponentModel;

    public class PlacedPieceVM : INotifyPropertyChanged, IChessPieceVisitable
    {
        private bool removed;

        private PositionVM position;

        private ChessPiece piece;

        public PlacedPieceVM(PlacedPiece piece) : this(piece.Position, piece.Piece)
        {
        }

        public PlacedPieceVM(Position position, ChessPiece piece)
        {
            this.Removed = false;
            this.Position = new PositionVM(position);
            this.Piece = piece;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Removed
        {
            get
            {
                return this.removed;
            }

            set
            {
                if (this.removed != value)
                {
                    this.removed = value;
                    this.OnPropertyChanged(nameof(this.Removed));
                }
            }
        }

        public PositionVM Position
        {
            get
            {
                return this.position;
            }

            set
            {
                if (this.position != value)
                {
                    this.position = value ?? throw new ArgumentNullException(nameof(this.Position));
                    this.OnPropertyChanged(nameof(this.Position));
                }
            }
        }

        public ChessPiece Piece
        {
            get
            {
                return this.piece;
            }

            private set
            {
                if (this.piece != value)
                {
                    this.piece = value ?? throw new ArgumentNullException(nameof(this.Piece));
                    this.OnPropertyChanged(nameof(this.Piece));
                }
            }
        }

        public void Accept(IChessPieceVisitor visitor)
        {
            this.Piece.Accept(visitor);
        }

        public T Accept<T>(IChessPieceVisitor<T> visitor)
        {
            return this.Piece.Accept(visitor);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}