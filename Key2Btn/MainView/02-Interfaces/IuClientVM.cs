using Key2Btn.MainView.Models;

namespace Key2Btn.MainView.Interfaces
{
    public interface IuClientVM : IVM
    {
        bool IsTesterMode { get; set; }
        UserInputContainer ContentContainer { get; set; }
    }
}