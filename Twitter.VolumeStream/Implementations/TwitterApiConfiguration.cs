// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class TwitterApiConfiguration : ITwitterApiConfiguration
    {
        public const string Prefix = "TWITTER_API_";

        private readonly ILogger<TwitterApiConfiguration> _logger;

        public string BearerTokenName => Prefix + "BEARER_TOKEN";

        private readonly IConfiguration _configuration;

        public TwitterApiConfiguration(
                    ILogger<TwitterApiConfiguration> logger,
                    IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public string BearerToken => _configuration.GetValue<string>(BearerTokenName);

        public string TwitterApiUrlAttributePath => "TwitterApi:EndPoint:Https:Url";

        public string TwitterApiUrl => _configuration.GetValue<string>(TwitterApiUrlAttributePath);
    }
}
