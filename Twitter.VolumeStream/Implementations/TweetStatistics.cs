// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class TweetStatistics : ITweetStatistics
    {
        private ulong _totalTweets = 0UL;

        private readonly Dictionary<string, ulong> _hashTagCounts = new Dictionary<string, ulong>();

        private readonly ITweetHashtagsStatistics _tweetHashtagStatistics;

        private readonly ILogger<TweetStatistics> _logger;

        public TweetStatistics(ILogger<TweetStatistics> logger, ITweetHashtagsStatistics tweetHashtagStatistics)
        {
            _logger = logger;
            _tweetHashtagStatistics = tweetHashtagStatistics;
        }

        public ulong TotalTweets
        {
            get
            {
                return Interlocked.Read(ref _totalTweets);
            }
        }

        public IEnumerable<string> TopHashtags => _tweetHashtagStatistics.TopHashtags;

        public void Increment()
        {
            _logger.LogInformation($"Invoking {nameof(Increment)}(current={_totalTweets}, next={_totalTweets + 1ul}");
            Interlocked.Increment(ref _totalTweets);
        }

        public void Increment(IEnumerable<string> hashtags)
        {
            var currentHashtagCount = 0UL;

            Increment();
            _logger.LogInformation($"Invoking {nameof(Increment)} hashtags={string.Join(',', hashtags)}");
            foreach (var hashtag in hashtags)
            {
                if (_hashTagCounts.ContainsKey(hashtag))
                {
                    currentHashtagCount = ++_hashTagCounts[hashtag];
                }

                else
                {
                    _hashTagCounts.Add(hashtag, 1);
                    currentHashtagCount = 1;
                }

                _tweetHashtagStatistics.Add(hashtag, currentHashtagCount);
            }
        }
    }
}
