using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;

namespace Veith.WPFLocalizator.View
{
    public class PathToFileNameConverter : MarkupExtension, IValueConverter
    {
        public PathToFileNameConverter()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new PathToFileNameConverter();
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is string) || string.IsNullOrEmpty((string)value))
            {
                return value;
            }

            var fileName = Path.GetFileName((string)value);

            return fileName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
