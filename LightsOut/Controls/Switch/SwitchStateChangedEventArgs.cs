using System;

namespace LightsOut
{
    public class SwitchStateChangedEventArgs : EventArgs
    {
        public SwitchStateChangedEventArgs(Position position, SwitchState state)
        {
            Position = position;
            State = state;
        }

        public Position Position { get; }
        public SwitchState State { get; }
    }
}