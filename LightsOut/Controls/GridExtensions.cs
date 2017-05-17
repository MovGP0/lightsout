using System;
using System.Windows;
using System.Windows.Controls;
using MoreLinq;

namespace LightsOut
{
    public static class GridExtensions
    {
        public static void SetupColumnDefinitions(this Grid grid, int columns)
        {
            grid.ColumnDefinitions.Clear();
            for (var column = 0; column < columns; column++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(100d)
                });
            }
        }

        public static void SetupRowDefinitions(this Grid grid, int rows)
        {
            grid.RowDefinitions.Clear();
            for (var row = 0; row < rows; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(100d)
                });
            }
        }
        
        public static void PopulizeWithSwitches(this Panel panel, Level level, Func<string, object> findResource)
        {
            level.GetAllPositions().ForEach(pos =>
            {
                var @switch = new Switch
                {
                    State = level[pos],
                    Style = (Style)findResource("SwitchStyle")
                };
                @switch.SetValue(Grid.RowProperty, pos.Row);
                @switch.SetValue(Grid.ColumnProperty, pos.Column);
                panel.Children.Add(@switch);
            });
        }
    }
}