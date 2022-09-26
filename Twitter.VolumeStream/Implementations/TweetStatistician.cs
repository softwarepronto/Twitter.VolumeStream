// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class TweetStatistician : ITweetStatistician
    {
        public const string ContainsHashtagMarker = "\"hashtags\":";

        private readonly ILogger<TweetStatistician> _logger;

        private readonly ITweetClient _tweetClient;

        private readonly ITweetStatistics _tweetStatistics;

        public TweetStatistician(
                    ILogger<TweetStatistician> logger,
                    ITweetClient tweetClient,
                    ITweetStatistics tweetStatistics)
        {
            _logger = logger;
            _tweetClient = tweetClient;
            _tweetStatistics = tweetStatistics;
        }

        public async Task GenerateAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Invoking {nameof(GenerateAsync)}");

            using var tweetReader = await _tweetClient.GetAsync();

            while (!(stoppingToken.IsCancellationRequested))
            {
                var tweetJson = await tweetReader.ReadLineAsync();

                if (string.IsNullOrEmpty(tweetJson))
                {
                    continue;
                }

                if (tweetJson.Contains(ContainsHashtagMarker))
                {
                    var root = JsonSerializer.Deserialize<Root>((string)tweetJson);

                    if (root == null)
                    {
                        continue; // warning
                    }

                    _tweetStatistics.Increment(root.data.entities.GetHashtagNames());
                }

                else
                {
                    _tweetStatistics.Increment();
                }
            }

        }
    }
}
