﻿using System;
using System.Linq;
using Serilog;

namespace LightsOut
{
    public sealed class LightsOutGameModel : NotifyPropertyChanged
    {
        private static ILogger Log => Serilog.Log.Logger.ForContext<LightsOutGameModel>();

        #region Properties
        private Level _level;
        private bool _isWon;
        private int _moveCounter;

        public bool IsWon
        {
            get => _isWon;
            set
            {
                if (value == _isWon) return;
                _isWon = value;
                Log.Information($"{nameof(IsWon)} set to {value}");
                OnPropertyChanged();
            }
        }

        public Level Level
        {
            get => _level;
            set
            {
                if (Equals(value, _level)) return;
                _level = value;
                Log.Information($"{nameof(Level)} set to {value}");
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
                Log.Information($"{nameof(MoveCounter)} set to {value}");
                OnPropertyChanged();
            }
        }
        #endregion

        [Obsolete("", true)]
        public void SetSwitch(SwitchState state, Position position)
        {
            IsWon = IsGameWon(Level);
        }
        
        private static bool IsGameWon(Level level)
        {
            return level.GetAllPositions().All(pos => level[pos] == SwitchState.Off);
        }
    }
}