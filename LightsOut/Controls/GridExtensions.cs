using System;
using System.Windows;
using System.Windows.Controls;
using Serilog;

namespace LightsOut
{
    public static class GridExtensions
    {
        private static ILogger Log => Serilog.Log.Logger;

        public static void SetupColumnDefinitions(this Grid grid, int columns)
        {
            Log.Information($"Setting up {columns} ColumnDefinitions.");

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
            Log.Information($"Setting up {rows} RowDefinitions.");

            grid.RowDefinitions.Clear();
            for (var row = 0; row < rows; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(100d)
                });
            }
        }

        public static Switch CreateSwitch(this SwitchViewModel switchViewModel, Func<string, object> findResource)
        {
            var @switch = new Switch
            {
                Position = switchViewModel.Position,
                State = switchViewModel.State,
                Style = (Style)findResource("SwitchStyle")
            };
            @switch.SetValue(Grid.RowProperty, switchViewModel.Position.Row);
            @switch.SetValue(Grid.ColumnProperty, switchViewModel.Position.Column);
            @switch.ViewModel = switchViewModel;
            return @switch;
        }
    }
}