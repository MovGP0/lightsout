using System.Collections.Generic;
using System.Text;
using LightsOut.Properties;
using Newtonsoft.Json;

namespace LightsOut
{
    public static class LevelsLoader
    {
        public static IEnumerable<Level> GetLevels()
        {
            var file = Resources.lights_out_levels;
            var json = Encoding.Default.GetString(file);
            var levels = JsonConvert.DeserializeObject<List<Level>>(json);
            return levels;
        }
    }
}
