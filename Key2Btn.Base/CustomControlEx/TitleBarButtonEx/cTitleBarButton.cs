using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlEx.TitleBarButtonEx
{
    public partial class cTitleBarButton : Button
    {
        static readonly SemaphoreSlim menuClosedEvent = new(1);

        static cTitleBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cTitleBarButton), new FrameworkPropertyMetadata(typeof(cTitleBarButton)));
        }

        public cTitleBarButton()
        {
            // 打断原生的展开菜单行为
            this.ContextMenuOpening += (s, e) =>
            {
                e.Handled = true;
            };
        }

        /// <summary>
        /// 已关闭菜单
        /// </summary>
        [RelayCommand]
        private void OnMenuClosed()
        {
            menuClosedEvent.Release(); Debug.WriteLine("TitleBarButton: OnMenuClosed");
        }

        /// <summary>
        /// 关闭菜单
        /// </summary>
        [RelayCommand]
        private void OnCloseMenu()
        {
            if (this.ContextMenuIsOpen)
            {
                this.ContextMenuIsOpen = false; Debug.WriteLine("TitleBarButton: OnCloseMenu");
            }
        }

        /// <summary>
        /// 打开菜单
        /// </summary>
        [RelayCommand]
        public async Task OnOpenContextMenuAsync()
        {
            if (ContextMenuSource == null) { return; }

            // 同步，防闪
            await menuClosedEvent.WaitAsync(); Debug.WriteLine("TitleBarButton: OnOpenContextMenuAsync");

            this.ContextMenuIsOpen = false;
            this.ContextMenuIsOpen = true;
        }
    }

    public partial class cTitleBarButton
    {
        public object ContextMenuSource
        {
            get { return (object)GetValue(ContextMenuSourceProperty); }
            set { SetValue(ContextMenuSourceProperty, value); }
        }
        public static readonly DependencyProperty ContextMenuSourceProperty = DependencyProperty.Register(
            name: "ContextMenuSource",
            propertyType: typeof(object),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool ContextMenuIsOpen
        {
            get { return (bool)GetValue(ContextMenuIsOpenProperty); }
            set { SetValue(ContextMenuIsOpenProperty, value); }
        }
        public static readonly DependencyProperty ContextMenuIsOpenProperty = DependencyProperty.Register(
            name: "ContextMenuIsOpen",
            propertyType: typeof(bool),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ToolBtnType AsTarget
        {
            get { return (ToolBtnType)GetValue(AsTargetProperty); }
            set { SetValue(AsTargetProperty, value); }
        }
        public static readonly DependencyProperty AsTargetProperty = DependencyProperty.Register(
            name: "AsTarget",
            propertyType: typeof(ToolBtnType),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata(ToolBtnType.Unknown, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
            name: "IsActive",
            propertyType: typeof(bool),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata("cTitleBarButton", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ButtonType Type
        {
            get { return (ButtonType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            name: "Type",
            propertyType: typeof(ButtonType),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata(ButtonType.EmptyBtn, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register(
            name: "BorderCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
