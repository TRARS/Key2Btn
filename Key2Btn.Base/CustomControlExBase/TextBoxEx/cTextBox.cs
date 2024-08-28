using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlExBase.TextBoxEx
{
    public class cTextBox : TextBox
    {
        static cTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cTextBox), new FrameworkPropertyMetadata(typeof(cTextBox)));
        }
    }
}
