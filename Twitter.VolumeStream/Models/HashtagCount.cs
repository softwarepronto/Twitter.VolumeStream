// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Models
{
    // jdn more tests -- how many construtors do you need
    public class HashtagCount
    {
        private ulong _count;

        private string _hashtag;

        public HashtagCount(string hashtag)
        {
            _hashtag = hashtag;
            _count = 1;
        }

        public HashtagCount(string hashtag, ulong count)
        {
            _hashtag = hashtag;
            _count = count;
        }

        public void Overwrite(string hashtag, ulong count)
        {
            Interlocked.Exchange(ref _count, count);
            Interlocked.Exchange(ref _hashtag, hashtag);
        }

        public void Update(ulong count)
        {
            Interlocked.Exchange(ref _count, count);
        }

        public string Hashtag => _hashtag;

        public ulong Count => _count;

        public void Increment()
        {
            Interlocked.Increment(ref _count);
        }
    }
}
