using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlEx.MacroButtonEditorEx
{
    public partial class cMacroButtonEditor : ContentControl
    {
        static cMacroButtonEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cMacroButtonEditor), new FrameworkPropertyMetadata(typeof(cMacroButtonEditor)));
        }
    }
}
