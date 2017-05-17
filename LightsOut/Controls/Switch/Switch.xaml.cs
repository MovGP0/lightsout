using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Serilog;

namespace LightsOut
{
    public partial class Switch
    {
        private static ILogger Log => Serilog.Log.Logger.ForContext<Switch>();

        #region DependencyProperties
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(SwitchState), typeof(Switch),
                new PropertyMetadata(SwitchState.Off));
        
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
            InitializeBindingsToViewModel(ViewModel);
        }
        
        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BindingOperations.ClearBinding(this, StateProperty);
            BindingOperations.ClearBinding(this, PositionProperty);

            if (e.OldValue is SwitchViewModel oldViewModel)
            {
            }

            if (e.NewValue is SwitchViewModel viewModel)
            {
                InitializeBindingsToViewModel(viewModel);
            }
        }

        private void InitializeBindingsToViewModel<T>(T viewModel)
            where T:ISwitch, INotifyPropertyChanged
        {
            if (viewModel == null) return;
            Log.Information("Initializing Bindings");

            Position = viewModel.Position;

            SetBinding(StateProperty, new Binding(nameof(viewModel.State)) { Mode = BindingMode.TwoWay });
            SetBinding(PositionProperty, new Binding(nameof(viewModel.Position)) { Mode = BindingMode.TwoWay });
        }
        
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