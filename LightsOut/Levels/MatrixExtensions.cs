using System;

namespace LightsOut
{
    public static class MatrixExtensions 
    {
        public static bool MatrixEquals<T>(this T[,] left, T[,] right)
            where T : IComparable
        {
            if (ReferenceEquals(left, right)) return true;
            if (ReferenceEquals(left, null)) return false;
            if (ReferenceEquals(right, null)) return false;

            if (left.GetLength(0) != right.GetLength(0)) return false;
            if (left.GetLength(1) != right.GetLength(1)) return false;

            for (var row = 0; row < left.GetLength(0); row++)
            for (var col = 0; col < left.GetLength(1); col++)
            {
                if (left[row, col].CompareTo(right[row, col]) != 0)
                    return false;
            }

            return true;
        }
    }
}