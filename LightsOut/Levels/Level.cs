using System;
using System.Collections.Generic;
using System.Linq;

namespace LightsOut
{
    public sealed class Level
    {
        public Level(string name, int rows, int columns, IEnumerable<int> on)
            : this(name, rows, columns)
        {
            var onArray = on?.OrderBy(t => t).ToArray() 
                ?? throw new ArgumentException(nameof(on));

            if(onArray.Any(o => o < 0))
                throw new ArgumentOutOfRangeException(nameof(on));

            if (onArray.Any(o => o > columns * rows))
                throw new ArgumentOutOfRangeException(nameof(on));

            var onMatrix = new SwitchState[rows, columns];

            foreach (var item in onArray)
            {
                onMatrix[item % columns, item / columns] = SwitchState.On;
            }

            OnMatrix = onMatrix;
        }

        internal Level(string name, int rows, int columns, SwitchState[,] onMatrix) : this(name, rows, columns)
        {
            if(onMatrix.GetLength(0) != rows) throw new ArgumentException(nameof(rows));
            if(onMatrix.GetLength(1) != columns) throw new ArgumentException(nameof(columns));

            OnMatrix = onMatrix;
        }

        private Level(string name, int rows, int columns)
        {
            Name = name ?? throw new ArgumentException(nameof(name));
            if (columns < 1) throw new ArgumentOutOfRangeException(nameof(columns), columns, "Must be at least 1.");
            if (rows < 1) throw new ArgumentOutOfRangeException(nameof(rows), columns, "Must be at least 1.");
        }

        public string Name { get; }
        public int Columns => OnMatrix.GetLength(1);
        public int Rows => OnMatrix.GetLength(0);
        public SwitchState[,] OnMatrix { get; }
        
        private bool Equals(Level other)
        {
            return string.Equals(Name, other.Name) 
                && Columns == other.Columns 
                && Rows == other.Rows 
                && OnMatrix.MatrixEquals(other.OnMatrix);
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
                hashCode = (hashCode * magic) ^ Columns;
                hashCode = (hashCode * magic) ^ Rows;
                hashCode = (hashCode * magic) ^ (OnMatrix != null ? OnMatrix.GetHashCode() : 0b0);
                return hashCode;
            }
        }
    }
}
