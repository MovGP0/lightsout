using System;
using System.Windows;
using System.Windows.Input;

namespace LightsOut
{
    public partial class Switch : ISwitch
    {
        #region DependencyProperties
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(SwitchState), typeof(Switch),
                new PropertyMetadata(SwitchState.Off, StatePropertyChangedCallback));

        private static void StatePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is Switch @switch)
            {
                @switch.SwitchStateChanged?.Invoke(@switch, new SwitchStateChangedEventArgs(@switch.Position, (SwitchState)args.NewValue));
            }
        }

        public SwitchState State
        {
            get => (SwitchState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(Position), typeof(Switch),
                new PropertyMetadata(new Position(0, 0)));

        public Position Position
        {
            get => (Position)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }
        #endregion

        public event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged;

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