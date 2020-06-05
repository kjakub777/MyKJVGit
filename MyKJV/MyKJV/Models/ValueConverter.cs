using System;

using Xamarin.Forms;

namespace MyKJV
{
    public class VerseIsMemorizedValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Color.LightGreen : Color.LightGray;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            // You probably don't need this, this is used to convert the other way around
            // so from color to yes no or maybe
            throw new NotImplementedException();
        }
    }
}