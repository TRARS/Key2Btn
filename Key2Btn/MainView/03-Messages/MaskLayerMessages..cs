using CommunityToolkit.Mvvm.Messaging.Messages;
using System;

namespace Key2Btn.MainView.Messages
{
    public class MaskLayerFadeOutMessage : ValueChangedMessage<(int, Action, Action<int>)>
    {
        public MaskLayerFadeOutMessage((int, Action, Action<int>) value) : base(value)
        {
        }
    }

    public class MaskLayerFadeInMessage : ValueChangedMessage<int>
    {
        public MaskLayerFadeInMessage(int value) : base(value)
        {
        }
    }
}
