using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Key2Btn.MainView.Messages
{
    public class GrayLayerTakeScreenshotMessage : ValueChangedMessage<bool>
    {
        public GrayLayerTakeScreenshotMessage(bool value) : base(value)
        {
        }
    }

    public class GrayLayerSaveScreenshotMessage : ValueChangedMessage<string>
    {
        public GrayLayerSaveScreenshotMessage(string value) : base(value)
        {
        }
    }
}
