using Key2Btn.Base.Helper.ExClass;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Key2Btn.Base.CustomControlEx.MacroButtonEditorEx
{
    internal class cMacroButtonEditor_converter_rgb2hsv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // hsv2rgb
            if (!(value is HSVA hsva)) { return Binding.DoNothing; }

            var rgb = ColorHelper.ColorConverter.HsvToRgb(hsva);
            var rgbValue = $"#{(byte)Math.Clamp(hsva.Alpha * 255, 0, 255):x2}{rgb.R:x2}{rgb.G:x2}{rgb.B:x2}".ToUpper();

            return rgbValue;
        }
    }

    internal class cMacroButtonEditor_converter_double2int : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return int.Parse($"{value}");
            }
            catch
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return double.Parse($"{value}");
            }
            catch
            {
                return 0d;
            }
        }
    }

    internal class cMacroButtonEditor_converter_actkey2bool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty($"{value}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButtonEditor_converter_validcheck4actkey : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
