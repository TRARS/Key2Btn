using Key2Btn.MainView.Interfaces;
using System.Collections.ObjectModel;

namespace Key2Btn.MainView
{
    public interface IMainWindow_viewmodel
    {
        ObservableCollection<IVM> SubViewModelList { get; init; }
    }
}
