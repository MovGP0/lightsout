using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LightsOut
{
    public partial class LightsOutGame
    {
        public LightsOutGame()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            if (DataContext is LightsOutGameViewModel model)
            {
                SetupWithModel(model);
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is LightsOutGameViewModel oldModel)
            {
                oldModel.PropertyChanged -= OnModelChanged;
            }

            if (e.NewValue is LightsOutGameViewModel model)
            {
                SetupWithModel(model);
            }
        }

        private void SetupWithModel(LightsOutGameViewModel model)
        {
            model.PropertyChanged += OnModelChanged;
            model.Levels = LevelsLoader.GetLevels();
            Initialize(model);
        }

        private void OnModelChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is LightsOutGameViewModel model)
                Initialize(model);
        }

        public void Initialize(LightsOutGameViewModel model)
        {
            GameGrid.Children.Clear();

            var rows = model.CurrentLevelState.Rows;
            SetupRowDefinitions(GameGrid, rows);

            var columns = model.CurrentLevelState.Columns;
            SetupColumnDefinitions(GameGrid, columns);

            var grid = GameGrid;

            PopulizeWithSwitches(grid, model.CurrentLevelState);
        }

        private void PopulizeWithSwitches(Panel panel, Level level)
        {
            for (var row = 0; row < level.Rows; row++)
            for (var column = 0; column < level.Columns; column++)
            {
                var @switch = new Switch
                {
                    State = level.OnMatrix[row, column],
                    Style = (Style)FindResource("SwitchStyle")
                };
                @switch.SetValue(Grid.RowProperty, row);
                @switch.SetValue(Grid.ColumnProperty, column);
                
                panel.Children.Add(@switch);
            }
        }

        private static void SetupColumnDefinitions(Grid grid, int columns)
        {
            grid.ColumnDefinitions.Clear();
            for (var column = 0; column < columns; column++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(40d)
                });
            }
        }

        private static void SetupRowDefinitions(Grid grid, int rows)
        {
            grid.RowDefinitions.Clear();
            for (var row = 0; row < rows; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(40d)
                });
            }
        }
    }
}
