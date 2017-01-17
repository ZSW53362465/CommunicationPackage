using System;
using System.Globalization;
using System.Windows.Data;

namespace Chioy.Communication.Networking.KRNetWorkingTool.Converter
{
    public class BooleanToReBoolean : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ReturnValue(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ReturnValue(value);
        }

        #endregion

        private static object ReturnValue(object value)
        {
            var bValue = value as bool?;

            if (bValue == null)
            {
                return false;
            }

            return !bValue.Value;
        }
    }
}