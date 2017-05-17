namespace LightsOut
{
    public sealed class SwitchViewModel : NotifyPropertyChanged, ISwitch
    {
        private Position _position;
        private SwitchState _state;

        public Position Position
        {
            get => _position;
            set
            {
                if (value.Equals(_position)) return;
                _position = value;
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