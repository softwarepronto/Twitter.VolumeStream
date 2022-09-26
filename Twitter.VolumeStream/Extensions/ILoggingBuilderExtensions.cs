// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Extensions
{
    public static class ILoggingBuilderExtensions
    {
        private const string LoggingSectionName = "Logging";

        public static void AddTwitterApiLogging(
                            this ILoggingBuilder loggingBuilder,
                            HostBuilderContext hostingContext)
        {
            loggingBuilder.AddConfiguration(
                hostingContext.Configuration.GetSection(LoggingSectionName));
        }
    }
}
