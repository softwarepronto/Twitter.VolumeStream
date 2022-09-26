// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Tests.Implementations
{
    public class TweetHashtagStatisticsTests
    {
        private readonly Mock<ILogger<TweetHashtagsStatistics>> _loggerTweetHashtagStatisticsMock =
                                new Mock<ILogger<TweetHashtagsStatistics>>();

        [Fact]
        public void AddOneTweetTest()
        {
            const int MaxNumberOfHashtagInstances = 50;
            var hashtag = "buddy_holly";
            var tweetHashtagStatistics =
                        new TweetHashtagsStatistics(_loggerTweetHashtagStatisticsMock.Object);

            Assert.Empty(tweetHashtagStatistics.TopHashtags);
            for (var count = 1; count <= MaxNumberOfHashtagInstances; count++)
            {
                tweetHashtagStatistics.Add(hashtag, (ulong)count);
                Assert.Contains(hashtag, tweetHashtagStatistics.TopHashtags);
            }
        }

        [Fact]
        public void AddMultipleTweetsTest()
        {
            const int MaxNumberOfHashtagInstances = 50;
            var hashtags = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            var tweetHashtagStatistics =
                        new TweetHashtagsStatistics(_loggerTweetHashtagStatisticsMock.Object);

            Assert.Empty(tweetHashtagStatistics.TopHashtags);
            for (var count = 1; count <= MaxNumberOfHashtagInstances; count++)
            {
                foreach (var hashtag in hashtags)
                {
                    tweetHashtagStatistics.Add(hashtag, (ulong)count);
                    Assert.Contains(hashtag, tweetHashtagStatistics.TopHashtags);
                }
            }

            var bumpHashtag = "k";

            for (var count = 1; count <= (2 * MaxNumberOfHashtagInstances); count++)
            {
                tweetHashtagStatistics.Add(bumpHashtag, (ulong)count);
                if (count > MaxNumberOfHashtagInstances)
                {
                    Assert.Contains(bumpHashtag, tweetHashtagStatistics.TopHashtags);
                }

                else
                {
                    Assert.DoesNotContain(bumpHashtag, tweetHashtagStatistics.TopHashtags);
                }
            }
        }

        [Fact]
        public void AddMultipleTweetsReplaceTweeksTest()
        {
            const int MaxNumberOfHashtagInstances = 50;
            var hashtags = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            var tweetHashtagStatistics =
                        new TweetHashtagsStatistics(_loggerTweetHashtagStatisticsMock.Object);

            Assert.Empty(tweetHashtagStatistics.TopHashtags);
            for (var count = 1; count <= MaxNumberOfHashtagInstances; count++)
            {
                foreach (var hashtag in hashtags)
                {
                    tweetHashtagStatistics.Add(hashtag, (ulong)count);
                    Assert.Contains(hashtag, tweetHashtagStatistics.TopHashtags);
                }
            }

            var replaceHashtags = new string[] { "k", "l", "m", "n", "o", "p", "q", "r", "s", "t" };

            for (var replacementHashtagsCount = 0; replacementHashtagsCount < replaceHashtags.Length; replacementHashtagsCount++)
            {
                var replaceHashtag = replaceHashtags[replacementHashtagsCount];

                for (var count = 1; count <= (2 * MaxNumberOfHashtagInstances); count++)
                {
                    tweetHashtagStatistics.Add(replaceHashtag, (ulong)count);
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
