// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Tests.TestUtilities
{
    public class TweetArchiveReader : TweatArchive, ITweetReader
    {
        public const string Tweets20220917092751 = "Tweets20220917092751";

        public const string Tweets20220917093203 = "Tweets20220917093203";

        public const string Tweets20220917093621 = "Tweets20220917093621";

        public const string Tweets20220917094037 = "Tweets20220917094037";

        public static readonly string[] TweetArchivesFilenames =
            {
                Tweets20220917092751,
                Tweets20220917093203,
                Tweets20220917093621,
                Tweets20220917094037
            };

        private StreamReader? _streamReader;

        private bool _disposedValue;

        private readonly ushort _duplicateCount;

        private string? _tweetJason = String.Empty;

        private ushort _numberOfDuplicates = 0;

        public TweetArchiveReader(ushort duplicateCount = 0)
        {
            FilePath = Path.Combine(TweetArchiveFolderPath, Tweets20220917092751);
            _disposedValue = false;
            _duplicateCount = duplicateCount;
            _streamReader = new StreamReader(FilePath);
        }


        public string FilePath { get; }

        public async Task<string?> ReadLineAsync()
        {
            if (_duplicateCount == 0)
            {
                return await _streamReader!.ReadLineAsync();
            }

            else
            {
                if ((_numberOfDuplicates == 0) || (_numberOfDuplicates == _duplicateCount))
                {
                    var result = await _streamReader!.ReadLineAsync();

                    _numberOfDuplicates = 1;
                    _tweetJason = result;

                    return result;
                }

                else
                {
                    _numberOfDuplicates++;

                    return _tweetJason;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_streamReader != null)
                    {
                        _streamReader.Dispose();
                        _streamReader = null;
                    }
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

