using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper;
using Key2Btn.Base.Helper.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Key2Btn.MainView.SubWindow
{
    public partial class MaskLayer
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
    public partial class MaskLayer : Window, IWindowHelper
    {
        public double WindowOpacity
        {
            get { return (double)GetValue(WindowOpacityProperty); }
            set { SetValue(WindowOpacityProperty, value); }
        }
        public static readonly DependencyProperty WindowOpacityProperty = DependencyProperty.Register(
            name: "WindowOpacity",
            propertyType: typeof(double),
            ownerType: typeof(MaskLayer),
            typeMetadata: new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public MaskLayer()
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
                    //Win32_Ex_Transparency.SetTransparency(this);
                };
                storyboard.Begin();
            };

            this.Show();
        }
    }

    // Helper
    public partial class MaskLayer
    {
        private WindowInteropHelper helper;

        /// <summary>
        /// 显示窗体
        /// </summary>
        public void ShowEx(params object[] optionalParams)
        {
            var mainDispatcher = (Dispatcher)optionalParams[0];
            var bmp = (BitmapSource)optionalParams[1];
            var left = (double)optionalParams[2];
            var top = (double)optionalParams[3];

            double actualWidth = default;
            double actualHeight = default;
            PixelFormat format = default;
            int width = default;
            int height = default;
            int stride = default;
            byte[] pixelData = default;
            double dpiX = default;
            double dpiY = default;

            // 在MainUI线程读取数据
            mainDispatcher.Invoke(() =>
            {
                // 窗体尺寸
                actualWidth = bmp.Width;
                actualHeight = bmp.Height;

                // 获取像素格式和尺寸
                format = bmp.Format;
                width = bmp.PixelWidth;
                height = bmp.PixelHeight;

                // 计算字节数组长度
                stride = (width * format.BitsPerPixel + 7) / 8;
                pixelData = new byte[height * stride];

                dpiX = bmp.DpiX;
                dpiY = bmp.DpiY;

                // 从原始 BitmapSource 复制像素数据到字节数组
                bmp.CopyPixels(pixelData, stride, 0);
            });

            // 在MaskUI线程显示
            this.Dispatcher.Invoke(() =>
            {
                this.Left = left;
                this.Top = top;
                this.Width = actualWidth;
                this.Height = actualHeight;

                this.DataContext = BitmapSource.Create(width, height, dpiX, dpiY, format, null, pixelData, stride);
                this.Show();
            });
        }


        /// <summary>
        /// 隐藏窗体
        /// </summary>
        public void HideEx() { throw new NotImplementedException(); }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public void CloseEx()
        {
            this.Dispatcher.Invoke(() => { this.Close(); });
        }

        /// <summary>
        /// 淡出
        /// </summary>
        public void FadeOut(Dispatcher mainDispatcher, double duration, Action onCompleted)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, 1.0, 0d, duration);
                storyboard.Completed += async (s, e) =>
                {
                    // 清除上下文
                    this.DataContext = null;

                    // 对轴
                    await Task.Yield();

                    // 不透明度复位
                    var sb = this.SetDoubleAnimation(WindowOpacityProperty, WindowOpacity, 1d, 1);
                    sb.Completed += async (s, e) =>
                    {
                        // 隐藏
                        this.Hide(); await Task.Yield();

                        // MainWindow 淡入
                        mainDispatcher.Invoke(() => { onCompleted?.Invoke(); });
                    };
                    sb.Begin();
                };
                storyboard.Begin();
            });
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
    }
}
