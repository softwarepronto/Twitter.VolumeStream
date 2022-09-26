// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class TweetHashtagsStatistics : ITweetHashtagsStatistics
    {
        private const ushort TopHashtagCount = 10;

        private readonly ILogger<TweetHashtagsStatistics> _logger;

        private readonly HashtagCount[] _topHashtagStatistics = new HashtagCount[TopHashtagCount];

        private string[] _topHashTags = new string[0];

        private ulong _leastMostPopularHashtagCount = 0UL;

        public TweetHashtagsStatistics(ILogger<TweetHashtagsStatistics> logger)
        {
            _logger = logger;
        }

        public IEnumerable<string> TopHashtags
        {
            get
            {
                IEnumerable<string>? currentTopHashtags = null;

                Interlocked.Exchange<IEnumerable<string>?>(ref currentTopHashtags, _topHashTags);

                return currentTopHashtags;
            }
        }

        private bool AttemptUpdateExisting(string hashtag, ulong count)
        {
            foreach (var hashtagStatistics in _topHashtagStatistics)
            {
                if (hashtagStatistics == null)
                {
                    break;
                }

                if (hashtag == hashtagStatistics.Hashtag)
                {
                    if (count < hashtagStatistics.Count)
                    {
                        throw new ArgumentException($"Hashtag ({hashtag}) count ({hashtagStatistics.Count} cannot decrease ({count})");
                    }

                    hashtagStatistics.Update(count);

                    return true;
                }
            }

            return false;
        }

        private void UpadateLeastMostPopularHashtagCount()
        {
            _logger.LogInformation("UpadateLeastMostPopularHashtagCount");
            _leastMostPopularHashtagCount = _topHashtagStatistics.Where(hs => hs != null).Min(hs => hs.Count);
        }

        public void Add(string hashtag, ulong count)
        {
            _logger.LogInformation($"Add(hashtag={hashtag}, count={count})");
            if (count <= _leastMostPopularHashtagCount)
            {
                return;
            }

            if (AttemptUpdateExisting(hashtag, count))
            {
                UpadateLeastMostPopularHashtagCount();
            }

            else
            {
                if (_topHashTags.Length == TopHashtagCount)
                {
                    for (var i = 0; i < _topHashTags.Length; i++)
                    {
                        var hashtagCount = _topHashtagStatistics[i];

                        if (hashtagCount.Count == _leastMostPopularHashtagCount)
                        {
                            hashtagCount.Overwrite(hashtag, count);
                            Interlocked.Exchange(ref _topHashTags[i], hashtag);

                            break;
                        }
                    }

                    UpadateLeastMostPopularHashtagCount();
                }

                else // Initiaizing (_topHashTags.Length < TopHashtagCount)
                {
                    var nextTopHashtags = new string[_topHashTags.Length + 1];

                    _topHashtagStatistics[_topHashTags.Length] = new HashtagCount(hashtag, count);
                    for (var i = 0; i < _topHashTags.Length; i++)
                    {
                        nextTopHashtags[i] = _topHashTags[i];
                    }

                    nextTopHashtags[_topHashTags.Length] = hashtag;
                    Interlocked.Exchange<string[]>(ref _topHashTags, nextTopHashtags);
                    if (_topHashTags.Length == TopHashtagCount)
                    {
                        UpadateLeastMostPopularHashtagCount();
                    }
                }
            }
        }
    }
}
