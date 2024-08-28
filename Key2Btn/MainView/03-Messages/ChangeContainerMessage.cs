using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Key2Btn.MainView.Messages
{
    public class ChangeContainerMessage : ValueChangedMessage<string>
    {
        public ChangeContainerMessage(string value) : base(value)
        {
        }
    }
}
