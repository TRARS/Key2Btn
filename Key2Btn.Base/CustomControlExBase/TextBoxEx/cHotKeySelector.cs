using CommunityToolkit.Mvvm.Input;
using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Key2Btn.Base.CustomControlExBase.TextBoxEx
{
    partial class cHotKeySelector_math
    {
        private static readonly Lazy<cHotKeySelector_math> lazyObject = new(() => new cHotKeySelector_math());
        public static cHotKeySelector_math Instance => lazyObject.Value;

        private cHotKeySelector_math() { }

        public double WidthCalculator(double value)
        {
            return (double)value * 2 + 1;
        }
    }

    public partial class cHotKeySelector : TextBox
    {
        static cHotKeySelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cHotKeySelector), new FrameworkPropertyMetadata(typeof(cHotKeySelector)));
        }

        public cHotKeySelector()
        {
            DotCornerRadius = new(5.5);
            //ToolTip = "Temporarily allow DS4W to recognize a virtual DS4 controller as a real DS4 controller once.";
        }
    }

    // ToggleButton
    public partial class cHotKeySelector
    {
        public KeyCodeType KeyCodeType
        {
            get { return (KeyCodeType)GetValue(KeyCodeTypeProperty); }
            set { SetValue(KeyCodeTypeProperty, value); }
        }
        public static readonly DependencyProperty KeyCodeTypeProperty = DependencyProperty.Register(
            name: "KeyCodeType",
            propertyType: typeof(KeyCodeType),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(KeyCodeType.Unknown, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        [RelayCommand]
        private void OnChecked()
        {
            CheckedAnimation();
        }

        [RelayCommand]
        private void OnUnChecked()
        {
            UncheckedAnimation();
        }
    }
    public partial class cHotKeySelector
    {
        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            name: "BackgroundColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Thickness DotBorderThickness
        {
            get { return (Thickness)GetValue(DotBorderThicknessProperty); }
            set { SetValue(DotBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty DotBorderThicknessProperty = DependencyProperty.Register(
            name: "DotBorderThickness",
            propertyType: typeof(Thickness),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(new Thickness(1), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public CornerRadius DotCornerRadius
        {
            get { return (CornerRadius)GetValue(DotCornerRadiusProperty); }
            set { SetValue(DotCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty DotCornerRadiusProperty = DependencyProperty.Register(
            name: "DotCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(2d), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double DotDiameter
        {
            get { return (double)GetValue(DotDiameterProperty); }
            set { SetValue(DotDiameterProperty, value); }
        }
        public static readonly DependencyProperty DotDiameterProperty = DependencyProperty.Register(
            name: "DotDiameter",
            propertyType: typeof(double),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(11d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool Enable
        {
            get { return (bool)GetValue(EnableProperty); }
            set { SetValue(EnableProperty, value); }
        }
        public static readonly DependencyProperty EnableProperty = DependencyProperty.Register(
            name: "Enable",
            propertyType: typeof(bool),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
    public partial class cHotKeySelector
    {
        public Color SliderBackgroundColor
        {
            get { return (Color)GetValue(SliderBackgroundColorProperty); }
            set { SetValue(SliderBackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty SliderBackgroundColorProperty = DependencyProperty.Register(
            name: "SliderBackgroundColor",
            propertyType: typeof(Color),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata((Color)ColorConverter.ConvertFromString(primary_color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double SliderSeparatorOffset
        {
            get { return (double)GetValue(SliderSeparatorOffsetProperty); }
            set { SetValue(SliderSeparatorOffsetProperty, value); }
        }
        public static readonly DependencyProperty SliderSeparatorOffsetProperty = DependencyProperty.Register(
            name: "SliderSeparatorOffset",
            propertyType: typeof(double),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double DotTransformX
        {
            get { return (double)GetValue(DotTransformXProperty); }
            set { SetValue(DotTransformXProperty, value); }
        }
        public static readonly DependencyProperty DotTransformXProperty = DependencyProperty.Register(
            name: "DotTransformX",
            propertyType: typeof(double),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double DotDiet
        {
            get { return (double)GetValue(DotDietProperty); }
            set { SetValue(DotDietProperty, value); }
        }
        public static readonly DependencyProperty DotDietProperty = DependencyProperty.Register(
            name: "DotDiet",
            propertyType: typeof(double),
            ownerType: typeof(cHotKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        private static IEasingFunction easing = new PowerEase() { EasingMode = EasingMode.EaseInOut, Power = 3.5 };
        private const string primary_color = "#FFC62626";
        private const double duration = 192;
        private double dot_distance => cHotKeySelector_math.Instance.WidthCalculator(DotDiameter) - DotDiameter - (DotBorderThickness.Left + DotBorderThickness.Right);

        private void CheckedAnimation()
        {
            this.SetDoubleAnimation(DotTransformXProperty, DotTransformX, dot_distance, duration * Factor(DotTransformX, dot_distance, dot_distance), easing).Begin();
            this.SetDoubleAnimation(SliderSeparatorOffsetProperty, SliderSeparatorOffset, 1d, duration * Factor(SliderSeparatorOffset, 1d, 1d)).Begin();
        }
        private void UncheckedAnimation()
        {
            this.SetDoubleAnimation(DotTransformXProperty, DotTransformX, 0, duration * Factor(DotTransformX, 0, dot_distance), easing).Begin();
            this.SetDoubleAnimation(SliderSeparatorOffsetProperty, SliderSeparatorOffset, 0d, duration * Factor(SliderSeparatorOffset, 0d, 1d)).Begin();
        }
        private double Factor(double from, double to, double distance)
        {
            return (Math.Abs(from - to) / distance);
        }
    }
}
