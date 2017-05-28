using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Serilog;

namespace LightsOut
{
    public partial class LightsOutGame
    {
        private static ILogger Log => Serilog.Log.Logger.ForContext<LightsOutGame>();

        #region DependencyProperties

        public static DependencyProperty GameBackgroundProperty
            = DependencyProperty.Register(nameof(GameBackground), typeof(Brush), typeof(LightsOutGame), new PropertyMetadata(Brushes.Transparent));

        public Brush GameBackground
        {
            get => (Brush)GetValue(GameBackgroundProperty);
            set => SetValue(GameBackgroundProperty, value);
        }

        public static DependencyProperty IsWonProperty
            = DependencyProperty.Register(nameof(IsWon), typeof(bool), typeof(LightsOutGame), new PropertyMetadata(false));

        public bool IsWon
        {
            get => (bool)GetValue(IsWonProperty);
            set => SetValue(IsWonProperty, value);
        }

        public static readonly DependencyProperty WinMessageVisibilityProperty = DependencyProperty.Register(
            nameof(WinMessageVisibility), typeof(Visibility), typeof(LightsOutGame), new PropertyMetadata(Visibility.Collapsed));

        public Visibility WinMessageVisibility
        {
            get => (Visibility) GetValue(WinMessageVisibilityProperty);
            set => SetValue(WinMessageVisibilityProperty, value);
        }
        #endregion

        public LightsOutGame()
        {
            InitializeComponent();
            DataContextChanged += HandleDataContextChanged;
            if (DataContext is ILightsOutGameViewModel viewModel)
            {
                SetupWithViewModel(viewModel);
            }
        }

        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Log.Verbose("Setting Data Context");

            if (e.OldValue is ILightsOutGameViewModel oldModel)
            {
                BindingOperations.ClearBinding(this, IsWonProperty);
                oldModel.PropertyChanged -= HandleViewModelPropertyChanged;
                oldModel.SwitchViewModels.CollectionChanged -= HandleSwitchViewModelCollectionChanged;
            }

            if (e.NewValue is ILightsOutGameViewModel viewModel)
            {
                SetupWithViewModel(viewModel);
            }
        }

        private void SetupWithViewModel(ILightsOutGameViewModel viewModel)
        {
            viewModel.PropertyChanged += HandleViewModelPropertyChanged;
            viewModel.SwitchViewModels.CollectionChanged += HandleSwitchViewModelCollectionChanged;

            SetBinding(IsWonProperty, new Binding(nameof(viewModel.IsGameWon)) {Mode = BindingMode.TwoWay});

            viewModel.Setup();
            Initialize(viewModel);
        }

        public ILightsOutGameViewModel ViewModel
        {
            get => DataContext as ILightsOutGameViewModel;
            set => DataContext = value;
        }

        private void HandleViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ILightsOutGameViewModel viewModel)
            {
            }
        }

        public void Initialize(ILightsOutGameViewModel viewModel)
        {
            Log.Verbose("Initializing LightsOutGame");
            GameGrid.Children.Clear();

            viewModel.SetLevel(0);

            var rows = viewModel.Rows;
            GameGrid.SetupRowDefinitions(rows);

            var columns = viewModel.Columns;
            GameGrid.SetupColumnDefinitions(columns);
        }

        private void HandleSwitchViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Log.Verbose("Handle switches changed");

            var switches = GameGrid.Children.OfType<Switch>().ToArray();
            var oldItems = e.OldItems ?? new ReadOnlyCollection<ISwitchViewModel>(new List<ISwitchViewModel>());
            foreach (var item in oldItems)
            {
                var switchViewModel = item as ISwitchViewModel;
                if(switchViewModel == null) continue;
                Log.Verbose($"Removing old switch at {switchViewModel.Position}");

                var switchToRemove = switches.SingleOrDefault(s => s.Position.Equals(switchViewModel.Position));
                if (switchToRemove == null) continue;
                GameGrid.Children.Remove(switchToRemove);
            }

            var newItems = e.NewItems ?? new ReadOnlyCollection<ISwitchViewModel>(new List<ISwitchViewModel>());
            foreach (var item in newItems)
            {
                var switchViewModel = item as ISwitchViewModel;
                if(switchViewModel == null) continue;
                Log.Verbose($"Adding new switch at {switchViewModel.Position}");

                var @switch = switchViewModel.CreateSwitch(FindResource);
                GameGrid.Children.Add(@switch);
            }
        }

        private void OnNextLevelButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.NextLevel();
        }

        private void OnResetLevelButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.ResetLevel();
        }

        private void OnResetGameButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.ResetGame();
        }
    }
}
