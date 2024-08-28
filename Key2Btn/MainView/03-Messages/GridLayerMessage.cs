using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Windows;
using System.Windows.Media;

namespace Key2Btn.MainView.Messages
{
    public class GridLayerOnOffMessage : ValueChangedMessage<bool>
    {
        public GridLayerOnOffMessage(bool value) : base(value)
        {
        }
    }

    public class GridLayerBlurRadiusMessage : ValueChangedMessage<double>
    {
        public GridLayerBlurRadiusMessage(double value) : base(value)
        {
        }
    }

    public class GridLayerBrushMessage : ValueChangedMessage<(Vector, Brush)>
    {
        public GridLayerBrushMessage((Vector, Brush) value) : base(value)
        {
        }
    }

    public class GridLayerMoveTo1stMonitorMessage : ValueChangedMessage<Vector>
    {
        public GridLayerMoveTo1stMonitorMessage(Vector value) : base(value)
        {
        }
    }

    public class GridLayerMoveTo2ndMonitorMessage : ValueChangedMessage<Vector>
    {
        public GridLayerMoveTo2ndMonitorMessage(Vector value) : base(value)
        {
        }
    }
}
