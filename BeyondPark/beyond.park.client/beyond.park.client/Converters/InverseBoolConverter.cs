using System;
using System.Globalization;
using Xamarin.Forms;

namespace beyond.park.client.Converters {
    public class InverseBoolConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is bool) {
				return !(bool)value;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
