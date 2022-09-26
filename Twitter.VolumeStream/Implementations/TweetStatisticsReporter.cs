// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class TweetStatisticsReporter : ITweetStatisticsReporter
    {
        private readonly ILogger<ITweetStatisticsReporter> _logger;

        private readonly ITweetStatistics _tweetStatistics;

        private readonly IReportSource _resportSource;

        private DateTime _lastReportTime = DateTime.UtcNow;

        public TweetStatisticsReporter(
                    ILogger<ITweetStatisticsReporter> logger,
                    ITweetStatistics tweetStatistics,
                    IReportSource resportSource)
        {
            _logger = logger;
            _tweetStatistics = tweetStatistics;
            _resportSource = resportSource;
        }

        private string GetReportText()
        {
            var hashtagsText = string.Join(",", _tweetStatistics.TopHashtags);
            var currentReportTime = DateTime.UtcNow;
            var result =
                  $"{currentReportTime.Subtract(_lastReportTime).TotalSeconds.ToString("N2")}: " +
                  $"Total tweets: {_tweetStatistics.TotalTweets}, " +
                  $"Top {_tweetStatistics.TopHashtags.Count()} hashtags {hashtagsText}";

            _lastReportTime = currentReportTime;

            return result;
        }

        public void Report()
        {
            _logger.LogInformation("Tweet statitics reported");
            _resportSource.Write(GetReportText());
        }
    }
}
