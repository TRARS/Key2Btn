using Key2Btn.MainView.Interfaces;

namespace Key2Btn.MainView.Services
{
    internal class MessageBoxService : IMessageBoxService
    {
        public void Show(string message)
        {
            System.Windows.MessageBox.Show(message);
        }
    }
}
