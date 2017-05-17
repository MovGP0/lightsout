using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace LightsOut
{
    public static class LevelExtensions
    {
        public static Level SetSwitchState(this Level level, SwitchState state, Position position)
        {
            var matrix = level.Matrix;
            matrix[position.Row, position.Column] = state;
            return new Level(level.Name, level.Rows, level.Columns, matrix);
        }

        public static IEnumerable<Position> GetAllPositions(this Level level)
        {
            for (var row = 0; row < level.Rows; row++)
            for (var colunm = 0; colunm < level.Columns; colunm++)
            {
                yield return new Position(row, colunm);
            }
        }
        
        public static Level Switch8Positions(this Level level, Position position)
        {
            var newLevelState = level;
            Get8Positions(position, level)
                .ForEach(pos => newLevelState = Switch8Position(newLevelState, pos));
            return newLevelState;
        }

        private static IEnumerable<Position> Get8Positions(Position position, Level level)
        {
            return Get9Grid(position)
                .Where(pos => IsInBounds(level, pos))
                .Where(pos => !IsCurrentPosition(pos, position));
        }

        private static Level Switch8Position(Level level, Position p)
        {
            var currentState = level[p];
            switch (currentState)
            {
                case SwitchState.OnPressed:
                case SwitchState.Off:
                    return level.SetSwitchState(SwitchState.On, p);

                case SwitchState.OffPressed:
                case SwitchState.On:
                    return level.SetSwitchState(SwitchState.Off, p);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool IsCurrentPosition(Position position, Position current)
        {
            return position.Row == current.Row
                   && position.Column == current.Column;
        }

        private static IEnumerable<Position> Get9Grid(Position position)
        {
            for (var r = position.Row - 1; r <= position.Row + 1; r++)
            for (var c = position.Column - 1; c <= position.Column + 1; c++)
            {
                yield return new Position(r, c);
            }
        }

        public static bool IsInBounds(this Level level, Position position)
        {
            return level != null
                   && position.Row >= 0
                   && position.Column >= 0
                   && position.Row <= level.Rows
                   && position.Column <= level.Columns;
        }
    }
}