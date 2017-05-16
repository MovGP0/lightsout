using System.Collections.Generic;
using System.Linq;

namespace LightsOut.Tests
{
    public static class LevelTestDataBuilderExtensions
    {
        public static Level Level(this TestDataBuilder builder)
        {
            return new Level(string.Empty, 1, 1, Enumerable.Empty<int>());
        }

        public static Level WithName(this Level level, string name)
        {
            return new Level(name, level.Columns, level.Rows, level.On);
        }

        public static Level WithColumns(this Level level, int columns)
        {
            return new Level(level.Name, columns, level.Rows, level.On);
        }

        public static Level WithRows(this Level level, int rows)
        {
            return new Level(level.Name, level.Columns, rows, level.On);
        }

        public static Level WithOn(this Level level, IEnumerable<int> on)
        {
            return new Level(level.Name, level.Columns, level.Rows, on);
        }
    }
}