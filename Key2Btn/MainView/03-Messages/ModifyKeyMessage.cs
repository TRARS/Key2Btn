using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Key2Btn.MainView.Messages
{
    public class ModifyKeyMessage : ValueChangedMessage<string>
    {
        public ModifyKeyMessage(string value) : base(value)
        {
        }
    }
}
