using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Serilog;

namespace LightsOut
{
    public partial class Switch
    {
        private static ILogger Log => Serilog.Log.Logger;

        #region DependencyProperties
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(SwitchState), typeof(Switch),
                new PropertyMetadata(SwitchState.Off, StatePropertyChangedCallback));

        private static void StatePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is Switch @switch)
            {
                var oldValue = (SwitchState) args.OldValue;
                var newValue = (SwitchState)args.NewValue;
                var position = @switch.Position;

                Log.Information($"Setting switch at {position} from {oldValue} to {newValue}");
                @switch.SwitchStateChanged?.Invoke(@switch, new SwitchStateChangedEventArgs(position, newValue));
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

        public Switch()
        {
            InitializeComponent();
            DataContextChanged += HandleDataContextChanged;
        }
        
        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is SwitchViewModel oldViewModel)
            {
                oldViewModel.PropertyChanged -= HandleViewModelChanged;
            }

            if (e.NewValue is SwitchViewModel viewModel)
            {
                Position = viewModel.Position;
                viewModel.PropertyChanged += HandleViewModelChanged;
            }
        }

        private void HandleViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            var viewModel = (SwitchViewModel)sender;
            if (e.PropertyName == nameof(viewModel.Position))
            {
                Position = viewModel.Position;
            }

            if (e.PropertyName == nameof(viewModel.State))
            {
                State = viewModel.State;
            }
        }

        public event EventHandler<SwitchStateChangedEventArgs> SwitchStateChanged;
        
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel?.PressSwitch();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel?.ReleaseSwitch();
        }

        public SwitchViewModel ViewModel
        {
            get => DataContext as SwitchViewModel;
            set => DataContext = value;
        }
    }
}