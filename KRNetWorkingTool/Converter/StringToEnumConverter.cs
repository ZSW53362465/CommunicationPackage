using System;
using System.Globalization;
using System.Windows.Data;

namespace Chioy.Communication.Networking.KRNetWorkingTool.Converter
{
    public class StringToEnumConverter : IValueConverter
    {
        public Type EnumType { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return Enum.GetName(EnumType, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return Enum.Parse(EnumType, value.ToString());
        }

        #endregion
    }
}