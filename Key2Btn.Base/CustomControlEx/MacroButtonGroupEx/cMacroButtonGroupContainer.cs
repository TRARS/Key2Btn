using Key2Btn.Base.CustomInterfaces;
using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlEx.MacroButtonGroupEx
{
    public partial class cMacroButtonGroupContainer : ItemsControl, IDebugMode
    {
        static cMacroButtonGroupContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cMacroButtonGroupContainer), new FrameworkPropertyMetadata(typeof(cMacroButtonGroupContainer)));
        }
    }

    public partial class cMacroButtonGroupContainer
    {
        public bool IsDebugMode
        {
            get { return (bool)GetValue(IsDebugModeProperty); }
            set { SetValue(IsDebugModeProperty, value); }
        }
        public static readonly DependencyProperty IsDebugModeProperty = DependencyProperty.Register(
            name: "IsDebugMode",
            propertyType: typeof(bool),
            ownerType: typeof(cMacroButtonGroupContainer),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
