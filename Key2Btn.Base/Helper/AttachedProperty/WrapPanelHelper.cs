using System.Windows;

namespace Key2Btn.Base.Helper.AttachedProperty
{
    public static class WrapPanelHelper
    {
        public static readonly DependencyProperty LineIndexProperty = DependencyProperty.RegisterAttached(
            name: "LineIndex",
            propertyType: typeof(int),
            ownerType: typeof(WrapPanelHelper),
            defaultMetadata: new FrameworkPropertyMetadata(0)
        );
        public static int GetLineIndex(DependencyObject element)
        {
            return (int)element.GetValue(LineIndexProperty);
        }
        public static void SetLineIndex(DependencyObject element, int value)
        {
            element.SetValue(LineIndexProperty, value);
        }

        public static readonly DependencyProperty IsFirstInLineProperty = DependencyProperty.RegisterAttached(
            name: "IsFirstInLine",
            propertyType: typeof(bool),
            ownerType: typeof(WrapPanelHelper),
            defaultMetadata: new FrameworkPropertyMetadata(false)
        );
        public static bool GetIsFirstInLine(UIElement element)
        {
            return (bool)element.GetValue(IsFirstInLineProperty);
        }
        public static void SetIsFirstInLine(UIElement element, bool value)
        {
            element.SetValue(IsFirstInLineProperty, value);
        }
    }
}
