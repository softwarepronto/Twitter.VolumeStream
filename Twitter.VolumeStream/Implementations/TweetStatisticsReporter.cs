// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class TweetStatisticsReporter : ITweetStatisticsReporter
    {
        private const char Separator = '\t';

        private const char IntraSeparator = ':';

        private const char HashtagSeparator = ',';


        private readonly ILogger<ITweetStatisticsReporter> _logger;

        private readonly ITweetStatistics _tweetStatistics;

        private readonly IReportSource _resportSource;

        private DateTime _lastReportTime = DateTime.UtcNow;

        private enum DataValueSlots
        {
            Label,
            Data
        }

        private enum ReportSlots
        {
            DateTimeUtc,
            SecondsSinceLastReport,
            Totaltweets,
            TopHashtagCount,
            Hashtags
        }

        public TweetStatisticsReporter(
                    ILogger<ITweetStatisticsReporter> logger,
                    ITweetStatistics tweetStatistics,
                    IReportSource resportSource)
        {
            _logger = logger;
            _tweetStatistics = tweetStatistics;
            _resportSource = resportSource;
        }

        private static string GetValue(string dataValue)
        {
            var labelAndData = dataValue.Split(IntraSeparator);

            return labelAndData[(int)DataValueSlots.Data].Trim();
        }

        public static DateTime GetReportDateTime(string reportText)
        {
            var reportItems = reportText.Split(Separator);
            var dateText = reportItems[(int)ReportSlots.DateTimeUtc];

            return DateTime.Parse(dateText).ToUniversalTime();
        }

        public static decimal GetSecondsSinceLastReport(string reportText)
        {
            var reportItems = reportText.Split(Separator);

            return decimal.Parse(GetValue(reportItems[(int)ReportSlots.SecondsSinceLastReport]));
        }

        public static ulong GetTotalTweets(string reportText)
        {
            var reportItems = reportText.Split(Separator);

            return ulong.Parse(GetValue(reportItems[(int)ReportSlots.Totaltweets]));
        }

        public static ushort GetTopHashtagCount(string reportText)
        {
            var reportItems = reportText.Split(Separator);

            return ushort.Parse(GetValue(reportItems[(int)ReportSlots.TopHashtagCount]));
        }

        public static string[] GetTopHashtags(string reportText)
        {
            var reportItems = reportText.Split(Separator);
            var result = GetValue(reportItems[(int)ReportSlots.Hashtags]).Split(HashtagSeparator);

            if ((result.Length == 1) && string.IsNullOrEmpty(result[0]?.Trim()))
            {
                result = new string[0];
            }

            return result;
        }

        private string GetReportText()
        {
            var hashtagsText = string.Join(HashtagSeparator, _tweetStatistics.TopHashtags);
            var currentReportTime = DateTime.UtcNow;
            var result =
                  $"{currentReportTime.ToString("O")}\t" +
                  $"Report Interval{IntraSeparator} {currentReportTime.Subtract(_lastReportTime).TotalSeconds.ToString("N2")}{Separator}" +
                  $"Total Tweets{IntraSeparator} {_tweetStatistics.TotalTweets}{Separator}" +
                  $"Top Hashtag Count{IntraSeparator} {_tweetStatistics.TopHashtags.Count()}{Separator}" +
                  $"Hashtags{IntraSeparator} {hashtagsText}";

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
