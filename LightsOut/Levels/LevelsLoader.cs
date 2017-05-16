using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LightsOut
{
    public static class LevelsLoader
    {
        private static readonly Uri Url = 
            new Uri("https://raw.githubusercontent.com/dranner-bgt/lights-out-assignment/master/levels/lights-out-levels.json");

        public static ICollection<Level> GetLevels()
        {
            using (var source = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                var json = DownloadFromUrlAsync(Url, source.Token).Result;
                var levels = JsonConvert.DeserializeObject<List<Level>>(json);
                return levels;
            }
        }

        private static async Task<string> DownloadFromUrlAsync(Uri uri, CancellationToken token)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(uri, token)
                    .ConfigureAwait(false);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
