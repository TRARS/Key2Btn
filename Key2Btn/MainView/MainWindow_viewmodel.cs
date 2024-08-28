using Key2Btn.Base.Helper;
using Key2Btn.MainView.Interfaces;
using System.Collections.ObjectModel;

namespace Key2Btn.MainView
{
    partial class MainWindow_viewmodel : IMainWindow_viewmodel
    {
        private IMessageBoxService _messageBox;
        private IAbstractFactory<IuTitleBarVM> _titleBarFactory;
        private IAbstractFactory<IuRainbowLineVM> _rainbowLineFactory;
        private IAbstractFactory<IuClientVM> _clientFactory;
        private IAbstractFactory<ITaskbarIconVM> _taskbarIconFactory;

        public ObservableCollection<IVM> SubViewModelList { get; init; }

        public MainWindow_viewmodel(IMessageBoxService messageBox,
                                    IAbstractFactory<IuTitleBarVM> titleBarFactory,
                                    IAbstractFactory<IuRainbowLineVM> rainbowLineFactory,
                                    IAbstractFactory<IuClientVM> clientFactory,
                                    IAbstractFactory<ITaskbarIconVM> taskbarIconFactory)
        {
            _messageBox = messageBox;
            _titleBarFactory = titleBarFactory;
            _rainbowLineFactory = rainbowLineFactory;
            _clientFactory = clientFactory;
            _taskbarIconFactory = taskbarIconFactory;

            this.SubViewModelList = new()
            {
                _titleBarFactory.Create(),
                _rainbowLineFactory.Create(),
                _clientFactory.Create(),
                _taskbarIconFactory.Create()
            };
        }
    }
}
