using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Key2Btn.MainView.Interfaces;
using Key2Btn.MainView.Messages;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Key2Btn.MainView.ViewModels
{
    public partial class TaskbarIconVM : ObservableObject, ITaskbarIconVM
    {
        private bool isInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());
        private IMessageBoxService _messageBox;

        [ObservableProperty]
        private string toolTipText;

        [ObservableProperty]
        private Visibility visibility;

        [ObservableProperty]
        private bool contextMenuIsOpen;

        public TaskbarIconVM(IMessageBoxService messageBox)
        {
            this._messageBox = messageBox;

            this.ToolTipText = "Left-click to show window, right-click for menu";
            this.Visibility = isInDesignMode ? Visibility.Collapsed : Visibility.Visible;

            WeakReferenceMessenger.Default.Register<TaskbarIconCloseMessage>(this, (r, m) =>
            {
                ((TaskbarIconVM)r).Visibility = Visibility.Collapsed;
            });
        }
    }

    public partial class TaskbarIconVM
    {
        readonly SemaphoreSlim menuClosedEvent = new(1);

        [RelayCommand]
        private void OnMenuClosed()
        {
            menuClosedEvent.Release();
        }

        [RelayCommand]
        private void OnLeftClick()
        {
            WeakReferenceMessenger.Default.Send(new WindowShowHideMessage(string.Empty));
        }

        [RelayCommand]
        private async Task OnRightClickAsync()
        {
            // 同步，防闪
            await menuClosedEvent.WaitAsync();

            // 抢夺焦点
            WeakReferenceMessenger.Default.Send(new WindowActivateMessage(string.Empty));

            // 弹出菜单
            ContextMenuIsOpen = false;
            ContextMenuIsOpen = true;
        }

        [RelayCommand]
        private void OnShowHideApp()
        {
            WeakReferenceMessenger.Default.Send(new WindowShowHideMessage(string.Empty));
        }

        [RelayCommand]
        private void OnShowHideTitleBar()
        {
            WeakReferenceMessenger.Default.Send(new TitleBarShowHideMessage(string.Empty));
        }

        [RelayCommand]
        private void OnMoveTo1stMonitor()
        {
            WeakReferenceMessenger.Default.Send(new WindowMoveTo1stMonitorMessage(null));
        }

        [RelayCommand]
        private void OnMoveTo2ndMonitor()
        {
            WeakReferenceMessenger.Default.Send(new WindowMoveTo2ndMonitorMessage(null));
        }

        [RelayCommand]
        private void OnExitApp()
        {
            WeakReferenceMessenger.Default.Send(new WindowCloseMessage(string.Empty));
        }
    }
}
