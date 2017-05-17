﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MoreLinq;
using Serilog;

namespace LightsOut
{
    public partial class LightsOutGame
    {
        private static ILogger Log => Serilog.Log.Logger;

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
            Log.Information("Setting Data Context");

            if (e.OldValue is LightsOutGameViewModel oldModel)
            {
                oldModel.PropertyChanged -= HandleViewModelPropertyChanged;
                oldModel.Switches.CollectionChanged -= HandleSwitchViewModelCollectionChanged;
            }

            if (e.NewValue is LightsOutGameViewModel viewModel)
            {
                SetupWithViewModel(viewModel);
            }
        }

        private void SetupWithViewModel(LightsOutGameViewModel viewModel)
        {
            viewModel.PropertyChanged += HandleViewModelPropertyChanged;
            viewModel.Switches.CollectionChanged += HandleSwitchViewModelCollectionChanged;

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

            var switches = GameGrid.Children.OfType<Switch>();
            switches.ForEach(UnregisterSwitch);

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
                if(switchToRemove == null) continue;
                UnregisterSwitch(switchToRemove);
                GameGrid.Children.Remove(switchToRemove);
            }
            
            var newItems = e.NewItems ?? new ReadOnlyCollection<SwitchViewModel>(new List<SwitchViewModel>());
            foreach (var item in newItems)
            {
                var switchViewModel = item as SwitchViewModel;
                Log.Information($"Adding new switch at {switchViewModel.Position}");

                var @switch = switchViewModel.CreateSwitch(FindResource);
                RegisterSwitch(@switch);
                GameGrid.Children.Add(@switch);
            }
        }
        
        private void UnregisterSwitch(Switch @switch)
        {
            @switch.SwitchStateChanged -= HandleSwitchChanged;
        }

        private void RegisterSwitch(Switch @switch)
        {
            @switch.SwitchStateChanged += HandleSwitchChanged;
        }

        public void HandleSwitchChanged(object sender, SwitchStateChangedEventArgs args)
        {
            Log.Information("Handling switch change");
        }
    }
}
