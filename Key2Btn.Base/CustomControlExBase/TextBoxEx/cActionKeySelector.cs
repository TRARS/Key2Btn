using CommunityToolkit.Mvvm.Input;
using Key2Btn.Base.Helper.ExClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlExBase.TextBoxEx
{
    public partial class cActionKeySelector : TextBox
    {
        static cActionKeySelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cActionKeySelector), new FrameworkPropertyMetadata(typeof(cActionKeySelector)));
        }
    }

    public partial class cActionKeySelector
    {
        [RelayCommand]
        private void OnPopupOpend(object para)
        {
            // check if key is valid
            if (this.ActionPackets.Any(ap => ap.Key == this.Text))
            {
                Debug.WriteLine($"OnPopupOpend: key '{this.Text}' is vaild");
            }
            else
            {
                Debug.WriteLine($"OnPopupOpend: key '{this.Text}' is invalid");
                this.Text = string.Empty;
            }

        }

        [RelayCommand]
        private void OnListBoxSelectionChanged(object para)
        {
            if (para is ActionPacket packet)
            {
                this.Text = packet.Key;
                Debug.WriteLine($"OnListBoxSelectionChanged: ActionKey is {packet.Key}");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

    public partial class cActionKeySelector
    {
        public IEnumerable<ActionPacket> ActionPackets
        {
            get { return (IEnumerable<ActionPacket>)GetValue(ActionPacketsProperty); }
            set { SetValue(ActionPacketsProperty, value); }
        }
        public static readonly DependencyProperty ActionPacketsProperty = DependencyProperty.Register(
            name: "ActionPackets",
            propertyType: typeof(IEnumerable<ActionPacket>),
            ownerType: typeof(cActionKeySelector),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
