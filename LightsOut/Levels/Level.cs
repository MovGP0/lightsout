using System;
using System.Collections.Generic;
using System.Linq;
using LightsOut.Properties;

namespace LightsOut
{
    public sealed class Level
    {
        #region Constructors
        public Level(string name, int rows, int columns, IEnumerable<int> on)
            : this(name, rows, columns)
        {
            var onArray = on?.OrderBy(t => t).ToArray()
                ?? throw new ArgumentException(nameof(on));

            if (onArray.Any(o => o < 0))
                throw new ArgumentOutOfRangeException(nameof(on));

            if (onArray.Any(o => o > columns * rows))
                throw new ArgumentOutOfRangeException(nameof(on));

            var onMatrix = new SwitchState[rows, columns];

            foreach (var item in onArray)
            {
                onMatrix[item % columns, item / columns] = SwitchState.On;
            }

            Matrix = onMatrix;
        }

        internal Level(string name, int rows, int columns, SwitchState[,] matrix) : this(name, rows, columns)
        {
            if (matrix.GetLength(0) != rows) throw new ArgumentException(nameof(rows));
            if (matrix.GetLength(1) != columns) throw new ArgumentException(nameof(columns));

            Matrix = matrix;
        }

        private Level(string name, int rows, int columns)
        {
            Name = name ?? throw new ArgumentException(nameof(name));
            if (columns < 1) throw new ArgumentOutOfRangeException(nameof(columns), columns, Resources.MustBeAtLeastOne);
            if (rows < 1) throw new ArgumentOutOfRangeException(nameof(rows), columns, Resources.MustBeAtLeastOne);
        }
        #endregion

        public string Name { get; }
        public int Columns => Matrix.GetLength(1);
        public int Rows => Matrix.GetLength(0);

        internal SwitchState[,] Matrix { get; }
        public SwitchState this[Position position] => Matrix[position.Row, position.Column];

        #region Equality
        private bool Equals(Level other)
        {
            return string.Equals(Name, other.Name)
                && Columns == other.Columns
                && Rows == other.Rows
                && Matrix.MatrixEquals(other.Matrix);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var a = obj as Level;
            return a != null && Equals(a);
        }

        public override int GetHashCode()
        {
            const int magic = 0x18D;
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0b0);
                hashCode = (hashCode * magic) ^ (Matrix != null ? Matrix.GetHashCode() : 0b0);
                return hashCode;
            }
        }
        #endregion

        public override string ToString()
        {
            return $"{Name} [{GetHashCode()}]";
        }
    }
}
