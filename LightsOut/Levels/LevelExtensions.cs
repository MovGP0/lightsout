using System;
using System.Collections.Generic;

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
        
        public static void Switch8Position(this ISwitchViewModel viewModel)
        {
            var currentState = viewModel.State;
            switch (currentState)
            {
                case SwitchState.OnPressed:
                case SwitchState.Off:
                    viewModel.State = SwitchState.On;
                    return;
                case SwitchState.OffPressed:
                case SwitchState.On:
                    viewModel.State = SwitchState.Off;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}