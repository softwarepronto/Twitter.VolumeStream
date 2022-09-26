// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class TweetClient : ITweetClient
    {
        private const string VolumeStreamSubUrl = @"/tweets/sample/stream?tweet.fields=entities,created_at";

        private readonly string _twitterTweetsStreamUrlV2;

        private readonly ILogger<TweetClient> _logger;

        private readonly IServiceProvider _serviceProvider;

        private readonly ITwitterApiConfiguration _twitterApiConfiguration;

        public TweetClient(
                ILogger<TweetClient> logger,
                IServiceProvider serviceProvider,
                ITwitterApiConfiguration twitterApiConfiguration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _twitterApiConfiguration = twitterApiConfiguration;
            _twitterTweetsStreamUrlV2 =
                _twitterApiConfiguration!.TwitterApiUrl + VolumeStreamSubUrl;
        }

        public async Task<ITweetReader> GetAsync()
        {
            var bearerToken = _twitterApiConfiguration.BearerToken;

            if (bearerToken == null)
            {
                throw new Exception($"Twitter API bearer token not set.");
            }

            HttpClientExtensions.Client.AssignBearerToken(bearerToken);

            var response = await HttpClientExtensions.Client.GetAsync(_twitterTweetsStreamUrlV2, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            return (ITweetReader)ActivatorUtilities.CreateInstance(
                _serviceProvider,
                typeof(TweetReader),
                await response.Content.ReadAsStreamAsync());
        }
    }
}
