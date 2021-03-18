using System;
using System.Globalization;
using Xamarin.Forms;

namespace beyond.park.client.Converters {
    public class StringToBoolConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string res = value as string;

            return !string.IsNullOrEmpty(res);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new InvalidOperationException("GenericValueToBoolConverter.ConvertBack");
        }
    }
}
