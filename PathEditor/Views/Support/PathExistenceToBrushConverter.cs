using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PathEditor.Views.Support
{
    public class PathExistenceToBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush NotExistsBrush = new SolidColorBrush(Colors.Yellow);
        private static readonly SolidColorBrush ExistsBrush = new SolidColorBrush(Colors.Transparent);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return ExistsBrush;

            var exists = (bool) value;
            return exists ? ExistsBrush : NotExistsBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
