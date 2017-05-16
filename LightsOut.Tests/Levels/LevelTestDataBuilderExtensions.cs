using System.Collections.Generic;
using System.Linq;

namespace LightsOut.Tests
{
    public static class LevelTestDataBuilderExtensions
    {
        public static Level Level(this TestDataBuilder builder, string name, int rows, int columns)
        {
            return new Level(name, rows, columns, Enumerable.Empty<int>());
        }
        
        public static Level WithOn(this Level level, IEnumerable<int> on)
        {
            return new Level(level.Name, level.Rows, level.Columns, on);
        }
    }
}