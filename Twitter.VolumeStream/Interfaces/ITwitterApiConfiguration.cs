// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Interfaces
{
    public interface ITwitterApiConfiguration
    {
        string BearerTokenName { get; }

        string BearerToken { get; }

        string TwitterApiUrlAttributePath { get; }

        string TwitterApiUrl { get; }
    }
}
