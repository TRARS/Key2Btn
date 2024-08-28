using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Key2Btn.Base.CustomControlExBase.ContextMenuEx
{
    public partial class cContextMenu : ContextMenu
    {
        static cContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cContextMenu), new FrameworkPropertyMetadata(typeof(cContextMenu)));
        }

        public cContextMenu()
        {

        }

        protected override void OnClosed(RoutedEventArgs e)
        {
            base.OnClosed(e); CloseMenuCommand?.Execute(null);
        }
    }

    public partial class cContextMenu
    {
        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }
        public static new readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
            name: "BorderThickness",
            propertyType: typeof(Thickness),
            ownerType: typeof(cContextMenu),
            typeMetadata: new FrameworkPropertyMetadata(new Thickness(2), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public new CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static new readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register(
            name: "BorderCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cContextMenu),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(4), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ICommand CloseMenuCommand
        {
            get { return (ICommand)GetValue(CloseMenuCommandProperty); }
            set { SetValue(CloseMenuCommandProperty, value); }
        }
        public static readonly DependencyProperty CloseMenuCommandProperty = DependencyProperty.Register(
            name: "CloseMenuCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cContextMenu),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ICommand ResetFocusCommand
        {
            get { return (ICommand)GetValue(ResetFocusCommandProperty); }
            set { SetValue(ResetFocusCommandProperty, value); }
        }
        public static readonly DependencyProperty ResetFocusCommandProperty = DependencyProperty.Register(
            name: "ResetFocusCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cContextMenu),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ICommand RemoveItemCommand
        {
            get { return (ICommand)GetValue(RemoveItemCommandProperty); }
            set { SetValue(RemoveItemCommandProperty, value); }
        }
        public static readonly DependencyProperty RemoveItemCommandProperty = DependencyProperty.Register(
            name: "RemoveItemCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cContextMenu),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public object Parent
        {
            get { return (object)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }
        public static readonly DependencyProperty ParentProperty = DependencyProperty.Register(
            name: "Parent",
            propertyType: typeof(object),
            ownerType: typeof(cContextMenu),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
