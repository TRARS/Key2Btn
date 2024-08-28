using System.Windows;
using System.Windows.Media;

namespace Key2Btn.Base.CustomInterfaces
{
    public interface IKeyboardButtonInfo
    {
        bool IsEnable { get; set; }

        double AbsWidth { get; set; }
        double KeySpacing { get; set; }
        string Key { get; set; }
        string ShiftKey { get; set; }
        bool IsModifyKeyDown { get; set; }

        bool IsAlphabet { get; set; }
        bool IsCapsActive { get; set; }
        bool IsShiftActive { get; set; }
        bool IsCapsOrShiftActive { get; }

        bool IsMarqueeActive { get; set; }

        Geometry PathData { get; set; }
        Thickness PathMargin { get; set; }
    }
}
