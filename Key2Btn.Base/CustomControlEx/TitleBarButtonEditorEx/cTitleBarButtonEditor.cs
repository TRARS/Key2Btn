using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlEx.TitleBarButtonEditorEx
{
    public partial class cTitleBarButtonEditor : ContentControl
    {
        static cTitleBarButtonEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cTitleBarButtonEditor), new FrameworkPropertyMetadata(typeof(cTitleBarButtonEditor)));
        }
    }

    public partial class cTitleBarButtonEditor
    {
        public string TargetName
        {
            get { return (string)GetValue(TargetNameProperty); }
            set { SetValue(TargetNameProperty, value); }
        }
        public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register(
            name: "TargetName",
            propertyType: typeof(string),
            ownerType: typeof(cTitleBarButtonEditor),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
