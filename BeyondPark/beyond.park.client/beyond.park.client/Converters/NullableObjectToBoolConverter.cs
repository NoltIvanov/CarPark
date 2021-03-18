using System;
using System.Globalization;
using Xamarin.Forms;

namespace beyond.park.client.Converters {
    public class NullableObjectToBoolConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (parameter as string == "inverse") {
				return (value == null);
			}

			if (value is string) {
				return !string.IsNullOrEmpty(value.ToString());
			}

			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new InvalidOperationException("NullableObjectToBoolConverter ConvertBack is invalid");
		}
	}
}
