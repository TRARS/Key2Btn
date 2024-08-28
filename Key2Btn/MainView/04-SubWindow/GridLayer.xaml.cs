using CommunityToolkit.Mvvm.Messaging;
using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper;
using Key2Btn.Base.Helper.Extensions;
using Key2Btn.MainView.Messages;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Key2Btn.MainView.SubWindow
{
    public partial class GridLayer
    {
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var handle = new WindowInteropHelper(this).Handle;
            if (handle != IntPtr.Zero)
            {
                var style = (int)Win32.WindowStyles.WS_EX_NOACTIVATE;
                Win32.SetWindowLong(handle, (int)Win32.GetWindowLongIndex.GWL_EXSTYLE, style);
            }
        }
    }

    // Initializer
    public partial class GridLayer : Window, IWindowHelper
    {
        public double WindowOpacity
        {
            get { return (double)GetValue(WindowOpacityProperty); }
            set { SetValue(WindowOpacityProperty, value); }
        }
        public static readonly DependencyProperty WindowOpacityProperty = DependencyProperty.Register(
            name: "WindowOpacity",
            propertyType: typeof(double),
            ownerType: typeof(GridLayer),
            typeMetadata: new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double BlurRadius
        {
            get { return (double)GetValue(BlurRadiusProperty); }
            set { SetValue(BlurRadiusProperty, value); }
        }
        public static readonly DependencyProperty BlurRadiusProperty = DependencyProperty.Register(
            name: "BlurRadius",
            propertyType: typeof(double),
            ownerType: typeof(GridLayer),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Brush GridTile
        {
            get { return (Brush)GetValue(GridTileProperty); }
            set { SetValue(GridTileProperty, value); }
        }
        public static readonly DependencyProperty GridTileProperty = DependencyProperty.Register(
            name: "GridTile",
            propertyType: typeof(Brush),
            ownerType: typeof(GridLayer),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public GridLayer()
        {
            InitializeComponent();

            helper = new WindowInteropHelper(this);

            this.Left = this.Top = 0;
            this.Opacity = 0;
            this.Visibility = Visibility.Hidden;

            this.Loaded += (s, e) =>
            {
                // 防闪
                var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, WindowOpacity, 1d, 1);
                storyboard.Completed += async (s, e) =>
                {
                    await Task.Yield();
                    this.Opacity = 1;
                    Win32_Ex_Transparency.SetTransparency(this);
                };
                storyboard.Begin();
            };

            this.Show(); this.WindowSizeCheck(); this.MessagesInit();
        }

        /// <summary>
        /// 注册交互消息
        /// </summary>
        private void MessagesInit()
        {
            // 首次运行同步窗体尺寸
            var pMonitor = WindowExt.pMonitor;
            var factor = PresentationSource.FromVisual(this) is null ? 1 : 1 / PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;
            if (pMonitor is not null)
            {
                this.Width = pMonitor.Bounds.Width * factor;
                this.Height = pMonitor.Bounds.Height * factor;
            }

            WeakReferenceMessenger.Default.Register<GridLayerBrushMessage>(this, (r, m) =>
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    var factor = PresentationSource.FromVisual(this) is null ? 1 : 1 / PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;
                    var wh = m.Value.Item1;
                    var brush = m.Value.Item2;
                    this.Width = wh.X * factor;
                    this.Height = wh.Y * factor;
                    this.GridTile = brush;
                });
            });

            WeakReferenceMessenger.Default.Register<GridLayerMoveTo1stMonitorMessage>(this, (r, m) =>
            {
                this.Dispatcher.BeginInvoke(() => { this.TryMoveToPrimaryMonitor(m.Value); });
            });

            WeakReferenceMessenger.Default.Register<GridLayerMoveTo2ndMonitorMessage>(this, (r, m) =>
            {
                this.Dispatcher.BeginInvoke(() => { this.TryMoveToSecondaryMonitor(m.Value); });
            });

            WeakReferenceMessenger.Default.Register<GridLayerBlurRadiusMessage>(this, (r, m) =>
            {
                this.Dispatcher.BeginInvoke(() => { this.BlurRadius = m.Value; });
            });
        }
    }

    // Helper
    public partial class GridLayer
    {
        private WindowInteropHelper helper;
        private bool isShown;

        /// <summary>
        /// 显示窗体
        /// </summary>
        public void ShowEx(params object[] optionalParams)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                if (isShown)
                {
                    this.Hide(); isShown = false;
                }
                else
                {
                    this.Show(); isShown = true;
                }
            });
        }

        /// <summary>
        /// 隐藏窗体
        /// </summary>
        public void HideEx()
        {
            this.Dispatcher.BeginInvoke(() => { this.Hide(); isShown = false; });
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public void CloseEx()
        {
            this.Dispatcher.Invoke(() => { this.Close(); });
        }

        /// <summary>
        /// 获取窗体句柄
        /// </summary>
        public nint GetHandle()
        {
            nint ptr = 0;

            this.Dispatcher.Invoke(() => { ptr = helper.Handle; });

            return ptr;
        }

        /// <summary>
        /// 设置Owner
        /// </summary>
        public void SetOwner(nint ptr)
        {
            this.Dispatcher.BeginInvoke(() => { helper.Owner = ptr; });
        }

        /// <summary>
        /// 检查屏幕尺寸
        /// </summary>
        private void WindowSizeCheck()
        {
            if (this.ActualWidth != SystemParameters.VirtualScreenWidth || this.ActualHeight != SystemParameters.VirtualScreenHeight)
            {
                this.Left = SystemParameters.VirtualScreenLeft;//绝对值不受DPI影响
                this.Top = SystemParameters.VirtualScreenTop;
                this.Width = SystemParameters.VirtualScreenWidth;
                this.Height = SystemParameters.VirtualScreenHeight;
            }
        }
    }
}
