using Key2Btn.Base.CustomControlEx.MacroButtonGroupEx;
using Key2Btn.Base.CustomInterfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static Key2Btn.Base.CustomControlEx.MacroButtonEx.cMacroButton;

namespace Key2Btn.Base.CustomControlEx.MacroButtonEx
{
    internal class cMacroButton_converter_keycodetype2brushcolor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (KeyCodeType)value switch
            {
                KeyCodeType.CombineKeys => new SolidColorBrush(Colors.DodgerBlue),
                _ => new SolidColorBrush(Colors.White),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_keycode2textdecorations : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty($"{value}") ? TextDecorations.Underline : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_state2opacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is cMacroButton.State state)
            {
                if (state is cMacroButton.State.MouseEnterEx)
                {
                    return 0.75;
                }
            }

            return 0.25;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_modifykey2color : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool flag)) { return Binding.DoNothing; }

            return new SolidColorBrush(flag ? Colors.SteelBlue : Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_containertype2minwidth : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var unitWidth = double.Parse($"{values[0]}");
            var containerType = (ButtonGroupType)values[1];

            return containerType is ButtonGroupType.SimpleKeyboardContainer ? 5d : unitWidth;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_width2multiplesofunitwidth : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var actualWidth = double.Parse($"{values[0]}");
            var unitWidth = double.Parse($"{values[1]}");
            var spacing = double.Parse($"{values[2]}");
            var debugMode = bool.Parse($"{values[3]}");
            var maxWidthScale = double.Parse($"{values[4]}");
            var containerType = (ButtonGroupType)values[5];
            var absWidth = double.Parse($"{values[6]}");

            if (containerType is ButtonGroupType.SimpleKeyboardContainer && !debugMode)
            {
                if (absWidth <= 0) { return Binding.DoNothing; }
                return absWidth; //KeyboardButton使用具体宽度
            }

            var newUnitWidth = debugMode ? Math.Ceiling((unitWidth) * (Math.PI + 0.45)) : (unitWidth);
            var multiple = Math.Clamp(Math.Ceiling(actualWidth / (newUnitWidth + spacing)), 1.0, maxWidthScale); //限制放大倍率
            var result = multiple * newUnitWidth + (multiple - 1) * spacing;

            return actualWidth < newUnitWidth ? newUnitWidth : result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_bool2visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool flag)
            {
                return flag ? Visibility.Visible : Visibility.Collapsed;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_colornullcheck : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null ? new SolidColorBrush(Colors.DarkGray) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_backgroundullcheck : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null ? new SolidColorBrush(Colors.Transparent) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_pos2margin : IMultiValueConverter
    {
        static int count = 0;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(int.TryParse($"{values[0]}", out var posx)) ||
                !(int.TryParse($"{values[1]}", out var posy)) ||
                !(double.TryParse($"{values[2]}", out var spacing)) ||
                !(bool.TryParse($"{values[3]}", out var isFirstInLine)) ||
                !(int.TryParse($"{values[4]}", out var lineIndex)) ||
                !(values[5] is ButtonGroupType containerType) ||
                !(double.TryParse($"{values[6]}", out var keySpacing)))
            {
                return Binding.DoNothing;
            }

            if (containerType is ButtonGroupType.SimpleKeyboardContainer)
            {
                spacing = keySpacing; //KeyboardButton使用专属间距
            }

            var cx = posx == 0 ? 0 : spacing;
            var cy = posy == 0 ? 0 : spacing;
            var left = isFirstInLine ? spacing : 0;
            var top = (posy == 0 && lineIndex > 0) ? spacing : 0;
            var result = new Thickness(spacing - cx + left, spacing - cy - top, spacing, spacing);

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_wh2ripplesize : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var w = double.Parse($"{values[0]}");
                var h = double.Parse($"{values[1]}");
                return Math.Sqrt(Math.Pow(w, 2) + Math.Pow(h, 2)) * 2.5;
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_ripplecenter2canvasleft : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var w = double.Parse($"{values[0]}");
                var center = (Point)values[1];
                var o = new Point(w / 2, w / 2);

                return (center.X - o.X);
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_ripplecenter2canvastop : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var h = double.Parse($"{values[0]}");
                var center = (Point)values[1];
                var o = new Point(h / 2, h / 2);

                return (center.Y - o.Y);
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_cornerradius2double : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((CornerRadius)value).TopLeft;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_size2rect : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var w = (double)values[0];
            var h = (double)values[1];
            return new Rect(0, 0, w, h);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_rippleprogress2opacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var t = double.Parse($"{value}");
            return 4 * t * (1 - t);

            //var left = 0.2;
            //var right = 0.8;
            //if (t < left)
            //{
            //    var nt = t * (0.5 / left);//0~left -> 0~0.5
            //    return 4 * nt * (1 - nt);
            //}
            //else if (t >= left && t <= right)
            //{
            //    var nt = 0.5;
            //    return 4 * nt * (1 - nt);
            //}
            //else
            //{
            //    var nt = 1 - (1 - t) * ((0.5) / (1 - right));//right~1.0 -> 0.5~1.0
            //    return 4 * nt * (1 - nt);
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        double MapValue(double value)
        {
            double oldMin = 0.8;
            double oldMax = 1.0;
            double newMin = 0.5;
            double newMax = 1.0;

            return newMin + ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin);
        }
    }

    internal class cMacroButton_converter_width2stretch : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var str_width = double.Parse($"{values[0]}");
                var btn_width = double.Parse($"{values[1]}");
                if (str_width > btn_width) { return Stretch.Uniform; }
                return Stretch.None;
            }
            catch { return Stretch.None; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_namekeycode2visibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var name = $"{values[0]}";
            var keycode = $"{values[1]}";
            var containerType = (ButtonGroupType)values[2];

            if (containerType is ButtonGroupType.SimpleKeyboardContainer) { return Visibility.Visible; }
            return string.Equals(name, keycode) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_pathdata2visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ($"{value}".Count() > 0) { return Visibility.Collapsed; }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_shiftkeynullcheck : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace($"{value}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_leftright2visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (LeftRight)value switch
            {
                LeftRight.None => Visibility.Collapsed,
                _ => Visibility.Visible,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_leftright2horizontalalignment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (LeftRight)value switch
            {
                LeftRight.None => Binding.DoNothing,
                LeftRight.Left => HorizontalAlignment.Left,
                LeftRight.Right => HorizontalAlignment.Right,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_insertmarkerheightlimit : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double.NaN) { return Binding.DoNothing; }
            var result = Math.Min((double)value - 5 * 2, double.Parse($"{parameter}"));

            return result > 0 ? result : Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cMacroButton_converter_editor4macrobutton : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] == DependencyProperty.UnsetValue) { return Binding.DoNothing; }

            var flag = (bool)values[0];
            var containerType = (ButtonGroupType)values[1];

            if (flag && containerType is ButtonGroupType.MacroButtonContainer)
            {
                return true;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
