using System.Windows;
using System.Windows.Input;

namespace LightsOut
{
    public partial class Switch : ISwitch
    {
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(SwitchState), typeof(Switch),
                new PropertyMetadata(SwitchState.Off));

        public SwitchState State
        {
            get => (SwitchState) GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public Switch()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            PressSwitch.Execute(this);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseSwitch.Execute(this);
        }

        public PressSwitchCommand PressSwitch { get; } = new PressSwitchCommand();
        public ReleaseSwitchCommand ReleaseSwitch { get; } = new ReleaseSwitchCommand();
    }
}