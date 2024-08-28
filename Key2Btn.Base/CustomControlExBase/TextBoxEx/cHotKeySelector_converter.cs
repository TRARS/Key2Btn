using Key2Btn.Base.CustomInterfaces;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Key2Btn.Base.CustomControlExBase.TextBoxEx
{
    class cHotKeySelector_converter_radius2cornerradius : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] == DependencyProperty.UnsetValue) { return Binding.DoNothing; }

            var ActualHeight = (double)values[0];
            var CornerRadius = (CornerRadius)values[1];

            return new CornerRadius(Math.Min(CornerRadius.TopLeft + 0.5, (ActualHeight + 0.5) / 2));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cHotKeySelector_converter_diameter2height : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] == DependencyProperty.UnsetValue) { return Binding.DoNothing; }

            var BorderThickness = (Thickness)values[0];
            var Diameter = (double)values[1];

            return Diameter + BorderThickness.Top + BorderThickness.Bottom;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cHotKeySelector_converter_diameter2width : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return cHotKeySelector_math.Instance.WidthCalculator((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cHotKeySelector_converter_diameterDiet : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue) { return Binding.DoNothing; }

            var old = (double)values[0];
            var diet = (double)values[1];
            return Math.Max(old - diet * 2, 1d);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cHotKeySelector_converter_transformXcalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is double transformX)
            {
                var diet = (double)values[1];

                return Math.Max(transformX + diet, 0d);
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cHotKeySelector_converter_keycodetype2bool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (KeyCodeType)value switch
            {
                KeyCodeType.CombineKeys => true,
                KeyCodeType.Text => false,
                _ => false,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value switch
            {
                true => KeyCodeType.CombineKeys,
                false => KeyCodeType.Text,
            };
        }
    }

    class cHotKeySelector_converter_separator2brush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var offsetL = (double)values[0];
            var offsetR = (double)values[1];
            var colorL = (Color)values[2];
            var colorR = (Color)values[3];

            return new SolidColorBrush(offsetL > 0.5 ? colorL : colorR);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cHotKeySelector_converter_thicknessminus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var old = (Thickness)value;
            return new Thickness(Math.Max(old.Left - 0.5, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
