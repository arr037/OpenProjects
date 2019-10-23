using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenProjects.Converters
{
    public class AuthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = value as string;
            b = b?.Trim();
            if (string.IsNullOrEmpty(b) || b == "Автор") return "null";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}