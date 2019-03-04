using System;
using System.Globalization;
using Xamarin.Forms;

namespace HMNGasApp.Converters
{
    public class EntryHasDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value > 0)
                return true;    // data has been entered
            else
                return false;   // input is empty
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
