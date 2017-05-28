using System.Collections.Generic;

namespace LightsOut
{
    public interface ILevelsLoader
    {
        ICollection<Level> GetLevels();
    }
}