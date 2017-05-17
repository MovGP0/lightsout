using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MoreLinq;
using Serilog;

namespace LightsOut
{
    public sealed class LightsOutGameViewModel : NotifyPropertyChanged
    {
        private static ILogger Log => Serilog.Log.Logger.ForContext<LightsOutGameViewModel>();
        public ObservableCollection<SwitchViewModel> SwitchViewModels { get; } = new ObservableCollection<SwitchViewModel>();
        public Collection<Level> Levels { get; } = new Collection<Level>();

        private int _moveCounter;
        private int _currentLevelNumber;
        private int _columns;
        private int _rows;
        private bool _isGameWon;

        public LightsOutGameViewModel()
        {
            PropertyChanged += HandlePropertyChanged;
            SwitchViewModels.CollectionChanged += HandleSwitchViewModelCollectionChanged;
        }

        private void HandleSwitchViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems ?? new List<SwitchViewModel>(0);
            foreach (var oldItem in oldItems)
            {
                var svm = (SwitchViewModel)oldItem;
                UnregisterSwitchViewModel(svm);
            }

            var newItems = e.NewItems ?? new List<SwitchViewModel>(0);
            foreach (var newItem in newItems)
            {
                var svm = (SwitchViewModel)newItem;
                RegisterSwitchViewModel(svm);
            }
        }

        ~LightsOutGameViewModel()
        {
            PropertyChanged -= HandlePropertyChanged;
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Levels))
            {
                Reset();
            }
        }
        
        public int CurrentLevelNumber
        {
            get => _currentLevelNumber;
            set
            {
                if (value == _currentLevelNumber) return;
                _currentLevelNumber = value;
                OnPropertyChanged();
            }
        }

        public int MoveCounter
        {
            get => _moveCounter;
            set
            {
                if (value == _moveCounter) return;
                _moveCounter = value;
                OnPropertyChanged();
            }
        }

        public int Rows
        {
            get => _rows;
            private set
            {
                if (value == _rows) return;
                _rows = value;
                OnPropertyChanged();
            }
        }

        public int Columns
        {
            get => _columns;
            private set
            {
                if (value == _columns) return;
                _columns = value;
                OnPropertyChanged();
            }
        }

        public bool IsGameWon
        {
            get => _isGameWon;
            set
            {
                if (value == _isGameWon) return;
                _isGameWon = value;
                OnPropertyChanged();
            }
        }

        public ICommand RestartCommand { get; }
        
        private void Reset()
        {
            Log.Information("Resetting LightsOutGameViewModel");

            if (CurrentLevelNumber+1 > Levels.Count)
            {
                CurrentLevelNumber = 0;
            }

            var currentLevel = Levels[CurrentLevelNumber];
            Rows = currentLevel.Rows;
            Columns = currentLevel.Columns;

            SwitchViewModels.Clear();

            currentLevel.GetAllPositions().ForEach(pos =>
            {
                var switchViewModel = new SwitchViewModel
                {
                    Position = pos, 
                    State = currentLevel[pos]
                };
                SwitchViewModels.Add(switchViewModel);
            });
            
            MoveCounter = 0;
        }

        public void SetLevel(int number)
        {
            if(Levels.Count == 0) throw new InvalidOperationException("Levels not initialized");
            if(number < 0) throw new ArgumentOutOfRangeException("Must not be negative.");
            if(number >= Levels.Count) throw new ArgumentOutOfRangeException("Must not be greater that the number of loaded levels.");

            Log.Information($"Setting Level {number}");

            SwitchViewModels.Clear();
            var level = Levels[number];
            level.GetAllPositions().ForEach(position =>
            {
                Log.Information("adding switch model");
                SwitchViewModels.Add(new SwitchViewModel
                {
                    State = level[position], 
                    Position = position
                });
            });

            Rows = level.Rows;
            Columns = level.Columns;
        }

        private void Set8PosSwitches(Position position, SwitchState state)
        {
            if(state == SwitchState.OffPressed || state == SwitchState.OnPressed) return;

            Log.Information("Setting 8 Positions");
            var positions = position.Get9Positions()
                .Where(pos => pos.IsInBounds(Rows, Columns))
                .Where(pos => pos != position);
            
            var switchViewModels = SwitchViewModels.Where(svm => positions.Contains(svm.Position));
            switchViewModels.ForEach(svm =>
            {
                // temp. unregister event handler to prevent endless loop
                UnregisterSwitchViewModel(svm);
                svm.Switch8Position();
                RegisterSwitchViewModel(svm);
            });

            IsGameWon = AreAllSwitchesOff(SwitchViewModels);
        }

        public bool AreAllSwitchesOff(IEnumerable<SwitchViewModel> switchViewModels)
        {
            return switchViewModels.All(svm => svm.State == SwitchState.Off);
        }

        private void UnregisterSwitchViewModel(SwitchViewModel @switch)
        {
            @switch.SwitchStateChanged -= HandleSwitchChanged;
        }

        private void RegisterSwitchViewModel(SwitchViewModel @switch)
        {
            @switch.SwitchStateChanged += HandleSwitchChanged;
        }

        private void HandleSwitchChanged(object sender, SwitchStateChangedEventArgs args)
        {
            if (args.State == SwitchState.Off || args.State == SwitchState.On)
            {
                MoveCounter += 1;
            }

            Set8PosSwitches(args.Position, args.State);
        }
    }
}