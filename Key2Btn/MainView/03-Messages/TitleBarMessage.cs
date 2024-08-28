using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Key2Btn.MainView.Messages
{
    public class TitleBarShowHideMessage : ValueChangedMessage<string>
    {
        public TitleBarShowHideMessage(string value) : base(value)
        {
        }
    }
}
