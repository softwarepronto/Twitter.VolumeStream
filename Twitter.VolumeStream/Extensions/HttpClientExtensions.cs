
namespace Twitter.VolumeStream.Extensions
{
    public static class HttpClientExtensions
    {
        private const string BearerTokenName = "Bearer";

        public static readonly HttpClient Client = new HttpClient();

        public static void AssignBearerToken(this HttpClient httpClient, string bearerToken)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(BearerTokenName, bearerToken);
        }
    }
}
