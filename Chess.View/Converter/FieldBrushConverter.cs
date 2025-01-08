namespace Chess.View.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class FieldBrushConverter : IMultiValueConverter
    {
        private readonly Brush whiteBrush;

        private readonly Brush blackBrush;

        public FieldBrushConverter()
        {
            this.whiteBrush = new SolidColorBrush(Colors.NavajoWhite);
            this.blackBrush = new SolidColorBrush(Colors.Peru);
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !typeof(Brush).IsAssignableFrom(targetType))
            {
                return null;
            }

            var rowObj = values[0];
            var columnObj = values[1];

            if (rowObj is int row && columnObj is int column)
            {
                return
                    (row + column) % 2 == 0
                        ? this.blackBrush
                        : this.whiteBrush;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}