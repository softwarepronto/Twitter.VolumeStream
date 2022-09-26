// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Interfaces
{
    public interface ITweetStatistics
    {
        ulong TotalTweets { get; }

        IEnumerable<string> TopHashtags { get; }

        void Increment();

        void Increment(IEnumerable<string> hashtags);
    }
}
