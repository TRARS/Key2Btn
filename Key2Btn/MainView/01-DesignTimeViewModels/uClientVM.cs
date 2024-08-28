using Key2Btn.MainView.Interfaces;
using Key2Btn.MainView.Models;

namespace Key2Btn.MainView.DesignTimeViewModels
{
    internal class uClientVM : IuClientVM
    {
        public UserInputContainer ContentContainer { get; set; } = new UserInputContainer().SimpleKeyboardContainer();
        public bool IsTesterMode { get; set; } = true;
    }
}
