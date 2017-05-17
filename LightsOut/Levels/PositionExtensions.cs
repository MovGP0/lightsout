using System.Collections.Generic;

namespace LightsOut
{
    public static class PositionExtensions
    {
        public static IEnumerable<Position> Get9Positions(this Position position)
        {
            for (var r = position.Row - 1; r <= position.Row + 1; r++)
            for (var c = position.Column - 1; c <= position.Column + 1; c++)
            {
                yield return new Position(r, c);
            }
        }
        
        public static bool IsInBounds(this Position position, int rows, int columns)
        {
            return position.Row >= 0
                   && position.Column >= 0
                   && position.Row <= rows
                   && position.Column <= columns;
        }
    }
}