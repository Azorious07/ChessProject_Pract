namespace Chess.View.Window
{
    using Chess.Model.Game;
    using Chess.View.Selector;
    using Chess.ViewModel.Game;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        private readonly ChessGameVM game;

        private readonly PromotionSelector promotionSelector;

        public MainWindow()
        {
            this.InitializeComponent();
            this.game = new ChessGameVM(this.Choose);
            this.promotionSelector = new PromotionSelector();
            this.DataContext = this.game;
        }

        private void BoardMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(sender as Canvas);
            var row = 7 - (int)point.Y;
            var column = (int)point.X;
            var validRow = Math.Max(0, Math.Min(7, row));
            var validColumn = Math.Max(0, Math.Min(7, column));

            this.game.Select(validRow, validColumn);
        }

        private void ExitClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RemoveCompleted(object sender, EventArgs e)
        {
            this.game.Board.CleanUp();
        }

        private Update Choose(IList<Update> updates)
        {
            if (updates.Count == 0)
            {
                return null;
            }
            
            if (updates.Count == 1)
            {
                return updates[0];
            }

                        var promotions = this.promotionSelector.Find(updates);
            var pieceWindow = new PieceWindow() { Owner = this };
            var selectedPiece = pieceWindow.Show(promotions.Keys);

            return
                selectedPiece != null
                    ? promotions[selectedPiece]
                    : null;
        }
    }
}