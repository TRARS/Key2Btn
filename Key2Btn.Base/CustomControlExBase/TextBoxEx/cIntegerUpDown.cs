using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Key2Btn.Base.CustomControlExBase.TextBoxEx
{
    public partial class cIntegerUpDown : TextBox
    {
        static cIntegerUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cIntegerUpDown), new FrameworkPropertyMetadata(typeof(cIntegerUpDown)));
        }
    }

    public partial class cIntegerUpDown
    {
        [RelayCommand]
        private void OnPreviewMouseWheel(object para)
        {
            if (para is MouseWheelEventArgs e && this.IsFocused)
            {
                e.Handled = true;

                if (e.Delta > 0) { IntegerValue++; }
                if (e.Delta < 0) { IntegerValue--; }
            }
        }

        [RelayCommand]
        private void OnRepeatUp()
        {
            IntegerValue++;
        }

        [RelayCommand]
        private void OnRepeatDown()
        {
            IntegerValue--;
        }
    }

    public partial class cIntegerUpDown
    {
        public int IntegerValue
        {
            get { return (int)GetValue(IntegerValueProperty); }
            set { SetValue(IntegerValueProperty, value); }
        }
        public static readonly DependencyProperty IntegerValueProperty = DependencyProperty.Register(
            name: "IntegerValue",
            propertyType: typeof(int),
            ownerType: typeof(cIntegerUpDown),
            typeMetadata: new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
