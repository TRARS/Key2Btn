using Key2Btn.Base.CustomControlEx.MacroButtonGroupEx;
using System.Collections.ObjectModel;

namespace Key2Btn.Base.CustomInterfaces
{
    public interface IMacroButtonGroup<T> where T : IMacroButtonInfo, IKeyboardButtonInfo
    {
        bool DebugMode { get; set; }
        double MacroButtonBackgroundOpacity { get; set; }
        ButtonGroupType Type { get; set; }
        ObservableCollection<ObservableCollection<T>> ItemsSourceGroup { get; }
    }
}
