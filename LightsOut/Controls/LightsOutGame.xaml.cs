using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MoreLinq;
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
            if (DataContext is LightsOutGameViewModel viewModel)
            {
                SetupWithViewModel(viewModel);
            }
        }

        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Log.Information("Setting Data Context");

            if (e.OldValue is LightsOutGameViewModel oldModel)
            {
                BindingOperations.ClearBinding(this, IsWonProperty);
                oldModel.PropertyChanged -= HandleViewModelPropertyChanged;
                oldModel.SwitchViewModels.CollectionChanged -= HandleSwitchViewModelCollectionChanged;
            }

            if (e.NewValue is LightsOutGameViewModel viewModel)
            {
                SetupWithViewModel(viewModel);
            }
        }

        private void SetupWithViewModel(LightsOutGameViewModel viewModel)
        {
            viewModel.PropertyChanged += HandleViewModelPropertyChanged;
            viewModel.SwitchViewModels.CollectionChanged += HandleSwitchViewModelCollectionChanged;

            SetBinding(IsWonProperty, new Binding(nameof(viewModel.IsGameWon)) {Mode = BindingMode.TwoWay});

            viewModel.Levels.Clear();
            LevelsLoader.GetLevels().ForEach(viewModel.Levels.Add);
            Initialize(viewModel);
        }

        public LightsOutGameViewModel ViewModel
        {
            get => DataContext as LightsOutGameViewModel;
            set => DataContext = value;
        }

        private void HandleViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is LightsOutGameViewModel viewModel)
            {
            }
        }

        public void Initialize(LightsOutGameViewModel viewModel)
        {
            Log.Information("Initializing LightsOutGame");
            GameGrid.Children.Clear();

            viewModel.SetLevel(0);

            var rows = viewModel.Rows;
            GameGrid.SetupRowDefinitions(rows);

            var columns = viewModel.Columns;
            GameGrid.SetupColumnDefinitions(columns);
        }

        private void HandleSwitchViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Log.Information("Handle switches changed");

            var switches = GameGrid.Children.OfType<Switch>().ToArray();
            var oldItems = e.OldItems ?? new ReadOnlyCollection<SwitchViewModel>(new List<SwitchViewModel>());
            foreach (var item in oldItems)
            {
                var switchViewModel = item as SwitchViewModel;
                Log.Information($"Removing old switch at {switchViewModel.Position}");

                var switchToRemove = switches.SingleOrDefault(s => s.Position.Equals(switchViewModel.Position));
                if (switchToRemove == null) continue;
                GameGrid.Children.Remove(switchToRemove);
            }

            var newItems = e.NewItems ?? new ReadOnlyCollection<SwitchViewModel>(new List<SwitchViewModel>());
            foreach (var item in newItems)
            {
                var switchViewModel = item as SwitchViewModel;
                Log.Information($"Adding new switch at {switchViewModel.Position}");

                var @switch = switchViewModel.CreateSwitch(FindResource);
                GameGrid.Children.Add(@switch);
            }
        }
    }
}
