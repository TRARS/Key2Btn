using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Key2Btn.MainView.Interfaces;
using Key2Btn.MainView.Messages;
using System.Threading.Tasks;
using System.Windows;

namespace Key2Btn.MainView.ViewModels
{
    public partial class uTitleBarVM : ObservableObject, IuTitleBarVM
    {
        private IMessageBoxService _messageBox;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private Visibility visibility;

        [ObservableProperty]
        private GridBtnMenuVM gridBtnMenuVM;

        public uTitleBarVM(IMessageBoxService messageBox, GridBtnMenuVM gridBtnMenuVM)
        {
            this._messageBox = messageBox;

            this.Title = $"Key2Btn ({System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location):yyyy-MM-dd HH:mm:ss})";

            this.GridBtnMenuVM = gridBtnMenuVM;

            WeakReferenceMessenger.Default.Register<TitleBarShowHideMessage>(this, (r, m) =>
            {
                var host = ((uTitleBarVM)r);
                host.Visibility = host.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            });
        }
    }

    public partial class uTitleBarVM
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
        private bool isMacroButtonContainer;

        [RelayCommand]
        private async Task OnLoadedAsync()
        {
            IsMacroButtonContainer = await WeakReferenceMessenger.Default.Send(new CheckContainerTypeMessage());
        }

        [RelayCommand]
        private void OnSaveMainWindowScreenshot()
        {
            WeakReferenceMessenger.Default.Send(new WindowSaveToTransparentPngMessage("OnSaveMainWindowScreenshot"));
        }

        [RelayCommand]
        private void OnSaveScreenshot()
        {
            WeakReferenceMessenger.Default.Send(new GrayLayerSaveScreenshotMessage("OnSaveScreenshot"));
        }
        [RelayCommand]
        private void OnShowScreenshot()
        {
            WeakReferenceMessenger.Default.Send(new GrayLayerTakeScreenshotMessage(true));
        }
        [RelayCommand]
        private void OnHideScreenshot()
        {
            WeakReferenceMessenger.Default.Send(new GrayLayerTakeScreenshotMessage(false));
        }


        [RelayCommand(CanExecute = nameof(CanAddItem))]
        private void OnAddItem()
        {
            WeakReferenceMessenger.Default.Send(new AddItemMessage("OnAddItem"));
        }
        private bool CanAddItem() => IsMacroButtonContainer;

        [RelayCommand]
        private async Task OnChangeContainerAsync()
        {
            WeakReferenceMessenger.Default.Send(new ChangeContainerMessage("OnChangeContainerAsync"));
            await Task.Delay(128);
            IsMacroButtonContainer = await WeakReferenceMessenger.Default.Send(new CheckContainerTypeMessage());
        }

        [RelayCommand]
        private void OnTester()
        {
            WeakReferenceMessenger.Default.Send(new TesterMessage("OnTester"));
        }

        [RelayCommand]
        private void OnGridLayerOnOff()
        {
            WeakReferenceMessenger.Default.Send(new GridLayerOnOffMessage(true));
        }

        [RelayCommand]
        private void OnResetPos()
        {
            WeakReferenceMessenger.Default.Send(new WindowPosResetMessage(null));
        }

        [RelayCommand]
        private void OnMinimize(object para)
        {
            WeakReferenceMessenger.Default.Send(new WindowMinimizeMessage("OnMinimize"));
        }

        [RelayCommand]
        private void OnMaximize(object para)
        {
            WeakReferenceMessenger.Default.Send(new WindowMaximizeMessage("OnMaximize"));
        }

        [RelayCommand]
        private void OnClose()
        {
            WeakReferenceMessenger.Default.Send(new WindowCloseMessage("OnClose"));
        }
    }
}
