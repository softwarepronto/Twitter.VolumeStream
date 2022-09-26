// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Tests.Implementations
{
    public class TweetStatisticsTests
    {
        private readonly Mock<ILogger<TweetHashtagsStatistics>> _loggerTweetHashtagStatisticsMock =
                                new Mock<ILogger<TweetHashtagsStatistics>>();

        private readonly Mock<ILogger<TweetStatistics>> _loggerTweetStatisticsMock =
                                            new Mock<ILogger<TweetStatistics>>();

        [Fact]
        public void AddOneTweetTest()
        {
            var tweetHashtagStatistics =
                        new TweetHashtagsStatistics(_loggerTweetHashtagStatisticsMock.Object);
            var tweetStatistics = new TweetStatistics(
                                        _loggerTweetStatisticsMock.Object,
                                        tweetHashtagStatistics);
            const ulong MaxNumberOfHashtagInstances = 50;
            var hashtag = "pheonix";
            var hashtags = new string[] { hashtag };

            Assert.Empty(tweetStatistics.TopHashtags);
            for (var count = 1ul; count <= MaxNumberOfHashtagInstances; count++)
            {
                tweetStatistics.Increment(hashtags);
                Assert.Contains(hashtag, tweetHashtagStatistics.TopHashtags);
                Assert.Equal(count, tweetStatistics.TotalTweets);
            }
        }

        [Fact]
        public void AddMultipleTweetsTest()
        {
            const int MaxNumberOfHashtagInstances = 50;
            var hashtags = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            var tweetHashtagStatistics =
                        new TweetHashtagsStatistics(_loggerTweetHashtagStatisticsMock.Object);
            var tweetStatistics = new TweetStatistics(
                                        _loggerTweetStatisticsMock.Object,
                                        tweetHashtagStatistics);
            var totalTweets = 0ul;

            Assert.Empty(tweetStatistics.TopHashtags);
            for (var count = 1ul; count <= MaxNumberOfHashtagInstances; count++)
            {
                tweetStatistics.Increment(hashtags);
                totalTweets++;
                Assert.Equal(totalTweets, tweetStatistics.TotalTweets);
                foreach (var hashtag in hashtags)
                {
                    Assert.Contains(hashtag, tweetHashtagStatistics.TopHashtags);
                }
            }

            var bumpHashtag = "k";
            var bumpHashtags = new string[] { bumpHashtag };

            for (var count = 1ul; count <= (2ul * MaxNumberOfHashtagInstances); count++)
            {
                tweetStatistics.Increment(bumpHashtags);
                totalTweets++;
                Assert.Equal(totalTweets, tweetStatistics.TotalTweets);
                if (count > MaxNumberOfHashtagInstances)
                {
                    Assert.Contains(bumpHashtag, tweetStatistics.TopHashtags);
                }

                else
                {
                    Assert.DoesNotContain(bumpHashtag, tweetStatistics.TopHashtags);
                }
            }
        }

        [Fact]
        public void AddMultipleTweetsReplaceTweeksTest()
        {
            const ulong MaxNumberOfHashtagInstances = 50;
            var hashtags = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            var tweetHashtagStatistics =
                        new TweetHashtagsStatistics(_loggerTweetHashtagStatisticsMock.Object);
            var tweetStatistics = new TweetStatistics(
                                        _loggerTweetStatisticsMock.Object,
                                        tweetHashtagStatistics);
            var totalTweets = 0ul;

            Assert.Empty(tweetStatistics.TopHashtags);
            for (var count = 1ul; count <= MaxNumberOfHashtagInstances; count++)
            {
                tweetStatistics.Increment(hashtags);
                totalTweets++;
                Assert.Equal(totalTweets, tweetStatistics.TotalTweets);
                foreach (var hashtag in hashtags)
                {
                    Assert.Contains(hashtag, tweetHashtagStatistics.TopHashtags);
                }
            }

            var replaceHashtags = new string[] { "k", "l", "m", "n", "o", "p", "q", "r", "s", "t" };

            for (var count = 1ul; count <= (2 * MaxNumberOfHashtagInstances); count++)
            {
                tweetStatistics.Increment(replaceHashtags);
                totalTweets++;
                Assert.Equal(totalTweets, tweetStatistics.TotalTweets);
                foreach (var replaceHashtag in replaceHashtags)
                {
                    if (count > MaxNumberOfHashtagInstances)
                    {
                        Assert.Contains(replaceHashtag, tweetHashtagStatistics.TopHashtags);
                    }

                    else
                    {
                        Assert.DoesNotContain(replaceHashtag, tweetHashtagStatistics.TopHashtags);
                    }
                }
            }
        }
    }
}
