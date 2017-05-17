using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using MoreLinq;
using Serilog;

namespace LightsOut
{
    public sealed class LightsOutGameViewModel : NotifyPropertyChanged
    {
        private static ILogger Log => Serilog.Log.Logger;
        public ObservableCollection<SwitchViewModel> Switches { get; } = new ObservableCollection<SwitchViewModel>();
        public Collection<Level> Levels { get; } = new Collection<Level>();

        private int _moveCounter;
        private int _currentLevelNumber;
        private int _columns;
        private int _rows;

        public LightsOutGameViewModel()
        {
            PropertyChanged += HandlePropertyChanged;
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

            Switches.Clear();

            currentLevel.GetAllPositions().ForEach(pos =>
            {
                var switchViewModel = new SwitchViewModel
                {
                    Position = pos, 
                    State = currentLevel[pos]
                };
                Switches.Add(switchViewModel);
            });
            
            MoveCounter = 0;
        }

        public void SetLevel(int number)
        {
            if(Levels.Count == 0) throw new InvalidOperationException("Levels not initialized");
            if(number < 0) throw new ArgumentOutOfRangeException("Must not be negative.");
            if(number >= Levels.Count) throw new ArgumentOutOfRangeException("Must not be greater that the number of loaded levels.");

            Log.Information($"Setting Level {number}");

            Switches.Clear();
            var level = Levels[number];
            level.GetAllPositions().ForEach(position =>
            {
                Log.Information("adding switch model");
                Switches.Add(new SwitchViewModel
                {
                    State = level[position], 
                    Position = position
                });
            });

            Rows = level.Rows;
            Columns = level.Columns;
        }
    }
}