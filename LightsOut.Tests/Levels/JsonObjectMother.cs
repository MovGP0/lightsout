using System.Text;
using LightsOut.Tests.Properties;

namespace LightsOut.Tests
{
    public static class JsonObjectMother
    {
        public static string AllLevelsJson => Encoding.Default.GetString(Resources.LightsOutLevels);
        public static string Level0Json => @"{'name': 'Level 0', 'columns': 4, 'rows': 4, 'on': [ 0, 5, 10, 15 ]}";
    }
}