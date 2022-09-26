// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class TweetStatisticianWorker : BackgroundService
    {
        private readonly ILogger<TweetStatisticianWorker> _logger;

        private readonly ITweetStatistician _tweetStatistics;

        public TweetStatisticianWorker(
                    ILogger<TweetStatisticianWorker> logger,
                    ITweetStatistician tweetStatistics)
        {
            _logger = logger;
            _tweetStatistics = tweetStatistics;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Invoking {nameof(ExecuteAsync)}");
            await _tweetStatistics.GenerateAsync(stoppingToken);
        }
    }
}
