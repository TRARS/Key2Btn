using System.Windows;

namespace Key2Btn.Base.Helper.AttachedProperty
{
    public partial class DataTemplateHelper : DependencyObject
    {
        public static readonly DependencyProperty ValueToCompareProperty = DependencyProperty.RegisterAttached(
            name: "ValueToCompare",
            propertyType: typeof(bool),
            ownerType: typeof(DataTemplateHelper),
            defaultMetadata: new FrameworkPropertyMetadata(false)
        );
        public static bool GetValueToCompare(DependencyObject target)
        {
            return (bool)target.GetValue(ValueToCompareProperty);
        }
        public static void SetValueToCompare(DependencyObject target, bool value)
        {
            target.SetValue(ValueToCompareProperty, value);
        }
    }
}
