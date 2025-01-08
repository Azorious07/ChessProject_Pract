namespace Chess.ViewModel.Game
{
    using System.ComponentModel;

    public class FieldVM : INotifyPropertyChanged
    {
        private bool isTarget;

        public FieldVM(int row, int column)
        {
            this.Row = row;
            this.Column = column;
            this.IsTarget = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Row { get; }

        public int Column { get; }

        public bool IsTarget
        {
            get
            {
                return this.isTarget;
            }

            set
            {
                if (this.isTarget != value)
                {
                    this.isTarget = value;
                    this.OnPropertyChanged(nameof(this.IsTarget));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}