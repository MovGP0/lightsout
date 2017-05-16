using System.Collections.Generic;

namespace LightsOut
{
    public partial class App
    {
        public IEnumerable<Level> Levels { get; } 
            = LevelsLoader.GetLevels();
        
        public App()
        {
            
        }

    }
}
