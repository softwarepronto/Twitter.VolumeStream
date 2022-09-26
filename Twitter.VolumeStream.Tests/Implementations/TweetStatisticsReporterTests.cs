// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Tests.Implementations
{
    public class TweetStatisticsReporterTests
    {
        private readonly Mock<ILogger<TweetStatisticsReporter>> _loggerTweetStatisticsReporterMock =
                                new Mock<ILogger<TweetStatisticsReporter>>();

        [Fact]
        public void ReportTest()
        {
            var startTime = DateTime.UtcNow;
            var tweetStatisticsMock = new Mock<ITweetStatistics>();
            var reportSourceMock = new Mock<IReportSource>();
            var totalTweets = 0ul;
            var hashtags = new List<string>();
            var hashtagCandidates = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };
            var reportText = string.Empty;
            const int hashTagLength = 12;

            tweetStatisticsMock.SetupGet(m => m.TotalTweets).Returns(() => { return totalTweets; });
            tweetStatisticsMock.SetupGet(m => m.TopHashtags).Returns(hashtags);
            reportSourceMock.Setup(m => m.Write(It.IsAny<string>())).Callback<string>(r => reportText = r);

            var tweetStatisticsReporter = new TweetStatisticsReporter(
                                                _loggerTweetStatisticsReporterMock.Object,
                                                tweetStatisticsMock.Object,
                                                reportSourceMock.Object);

            tweetStatisticsReporter.Report();

            var endTime = DateTime.UtcNow;

            _ = TweetStatisticsReporter.GetSecondsSinceLastReport(reportText);
            Assert.Equal(totalTweets, TweetStatisticsReporter.GetTotalTweets(reportText));
            Assert.Equal((ulong)hashtags.Count, TweetStatisticsReporter.GetTopHashtagCount(reportText));

            var reportDateTime = TweetStatisticsReporter.GetReportDateTime(reportText);
            var reportTopHashTags = TweetStatisticsReporter.GetTopHashtags(reportText);

            Assert.Empty(reportTopHashTags);
            Assert.True(reportDateTime >= startTime);
            Assert.True(reportDateTime <= endTime);

            foreach (var hashtagCandidate in hashtagCandidates)
            {
                totalTweets += 5ul;
                hashtags.Add(new string(hashtagCandidate, hashTagLength));
                startTime = DateTime.UtcNow;
                tweetStatisticsReporter.Report();
                endTime = DateTime.UtcNow;
                _ = TweetStatisticsReporter.GetSecondsSinceLastReport(reportText);
                Assert.Equal(totalTweets, TweetStatisticsReporter.GetTotalTweets(reportText));
                Assert.Equal((ulong)hashtags.Count, TweetStatisticsReporter.GetTopHashtagCount(reportText));
                reportDateTime = TweetStatisticsReporter.GetReportDateTime(reportText);
                reportTopHashTags = TweetStatisticsReporter.GetTopHashtags(reportText);
                Assert.Equal(hashtags.Count, reportTopHashTags.Length);
                Assert.True(reportDateTime >= startTime);
                Assert.True(reportDateTime <= endTime);
                foreach (var hashtag in hashtags)
                {
                    Assert.Contains(hashtag, reportTopHashTags);
                }
            }
        }
    }
}
