using System;
using System.ComponentModel;
using Serilog;

namespace LightsOut
{
    public sealed class SwitchViewModel : NotifyPropertyChanged, ISwitch
    {
        private static ILogger Log => Serilog.Log.Logger.ForContext<SwitchViewModel>();

        public event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged;

        public SwitchViewModel()
        {
            PropertyChanged += HandlePropertyChanged;
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(State))
            {
                SwitchStateChanged?.Invoke(this, new SwitchStateChangedEventArgs(Position, State));
            }
        }

        private Position _position;
        private SwitchState _state;

        public Position Position
        {
            get => _position;
            set
            {
                if (value.Equals(_position)) return;
                _position = value;

                Log.Verbose("Position changed");
                OnPropertyChanged();
            }
        }
        
        public SwitchState State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                _state = value;

                Log.Verbose("State changed");
                OnPropertyChanged();
            }
        }

        public void PressSwitch()
        {
            PressSwitchCommand.Execute(this);
        }

        public void ReleaseSwitch()
        {
            ReleaseSwitchCommand.Execute(this);
        }

        private PressSwitchCommand PressSwitchCommand { get; } = new PressSwitchCommand();
        private ReleaseSwitchCommand ReleaseSwitchCommand { get; } = new ReleaseSwitchCommand();
    }
}