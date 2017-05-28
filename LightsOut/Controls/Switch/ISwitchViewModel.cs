using System;

namespace LightsOut
{
    public interface ISwitchViewModel
    {
        SwitchState State { get; set; }
        Position Position { get; set; }
        event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged;
    }
}