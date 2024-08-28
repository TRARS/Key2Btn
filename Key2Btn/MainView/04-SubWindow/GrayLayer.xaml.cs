using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper;
using Key2Btn.Base.Helper.Extensions;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SD = System.Drawing;
using WF = System.Windows.Forms;

namespace Key2Btn.MainView.SubWindow
{
    public partial class GrayLayer
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
    public partial class GrayLayer : Window, IWindowHelper
    {
        public double WindowOpacity
        {
            get { return (double)GetValue(WindowOpacityProperty); }
            set { SetValue(WindowOpacityProperty, value); }
        }
        public static readonly DependencyProperty WindowOpacityProperty = DependencyProperty.Register(
            name: "WindowOpacity",
            propertyType: typeof(double),
            ownerType: typeof(GrayLayer),
            typeMetadata: new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public GrayLayer()
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
    }

    // Helper
    public partial class GrayLayer
    {
        private WindowInteropHelper helper;
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// 显示窗体
        /// </summary>
        public void ShowEx(params object[] optionalParams)
        {
            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Task.Run(async () =>
            {
                var startTime = DateTime.Now;
                while (token.IsCancellationRequested is false)
                {
                    var diffTime = DateTime.Now - startTime;
                    if (diffTime.TotalMilliseconds > 256)
                    {
                        await this.Dispatcher.BeginInvoke(() =>
                        {
                            this.ShowScreenshot(); this.Show();

                            if (token.IsCancellationRequested) { return; }

                            var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, WindowOpacity, 1d, 1);
                            storyboard.Completed += (s, e) => { tokenSource.Cancel(); };
                            storyboard.Begin();
                        });
                        return;
                    }
                    await Task.Delay(16, token);
                }
            }, token);
        }

        /// <summary>
        /// 隐藏窗体
        /// </summary>
        public void HideEx()
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                tokenSource.Cancel();

                var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, WindowOpacity, 0d, 1);
                storyboard.Completed += async (s, e) =>
                {
                    await Task.Delay(192);
                    this.Hide();
                };
                storyboard.Begin();
            });
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
        /// 储存截图
        /// </summary>
        public void SaveToDesktop()
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                if (WindowOpacity == 1)
                {
                    this.SaveToPng();
                    Debug.WriteLine("已保存截图");
                }
                else
                {
                    Debug.WriteLine("截图未就绪");
                }
            });
        }

        private void ShowScreenshot()
        {
            //尺寸检测
            this.WindowSizeCheck();

            //获取截图
            SD.Rectangle rc = WF.SystemInformation.VirtualScreen;
            using (SD.Bitmap _bmp = new SD.Bitmap(rc.Width, rc.Height, SD.Imaging.PixelFormat.Format32bppArgb))
            {
                IntPtr hBitmap = _bmp.GetHbitmap();
                using (SD.Graphics memoryGrahics = SD.Graphics.FromImage(_bmp))
                {
                    memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, SD.CopyPixelOperation.SourceCopy);
                }

                #region wpf部分
                {
                    //缓解内存泄露（由于固定了WriteableBitmap尺寸，导致屏幕数量变动时不方便处理）
                    Func<WriteableBitmap, SD.Bitmap, bool> SizeCheck = new Func<WriteableBitmap, SD.Bitmap, bool>((wb, bitmap) =>
                    {
                        var ws = wb.PixelWidth;
                        var hs = wb.PixelHeight;
                        var wt = bitmap.Width;
                        var ht = bitmap.Height;
                        return (ws != wt || hs != ht);
                    });

                    if (this.DataContext == null || SizeCheck.Invoke((WriteableBitmap)this.DataContext, _bmp))
                    {
                        //尽量只创建一次
                        this.DataContext = new WriteableBitmap(_bmp.Width, _bmp.Height, 96, 96, PixelFormats.Pbgra32, null);
                    }

                    //CopyFrom
                    new Action<WriteableBitmap, SD.Bitmap>((wb, bitmap) =>
                    {
                        if (wb == null) throw new ArgumentNullException(nameof(wb));
                        if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

                        var ws = wb.PixelWidth;
                        var hs = wb.PixelHeight;
                        var wt = bitmap.Width;
                        var ht = bitmap.Height;
                        if (ws != wt || hs != ht)
                        {
                            //throw new ArgumentException($"只支持相同尺寸图片的复制。{ws}{hs} <> {wt}{ht}");
                            Console.WriteLine($"{ws} <> {wt}");
                            bitmap.Dispose();
                            return;
                        }

                        var width = ws;
                        var height = hs;
                        var bytes = ws * hs * wb.Format.BitsPerPixel / 8;

                        var rBitmapData = bitmap.LockBits(new SD.Rectangle(0, 0, width, height),
                                                          SD.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

                        wb.Lock();//解锁
                        unsafe
                        {
                            Buffer.MemoryCopy(rBitmapData.Scan0.ToPointer(), wb.BackBuffer.ToPointer(), bytes, bytes);
                        }
                        wb.AddDirtyRect(new Int32Rect(0, 0, width, height));
                        wb.Unlock();

                        bitmap.UnlockBits(rBitmapData);
                        bitmap.Dispose();
                    }).Invoke((WriteableBitmap)this.DataContext, _bmp);
                }
                #endregion

                Win32.DeleteObject(hBitmap);
            }
        }
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

    public partial class GrayLayer
    {
        private void MessagesInit()
        {

        }
    }
}
