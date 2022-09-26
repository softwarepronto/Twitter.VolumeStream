// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Tests.TestUtilities
{
    public class TweetArchiveTests
    {
        [Fact]
        public async void ReadOneAtATimeTest()
        {
            var tweetArchiveReader = new TweetArchiveReader();
            var count = 0;

            while (true)
            {
                var tweetJson = await tweetArchiveReader.ReadLineAsync();

                if (tweetJson == null)
                {
                    break;
                }

                if (tweetJson.Contains(TweetStatistician.ContainsHashtagMarker))
                {
                    var root = JsonSerializer.Deserialize<Root>((string)tweetJson);

                    Assert.True(root!.data.entities.GetHashtagNames().Any());
                }

                count++;
            }

            var tweetsInArchive = File.ReadLines(tweetArchiveReader.FilePath).Count();

            Assert.Equal(tweetsInArchive, count);
        }

        [Fact]
        public async void ReadMultipleAtATimeTest()
        {
            var duplicateTweets = (ushort)10;
            var tweetArchiveReader = new TweetArchiveReader(duplicateTweets);
            var count = 0;
            var duplicateCount = (ushort)0;
            var currentTweet = string.Empty;

            while (true)
            {
                var tweetJson = await tweetArchiveReader.ReadLineAsync();

                if (tweetJson == null)
                {
                    break;
                }

                Assert.True(tweetJson.Length > 0);
                if (duplicateCount == 0)
                {
                    currentTweet = tweetJson;
                }

                else
                {
                    Assert.Equal(currentTweet, tweetJson);
                    duplicateCount++;
                    if (duplicateCount == duplicateTweets)
                    {
                        duplicateCount = 0;
                        currentTweet = string.Empty;
                    }
                }

                if (tweetJson.Contains(TweetStatistician.ContainsHashtagMarker))
                {
                    var root = JsonSerializer.Deserialize<Root>((string)tweetJson);

                    Assert.True(root!.data.entities.GetHashtagNames().Any());
                }

                Console.WriteLine($"Tweet number {count}");
                count++;
            }

            var tweetsInArchive = File.ReadLines(tweetArchiveReader.FilePath).Count();

            Assert.Equal(tweetsInArchive, count / duplicateTweets);
        }
    }
}
