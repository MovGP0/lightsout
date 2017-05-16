using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace LightsOut
{
    public sealed class LightsOutGameViewModel : NotifyPropertyChanged
    {
        private Level _currentLevelState;
        private ICollection<Level> _levels;
        private int _moveCounter;
        private int _currentLevel;

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

        public ICollection<Level> Levels
        {
            get => _levels;
            set
            {
                _levels = value;
                OnPropertyChanged();
            }
        }

        public Level CurrentLevelState
        {
            get => _currentLevelState;
            set
            {
                if (_currentLevelState != null && _currentLevelState.Equals(value)) return;
                _currentLevelState = value;
                OnPropertyChanged();
            }
        }
        
        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                if (value == _currentLevel) return;
                _currentLevel = value;
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

        public ICommand MoveCommand { get; }
        public ICommand RestartCommand { get; }
        
        private void Reset()
        {
            CurrentLevelState = Levels.FirstOrDefault();
            CurrentLevel = 0;
            MoveCounter = 0;
        }
    }
}