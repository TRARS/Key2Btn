using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Key2Btn.Base.CustomControlExBase.SliderEx;
using Key2Btn.Base.Helper;
using Key2Btn.MainView.Messages;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WF = System.Windows.Forms;

namespace Key2Btn.MainView.ViewModels
{
    public partial class GridBtnMenuVM : ObservableObject
    {
        public GridBtnMenuVM()
        {
            Task.Run(async () =>
            {
                PenSize = 1;
                GridOpacity = 0.1;
                GridSize = 128.0;
                GridBlurRadius = 4.5;

                await Task.Delay(2000);

                this.CreateBrush();
            });
        }
    }

    // 三个slider
    public partial class GridBtnMenuVM
    {
        [ObservableProperty]
        private double penSize;

        [ObservableProperty]
        private double gridOpacity;

        [ObservableProperty]
        private double gridSize;

        [ObservableProperty]
        private double gridBlurRadius;

        partial void OnPenSizeChanged(double oldValue, double newValue)
        {
            this.PenSize = Math.Round(newValue, 2);
            this.CreateBrush();
        }

        partial void OnGridOpacityChanged(double oldValue, double newValue)
        {
            this.GridOpacity = Math.Round(newValue, 2);
            this.CreateBrush();
        }

        partial void OnGridSizeChanged(double oldValue, double newValue)
        {
            this.GridSize = Math.Round(newValue, 2);
            this.CreateBrush();
        }

        partial void OnGridBlurRadiusChanged(double oldValue, double newValue)
        {
            this.GridBlurRadius = Math.Round(newValue, 2);
            WeakReferenceMessenger.Default.Send(new GridLayerBlurRadiusMessage(GridBlurRadius));
        }

        [RelayCommand]
        private void OnSliderLoaded(object para)
        {
            if (para is System.Windows.RoutedEventArgs e)
            {
                var slider = (cSlider)e.Source;

                if (slider.IsInitialized) { return; }

                slider.Value = Math.Clamp(slider.DefalutValue, slider.Minimum, slider.Maximum);
                slider.IsInitialized = true;
            }
        }

        [RelayCommand]
        private void OnSliderPreviewMouseWheel(object para)
        {
            if (para is System.Windows.Input.MouseWheelEventArgs e)
            {
                var slider = (cSlider)e.Source;
                if (e.Delta > 0) { slider.Value = Math.Min(slider.Value + slider.TickFrequency, slider.Maximum); }
                if (e.Delta < 0) { slider.Value = Math.Max(slider.Value - slider.TickFrequency, slider.Minimum); }
            }
        }

        [RelayCommand]
        private void OnSliderPreviewMouseRightButtonUp(object para)
        {
            if (para is System.Windows.Input.MouseButtonEventArgs e)
            {
                var slider = (cSlider)e.Source;
                slider.Value = Math.Clamp(slider.DefalutValue, slider.Minimum, slider.Maximum);
            }
        }
    }

    // 四个button
    public partial class GridBtnMenuVM
    {
        [RelayCommand]
        private void OnMoveToPrimaryScreen()
        {
            WeakReferenceMessenger.Default.Send(new GridLayerMoveTo1stMonitorMessage(new Vector(0, 0)));
            this.isOnPrimaryMonitor = true;
            this.CreateBrush();
        }

        [RelayCommand]
        private void OnMoveToSecondaryScreen()
        {
            WeakReferenceMessenger.Default.Send(new GridLayerMoveTo2ndMonitorMessage(new Vector(0, 0)));
            this.isOnPrimaryMonitor = false;
            this.CreateBrush();
        }

        [RelayCommand]
        private void OnToggleTiltOrStraighten()
        {
            isSlash = !isSlash;
            this.CreateBrush();
        }

        [RelayCommand]
        private void OnToggleBlackOrWhite()
        {
            isWhitePen = !isWhitePen;
            this.CreateBrush();
        }
    }

    // 更新笔刷
    public partial class GridBtnMenuVM
    {
        private static WF.Screen? pMonitor => WindowExt.pMonitor;
        private static WF.Screen? sMonitor => WindowExt.sMonitor;

        private bool isOnPrimaryMonitor = true;
        private bool isSlash = false;
        private bool isWhitePen = false;

        private void CreateBrush()
        {
            double w, h;

            if (pMonitor is null) { return; }

            if (isOnPrimaryMonitor) //isOnPrimaryMonitor
            {
                w = pMonitor.Bounds.Width;
                h = pMonitor.Bounds.Height;
            }
            else
            {
                w = (sMonitor is null ? pMonitor : sMonitor).Bounds.Width;
                h = (sMonitor is null ? pMonitor : sMonitor).Bounds.Height;
            }

            var rect = new Rect(0, 0, w, h);
            var penColor = new SolidColorBrush(isWhitePen ? Colors.White : Colors.Black);
            var brush = CreateGridTileBrush(rect, GridSize, PenSize, isSlash, penColor, GridOpacity);

            WeakReferenceMessenger.Default.Send(new GridLayerBrushMessage((new(w, h), brush)));
        }

        /// <summary>
        /// <para>bounds: 屏幕矩形</para>
        /// <para>sideLength: 方形尺寸</para>
        /// <para>thickness: 笔头粗细</para>
        /// <para>isSlash: 是否斜线</para>
        /// <para>penColor: 画笔颜色</para>
        /// </summary>
        private DrawingBrush CreateGridTileBrush(Rect bounds, double sideLength, double penSize, bool isSlash, SolidColorBrush penColor, double opacity)
        {
            var gridColor = penColor;
            var gridThickness = penSize;
            var tileRect = new Rect(new Size(sideLength, sideLength));

            var gridTile = new DrawingBrush
            {
                Opacity = opacity,
                Stretch = Stretch.None,
                TileMode = TileMode.Tile,
                Viewport = tileRect,
                ViewportUnits = BrushMappingMode.Absolute,
                Drawing = new GeometryDrawing
                {
                    Pen = new Pen(gridColor, gridThickness),
                    Geometry = new GeometryGroup
                    {
                        Children = isSlash ?
                        new GeometryCollection //斜线
                        {
                            new LineGeometry(tileRect.TopLeft, tileRect.BottomRight),
                            new LineGeometry(tileRect.BottomLeft, tileRect.TopRight)
                        } :
                        new GeometryCollection //横竖线
                        {
                            //new LineGeometry(tileRect.TopLeft, tileRect.TopRight),
                            //new LineGeometry(tileRect.TopLeft, tileRect.BottomLeft),
                            new LineGeometry(new Point(tileRect.TopLeft.X - gridThickness, tileRect.TopLeft.Y),
                                             new Point(tileRect.TopRight.X, tileRect.TopRight.Y)),
                            new LineGeometry(new Point(tileRect.TopLeft.X, tileRect.TopLeft.Y - gridThickness),
                                             new Point(tileRect.BottomLeft.X, tileRect.BottomLeft.Y))
                        }
                    }
                }
            };

            var offsetGrid = new DrawingBrush
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top,
                Transform = new TranslateTransform(bounds.Left, bounds.Top),
                Drawing = new GeometryDrawing
                {
                    Geometry = new RectangleGeometry(new Rect(bounds.Size)),
                    Brush = gridTile
                }
            };

            offsetGrid.Freeze();

            return offsetGrid;
        }
    }
}
