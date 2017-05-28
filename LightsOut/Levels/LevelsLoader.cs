using System;
using System.Collections.Generic;
using System.Net;
#if DEBUG
using System.Linq;
#endif
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LightsOut.Properties;
using Newtonsoft.Json;

namespace LightsOut
{
    public sealed class LevelsLoader : ILevelsLoader
    {
        public Func<HttpClient> HttpClientFactory { get; }

        private static readonly Uri Url =
            new Uri("https://raw.githubusercontent.com/dranner-bgt/lights-out-assignment/master/levels/lights-out-levels.json");

        public LevelsLoader(Func<HttpClient> httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public ICollection<Level> GetLevels()
        {
#if DEBUG
            return GetDebugLevels().ToList();
#else
            return LoadLevelsFromGithub();
#endif
        }

        private static IEnumerable<Level> GetDebugLevels()
        {
            yield return new Level("Level 1", 3, 3, new[] { 0, 1, 3, 4 });
            yield return new Level("Level 0", 3, 3, new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        private List<Level> LoadLevelsFromGithub()
        {
            using (var source = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                var json = DownloadFromUrlAsync(Url, source.Token).Result;
                return JsonConvert.DeserializeObject<List<Level>>(json);
            }
        }

        private async Task<string> DownloadFromUrlAsync(Uri uri, CancellationToken token)
        {
            using (var client = HttpClientFactory())
            {
                var result = await client.GetAsync(uri, token)
                    .ConfigureAwait(false);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return Encoding.Default.GetString(Resources.lights_out_levels);
                }

                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
