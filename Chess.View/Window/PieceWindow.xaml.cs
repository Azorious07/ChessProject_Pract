namespace Chess.View.Window
{
    using Chess.Model.Piece;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    public partial class PieceWindow : Window
    {
        private ChessPiece selectedPiece;

        public PieceWindow()
        {
            this.InitializeComponent();
        }

        public ChessPiece Show(IEnumerable<ChessPiece> pieces)
        {
            this.pieceControl.ItemsSource = pieces;
            this.ShowDialog();
            return this.selectedPiece;
        }

        private void ChooseClick(object sender, RoutedEventArgs e)
        {
                        //
                        //
                        if (sender is FrameworkElement element)
            {
                var piece = element.Tag as ChessPiece;

                if (piece != null)
                {
                    this.selectedPiece = piece;
                    this.Close();
                }
            }
        }
    }
}