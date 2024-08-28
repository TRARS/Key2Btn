using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Key2Btn.MainView.Messages
{
    public class CapsShiftMessage : ValueChangedMessage<string>
    {
        public CapsShiftMessage(string value) : base(value)
        {
        }
    }
}
