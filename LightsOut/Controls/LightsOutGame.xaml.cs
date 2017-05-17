using System.ComponentModel;
using System.Windows;

namespace LightsOut
{
    public partial class LightsOutGame
    {
        public LightsOutGame()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            if (DataContext is LightsOutGameViewModel viewModel)
            {
                SetupWithViewModel(viewModel);
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is LightsOutGameViewModel oldModel)
            {
                oldModel.PropertyChanged -= OnModelChanged;
            }

            if (e.NewValue is LightsOutGameViewModel viewModel)
            {
                SetupWithViewModel(viewModel);
            }
        }

        private void SetupWithViewModel(LightsOutGameViewModel viewModel)
        {
            viewModel.PropertyChanged += OnModelChanged;
            viewModel.Levels = LevelsLoader.GetLevels();
            Initialize(viewModel);
        }

        private void OnModelChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is LightsOutGameViewModel viewModel)
                Initialize(viewModel);
        }

        public void Initialize(LightsOutGameViewModel viewModel)
        {
            GameGrid.Children.Clear();

            var rows = viewModel.CurrentLevelState.Rows;
            GameGrid.SetupRowDefinitions(rows);

            var columns = viewModel.CurrentLevelState.Columns;
            GameGrid.SetupColumnDefinitions(columns);

            GameGrid.PopulizeWithSwitches(viewModel.CurrentLevelState, FindResource);
        }
    }
}
