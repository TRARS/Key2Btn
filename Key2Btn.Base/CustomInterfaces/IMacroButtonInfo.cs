using CommunityToolkit.Mvvm.Input;

namespace Key2Btn.Base.CustomInterfaces
{
    public enum KeyCodeType
    {
        CombineKeys, Text, Unknown
    }

    public interface IMacroButtonInfo
    {
        bool IsEnable { get; set; }

        string Name { get; set; }
        string KeyCode { get; set; }
        string Color { get; set; }
        double MaxWidthScale { get; set; }
        string ActionKey { get; set; }
        KeyCodeType Type { get; set; }
        AsyncRelayCommand ClickMacro { get; set; }
        AsyncRelayCommand LongPressDownMacro { get; set; }
        AsyncRelayCommand LongPressUpMacro { get; set; }
    }
}
