using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper;
using Key2Btn.Base.Helper.Extensions;
using Key2Btn.MainView.Interfaces;
using Key2Btn.MainView.Messages;
using Key2Btn.MainView.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using ContainerType = Key2Btn.Base.CustomControlEx.MacroButtonGroupEx.ButtonGroupType;

namespace Key2Btn.MainView.ViewModels
{
    public partial class uClientVM : ObservableObject, IuClientVM
    {
        private IMessageBoxService _messageBox;

        [ObservableProperty]
        private bool isTesterMode;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RollDataCommand))]
        [NotifyCanExecuteChangedFor(nameof(SaveProfileCommand))]
        [NotifyCanExecuteChangedFor(nameof(LoadProfileCommand))]
        [NotifyCanExecuteChangedFor(nameof(ChangeOpacityCommand))]
        private UserInputContainer contentContainer;

        private readonly UserInputContainer MacroButtonContainer;
        private readonly UserInputContainer SimpleKeyboardContainer;

        public uClientVM(IMessageBoxService messageBox, IProfileService profileService, IActionExecutor actionInvoker)
        {
            this._messageBox = messageBox;

            this.MacroButtonContainer = new UserInputContainer(() => this.SimpleKeyboardContainer).MacroButtonContainer(profileService);
            this.SimpleKeyboardContainer = new UserInputContainer(() => this.MacroButtonContainer).SimpleKeyboardContainer();

            this.ContentContainer = this.MacroButtonContainer.LocalProfileFound ? this.MacroButtonContainer : this.SimpleKeyboardContainer;
        }
    }

    // Command
    public partial class uClientVM
    {
        [RelayCommand]
        private void OnLoaded()
        {
            new KeyboardHook().Init(hook =>
            {
                hook.OnKeyDown += (s, e) =>
                {
                    Application.Current.Dispatcher.BeginInvoke(async () =>
                    {
                        await Task.Yield();
                        WeakReferenceMessenger.Default.Send(new ModifyKeyMessage(string.Empty));
                        WeakReferenceMessenger.Default.Send(new CapsShiftMessage(string.Empty));
                    });
                };
                hook.OnKeyUp += (s, e) =>
                {
                    Application.Current.Dispatcher.BeginInvoke(async () =>
                    {
                        await Task.Yield();
                        WeakReferenceMessenger.Default.Send(new ModifyKeyMessage(string.Empty));
                        WeakReferenceMessenger.Default.Send(new CapsShiftMessage(string.Empty));
                    });
                };
                hook.Hook();
            });

            WeakReferenceMessenger.Default.Register<ChangeContainerMessage>(this, (r, m) =>
            {
                int duration = 128;
                Action act1 = async () => this.ContentContainer = (await this.ContentContainer.SwitchToNextContainer())!;
                Action<int> act2 = (t) => WeakReferenceMessenger.Default.Send(new MaskLayerFadeInMessage(t));

                WeakReferenceMessenger.Default.Send(new MaskLayerFadeOutMessage((duration, act1, act2))); // 淡出动画
            });

            WeakReferenceMessenger.Default.Register<TesterMessage>(this, (r, m) =>
            {
                ((uClientVM)r).IsTesterMode = !((uClientVM)r).IsTesterMode;
            });

            WeakReferenceMessenger.Default.Register<AddItemMessage>(this, async (r, m) =>
            {
                await ((uClientVM)r).ContentContainer.AddItem();
            });

            WeakReferenceMessenger.Default.Register<CheckContainerTypeMessage>(this, (r, m) =>
            {
                m.Reply(((uClientVM)r).ContentContainer.Type == ContainerType.MacroButtonContainer);
            });
        }

        [RelayCommand(CanExecute = nameof(CanRollData))]
        private async Task OnRollDataAsync() => await this.ContentContainer.RollData();
        private bool CanRollData() => this.ContentContainer.Type is ContainerType.MacroButtonContainer;

        [RelayCommand]
        private async Task OnChangeContainerAsync() => this.ContentContainer = await this.ContentContainer.SwitchToNextContainer() ?? this.ContentContainer;

        [RelayCommand]
        private async Task OnChangeDebugModeAsync() => await this.ContentContainer.ChangeDebugMode();

        [RelayCommand(CanExecute = nameof(CanSaveProfile))]
        private async Task OnSaveProfileAsync() => await this.ContentContainer.Save();
        private bool CanSaveProfile() => this.ContentContainer.Type is ContainerType.MacroButtonContainer;

        [RelayCommand(CanExecute = nameof(CanLoadProfile))]
        private async Task OnLoadProfileAsync() => await this.ContentContainer.Load();
        private bool CanLoadProfile() => this.ContentContainer.Type is ContainerType.MacroButtonContainer;

        [RelayCommand(CanExecute = nameof(CanChangeOpacity))]
        private async Task OnChangeOpacityAsync() => await this.ContentContainer.ChangeMacroButtonBackgroundOpacity();
        private bool CanChangeOpacity() => this.ContentContainer.Type is ContainerType.MacroButtonContainer;
    }
}
