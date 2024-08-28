using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlExBase.SliderEx
{
    public partial class cSlider : Slider
    {
        public new bool IsInitialized { get; set; } = false;

        static cSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cSlider), new FrameworkPropertyMetadata(typeof(cSlider)));
        }
    }

    public partial class cSlider
    {
        public double DefalutValue
        {
            get { return (double)GetValue(DefalutValueProperty); }
            set { SetValue(DefalutValueProperty, value); }
        }
        public static readonly DependencyProperty DefalutValueProperty = DependencyProperty.Register(
            name: "DefalutValue",
            propertyType: typeof(double),
            ownerType: typeof(cSlider),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
