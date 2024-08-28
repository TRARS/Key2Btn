using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Key2Btn.Base.CustomControlEx.MacroButtonEx;
using Key2Btn.Base.CustomControlEx.MacroButtonGroupEx;
using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper;
using Key2Btn.Base.Helper.AttachedProperty;
using Key2Btn.Base.Helper.Extensions;
using Key2Btn.MainView.Interfaces;
using Key2Btn.MainView.Messages;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static Key2Btn.MainView.Models.MacroButtonInfo;
using KeyboardKey = System.Windows.Forms.Keys;

namespace Key2Btn.MainView.Models
{
    /// <summary>
    /// 按钮容器
    /// </summary>
    public partial class UserInputContainer : ObservableObject, IMacroButtonGroup<MacroButtonInfo>
    {
        private IProfileService ProfileService { get; set; }
        private ObservableCollection<MacroButtonInfo> TempContainer { get; set; } = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Message))]
        private bool debugMode;

        /// <summary>
        /// 色块不透明度
        /// </summary>
        [ObservableProperty]
        private double macroButtonBackgroundOpacity;

        /// <summary>
        /// 容器类型
        /// </summary>
        [ObservableProperty]
        private ButtonGroupType type;

        [ObservableProperty]
        private ObservableCollection<ObservableCollection<MacroButtonInfo>> itemsSourceGroup;

        private Func<UserInputContainer?>? switchToNextContainer;

        public UserInputContainer(Func<UserInputContainer?>? nextContainer = null)
        {
            switchToNextContainer = nextContainer;
            macroButtonBackgroundOpacity = 0.33;
        }
    }

    public partial class UserInputContainer
    {
        [ObservableProperty]
        private bool isLeftPress;

        [ObservableProperty]
        private bool isMouseDown;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Message))]
        private bool isLongPress;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Message))]
        private string messagePrefix;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Message))]
        private string messageSuffix;

        public string Message => $"DebugMode: {DebugMode}\n" +
                                 $"IsMouseDown: {IsMouseDown}\n" +
                                 $"IsLongPress: {IsLongPress}\n" +
                                 $"{MessagePrefix} -> {MessageSuffix}";

        [ObservableProperty]
        private Point traceLineStartPoint;
        [ObservableProperty]
        private Point traceLineEndPoint;
        [ObservableProperty]
        private double traceLineOpacity;

        [ObservableProperty]
        private bool localProfileFound;

        /// <summary>
        /// 快捷键容器
        /// </summary>
        public UserInputContainer MacroButtonContainer(IProfileService profileService)
        {
            ProfileService = profileService;
            Type = ButtonGroupType.MacroButtonContainer;

            Task.Run(async () =>
            {
                var list = await ProfileService.LoadProfile<ObservableCollection<ObservableCollection<MacroButtonInfo>>>();
                if (HasAtLeastOneMember(list))
                {
                    this.LocalProfileFound = true;
                }
                else
                {
                    this.LocalProfileFound = false;

                    // 示例
                    list = new()
                    {
                        new()
                        {
                            new(){ Name = "1", KeyCode = "1", Color = "Gray" },
                            new(){ Name = "2", KeyCode = "2", Color = "Gray" },
                            new(){ Name = "3", KeyCode = "3", Color = "Gray" },
                            new(){ Name = "超长文本1", KeyCode = "4", Color = "Gray", MaxWidthScale = 2d },
                            new(){ Name = "超长文本2", KeyCode = "5", Color = "Gray" },
                        },
                        new()
                        {
                            new(){ Name = "复制", KeyCode = "^c", Color = "#FFE40A93", Type = KeyCodeType.CombineKeys},
                            new(){ Name = "粘贴", KeyCode = "^v", Color = "#FF0998DC", Type = KeyCodeType.CombineKeys},
                            new(){ Name = "全选", KeyCode = "^a", Color = "#FF1FF4DF", Type = KeyCodeType.CombineKeys},
                            new(){ Name = "剪切", KeyCode = "^x", Color = "#FFE7AC19", Type = KeyCodeType.CombineKeys},
                        }
                    };
                }

                await this.Load(list);
            }).Wait();

            return this;
        }

        /// <summary>
        /// 键盘容器
        /// </summary>
        public UserInputContainer SimpleKeyboardContainer()
        {
            Type = ButtonGroupType.SimpleKeyboardContainer;

            var absw = 40.0; // 按键宽度
            var spacing = 1.5; // 按键间距

            this.ItemsSourceGroup = new()
            {
               new()
               {
                   new("Esc","",KeyboardKey.Escape, spacing, absw),
                   new("`", "~", KeyboardKey.Oemtilde, spacing, absw),
                   new("1", "!", KeyboardKey.D1, spacing, absw),
                   new("2", "@", KeyboardKey.D2, spacing, absw),
                   new("3", "#", KeyboardKey.D3, spacing, absw),
                   new("4", "$", KeyboardKey.D4, spacing, absw),
                   new("5", "%", KeyboardKey.D5, spacing, absw),
                   new("6", "^", KeyboardKey.D6, spacing, absw),
                   new("7", "&", KeyboardKey.D7, spacing, absw),
                   new("8", "*", KeyboardKey.D8, spacing, absw),
                   new("9", "(", KeyboardKey.D9, spacing, absw),
                   new("0", ")", KeyboardKey.D0, spacing, absw),
                   new("-", "_", KeyboardKey.OemMinus, spacing, absw),
                   new("=", "+", KeyboardKey.Oemplus, spacing, absw),
                   new("Backspace", "", KeyboardKey.Back, spacing, absw * 2)
                   {
                       PathData = Geometry.Parse("M844.373333 468.906667H282.453333L405.333333 346.026667a42.496 42.496 0 0 0 0-60.16c-8.106667-7.68-18.773333-12.373333-29.866666-12.373334-11.093333 0-22.186667 4.693333-29.866667 12.373334l-196.266667 195.84a42.496 42.496 0 0 0 0 60.16l195.84 195.84a42.496 42.496 0 1 0 60.16-60.16l-122.88-123.306667h561.92c23.466667 0 42.666667-19.2 42.666667-42.666667s-19.2-42.666667-42.666667-42.666666z".ToString(CultureInfo.InvariantCulture)),
                       PathMargin = new Thickness(3,0,0,0)
                   },
               },
               new()
               {
                   new("Tab", "", KeyboardKey.Tab, spacing, absw * (1 + 0.5) + spacing),
                   new("q", "Q", KeyboardKey.Q, spacing, absw),
                   new("w", "W", KeyboardKey.W, spacing, absw) { Color = null },
                   new("e", "E", KeyboardKey.E, spacing, absw),
                   new("r", "R", KeyboardKey.R, spacing, absw),
                   new("t", "T", KeyboardKey.T, spacing, absw),
                   new("y", "Y", KeyboardKey.Y, spacing, absw),
                   new("u", "U", KeyboardKey.U, spacing, absw),
                   new("i", "I", KeyboardKey.I, spacing, absw),
                   new("o", "O", KeyboardKey.O, spacing, absw),
                   new("p", "P", KeyboardKey.P, spacing, absw),
                   new("[", "{", KeyboardKey.OemOpenBrackets, spacing, absw),
                   new("]", "}", KeyboardKey.OemCloseBrackets, spacing, absw),
                   new(@"\", "|", KeyboardKey.OemBackslash, spacing, absw),
                   new("Del", "", KeyboardKey.Delete, spacing, absw * (2 - 0.5) - spacing),
               },
               new()
               {
                   new("Caps", "", KeyboardKey.CapsLock, spacing, absw * (1 + 1) + spacing, UseModifyKey.Yes),
                   new("a", "A", KeyboardKey.A, spacing, absw) { Color = null },
                   new("s", "S", KeyboardKey.S, spacing, absw) { Color = null },
                   new("d", "D", KeyboardKey.D, spacing, absw) { Color = null },
                   new("f", "F", KeyboardKey.F, spacing, absw),
                   new("g", "G", KeyboardKey.G, spacing, absw),
                   new("h", "H", KeyboardKey.H, spacing, absw),
                   new("j", "J", KeyboardKey.J, spacing, absw),
                   new("k", "K", KeyboardKey.K, spacing, absw),
                   new("l", "L", KeyboardKey.L, spacing, absw),
                   new(";", ":", KeyboardKey.OemSemicolon, spacing, absw),
                   new("'", "\"", KeyboardKey.OemQuotes, spacing, absw),
                   new("Enter", "", KeyboardKey.Enter, spacing, absw * (2 + 1) + spacing),
               },
               new()
               {
                   new("Shift", "", KeyboardKey.LShiftKey, spacing, absw * (1 + 1 + 0.5) + spacing * 2, UseModifyKey.Yes),
                   new("z", "Z", KeyboardKey.Z, spacing, absw),
                   new("x", "X", KeyboardKey.X, spacing, absw),
                   new("c", "C", KeyboardKey.C, spacing, absw),
                   new("v", "V", KeyboardKey.V, spacing, absw),
                   new("b", "B", KeyboardKey.B, spacing, absw),
                   new("n", "N", KeyboardKey.N, spacing, absw),
                   new("m", "M", KeyboardKey.M, spacing, absw),
                   new(",", "<", KeyboardKey.Oemcomma, spacing, absw),
                   new(".", ">", KeyboardKey.OemPeriod, spacing, absw),
                   new("/", "?", KeyboardKey.Oem2, spacing, absw),
                   new("↑", "", KeyboardKey.Up, spacing, absw) { Color = null },
                   new("Shift", "", KeyboardKey.RShiftKey, spacing, absw * (1 + 1 + 0.5), UseModifyKey.Yes),
               },
               new()
               {
                   new("Fn", "", KeyboardKey.None, spacing, absw, isEnable: false),
                   new("Ctrl", "", KeyboardKey.LControlKey, spacing, absw, UseModifyKey.Yes),
                   new("Win", "", KeyboardKey.LWin, spacing, absw, UseModifyKey.Yes)
                   {
                       PathData = Geometry.Parse("M170.752 264.704l279.765333-38.570667v270.336H170.752V264.704z m0 494.592l279.765333 38.570667v-266.965334H170.752v228.394667z m310.570667 42.666667L853.418667 853.333333v-322.432h-372.096v271.061334z m0-579.925334v274.432h372.096V170.666667l-372.096 51.370666z".ToString(CultureInfo.InvariantCulture)),
                       PathMargin = new Thickness(3,3,0,0)
                   },
                   new("Alt", "", KeyboardKey.LMenu, spacing, absw, UseModifyKey.Yes),
                   new("", "", KeyboardKey.Space, spacing, absw + absw * 4.5 + spacing * 5),
                   new("Alt", "", KeyboardKey.RMenu, spacing, absw, UseModifyKey.Yes),
                   new("Ctrl", "", KeyboardKey.RControlKey, spacing, absw, UseModifyKey.Yes),
                   new("←", "", KeyboardKey.Left, spacing, absw) { Color = null },
                   new("↓", "", KeyboardKey.Down, spacing, absw) { Color = null },
                   new("→", "", KeyboardKey.Right, spacing, absw) { Color = null },
                   new("テスト", "", null, spacing, absw * (2 - 0.5) - spacing)
                   {
                       ClickMacro = new AsyncRelayCommand(async ()=>
                       {
                           async Task ProcessItems(ObservableCollection<MacroButtonInfo> itemsSource, bool reverse)
                           {
                               if (reverse)
                               {
                                   for (var i = itemsSource.Count - 1; i >= 0; i--)
                                   {
                                       await ProcessItem(itemsSource[i]);
                                   }
                               }
                               else
                               {
                                   for (var i = 0; i < itemsSource.Count; i++)
                                   {
                                       await ProcessItem(itemsSource[i]);
                                   }
                               }
                           }
                           async Task ProcessItem(MacroButtonInfo item)
                           {
                               if (!item.IsEnable) return;
                               item.IsMarqueeActive = false;
                               item.IsMarqueeActive = true;
                               await Task.Delay((int)Math.Ceiling(32 * (item.AbsWidth / absw)));
                           }

                           if (ItemsSourceGroup?.FirstOrDefault()?.Any() is true && ItemsSourceGroup[0][0].IsMarqueeActive is false)
                           {
                               int groupIndex = 0;

                               foreach (var itemsSource in ItemsSourceGroup)
                               {
                                   await ProcessItems(itemsSource, groupIndex % 2 != 0);
                                   groupIndex++;
                               }
                           }
                       })
                   },
               },
            };

            return this;
        }

        /// <summary>
        /// 滚动数据测试
        /// </summary>
        public async Task RollData()
        {
            if (ItemsSourceGroup.Count < (2 + 1)) { return; }

            var firstItem = ItemsSourceGroup[0];

            for (int i = 0; i < ItemsSourceGroup.Count - (1 + 1); i++)
            {
                ItemsSourceGroup[i] = ItemsSourceGroup[i + 1];
            }

            ItemsSourceGroup[ItemsSourceGroup.Count - (1 + 1)] = firstItem;

            await Task.CompletedTask;
        }

        /// <summary>
        /// 进入或退出DebugMode
        /// </summary>
        public async Task ChangeDebugMode(bool? flag = null)
        {
            this.DebugMode = flag ?? !this.DebugMode;
            await Task.CompletedTask;
        }

        /// <summary>
        /// 切换至下一个容器
        /// </summary>
        public async Task<UserInputContainer?> SwitchToNextContainer()
        {
            await Task.CompletedTask;
            return switchToNextContainer?.Invoke();
        }

        /// <summary>
        /// 添加成员
        /// </summary>
        public async Task AddItem()
        {
            if (Type is not ButtonGroupType.MacroButtonContainer) { return; }

            TempContainer.Add(new() { Name = "null", Color = "#FF000000", Type = KeyCodeType.Text });

            await Task.CompletedTask;
        }

        /// <summary>
        /// 载入配置
        /// </summary>
        public async Task Load(ObservableCollection<ObservableCollection<MacroButtonInfo>> newList)
        {
            if (Type is not ButtonGroupType.MacroButtonContainer) { return; }

            TempContainer.Clear();

            this.ItemsSourceGroup = newList;
            this.ItemsSourceGroup.Add(TempContainer);

            await Task.CompletedTask;
        }
        /// <summary>
        /// 载入配置
        /// </summary>
        public async Task Load()
        {
            if (Type is not ButtonGroupType.MacroButtonContainer) { return; }

            TempContainer.Clear();

            if (await ProfileService.LoadProfile<ObservableCollection<ObservableCollection<MacroButtonInfo>>>() is { Count: > 0 } list)
            {
                this.ItemsSourceGroup = list;
                this.ItemsSourceGroup.Add(TempContainer);
            }
        }

        /// <summary>
        /// 储存配置
        /// </summary>
        public async Task Save()
        {
            if (Type is not ButtonGroupType.MacroButtonContainer) { return; }

            var filteredList = this.ItemsSourceGroup.Where(subList => subList.Count > 0).ToList();

            await ProfileService.SaveProfile(filteredList);
        }

        /// <summary>
        /// 调整按钮专属颜色不透明度
        /// </summary>
        public async Task ChangeMacroButtonBackgroundOpacity()
        {
            Debug.WriteLine($"ChangeMacroButtonBackgroundOpacity");

            if ((this.MacroButtonBackgroundOpacity += 1.0) > 2) { this.MacroButtonBackgroundOpacity -= 2; }

            await Task.CompletedTask;
        }
    }

    // 成员数量检测
    public partial class UserInputContainer
    {
        private DispatcherTimer? _pressDispatcherTimer;

        private cMacroButton? sourceBtn;
        private cMacroButton? targetBtn;
        private cMacroButton? previousBtn;// for MouseMove
        private int hitCount = 0;

        private bool HasAtLeastOneMember<T>(ObservableCollection<ObservableCollection<T>>? list)
        {
            return list?.Any(item => item?.Any() is true) is true;
        }
    }

    // 互动事件
    public partial class UserInputContainer
    {
        /// <summary>
        /// 复位
        /// </summary>
        private void Reset()
        {
            hitCount = 0;

            sourceBtn?.ChangeState(cMacroButton.Trigger.None);
            targetBtn?.ChangeState(cMacroButton.Trigger.None);
            previousBtn?.ChangeState(cMacroButton.Trigger.None);
            sourceBtn = null;
            targetBtn = null;
            previousBtn = null;

            this.TraceLineOpacity = 0;
            this.IsMouseDown = false;

            // ResetTimer
            _pressDispatcherTimer?.Stop();
            _pressDispatcherTimer = null;
            this.IsLongPress = false;
        }

        /// <summary>
        /// 打印
        /// </summary>
        private void Print(string str)
        {
            Debug.WriteLineIf(false, str);
        }

        /// <summary>
        /// 交换数据
        /// </summary>
        private void SwapMacroButton(cMacroButton source, cMacroButton target)
        {
            var temp = this.ItemsSourceGroup[source.PosY][source.PosX];
            this.ItemsSourceGroup[source.PosY][source.PosX] = this.ItemsSourceGroup[target.PosY][target.PosX];
            this.ItemsSourceGroup[target.PosY][target.PosX] = temp;
        }

        /// <summary>
        /// 移动数据
        /// </summary>
        private void MoveMacroButton(cMacroButton source, int targetLineIdx)
        {
            var srcX = source.PosX;
            var srcY = source.PosY;

            var temp = this.ItemsSourceGroup[srcY][srcX];
            this.ItemsSourceGroup[source.PosY].Remove(temp);
            this.ItemsSourceGroup[targetLineIdx].Add(temp);
        }
        /// <summary>
        /// 移动数据
        /// </summary>
        private void MoveMacroButton(cMacroButton source, cMacroButton target, string msg)
        {
            if (source.Equals(target)) { return; }

            Print($"\n==============================================={msg}");
            Print($"({source.PosX},{source.PosY}) 移动至 ({target.PosX},{target.PosY})({(this.IsLeftPress ? "左" : "右")})");

            var srcX = source.PosX;
            var srcY = source.PosY;
            var dstX = target.PosX;
            var dstY = target.PosY;

            // 条件融合
            var a = (this.IsLeftPress) ? 0 : 1;
            var b = (srcY == dstY) ? (srcX > dstX ? 0 : -1) : 0;

            // 删除，然后插入指定位置
            var item = this.ItemsSourceGroup[srcY][srcX];
            this.ItemsSourceGroup[srcY].RemoveAt(srcX);
            this.ItemsSourceGroup[dstY].Insert(Math.Min(dstX + a + b, this.ItemsSourceGroup[dstY].Count), item);

            // 强制该行所有成员动画复位
            this.ItemsSourceGroup[dstY].ForEach(x =>
            {
                x.IsReflash = false;
                x.IsReflash = true;
            });

            // 容器不使用VirtualizingStackPanel，所以不需要这样刷新来抵消成员移动到行首时产生的异常行为。
            // this.ItemsSourceGroup[dstY] = new ObservableCollection<MacroButtonInfo>(this.ItemsSourceGroup[dstY]); 

            Print($"应该删除: ({srcX},{srcY})");
            Print("===============================================\n");
        }

        /// <summary>
        /// OnHitTestResult
        /// </summary>
        private HitTestResultBehavior OnHitTestResult<T>(HitTestResult result, Action<T, Point>? onHitDetected = null) where T : DependencyObject
        {
            if (result.VisualHit.FindVisualAncestor<T>() is T btn && result is PointHitTestResult pointResult)
            {
                onHitDetected?.Invoke(btn, pointResult.PointHit);
                return HitTestResultBehavior.Stop;
            }
            return HitTestResultBehavior.Continue;
        }
        private HitTestResultBehavior OnHitTestResult<T>(HitTestResult result, string elementName, Action<T, Point>? onHitDetected = null) where T : DependencyObject
        {
            if (result.VisualHit.FindVisualAncestor<T>() is T btn && result is PointHitTestResult pointResult)
            {
                if ((btn as FrameworkElement)?.Name == elementName)
                {
                    onHitDetected?.Invoke(btn, pointResult.PointHit);
                    return HitTestResultBehavior.Stop;
                }
            }
            return HitTestResultBehavior.Continue;
        }

        /// <summary>
        /// 左键按下
        /// </summary>
        [RelayCommand]
        private void OnPreviewMouseLeftButtonDown(object para)
        {
            if (para is MouseEventArgs e)
            {
                var sender = (UIElement)e.Source;
                var pt = e.GetPosition(sender);

                VisualTreeHelper.HitTest(sender, null, result =>
                {
                    return OnHitTestResult<cMacroButton>(result, (btn, hit) =>
                    {
                        this.IsMouseDown = true;
                        this.MessagePrefix = this.MessageSuffix = "";

                        sourceBtn = btn;
                        // sourceBtn.ChangeState(cMacroButton.Trigger.MouseDown);

                        if (_pressDispatcherTimer == null)
                        {
                            _pressDispatcherTimer = new DispatcherTimer();
                            _pressDispatcherTimer.Tick += (s, e) =>
                            {
                                sourceBtn.ChangeState(cMacroButton.Trigger.MouseDown); // 给Trigger.MouseLongPress垫个起手
                                this.MessagePrefix = this.MessageSuffix = $"{sourceBtn.PosX},{sourceBtn.PosY}";

                                // 如果是快捷键容器
                                if (this.Type is ButtonGroupType.MacroButtonContainer)
                                {
                                    this.TraceLineOpacity = 1;
                                    this.TraceLineStartPoint = this.TraceLineEndPoint = pt;
                                }

                                this.IsLongPress = true; // 设置长按Flag
                                {
                                    btn.ChangeState(cMacroButton.Trigger.MouseLongPress);
                                    this.MessageSuffix = $"{btn.PosX},{btn.PosY} (LongPress-Down)";
                                }
                                _pressDispatcherTimer?.Stop();
                            };
                            _pressDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 128);
                            _pressDispatcherTimer.Start();
                        }
                    });
                }, new PointHitTestParameters(pt));

                sender.CaptureMouse();
            }
        }

        /// <summary>
        /// 左键弹起
        /// </summary>
        [RelayCommand]
        private void OnPreviewMouseLeftButtonUp(object para)
        {
            if (para is MouseEventArgs e)
            {
                var isBtnHit = false;
                var sender = (UIElement)e.Source;
                var pt = e.GetPosition(sender);

                // 尝试命中cMacroButton
                VisualTreeHelper.HitTest(sender, null, result =>
                {
                    return OnHitTestResult<cMacroButton>(result, (btn, hit) =>
                    {
                        this.IsMouseDown = false;

                        // 如果在原位弹起
                        if (btn.Equals(sourceBtn))
                        {
                            // 如果是长按
                            if (this.IsLongPress)
                            {
                                btn.ChangeState(cMacroButton.Trigger.MouseUp);
                                this.MessageSuffix = $"{btn.PosX},{btn.PosY} (LongPress-Up)";
                                Print($"OnPreviewMouseLeftButtonUp: 长按-弹起 ({btn.PosX},{btn.PosY})");
                            }
                            else
                            {
                                btn.ChangeState(cMacroButton.Trigger.MouseClick, (s) => { s.RippleCenter = hit; });
                                this.MessageSuffix = $"{btn.PosX},{btn.PosY} (Click: {hit})";
                                Print($"OnPreviewMouseLeftButtonUp: 单击 ({btn.PosX},{btn.PosY})");
                            }
                        }

                        // 如果在别处弹起
                        if (btn.Equals(sourceBtn) is false)
                        {
                            // 如果是长按
                            if (this.IsLongPress && sourceBtn is not null && (sourceBtn.IsEnabled && btn.IsEnabled))
                            {
                                // 如果是快捷键容器
                                if (this.Type is ButtonGroupType.MacroButtonContainer)
                                {
                                    this.MessageSuffix = $"{btn.PosX},{btn.PosY} (Up)";

                                    var pt = "";
                                    if (result is PointHitTestResult pointResult)
                                    {
                                        var hitPoint = pointResult.PointHit;
                                        var rect = VisualTreeHelper.GetDescendantBounds(btn);
                                        pt = $"\n({hitPoint}) on [{rect}]";
                                    }
                                    MoveMacroButton(sourceBtn, btn, pt);
                                }

                                // 如果是键盘容器 
                                if (this.Type is ButtonGroupType.SimpleKeyboardContainer)
                                {
                                    // 接弹起sourceBtn
                                    sourceBtn.ChangeState(cMacroButton.Trigger.MouseUp);
                                    this.MessageSuffix = $"{sourceBtn.PosX},{sourceBtn.PosY} (LongPress-Up)";
                                    Print($"OnPreviewMouseLeftButtonUp: 长按-弹起 ({sourceBtn.PosX},{sourceBtn.PosY})");
                                }
                            }
                        }

                        isBtnHit = true;
                    });
                }, new PointHitTestParameters(pt));

                // 未命中cMacroButton时，尝试命中Grid
                if (isBtnHit is false)
                {
                    VisualTreeHelper.HitTest(sender, null, result =>
                    {
                        // 以Grid作为精准参照物
                        return OnHitTestResult<Grid>(result, "hitbox", (grid, hit) =>
                        {
                            if (sourceBtn is not null && sourceBtn.IsEnabled && grid.FindVisualChildByName<Panel>("cvsp") is Panel cvsp)
                            {
                                if (int.TryParse($"{cvsp.GetValue(UIElementHelper.AlternationIndexAttachedProperty)}", out var lineIdx))
                                {
                                    MoveMacroButton(sourceBtn, lineIdx);
                                    Print($"OnPreviewMouseLeftButtonUp: 于容器[{lineIdx}]空白位置弹起");
                                };
                            }
                        });
                    }, new PointHitTestParameters(pt));
                }

                sender.ReleaseMouseCapture(); this.Reset();
            }
        }

        /// <summary>
        /// （左键按下时）移动
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanHitOnPreviewMouseMove))]
        private void OnPreviewMouseMove(object para)
        {
            if (para is MouseEventArgs e)
            {
                var isBtnHit = false;
                var sender = (UIElement)e.Source;
                var pt = e.GetPosition(sender);

                this.TraceLineEndPoint = pt;

                // 尝试命中cMacroButton
                VisualTreeHelper.HitTest(sender, null, result =>
                {
                    return OnHitTestResult<cMacroButton>(result, (btn, hit) =>
                    {
                        // 判断具体方位
                        var plusStr = "";
                        if (result is PointHitTestResult pointResult)
                        {
                            var hitPoint = pointResult.PointHit;
                            var rect = VisualTreeHelper.GetDescendantBounds(btn);
                            plusStr = $"\n({hitPoint}) on [{rect}]";

                            this.IsLeftPress = (hitPoint.X / rect.Width) < 0.5;
                        }

                        // 无位移
                        if (btn == sourceBtn)
                        {
                            targetBtn?.ChangeState(cMacroButton.Trigger.MouseExit);
                            targetBtn = null;
                        }
                        // 有位移
                        if (btn != sourceBtn)
                        {
                            // btn.ChangeState(cMacroButton.Trigger.MouseEnter);
                            btn.ChangeState(IsLeftPress ? cMacroButton.Trigger.MouseEnterL : cMacroButton.Trigger.MouseEnterR);
                            {
                                if (btn != targetBtn)
                                {
                                    hitCount++;
                                    targetBtn?.ChangeState(cMacroButton.Trigger.MouseExit);
                                }
                            }
                            targetBtn = btn;// 设置为当前目标
                        }

                        this.MessageSuffix = $"{btn.PosX},{btn.PosY} (Move)({hitCount})" + plusStr;

                        isBtnHit = true;
                    });
                }, new PointHitTestParameters(pt));

                // 未命中cMacroButton时，尝试命中Grid
                if (isBtnHit is false)
                {
                    VisualTreeHelper.HitTest(sender, null, result =>
                    {
                        // 以Grid作为精准参照物
                        return OnHitTestResult<Grid>(result, "hitbox", (grid, hit) =>
                        {
                            if (sourceBtn != targetBtn && grid.FindVisualChildByName<Panel>("cvsp") is Panel cvsp)
                            {
                                targetBtn?.ChangeState(cMacroButton.Trigger.MouseExit);
                            }
                        });
                    }, new PointHitTestParameters(pt));
                }

                e.Handled = true; // 阻止传递
            }
        }
        private bool CanHitOnPreviewMouseMove() => this.IsMouseDown && sourceBtn is not null &&
                                                   this.IsLongPress && this.Type is ButtonGroupType.MacroButtonContainer;

        /// <summary>
        /// （左键弹起时）移动
        /// </summary>
        /// <param name="para"></param>
        [RelayCommand(CanExecute = nameof(CanHitOnMouseMove))]
        private void OnMouseMove(object para)
        {
            if (para is MouseEventArgs e)
            {
                var isBtnHit = false;
                var sender = (UIElement)e.Source;
                var pt = e.GetPosition(sender);

                // 尝试命中cMacroButton
                VisualTreeHelper.HitTest(sender, null, result =>
                {
                    return OnHitTestResult<cMacroButton>(result, (btn, hit) =>
                    {
                        btn.ChangeState(cMacroButton.Trigger.MouseEnterEx);
                        {
                            if (btn != previousBtn)
                            {
                                previousBtn?.ChangeState(cMacroButton.Trigger.MouseExitEx);
                            }
                        }
                        previousBtn = btn;
                        isBtnHit = true;
                    });
                }, new PointHitTestParameters(pt));

                // 未命中cMacroButton时，尝试命中Grid
                if (isBtnHit is false)
                {
                    VisualTreeHelper.HitTest(sender, null, result =>
                    {
                        // 以Grid作为精准参照物
                        return OnHitTestResult<Grid>(result, "hitbox", (grid, hit) =>
                        {
                            if (previousBtn is not null && grid.FindVisualChildByName<Panel>("cvsp") is Panel cvsp)
                            {
                                previousBtn?.ChangeState(cMacroButton.Trigger.MouseExitEx);
                            }
                        });
                    }, new PointHitTestParameters(pt));
                }
            }
        }
        private bool CanHitOnMouseMove() => !this.IsMouseDown;

        /// <summary>
        /// 超出容器时
        /// </summary>
        [RelayCommand]
        private void OnContainerMouseLeave(object para)
        {
            Print("OnContainerMouseLeave");
            Reset();
        }

        /// <summary>
        /// 右键弹起
        /// </summary>
        [RelayCommand]
        private void OnPreviewMouseRightButtonUp(object para)
        {
            if (para is MouseEventArgs e)
            {
                var sender = (UIElement)e.Source;
                var pt = e.GetPosition(sender);

                VisualTreeHelper.HitTest(sender, null, result =>
                {
                    return OnHitTestResult<cMacroButton>(result, async (btn, hit) =>
                    {
                        if (btn.ContainerType is ButtonGroupType.MacroButtonContainer)
                        {
                            // 储存活动窗体的句柄
                            foregroundWindow = Win32.GetForegroundWindow();

                            // 抢夺焦点
                            WeakReferenceMessenger.Default.Send(new WindowActivateMessage("OnPreviewMouseRightButtonUp"));

                            // 弹出菜单
                            await btn.OpenContextMenu();
                        }
                    });
                }, new PointHitTestParameters(pt));
            }
        }
    }

    // 焦点转移、删除成员
    public partial class UserInputContainer
    {
        IntPtr foregroundWindow = IntPtr.Zero;

        /// <summary>
        /// 原路返还焦点给前一个窗体
        /// </summary>
        [RelayCommand]
        private void OnResetFocus(object para)
        {
            Win32.SetForegroundWindow(foregroundWindow);
        }

        /// <summary>
        /// 删除指定成员
        /// </summary>
        [RelayCommand]
        private void OnRemoveItem(object para)
        {
            if (para is not cMacroButton btn) { throw new NotImplementedException(); }

            this.ItemsSourceGroup[btn.PosY].RemoveAt(btn.PosX);

            Debug.WriteLine($"OnRemoveItem: '{((dynamic)btn.DataContext).Name}'({btn.PosX},{btn.PosY})");
        }
    }
}
