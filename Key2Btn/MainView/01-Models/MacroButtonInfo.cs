using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper;
using Key2Btn.Base.Helper.ExClass;
using Key2Btn.MainView.Interfaces;
using Key2Btn.MainView.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using InputKey = System.Windows.Input.Key;
using KeyboardKey = System.Windows.Forms.Keys;

namespace Key2Btn.MainView.Models
{
    /// <summary>
    /// 按钮模型
    /// </summary>
    public partial class MacroButtonInfo : ObservableValidator
    {
        public enum UseModifyKey
        {
            Yes, No
        }

        private readonly IStringFactoryService stringFactoryService = App.GetRequiredService<IStringFactoryService>();
        private readonly IActionExecutor actionExecutor = App.GetRequiredService<IActionExecutor>();
        private string specialKey = string.Empty;
        private bool taskFlag = false;
        private CancellationTokenSource? cts;
        private bool ctsDisposed;

        /// <summary>
        /// 快捷键按键
        /// </summary>
        public MacroButtonInfo()
        {
            _ = new JsonIgnoreAttribute(); // 用意维持对 System.Text.Json.Serialization 的引用

            this.Name = string.Empty;
            this.KeyCode = string.Empty;
            this.Color = "#00000000";
            this.MaxWidthScale = -1;
            this.ActionKey = string.Empty;
            this.ActionPacketList = new List<ActionPacket>([new ActionPacket(), .. actionExecutor.ActionKeyList]);
            this.Type = KeyCodeType.Text;
            //
            this.IsEnable = true;
            this.AbsWidth = -1;
            this.KeySpacing = -1;
            this.Key = string.Empty;
            this.ShiftKey = string.Empty;

            this.ClickMacro = new(async () =>
            {
                await Task.Yield();
                var input = KeyCode.ToLower();

                // 空字符不予操作
                if (string.IsNullOrEmpty(input)) { return; }

                // 使用自定义Action则视为放弃使用SendKeys
                if (string.IsNullOrEmpty(ActionKey) is false)
                {
                    await Task.Run(() => { actionExecutor.Invoke(ActionKey); });

                    return;
                }

                // 作字符串
                if (Type is KeyCodeType.Text)
                {
                    for (int i = 0; i < input.Length; i++)
                    {
                        var @char = input.Substring(i, 1);
                        var keys = (@char == " ") ? @char : $"{{{@char}}}";
                        SendKeys.SendWait(keys);
                    }

                    return;
                }

                // 作组合键
                if (Type is KeyCodeType.CombineKeys)
                {
                    try
                    {
                        SendKeys.SendWait(input);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ClickMacro error: {ex.Message}");

                        for (int i = 0; i < input.Length; i++)
                        {
                            var @char = input.Substring(i, 1);
                            var keys = (@char == " ") ? @char : $"{{{@char}}}";
                            SendKeys.SendWait(keys);
                        }
                    }

                    return;
                }
            });
            this.LongPressDownMacro = null;
            this.LongPressUpMacro = null;
        }

        /// <summary>
        /// 键盘按键
        /// </summary>
        public MacroButtonInfo(string key, string shiftKey, KeyboardKey? keyEnum, double keySpacing, double absWidth, UseModifyKey useModifyKey = UseModifyKey.No, bool isEnable = true)
        {
            specialKey = stringFactoryService.GenerateRandomString(32);

            this.Name = string.Empty;
            this.KeyCode = string.Empty;
            this.Color = "#00000000";
            this.MaxWidthScale = -1;
            this.ActionKey = string.Empty;
            this.ActionPacketList = null;
            this.Type = KeyCodeType.Unknown;
            //
            this.IsEnable = isEnable;
            this.AbsWidth = absWidth;
            this.KeySpacing = keySpacing;
            this.Key = key;
            this.ShiftKey = shiftKey;

            bool isModifyKey = keyEnum is not null && IsModifyKey(keyEnum.Value);
            bool isCapsLock = keyEnum is not null && IsCapsLock(keyEnum.Value);

            // Shift Ctrl Alt
            if (isModifyKey && useModifyKey is UseModifyKey.Yes)
            {
                //WinFormKey2WpfKey.TryGetValue(keyEnum!.Value, out var mKey);
                var mKey = KeyInterop.KeyFromVirtualKey((int)(keyEnum!.Value));

                this.ClickMacro = new(async () =>
                {
                    if (keyEnum is null) { return; }
                    SendKBMInput.KeyDownOnce(!Keyboard.IsKeyDown(mKey), [keyEnum.Value]);
                    await Task.CompletedTask;
                });
                this.LongPressDownMacro = null;
                this.LongPressUpMacro = null;

                WeakReferenceMessenger.Default.Register<ModifyKeyMessage>(this, (r, m) =>
                {
                    ((MacroButtonInfo)r).IsModifyKeyDown = Keyboard.IsKeyDown(mKey);
                });

                return;
            }

            // CapsLock
            if (isCapsLock)
            {
                //WinFormKey2WpfKey.TryGetValue(keyEnum!.Value, out var mKey);
                var mKey = KeyInterop.KeyFromVirtualKey((int)(keyEnum!.Value));

                this.ClickMacro = new(async () =>
                {
                    if (keyEnum is null) { return; }
                    SendKBMInput.SendKeyBoardClick(keyEnum.Value);
                    await Task.CompletedTask;
                });
                this.LongPressDownMacro = null;
                this.LongPressUpMacro = null;

                WeakReferenceMessenger.Default.Register<ModifyKeyMessage>(this, (r, m) =>
                {
                    ((MacroButtonInfo)r).IsModifyKeyDown = Keyboard.IsKeyToggled(mKey);
                });

                return;
            }

            // NormalKey
            if (keyEnum is not null)
            {
                this.ClickMacro = new AsyncRelayCommand(async () =>
                {
                    if (keyEnum is null) { return; }
                    SendKBMInput.SendKeyBoardClick(keyEnum.Value);
                    await Task.CompletedTask;
                });
                this.LongPressDownMacro = new AsyncRelayCommand(async () =>
                {
                    if (keyEnum is null) { return; }
                    if (taskFlag is false)
                    {
                        taskFlag = true;
                        await Task.Run(async () =>
                        {
                            ctsDisposed = false;
                            using (cts = new())
                            {
                                try
                                {
                                    do
                                    {
                                        await Task.Delay(32, cts.Token); // 访问KeyDownEx()的频率不用太高
                                        SendKBMInput.KeyDownEx(this.specialKey, true, [keyEnum.Value], true, 384, 48); //长按384ms激活连发，连发时按下/弹起间隔48ms
                                    }
                                    while (cts.Token.IsCancellationRequested is false);
                                }
                                catch { }
                            }
                            ctsDisposed = true;
                        }).ContinueWith(_ =>
                        {
                            if (keyEnum is not null)
                            {
                                SendKBMInput.KeyDownEx(this.specialKey, false, [keyEnum.Value], false);
                            }
                            cts = null;
                            taskFlag = false;
                        });
                    }
                });
                this.LongPressUpMacro = new AsyncRelayCommand(async () =>
                {
                    if (keyEnum is null) { return; }
                    if (!ctsDisposed) { cts?.Cancel(); } // 弹起时取消上面的Task
                    await Task.CompletedTask;
                });

                if (IsAlphabetKey(keyEnum.Value))
                {
                    IsAlphabet = true;
                }

                WeakReferenceMessenger.Default.Register<CapsShiftMessage>(this, (r, m) =>
                {
                    ((MacroButtonInfo)r).IsCapsActive = Keyboard.IsKeyToggled(InputKey.CapsLock);
                    ((MacroButtonInfo)r).IsShiftActive = Keyboard.IsKeyDown(InputKey.LeftShift) || Keyboard.IsKeyDown(InputKey.RightShift);
                });
            }
        }

        // 一些验证
        private static bool IsValidColorCode(string input)
        {
            string pattern = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
        private static bool IsCapsLock(KeyboardKey key)
        {
            //return WinFormKey2WpfKey.ContainsKey(key) && WinFormKey2WpfKey[key] is InputKey.CapsLock;
            return KeyInterop.KeyFromVirtualKey((int)key) is InputKey.CapsLock;
        }
        private static bool IsAlphabetKey(KeyboardKey key)
        {
            return key >= KeyboardKey.A && key <= KeyboardKey.Z;
        }
        private static bool IsModifyKey(KeyboardKey key)
        {
            return key == KeyboardKey.ShiftKey || //Shift
                   key == KeyboardKey.LShiftKey ||
                   key == KeyboardKey.RShiftKey ||
                   key == KeyboardKey.Control || // Ctrl
                   key == KeyboardKey.ControlKey ||
                   key == KeyboardKey.LControlKey ||
                   key == KeyboardKey.RControlKey ||
                   key == KeyboardKey.Alt || // Alt
                   key == KeyboardKey.Menu ||
                   key == KeyboardKey.LMenu ||
                   key == KeyboardKey.RMenu ||
                   key == KeyboardKey.LWin || // Win
                   key == KeyboardKey.RWin;
        }

        //private static Dictionary<KeyboardKey, InputKey> WinFormKey2WpfKey = new()
        //{
        //    //{ KeyboardKey.Capital, InputKey.Capital }, // Capital = CapsLock
        //    { KeyboardKey.CapsLock, InputKey.CapsLock },

        //    { KeyboardKey.ShiftKey, InputKey.LeftShift },
        //    { KeyboardKey.LShiftKey, InputKey.LeftShift },
        //    { KeyboardKey.RShiftKey, InputKey.RightShift },
        //    { KeyboardKey.ControlKey, InputKey.LeftCtrl },
        //    { KeyboardKey.LControlKey, InputKey.LeftCtrl },
        //    { KeyboardKey.RControlKey, InputKey.RightCtrl },
        //    { KeyboardKey.Alt, InputKey.LeftAlt },
        //    { KeyboardKey.Menu, InputKey.LeftAlt },
        //    { KeyboardKey.LMenu, InputKey.LeftAlt },
        //    { KeyboardKey.RMenu, InputKey.RightAlt },
        //    { KeyboardKey.LWin, InputKey.LWin },
        //    { KeyboardKey.RWin, InputKey.RWin }
        //};
    }

    public partial class MacroButtonInfo : IMacroButtonInfo
    {
        /// <summary>
        /// 按钮名字
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(Name))]
        [property: JsonPropertyOrderAttribute(0)]
        private string name;

        /// <summary>
        /// 按键码, 详见 <see href="https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys.send?view=windowsdesktop-8.0"/>
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(KeyCode))]
        [property: JsonPropertyOrderAttribute(1)]
        private string keyCode;

        /// <summary>
        /// 色块颜色
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(Color))]
        [property: JsonPropertyOrderAttribute(2)]
        private string color = "#FF000000";

        /// <summary>
        /// 最大缩放比例
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(MaxWidthScale))]
        [property: JsonPropertyOrderAttribute(3)]
        private double maxWidthScale = double.MinValue;

        /// <summary>
        /// 若该值非空，则点击按钮时无视KeyCode，并通过该Key执行对应的Action
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(ActionKey))]
        [property: JsonPropertyOrderAttribute(4)]
        private string actionKey;

        /// <summary>
        /// KeyCode类型
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(Type))]
        [property: JsonPropertyOrderAttribute(4)]
        private KeyCodeType type = KeyCodeType.Unknown;

        /// <summary>
        /// 专用于令按钮动画复位
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(IsReflash))]
        [property: JsonIgnore]
        private bool isReflash;

        /// <summary>
        /// 从dll中载入的ActionKey集合
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(ActionPacketList))]
        [property: JsonIgnore]
        private IEnumerable<ActionPacket> actionPacketList;

        /// <summary>
        /// 单击时执行的命令
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(ClickMacro))]
        [property: JsonIgnore]
        private AsyncRelayCommand clickMacro;

        /// <summary>
        /// 长按按下时执行的命令
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(LongPressDownMacro))]
        [property: JsonIgnore]
        private AsyncRelayCommand longPressDownMacro;

        /// <summary>
        /// 长按弹起时执行的命令
        /// </summary>
        [ObservableProperty]
        [property: JsonPropertyName(nameof(LongPressUpMacro))]
        [property: JsonIgnore]
        private AsyncRelayCommand longPressUpMacro;

        partial void OnColorChanged(string? oldValue, string newValue)
        {
            if ((IsValidColorCode($"{newValue}")) is false)
            {
                this.Color = TypeDescriptor.GetConverter(typeof(Color)).ConvertToString(Colors.Black) ?? "#FFFFFFFF";
            }
        }
        partial void OnMaxWidthScaleChanged(double oldValue, double newValue)
        {
            if (newValue <= 1.0) { this.MaxWidthScale = 1.0; }
        }
    }

    public partial class MacroButtonInfo : IKeyboardButtonInfo
    {
        [ObservableProperty]
        [property: JsonPropertyName(nameof(IsEnable))]
        [property: JsonIgnore]
        private bool isEnable = true;

        [ObservableProperty]
        [property: JsonPropertyName(nameof(AbsWidth))]
        [property: JsonIgnore]
        private double absWidth = double.MinValue;

        [ObservableProperty]
        [property: JsonPropertyName(nameof(KeySpacing))]
        [property: JsonIgnore]
        private double keySpacing = double.MinValue;

        [ObservableProperty]
        [property: JsonPropertyName(nameof(Key))]
        [property: JsonIgnore]
        private string key;

        [ObservableProperty]
        [property: JsonPropertyName(nameof(ShiftKey))]
        [property: JsonIgnore]
        private string shiftKey;

        [ObservableProperty]
        [property: JsonPropertyName(nameof(IsModifyKeyDown))]
        [property: JsonIgnore]
        private bool isModifyKeyDown;

        [ObservableProperty]
        [property: JsonPropertyName(nameof(IsAlphabet))]
        [property: JsonIgnore]
        private bool isAlphabet;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCapsOrShiftActive))]
        [property: JsonPropertyName(nameof(IsCapsActive))]
        [property: JsonIgnore]
        private bool isCapsActive;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCapsOrShiftActive))]
        [property: JsonPropertyName(nameof(IsShiftActive))]
        [property: JsonIgnore]
        private bool isShiftActive;

        // 
        public bool IsCapsOrShiftActive => (Convert.ToInt32(IsCapsActive) + Convert.ToInt32(IsShiftActive)).Equals(1);

        [ObservableProperty]
        [property: JsonPropertyName(nameof(IsMarqueeActive))]
        [property: JsonIgnore]
        private bool isMarqueeActive;

        [ObservableProperty]
        [property: JsonPropertyName(nameof(PathData))]
        [property: JsonIgnore]
        private Geometry pathData;

        [ObservableProperty]
        [property: JsonPropertyName(nameof(PathMargin))]
        [property: JsonIgnore]
        private Thickness pathMargin;

        partial void OnAbsWidthChanged(double oldValue, double newValue)
        {
            if (newValue <= 10.0) { this.AbsWidth = 10; }
        }
        partial void OnKeySpacingChanged(double oldValue, double newValue)
        {
            if (newValue <= 0) { this.KeySpacing = 0; }
        }
    }
}
