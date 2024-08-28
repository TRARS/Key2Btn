using Key2Btn.Base.Helper.ExClass;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Key2Btn.Base.CustomControlExBase.TextBoxEx
{
    internal class cColorPicker_converter_hsv2brush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is HSVA hsv)) { return Binding.DoNothing; }

            var rgb = ColorHelper.ColorConverter.HsvToRgb(hsv);
            var rgbValue = $"#FF{rgb.R:x2}{rgb.G:x2}{rgb.B:x2}".ToUpper();
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(rgbValue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cColorPicker_converter_hsv2selectedIndex : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is HSVA hsva)) { return Binding.DoNothing; }

            var rgb = ColorHelper.ColorConverter.HsvToRgb(hsva);
            var rgbValue = $"#{(byte)Math.Clamp(hsva.Alpha * 255, 0, 255):x2}{rgb.R:x2}{rgb.G:x2}{rgb.B:x2}".ToUpper();

            var list = (ObservableCollection<ColorStruct>)values[1];
            int index = list.Select((color, idx) => new { color, idx })
                            .Where(x => x.color.ARGB.Equals(rgbValue) || x.color.HSVA.Equals(hsva))
                            .Select(x => x.idx)
                            .FirstOrDefault(-1);

            return index >= 0 ? index : Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cColorPicker_converter_hsv2hsvstring : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is HSVA hsva)) { return Binding.DoNothing; }

            return parameter switch
            {
                "h" => $"{hsva.H}°",
                "s" => $"{hsva.S}%",
                "v" => $"{hsva.V}%",
                "a" => $"{(int)(hsva.Alpha * 100)}%",
                _ => throw new NotImplementedException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cColorPicker_converter_hsv2hsvbrush : IValueConverter
    {
        static LinearGradientBrush hueBrush = HueBrush();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is HSVA hsva)) { return Binding.DoNothing; }

            return parameter switch
            {
                "h" => hueBrush,
                "s" => Hsv2SaturationBrush(hsva),
                "v" => Hsv2ValueBrush(hsva),
                "a" => Hsv2AlphaBrush(hsva),
                _ => throw new NotImplementedException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static LinearGradientBrush HueBrush()
        {
            LinearGradientBrush hueGradient = new LinearGradientBrush();
            hueGradient.StartPoint = new Point(0, 0);
            hueGradient.EndPoint = new Point(1, 0);
            hueGradient.GradientStops.Add(new GradientStop(ColorFromHue(0), 0.0));    // Red
            hueGradient.GradientStops.Add(new GradientStop(ColorFromHue(60), 0.1667));  // Yellow
            hueGradient.GradientStops.Add(new GradientStop(ColorFromHue(120), 0.3333)); // Green
            hueGradient.GradientStops.Add(new GradientStop(ColorFromHue(180), 0.5));    // Cyan
            hueGradient.GradientStops.Add(new GradientStop(ColorFromHue(240), 0.6667)); // Blue
            hueGradient.GradientStops.Add(new GradientStop(ColorFromHue(300), 0.8333)); // Magenta
            hueGradient.GradientStops.Add(new GradientStop(ColorFromHue(359), 1.0));    // Red again
            return hueGradient;
        }
        private static LinearGradientBrush Hsv2SaturationBrush(HSVA hsva)
        {
            var v = (hsva.V / 100.0);
            var rgb = ColorHelper.ColorConverter.HsvToRgb(new HSVA(hsva.H, 100, 100, 1.0));
            var colorL = Colors.White;
            {
                colorL.R = (byte)(colorL.R * v);
                colorL.G = (byte)(colorL.G * v);
                colorL.B = (byte)(colorL.B * v);
            }
            var colorR = (Color)ColorConverter.ConvertFromString($"#FF{rgb.R:x2}{rgb.G:x2}{rgb.B:x2}".ToUpper());
            {
                colorR.R = (byte)(colorR.R * v);
                colorR.G = (byte)(colorR.G * v);
                colorR.B = (byte)(colorR.B * v);
            }
            var linearGradient = new LinearGradientBrush();
            linearGradient.StartPoint = new Point(0, 1);
            linearGradient.EndPoint = new Point(1, 0);
            linearGradient.GradientStops.Add(new GradientStop(colorL, 0.0));
            linearGradient.GradientStops.Add(new GradientStop(colorR, 1.0));
            return linearGradient;
        }
        private static LinearGradientBrush Hsv2ValueBrush(HSVA hsva)
        {
            var s = (hsva.S / 100.0);
            var rgb = ColorHelper.ColorConverter.HsvToRgb(new HSVA(hsva.H, 100, 100, 1.0));
            var colorL = Colors.Black;
            var colorR = (Color)ColorConverter.ConvertFromString($"#FF{rgb.R:x2}{rgb.G:x2}{rgb.B:x2}".ToUpper());
            {
                colorR.R = (byte)(colorR.R + (255 - colorR.R) * (1 - s));
                colorR.G = (byte)(colorR.G + (255 - colorR.G) * (1 - s));
                colorR.B = (byte)(colorR.B + (255 - colorR.B) * (1 - s));
            }
            var linearGradient = new LinearGradientBrush();
            linearGradient.StartPoint = new Point(0, 0);
            linearGradient.EndPoint = new Point(1, 0);
            linearGradient.GradientStops.Add(new GradientStop(colorL, 0.0));
            linearGradient.GradientStops.Add(new GradientStop(colorR, 1.0));
            return linearGradient;
        }
        private static LinearGradientBrush Hsv2AlphaBrush(HSVA hsva)
        {
            var rgb = ColorHelper.ColorConverter.HsvToRgb(new HSVA(hsva.H, hsva.S, hsva.V, hsva.Alpha));
            var rgbValue = $"#FF{rgb.R:x2}{rgb.G:x2}{rgb.B:x2}".ToUpper();
            var colorL = Colors.Transparent;
            var colorR = (Color)ColorConverter.ConvertFromString(rgbValue);
            var linearGradient = new LinearGradientBrush();
            linearGradient.StartPoint = new Point(0, 0);
            linearGradient.EndPoint = new Point(1, 0);
            linearGradient.GradientStops.Add(new GradientStop(colorL, 0.0));
            linearGradient.GradientStops.Add(new GradientStop(colorR, 0.95));
            linearGradient.GradientStops.Add(new GradientStop(colorR, 1.0));
            return linearGradient;
        }

        // Helper method to convert a hue angle to a Color
        private static Color ColorFromHue(double hue)
        {
            int hi = (int)(hue / 60) % 6;
            double f = hue / 60 - hi;
            byte v = 255;
            byte p = 0;
            byte q = (byte)(255 * (1 - f));
            byte t = (byte)(255 * f);

            switch (hi)
            {
                case 0: return Color.FromRgb(v, t, p);
                case 1: return Color.FromRgb(q, v, p);
                case 2: return Color.FromRgb(p, v, t);
                case 3: return Color.FromRgb(p, q, v);
                case 4: return Color.FromRgb(t, p, v);
                default: return Color.FromRgb(v, p, q);
            }
        }
    }

    internal class cColorPicker_converter_hsv2slidervalue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is HSVA hsva)) { return Binding.DoNothing; }

            return $"{parameter}".ToLower() switch
            {
                "h" => (double)hsva.H,
                "s" => (double)hsva.S,
                "v" => (double)hsva.V,
                "a" => (double)((int)(hsva.Alpha * 100.0)),
                _ => throw new NotImplementedException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cColorPicker_converter_sliderwidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double.NaN) { return Binding.DoNothing; }

            return (double)value + double.Parse($"{parameter}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
