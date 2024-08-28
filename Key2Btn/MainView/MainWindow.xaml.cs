using CommunityToolkit.Mvvm.Messaging;
using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper;
using Key2Btn.Base.Helper.Extensions;
using Key2Btn.MainView.Messages;
using Key2Btn.MainView.SubWindow;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace Key2Btn
{
    // Borderless
    public partial class MainWindow
    {
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var handle = new WindowInteropHelper(this).Handle;
            if (handle != IntPtr.Zero)
            {
                var style = (int)Win32.WindowStyles.WS_EX_NOACTIVATE;
                Win32.SetWindowLong(handle, (int)Win32.GetWindowLongIndex.GWL_EXSTYLE, style);
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(this.WindowProc));
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            var handle = new WindowInteropHelper(this).Handle;
            if (handle != IntPtr.Zero)
            {
                HwndSource.FromHwnd(handle).RemoveHook(this.WindowProc);
            }
            base.OnClosing(e);
        }

        private IntPtr WindowProc(IntPtr handle, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)Win32.WindowMessages.WM_NCHITTEST)
            {
                if (this.OnNcHitTest(handle, wParam, lParam) is nint result)
                {
                    handled = true;
                    return result;
                }
            }
            if (msg == (int)Win32.WindowMessages.WM_SIZE)
            {
                //this.LayoutRoot.Margin = new Thickness(0);
            }
            if (msg == (int)Win32.WindowMessages.WM_DPICHANGED)
            {
                //handled = true;
            }
            return IntPtr.Zero;
        }
        private IntPtr? OnNcHitTest(IntPtr handle, IntPtr wParam, IntPtr lParam)
        {
            var screenPoint = new Point((int)lParam & 0xFFFF, ((int)lParam >> 16) & 0xFFFF);
            var clientPoint = this.PointFromScreen(screenPoint);
            //if (this.GetBorderHitTest(clientPoint) is Win32.HitTestResult borderHitTest)
            //{
            //    return (IntPtr)borderHitTest;
            //}
            clientPoint.Y -= this.BorderThickness.Top;// 边框补正
            clientPoint.X -= this.BorderThickness.Left;
            if (this.GetChromeHitTest(clientPoint) is Win32.HitTestResult chromeHitTest)
            {
                return (IntPtr)chromeHitTest;
            }
            return null;
        }
        private Win32.HitTestResult? GetBorderHitTest(Point point)
        {
            if (this.WindowState != WindowState.Normal) return null;
            if (this.ResizeMode == ResizeMode.NoResize) return null;

            var 边距 = (Math.Max(this.BorderThickness.Left * 2, 4));//MainWindow.BorderThickness
            var top = (point.Y <= 边距);
            var bottom = (point.Y >= this.Height - 边距);
            var left = (point.X <= 边距);
            var right = (point.X >= this.Width - 边距);

            if (top && left) return Win32.HitTestResult.HTTOPLEFT;
            if (top && right) return Win32.HitTestResult.HTTOPRIGHT;
            if (top) return Win32.HitTestResult.HTTOP;

            if (bottom && left) return Win32.HitTestResult.HTBOTTOMLEFT;
            if (bottom && right) return Win32.HitTestResult.HTBOTTOMRIGHT;
            if (bottom) return Win32.HitTestResult.HTBOTTOM;

            if (left) return Win32.HitTestResult.HTLEFT;
            if (right) return Win32.HitTestResult.HTRIGHT;

            return null;
        }
        private Win32.HitTestResult? GetChromeHitTest(Point point)
        {
            if (this.Chrome.Visibility is Visibility.Collapsed) { return null; }

            if (VisualTreeHelper.HitTest(this.Chrome, point) is HitTestResult result)
            {
                var button = result.VisualHit.FindVisualAncestor<Button>();
                var checkbox = result.VisualHit.FindVisualAncestor<CheckBox>();
                if ((button == null || !button.IsHitTestVisible) && (checkbox == null || !checkbox.IsHitTestVisible))
                {
                    return Win32.HitTestResult.HTCAPTION;
                }
            }

            return null;
        }
    }

    // SubWindowZIndex
    public partial class MainWindow
    {
        /// <summary>
        /// 收尾
        /// </summary>
        private Action Finalizer;

        /// <summary>
        /// 调节窗体之间遮挡关系
        /// </summary>
        private void SetZIndex()
        {
            var mainLayer = this;
            var maskLayer = CreateSubWindow<MaskLayer>(window =>
            {
                // 淡出
                WeakReferenceMessenger.Default.Register<MaskLayerFadeOutMessage>(this, (r, m) =>
                {
                    var mainWindow = ((MainWindow)r);

                    var duration = m.Value.Item1;
                    var act1 = m.Value.Item2; // 切换容器
                    var act2 = m.Value.Item3; // mainWindow.FadeIn

                    // 显示MaskLayer
                    window.ShowEx(this.Dispatcher, mainWindow.CaptureToBitmapSource(), mainWindow.Left, mainWindow.Top);

                    // 隐藏MainWindow
                    var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, WindowOpacity, 0d, 1);
                    storyboard.Completed += async (s, e) =>
                    {
                        act1.Invoke(); await Task.Yield();

                        // 隐藏MaskLayer
                        window.FadeOut(this.Dispatcher, duration, () => { act2.Invoke((int)(duration * 0.66)); });
                    };
                    storyboard.Begin();
                });
                // 淡入
                WeakReferenceMessenger.Default.Register<MaskLayerFadeInMessage>(this, (r, m) =>
                {
                    var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, WindowOpacity, 1d, m.Value);
                    storyboard.Begin();
                });
            });
            var grayLayer = CreateSubWindow<GrayLayer>(window =>
            {
                // 显示/隐藏全屏截图
                WeakReferenceMessenger.Default.Register<GrayLayerTakeScreenshotMessage>(this, (r, m) =>
                {
                    if (m.Value)
                    {
                        window.ShowEx();
                    }
                    else
                    {
                        window.HideEx();
                    }
                });
                // 储存全屏截图至桌面
                WeakReferenceMessenger.Default.Register<GrayLayerSaveScreenshotMessage>(this, (r, m) =>
                {
                    window.SaveToDesktop();
                });
            });
            var gridLayer = CreateSubWindow<GridLayer>(window =>
            {
                WeakReferenceMessenger.Default.Register<GridLayerOnOffMessage>(this, (r, m) =>
                {
                    if (m.Value)
                    {
                        window.ShowEx();
                    }
                    else
                    {
                        window.HideEx();
                    }
                });
            });

            mainLayer.SetOwner(maskLayer.GetHandle()); // MainLayer -> MaskLayer
            maskLayer.SetOwner(grayLayer.GetHandle()); // MaskLayer -> GrayLayer
            grayLayer.SetOwner(gridLayer.GetHandle()); // GrayLayer -> GridLayer

            Finalizer = () =>
            {
                gridLayer.CloseEx();
                grayLayer.CloseEx();
                maskLayer.CloseEx();
                mainLayer.CloseEx();
            };
        }

        /// <summary>
        /// 创建副窗体
        /// </summary>
        private T CreateSubWindow<T>(Action<T> init) where T : Window, IWindowHelper, new()
        {
            Func<T>? creator = null;

            var windowCreatedEvent = new AutoResetEvent(false);
            var thread = new Thread(() =>
            {
                var window = new T();
                window.Closed += (s, e) => window.Dispatcher.InvokeShutdown();

                creator = () => window;

                // 在窗口创建之后，设置事件以通知主线程
                windowCreatedEvent.Set();

                Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            // 等待线程通知窗口已经创建
            windowCreatedEvent.WaitOne();
            windowCreatedEvent.Dispose();

            var window = creator?.Invoke() ?? throw new NotImplementedException();

            init.Invoke(window);

            return window;
        }
    }

    // Initializer
    public partial class MainWindow : Window, IWindowHelper
    {
        public double WindowOpacity
        {
            get { return (double)GetValue(WindowOpacityProperty); }
            set { SetValue(WindowOpacityProperty, value); }
        }
        public static readonly DependencyProperty WindowOpacityProperty = DependencyProperty.Register(
            name: "WindowOpacity",
            propertyType: typeof(double),
            ownerType: typeof(MainWindow),
            typeMetadata: new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public MainWindow()
        {
            InitializeComponent();

            helper = new WindowInteropHelper(this);

            // 重置位置
            WeakReferenceMessenger.Default.Register<WindowPosResetMessage>(this, (r, m) =>
            {
                var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, 0d, 0d, 1);
                storyboard.Completed += (s, e) =>
                {
                    ((MainWindow)r).TryMoveToPrimaryMonitor(m.Value);
                    var sb = this.SetDoubleAnimation(WindowOpacityProperty, 0d, 1d, 192);
                    sb.Begin();
                };
                storyboard.Begin();
            });

            // 最小化
            WeakReferenceMessenger.Default.Register<WindowMinimizeMessage>(this, (r, m) =>
            {
                throw new System.NotImplementedException();
            });

            // 最大化
            WeakReferenceMessenger.Default.Register<WindowMaximizeMessage>(this, (r, m) =>
            {
                throw new System.NotImplementedException();
            });

            // 关闭
            WeakReferenceMessenger.Default.Register<WindowCloseMessage>(this, (r, m) =>
            {
                // 隐藏托盘图标
                WeakReferenceMessenger.Default.Send(new TaskbarIconCloseMessage(string.Empty));

                var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, WindowOpacity, 0d, 256);
                storyboard.Completed += (s, e) => { Finalizer?.Invoke(); Environment.Exit(0); };
                storyboard.Begin();
            });

            // 激活
            WeakReferenceMessenger.Default.Register<WindowActivateMessage>(this, (r, m) =>
            {
                ((MainWindow)r).Activate();
            });

            // 显示/隐藏
            WeakReferenceMessenger.Default.Register<WindowShowHideMessage>(this, (r, m) =>
            {
                var host = ((MainWindow)r);

                if (host.WindowOpacity < 0.5)
                {
                    host.Show();
                    var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, WindowOpacity, 1d, 256);
                    storyboard.Begin();
                }
                else
                {
                    var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, WindowOpacity, 0d, 256);
                    storyboard.Completed += async (s, e) =>
                    {
                        await Task.Yield();
                        host.Hide();
                    };
                    storyboard.Begin();
                }
            });

            // 移动至主屏幕
            WeakReferenceMessenger.Default.Register<WindowMoveTo1stMonitorMessage>(this, (r, m) =>
            {
                var host = ((MainWindow)r);
                if (host.WindowOpacity < 0.5) { host.Show(); }

                var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, 0d, 0d, 1);
                storyboard.Completed += (s, e) =>
                {
                    ((MainWindow)r).TryMoveToPrimaryMonitor(m.Value);
                    var sb = this.SetDoubleAnimation(WindowOpacityProperty, 0d, 1d, 192);
                    sb.Begin();
                };
                storyboard.Begin();
            });

            // 移动至副屏幕
            WeakReferenceMessenger.Default.Register<WindowMoveTo2ndMonitorMessage>(this, (r, m) =>
            {
                var host = ((MainWindow)r);
                if (host.WindowOpacity < 0.5) { host.Show(); }

                var storyboard = this.SetDoubleAnimation(WindowOpacityProperty, 0d, 0d, 1);
                storyboard.Completed += (s, e) =>
                {
                    ((MainWindow)r).TryMoveToSecondaryMonitor(m.Value);
                    var sb = this.SetDoubleAnimation(WindowOpacityProperty, 0d, 1d, 192);
                    sb.Begin();
                };
                storyboard.Begin();
            });

            // 窗体截图
            WeakReferenceMessenger.Default.Register<WindowSaveToTransparentPngMessage>(this, async (r, m) =>
            {
                await Task.Delay(1000);

                var host = ((MainWindow)r);
                host.SaveToPng(allowTransparency: true);
            });

            // 首次载入
            this.Loaded += (s, e) =>
            {
                WeakReferenceMessenger.Default.Send(new WindowPosResetMessage(null));
            };

            //设置Z序
            SetZIndex();
        }
    }

    // Helper
    public partial class MainWindow
    {
        private WindowInteropHelper helper;

        /// <summary>
        /// 显示窗体
        /// </summary>
        public void ShowEx(params object[] optionalParams)
        {
            this.Dispatcher.Invoke(() => { this.Show(); });
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
        /// 获取窗体句柄
        /// </summary>
        public nint GetHandle() { throw new NotImplementedException(); }

        /// <summary>
        /// 设置Owner
        /// </summary>
        public void SetOwner(nint ptr)
        {
            this.Dispatcher.BeginInvoke(() => { helper.Owner = ptr; });
        }
    }
}