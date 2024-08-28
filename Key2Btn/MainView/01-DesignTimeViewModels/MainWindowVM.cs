using Key2Btn.MainView.Interfaces;
using System.Collections.ObjectModel;

namespace Key2Btn.MainView.DesignTimeViewModels
{
    internal class MainWindowVM : IMainWindow_viewmodel
    {
        public ObservableCollection<IVM> SubViewModelList { get; init; } = new()
        {
            App.GetRequiredService<IuTitleBarVM>(),
            App.GetRequiredService<IuRainbowLineVM>(),
            App.GetRequiredService<IuClientVM>(),
            App.GetRequiredService<ITaskbarIconVM>()
        };
    }
}
