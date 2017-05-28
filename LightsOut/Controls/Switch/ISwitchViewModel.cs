using System;
using System.ComponentModel;

namespace LightsOut
{
    public interface ISwitchViewModel : INotifyPropertyChanged
    {
        SwitchState State { get; set; }
        Position Position { get; set; }
        event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged;
        void PressSwitch();
        void ReleaseSwitch();
    }
}