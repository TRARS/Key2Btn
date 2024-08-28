using CommunityToolkit.Mvvm.Input;
using Key2Btn.Base.CustomControlEx.MacroButtonGroupEx;
using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper.Extensions;
using Stateless;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Key2Btn.Base.CustomControlEx.MacroButtonEx
{
    public partial class cMacroButton : Button, IDebugMode
    {
        static readonly SemaphoreSlim menuClosedEvent = new(1);

        static cMacroButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cMacroButton), new FrameworkPropertyMetadata(typeof(cMacroButton)));
        }

        public cMacroButton()
        {
            StateMachineInit();
        }

        /// <summary>
        /// 转换状态
        /// </summary>
        public void ChangeState(Trigger targetState, Action<cMacroButton>? beforeStateChange = null)
        {
            if (!IsEnabled) { return; }
            beforeStateChange?.Invoke(this);
            machine.Fire(targetState);
        }

        /// <summary>
        /// 单击命令
        /// </summary>
        private void ExecuteClickCommand(bool canExecute)
        {
            if (!canExecute) { return; }
            this.ClickCommand?.Execute(null);
        }

        /// <summary>
        /// 长按(按下)命令
        /// </summary>
        private void ExecuteLongPressDownCommand(bool canExecute)
        {
            if (!canExecute) { return; }
            this.LongPressDownCommand?.Execute(null);
        }

        /// <summary>
        /// 长按(弹起)命令
        /// </summary>
        private void ExecuteLongPressUpCommand(bool canExecute)
        {
            if (!canExecute) { return; }
            this.LongPressUpCommand?.Execute(null);
        }

        /// <summary>
        /// 触发鼠标事件
        /// </summary>
        //public void RaiseMouseEvent(RoutedEvent ev)
        //{
        //    this.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, Environment.TickCount)
        //    {
        //        RoutedEvent = ev,
        //        Source = this,
        //    });
        //}
        //public void RaiseMouseEvent(RoutedEvent ev, System.Windows.Input.MouseButton btn)
        //{
        //    this.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, btn)
        //    {
        //        RoutedEvent = ev,
        //        Source = this,
        //    });
        //}

        /// <summary>
        /// 已关闭菜单
        /// </summary>
        [RelayCommand]
        private void OnMenuClosed()
        {
            menuClosedEvent.Release();
        }

        /// <summary>
        /// 关闭菜单
        /// </summary>
        [RelayCommand]
        private void OnCloseMenu()
        {
            if (this.ContextMenuIsOpen)
            {
                this.ContextMenuIsOpen = false;
            }
        }

        /// <summary>
        /// 打开菜单
        /// </summary>
        public async Task OpenContextMenu()
        {
            // 同步，防闪
            await menuClosedEvent.WaitAsync();

            this.ContextMenuIsOpen = false;
            this.ContextMenuIsOpen = true;
        }

        /// <summary>
        /// 等价判断
        /// </summary>
        public bool Equals(cMacroButton obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return (obj.PosX == this.PosX && obj.PosY == this.PosY);
        }
    }

    //属性
    public partial class cMacroButton
    {
        public ButtonGroupType ContainerType
        {
            get { return (ButtonGroupType)GetValue(ContainerTypeProperty); }
            set { SetValue(ContainerTypeProperty, value); }
        }
        public static readonly DependencyProperty ContainerTypeProperty = DependencyProperty.Register(
            name: "ContainerType",
            propertyType: typeof(ButtonGroupType),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(ButtonGroupType.UnKnown, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool ContextMenuIsOpen
        {
            get { return (bool)GetValue(ContextMenuIsOpenProperty); }
            set { SetValue(ContextMenuIsOpenProperty, value); }
        }
        public static readonly DependencyProperty ContextMenuIsOpenProperty = DependencyProperty.Register(
            name: "ContextMenuIsOpen",
            propertyType: typeof(bool),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public LeftRight LeftRightFlag
        {
            get { return (LeftRight)GetValue(LeftRightFlagProperty); }
            set { SetValue(LeftRightFlagProperty, value); }
        }
        public static readonly DependencyProperty LeftRightFlagProperty = DependencyProperty.Register(
            name: "LeftRightFlag",
            propertyType: typeof(LeftRight),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(LeftRight.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool Reflash
        {
            get { return (bool)GetValue(ReflashProperty); }
            set { SetValue(ReflashProperty, value); }
        }
        public static readonly DependencyProperty ReflashProperty = DependencyProperty.Register(
            name: "Reflash",
            propertyType: typeof(bool),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (s, e) =>
            {
                if ((bool)e.NewValue)
                {
                    ((cMacroButton)s).ChangeState(Trigger.None);
                }
            })
        );

        public bool IsDebugMode
        {
            get { return (bool)GetValue(IsDebugModeProperty); }
            set { SetValue(IsDebugModeProperty, value); }
        }
        public static readonly DependencyProperty IsDebugModeProperty = DependencyProperty.Register(
            name: "IsDebugMode",
            propertyType: typeof(bool),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool MarqueeRunning
        {
            get { return (bool)GetValue(MarqueeRunningProperty); }
            set { SetValue(MarqueeRunningProperty, value); }
        }
        public static readonly DependencyProperty MarqueeRunningProperty = DependencyProperty.Register(
            name: "MarqueeRunning",
            propertyType: typeof(bool),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (s, e) =>
            {
                if ((bool)e.NewValue)
                {
                    ((cMacroButton)s).ChangeState(Trigger.MarqueeRunning);
                }
            })
        );

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
            name: "ClickCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ICommand LongPressDownCommand
        {
            get { return (ICommand)GetValue(LongPressDownCommandProperty); }
            set { SetValue(LongPressDownCommandProperty, value); }
        }
        public static readonly DependencyProperty LongPressDownCommandProperty = DependencyProperty.Register(
            name: "LongPressDownCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ICommand LongPressUpCommand
        {
            get { return (ICommand)GetValue(LongPressUpCommandProperty); }
            set { SetValue(LongPressUpCommandProperty, value); }
        }
        public static readonly DependencyProperty LongPressUpCommandProperty = DependencyProperty.Register(
            name: "LongPressUpCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double UnitWidth
        {
            get { return (double)GetValue(UnitWidthProperty); }
            set { SetValue(UnitWidthProperty, value); }
        }
        public static readonly DependencyProperty UnitWidthProperty = DependencyProperty.Register(
            name: "UnitWidth",
            propertyType: typeof(double),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(42d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
            name: "Spacing",
            propertyType: typeof(double),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(0.5d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public int PosX
        {
            get { return (int)GetValue(PosXProperty); }
            set { SetValue(PosXProperty, value); }
        }
        public static readonly DependencyProperty PosXProperty = DependencyProperty.Register(
            name: "PosX",
            propertyType: typeof(int),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public int PosY
        {
            get { return (int)GetValue(PosYProperty); }
            set { SetValue(PosYProperty, value); }
        }
        public static readonly DependencyProperty PosYProperty = DependencyProperty.Register(
            name: "PosY",
            propertyType: typeof(int),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            name: "BackgroundColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3B3B3B")), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }
        public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(
            name: "BackgroundOpacity",
            propertyType: typeof(double),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public CornerRadius BackgroundCornerRadius
        {
            get { return (CornerRadius)GetValue(BackgroundCornerRadiusProperty); }
            set { SetValue(BackgroundCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BackgroundCornerRadiusProperty = DependencyProperty.Register(
            name: "BackgroundCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(2.5), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush OverlayBorderColor
        {
            get { return (SolidColorBrush)GetValue(OverlayBorderColorProperty); }
            set { SetValue(OverlayBorderColorProperty, value); }
        }
        public static readonly DependencyProperty OverlayBorderColorProperty = DependencyProperty.Register(
            name: "OverlayBorderColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Point RippleCenter
        {
            get { return (Point)GetValue(RippleCenterProperty); }
            set { SetValue(RippleCenterProperty, value); }
        }
        public static readonly DependencyProperty RippleCenterProperty = DependencyProperty.Register(
            name: "RippleCenter",
            propertyType: typeof(Point),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double RippleProgress
        {
            get { return (double)GetValue(RippleProgressProperty); }
            set { SetValue(RippleProgressProperty, value); }
        }
        public static readonly DependencyProperty RippleProgressProperty = DependencyProperty.Register(
            name: "RippleProgress",
            propertyType: typeof(double),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public State MachineState
        {
            get { return (State)GetValue(MachineStateProperty); }
            set { SetValue(MachineStateProperty, value); }
        }
        public static readonly DependencyProperty MachineStateProperty = DependencyProperty.Register(
            name: "MachineState",
            propertyType: typeof(State),
            ownerType: typeof(cMacroButton),
            typeMetadata: new FrameworkPropertyMetadata(State.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    //动画
    public partial class cMacroButton
    {
        // 鼠标按下/弹起动画
        private void AnimationOnMouseDown(bool canExecute, Action? onAnimationCompleted = null)
        {
            if (!canExecute) { return; }
            var storyboard = this.SetDoubleAnimation(BackgroundOpacityProperty, BackgroundOpacity, 0.5, 100);
            storyboard.Completed += (s, e) => { onAnimationCompleted?.Invoke(); };
            storyboard.Begin();
        }
        private void AnimationOnMouseUp(bool canExecute, Action? onAnimationCompleted = null)
        {
            if (!canExecute) { return; }
            var storyboard = this.SetDoubleAnimation(BackgroundOpacityProperty, BackgroundOpacity, 1.0, 100);
            storyboard.Completed += (s, e) => { onAnimationCompleted?.Invoke(); };
            storyboard.Begin();
        }

        // 鼠标按下时的进入/离开动画
        private void AnimationOnMouseEnter(bool canExecute, Action? onAnimationCompleted = null)
        {
            if (!canExecute) { return; }
            var storyboard = this.SetDoubleAnimation(BackgroundOpacityProperty, BackgroundOpacity, 0.35, 128);
            storyboard.Completed += (s, e) => { onAnimationCompleted?.Invoke(); };
            storyboard.Begin();
        }
        private void AnimationOnMouseExit(bool canExecute, Action? onAnimationCompleted = null, Action? onAnimationStarted = null)
        {
            if (!canExecute) { return; }
            var storyboard = this.SetDoubleAnimation(BackgroundOpacityProperty, BackgroundOpacity, 1.0, 256);
            storyboard.Completed += (s, e) => { onAnimationCompleted?.Invoke(); };
            storyboard.Begin();

            onAnimationStarted?.Invoke();
        }

        // 鼠标未按下时的进入/离开动画
        private void AnimationOnMouseEnterEx(bool canExecute, Action? onAnimationCompleted = null)
        {
            if (!canExecute) { return; }
            OverlayBorderColor = new SolidColorBrush();
            OverlayBorderColor.BeginColorAnimation(Colors.LightGray, Colors.Transparent, 500, true, RepeatBehavior.Forever, FillBehavior.Stop, onAnimationCompleted);
        }
        private void AnimationOnMouseExitEx(bool canExecute, Action? onAnimationCompleted = null, Action? onAnimationStarted = null)
        {
            if (!canExecute) { return; }
            OverlayBorderColor = new SolidColorBrush(OverlayBorderColor.Color);
            OverlayBorderColor.BeginColorAnimation(OverlayBorderColor.Color, Colors.Transparent, 500, false, new RepeatBehavior(1), FillBehavior.HoldEnd, onAnimationCompleted);

            onAnimationStarted?.Invoke();
        }

        // 点击动画
        private void AnimationOnClick(bool canExecute, Action? onAnimationCompleted = null, Action? onAnimationStarted = null)
        {
            if (!canExecute) { return; }
            var storyboard = this.SetDoubleAnimation(RippleProgressProperty, 0d, 1d, 768, behavior: FillBehavior.HoldEnd);
            storyboard.Completed += (s, e) => { onAnimationCompleted?.Invoke(); };
            storyboard.Begin();

            onAnimationStarted?.Invoke();
        }

        // 测试动画
        private void AnimationOnMarqueeRunning(bool canExecute, Action? onAnimationCompleted = null)
        {
            if (!canExecute) { return; }

            Color randomColor = colors[random.Next(colors.Count)];

            OverlayBorderColor = new SolidColorBrush();
            OverlayBorderColor.BeginColorAnimation(randomColor, Colors.Transparent, 640, false, new RepeatBehavior(1), FillBehavior.Stop, onAnimationCompleted);
        }
    }

    //状态机
    public partial class cMacroButton
    {
        public enum LeftRight
        {
            Left, Right, None
        }

        public enum State
        {
            None,
            MouseDown, MouseLongPress,
            MouseUp,
            MouseEnter, MouseEnterEx, MouseEnterL, MouseEnterR,
            MouseExit, MouseExitEx,
            MouseClick,
            MarqueeRunning,
        }
        public enum Trigger
        {
            None,
            MouseDown, MouseLongPress,
            MouseUp,
            MouseEnter, MouseEnterEx, MouseEnterL, MouseEnterR,
            MouseExit, MouseExitEx,
            MouseClick,
            MarqueeRunning,
        }

        private StateMachine<State, Trigger> machine;

        private Random random = new Random();
        private List<Color> colors = new List<Color>()
        {
            (Color)ColorConverter.ConvertFromString("#009fd9"),
            (Color)ColorConverter.ConvertFromString("#65b849"),
            (Color)ColorConverter.ConvertFromString("#f7b423"),
            (Color)ColorConverter.ConvertFromString("#f58122"),
            (Color)ColorConverter.ConvertFromString("#de3a3c"),
            (Color)ColorConverter.ConvertFromString("#943f96"),
        };

        void StateMachineInit()
        {
            machine = new(() => MachineState, s => MachineState = s);

            machine.OnTransitioned(t => Debug.WriteLine($"—({PosX},{PosY}) OnTransitioned: {t.Source} -> {t.Destination}"));

            machine.Configure(State.None)
                .Ignore(Trigger.None)
                .Ignore(Trigger.MouseUp)
                .Ignore(Trigger.MouseExit)
                .Ignore(Trigger.MouseExitEx)
                .Permit(Trigger.MouseDown, State.MouseDown)
                .Permit(Trigger.MouseLongPress, State.MouseLongPress)
                .Permit(Trigger.MouseEnter, State.MouseEnter)
                .Permit(Trigger.MouseEnterEx, State.MouseEnterEx)
                .Permit(Trigger.MouseClick, State.MouseClick)
                .Permit(Trigger.MarqueeRunning, State.MarqueeRunning)
                .Permit(Trigger.MouseEnterL, State.MouseEnterL)
                .Permit(Trigger.MouseEnterR, State.MouseEnterR)
                .OnEntry(t =>
                {
                    AnimationOnMouseUp(t.Source is State.MouseDown ||
                                       t.Source is State.MouseLongPress ||
                                       t.Source is State.MouseClick ||
                                       t.Source is State.MouseEnterL ||
                                       t.Source is State.MouseEnterR);
                    AnimationOnMouseExit(t.Source is State.MouseEnter);
                    AnimationOnMouseExitEx(t.Source is State.MouseEnterEx || t.Source is State.MouseLongPress);

                    LeftRightFlag = LeftRight.None;
                });

            machine.Configure(State.MouseDown)
                .SubstateOf(State.MouseEnterEx)
                .Ignore(Trigger.MouseDown)
                .Ignore(Trigger.MarqueeRunning)
                .Permit(Trigger.MouseLongPress, State.MouseLongPress)
                .Permit(Trigger.MouseUp, State.MouseUp)
                .Permit(Trigger.None, State.None)
                .OnEntry(t => { AnimationOnMouseDown(true); });

            machine.Configure(State.MouseLongPress)
                .SubstateOf(State.MouseDown)
                .Ignore(Trigger.MouseLongPress)
                .Ignore(Trigger.MarqueeRunning)
                .Permit(Trigger.MouseUp, State.MouseUp)
                .Permit(Trigger.None, State.None)
                .OnEntry(t => { AnimationOnMouseDown(true); ExecuteLongPressDownCommand(true); })
                .OnExit(() => { AnimationOnMouseExitEx(true); ExecuteLongPressUpCommand(true); });

            machine.Configure(State.MouseUp)
                .Ignore(Trigger.MouseUp)
                .Ignore(Trigger.MouseExit)
                .Ignore(Trigger.MarqueeRunning)
                .Permit(Trigger.MouseDown, State.MouseDown)
                .Permit(Trigger.MouseEnter, State.MouseEnter)
                .Permit(Trigger.MouseEnterEx, State.MouseEnterEx)
                .Permit(Trigger.None, State.None)
                .OnEntry(t =>
                {
                    AnimationOnMouseUp(true, () => { machine.Fire(Trigger.None); });
                    AnimationOnMouseExit(t.Source is State.MouseEnter);
                    AnimationOnMouseExitEx(t.Source is State.MouseEnterEx);
                });

            machine.Configure(State.MouseEnterL)
                .SubstateOf(State.MouseEnter)
                .Ignore(Trigger.MouseEnterL)
                .Permit(Trigger.MouseEnterR, State.MouseEnterR)
                .OnEntry(t => { LeftRightFlag = LeftRight.Left; });
            machine.Configure(State.MouseEnterR)
                .SubstateOf(State.MouseEnter)
                .Ignore(Trigger.MouseEnterR)
                .Permit(Trigger.MouseEnterL, State.MouseEnterL)
                .OnEntry(t => { LeftRightFlag = LeftRight.Right; });

            machine.Configure(State.MouseEnter)
                .SubstateOf(State.MouseEnterEx)
                .Ignore(Trigger.MouseEnter)
                .Ignore(Trigger.MarqueeRunning)
                .Permit(Trigger.MouseDown, State.MouseDown)
                .Permit(Trigger.MouseUp, State.MouseUp)
                .Permit(Trigger.MouseExit, State.MouseExit)
                .Permit(Trigger.None, State.None)
                .OnEntry(t => { AnimationOnMouseEnter(true); })
                .OnExit(() => { AnimationOnMouseExitEx(machine.IsInState(State.MouseEnterEx)); });

            machine.Configure(State.MouseExit)
                .Ignore(Trigger.MouseExit)
                .Ignore(Trigger.MouseDown)
                .Ignore(Trigger.MarqueeRunning)
                .Permit(Trigger.MouseEnter, State.MouseEnter)
                .Permit(Trigger.MouseEnterEx, State.MouseEnterEx)
                .Permit(Trigger.MouseClick, State.MouseClick)
                .Permit(Trigger.None, State.None)
                .OnEntry(t => { AnimationOnMouseExit(true, onAnimationStarted: () => { machine.Fire(Trigger.None); }); });

            machine.Configure(State.MouseEnterEx)
                .Ignore(Trigger.MouseEnterEx)
                .Ignore(Trigger.MarqueeRunning)
                .Permit(Trigger.MouseExitEx, State.MouseExitEx)
                .Permit(Trigger.MouseDown, State.MouseDown)
                .Permit(Trigger.MouseClick, State.MouseClick)
                .Permit(Trigger.None, State.None)
                .OnEntry(t => { AnimationOnMouseEnterEx(true); })
                .OnExit(() => { AnimationOnMouseExitEx(true); });

            machine.Configure(State.MouseExitEx)
                .Ignore(Trigger.MouseExitEx)
                .Ignore(Trigger.MouseDown)
                .Ignore(Trigger.MarqueeRunning)
                .Permit(Trigger.None, State.None)
                .OnEntry(t => { AnimationOnMouseExitEx(true, onAnimationStarted: () => { machine.Fire(Trigger.None); }); });

            machine.Configure(State.MouseClick)
                .Ignore(Trigger.MouseClick)
                .Ignore(Trigger.MarqueeRunning)
                .Permit(Trigger.None, State.None)
                .OnEntry(t => { AnimationOnClick(true, onAnimationStarted: () => { machine.Fire(Trigger.None); ExecuteClickCommand(true); }); });

            machine.Configure(State.MarqueeRunning)
                .Ignore(Trigger.MarqueeRunning)
                .Ignore(Trigger.MouseUp)
                .Ignore(Trigger.MouseExit)
                .Ignore(Trigger.MouseExitEx)
                .Ignore(Trigger.MouseDown)
                .Ignore(Trigger.MouseLongPress)
                .Ignore(Trigger.MouseEnter)
                .Ignore(Trigger.MouseEnterEx)
                .Permit(Trigger.None, State.None)
                .Permit(Trigger.MouseClick, State.MouseClick)
                .OnEntry(() => { AnimationOnMarqueeRunning(true, () => { machine.Fire(Trigger.None); }); })
                .OnExit(() => { this.MarqueeRunning = false; });
        }
    }
}
