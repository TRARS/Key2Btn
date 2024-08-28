using System;
using System.Globalization;
using System.Windows.Data;

namespace Key2Btn.Base.CustomControlExBase.TextBoxEx
{
    class cActionKeySelector_converter_comment2bool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty($"{value}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
