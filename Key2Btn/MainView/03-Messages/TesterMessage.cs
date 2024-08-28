using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Key2Btn.MainView.Messages
{
    internal class TesterMessage : ValueChangedMessage<string>
    {
        public TesterMessage(string value) : base(value)
        {
        }
    }
}
