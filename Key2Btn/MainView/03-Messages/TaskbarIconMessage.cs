using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Key2Btn.MainView.Messages
{
    public class TaskbarIconOpenMessage : ValueChangedMessage<string>
    {
        public TaskbarIconOpenMessage(string value) : base(value)
        {
        }
    }

    public class TaskbarIconCloseMessage : ValueChangedMessage<string>
    {
        public TaskbarIconCloseMessage(string value) : base(value)
        {
        }
    }
}
