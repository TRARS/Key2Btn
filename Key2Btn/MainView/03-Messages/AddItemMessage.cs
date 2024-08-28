using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Key2Btn.MainView.Messages
{
    public class AddItemMessage : ValueChangedMessage<string>
    {
        public AddItemMessage(string value) : base(value)
        {
        }
    }
}
