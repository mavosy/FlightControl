using System.Globalization;
using System.Windows.Data;

namespace FlightControl.Converters
{
	/// <summary>
	/// Converts a null value to a Boolean. This converter returns true if the input value is not null, and false otherwise.
	/// </summary>
	public class NullToBooleanConverter : IValueConverter
    {
		/// <summary>
		/// Converts the specified value to a Boolean based on its null status.
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack not supported for this converter.");
        }
    }
}