using System;
using System.Collections.Generic;
using System.Linq;

namespace LightsOut
{
    public sealed class Level
    {
        public Level(string name, int columns, int rows, IEnumerable<int> on)
        {
            Name = name ?? throw new ArgumentException(nameof(name));
            
            if (columns < 1) throw new ArgumentOutOfRangeException(nameof(columns), columns, "Must be at least 1.");
            Columns = columns;

            if (rows < 1) throw new ArgumentOutOfRangeException(nameof(rows), columns, "Must be at least 1.");
            Rows = rows;

            var onArray = on?.OrderBy(t => t).ToArray() 
                ?? throw new ArgumentException(nameof(on));

            if(onArray.Any(o => o < 0))
                throw new ArgumentOutOfRangeException(nameof(on));

            if (onArray.Any(o => o > columns * rows))
                throw new ArgumentOutOfRangeException(nameof(on));

            On = onArray;
        }

        public string Name { get; }
        public int Columns { get; }
        public int Rows { get; }
        public IEnumerable<int> On { get; }

        private bool Equals(Level other)
        {
            return string.Equals(Name, other.Name) 
                && Columns == other.Columns 
                && Rows == other.Rows 
                && On.SequenceEqual(other.On);
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
                hashCode = (hashCode * magic) ^ (On != null ? On.GetHashCode() : 0b0);
                return hashCode;
            }
        }
    }
}
