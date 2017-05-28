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
    public sealed class LightsOutGameViewModel : NotifyPropertyChanged, ILightsOutGameViewModel
    {
        private static ILogger Log => Serilog.Log.Logger.ForContext<LightsOutGameViewModel>();
        public ObservableCollection<ISwitchViewModel> SwitchViewModels { get; } = new ObservableCollection<ISwitchViewModel>();
        public Collection<Level> Levels { get; } = new Collection<Level>();
        private ILevelsLoader LevelsLoader { get; }
        private Func<ISwitchViewModel> SwitchViewModelFactory { get; }

        private int _moveCounter;
        private int _currentLevelNumber;
        private int _columns;
        private int _rows;
        private bool _isGameWon;
        private int _numberOfWonGames;

        public LightsOutGameViewModel(ILevelsLoader levelsLoader, Func<ISwitchViewModel> switchViewModelFactory)
        {
            LevelsLoader = levelsLoader;
            SwitchViewModelFactory = switchViewModelFactory;
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

        public int NumberOfWonGames
        {
            get => _numberOfWonGames;
            set
            {
                if (value == _numberOfWonGames) return;
                _numberOfWonGames = value;
                OnPropertyChanged();
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
            Log.Verbose("Resetting LightsOutGameViewModel");

            if (CurrentLevelNumber+1 > Levels.Count)
            {
                CurrentLevelNumber = 0;
            }

            var currentLevel = Levels[CurrentLevelNumber];
            Rows = currentLevel.Rows;
            Columns = currentLevel.Columns;

            SwitchViewModels.ToList().ForEach(item => SwitchViewModels.Remove(item));

            currentLevel.GetAllPositions().ForEach(pos =>
            {
                var switchViewModel = SwitchViewModelFactory();
                switchViewModel.Position = pos;
                switchViewModel.State = currentLevel[pos];
                SwitchViewModels.Add(switchViewModel);
            });
            
            MoveCounter = 0;
        }

        public void SetLevel(int number)
        {
            if(Levels.Count == 0) throw new InvalidOperationException("Levels not initialized");
            if(number < 0) throw new ArgumentOutOfRangeException("Must not be negative.");
            if(number >= Levels.Count)
            {
                SetLevel(0);
                return;
            }

            Log.Verbose($"Setting Level {number}");

            SwitchViewModels.ToList().ForEach(item => SwitchViewModels.Remove(item));

            var level = Levels[number];
            level.GetAllPositions().ForEach(position =>
            {
                Log.Verbose("adding switch model");
                SwitchViewModels.Add(new SwitchViewModel
                {
                    State = level[position], 
                    Position = position
                });
            });

            Rows = level.Rows;
            Columns = level.Columns;
            CurrentLevelNumber = number;
            IsGameWon = AreAllSwitchesOff(SwitchViewModels);
        }

        private void Set8PosSwitches(Position position, SwitchState state)
        {
            if(state == SwitchState.OffPressed || state == SwitchState.OnPressed) return;

            Log.Verbose("Setting 8 Positions");
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
            if (IsGameWon)
            {
                NumberOfWonGames++;
            }
        }

        public bool AreAllSwitchesOff(IEnumerable<ISwitchViewModel> switchViewModels)
        {
            return switchViewModels.All(svm => svm.State == SwitchState.Off);
        }

        private void UnregisterSwitchViewModel(ISwitchViewModel switchViewModel)
        {
            switchViewModel.SwitchStateChanged -= HandleSwitchChanged;
        }

        private void RegisterSwitchViewModel(ISwitchViewModel switchViewModel)
        {
            switchViewModel.SwitchStateChanged += HandleSwitchChanged;
        }

        private void HandleSwitchChanged(object sender, SwitchStateChangedEventArgs args)
        {
            if (args.State == SwitchState.Off || args.State == SwitchState.On)
            {
                MoveCounter += 1;
            }

            Set8PosSwitches(args.Position, args.State);
        }

        public void NextLevel()
        {
            Log.Information("Setting next level");
            var nextLevelNumber = CurrentLevelNumber + 1;
            SetLevel(nextLevelNumber);
        }

        public void ResetLevel()
        {
            Log.Information("Resetting level");
            SetLevel(CurrentLevelNumber);
        }

        public void ResetGame()
        {
            Log.Information("Resetting game");
            SetLevel(0);
        }

        public void Setup()
        {
            Levels.ToList().ForEach(level => Levels.Remove(level));
            LevelsLoader.GetLevels().ForEach(Levels.Add);
        }
    }
}