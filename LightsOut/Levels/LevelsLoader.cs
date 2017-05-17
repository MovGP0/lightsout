using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
#if DEBUG
            return GetDebugLevels().ToList();
#else
            return LoadLevelsFromGithub();
#endif
        }

        private static IEnumerable<Level> GetDebugLevels()
        {
            yield return new Level("Level 0", 3, 3, new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
            yield return new Level("Level 1", 3, 3, new [] { 0 });
        }

        private static List<Level> LoadLevelsFromGithub()
        {
            using (var source = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                var json = DownloadFromUrlAsync(Url, source.Token).Result;
                return JsonConvert.DeserializeObject<List<Level>>(json);
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
