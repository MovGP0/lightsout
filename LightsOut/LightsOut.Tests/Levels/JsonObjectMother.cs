namespace LightsOut.Tests
{
    public static class JsonObjectMother
    {
        public static string AllLevelsJson => @"[
{ 'name': 'Level 0', 'columns': 4, 'rows': 4, 'on': [ 0, 5, 10, 15 ] },
{ 'name': 'Level 1', 'columns': 5, 'rows': 5, 'on': [ 0, 5, 7, 9, 10, 11, 12, 13, 15, 16, 17, 20, 22, 23 ] },
{ 'name': 'Level 2', 'columns': 9, 'rows': 9, 'on': [ 7, 9, 14, 16, 23, 24, 25, 27, 28, 30, 31, 32, 33, 34, 35, 38, 40, 41, 42, 44, 46, 48, 50, 51, 52, 53, 55, 56, 59, 60, 62, 63, 66, 68, 69, 70, 72, 73, 78, 79 ] }
]";

        public static string Level0Json => @"{'name': 'Level 0', 'columns': 4, 'rows': 4, 'on': [ 0, 5, 10, 15 ]}";
    }
}