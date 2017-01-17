using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Chioy.Communication.Networking.KRNetWorkingTool.Converter
{
    public class StringToVisibility : IValueConverter
    {
        public StringToVisibility()
        {
            SameResult = Visibility.Visible;
        }

        public Visibility SameResult { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(parameter))
            {
                return SameResult;
            }

            string[] paramters = null;
            if (parameter != null)
            {
                paramters = parameter.ToString().Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
            }

            if (value != null && paramters != null && paramters.Contains(value.ToString()))
            {
                return SameResult;
            }

            return SameResult == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}