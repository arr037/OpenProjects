using System;
using System.Globalization;
using System.Windows.Data;
using OpenProjects.Model;

namespace OpenProjects
{
    public class ItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = value as Item;
            if (b == null) return "null";

            return b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}