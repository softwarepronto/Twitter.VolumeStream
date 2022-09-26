// Licensed to the softwarepronto.com blog under the GNU General Public License.

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Twitter.VolumeStream.Extensions
{
    public static class IConfigurationBuilderExtensions
    {
        public static void AddTwitterApiHostConfiguration(this IConfigurationBuilder configHost)
        {
            configHost.AddEnvironmentVariables(prefix: TwitterApiConfiguration.Prefix);
        }
    }
}
