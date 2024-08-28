using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlEx.MacroButtonGroupEx
{
    public partial class cMacroButtonGroup : ContentControl
    {
        static cMacroButtonGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cMacroButtonGroup), new FrameworkPropertyMetadata(typeof(cMacroButtonGroup)));
        }
    }
}
