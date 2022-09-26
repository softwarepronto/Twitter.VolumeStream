// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Extensions
{
    public static class HostBuilderContextExtensions
    {
        private const string AppSettingsFilenameExtension = ".json";

        private const string AppSettingsFilename = $"appsettings{AppSettingsFilenameExtension}";

        public static void AddTwitterApiAppConfiguration(
                            this HostBuilderContext hostingContext,
                            IConfigurationBuilder configHost)
        {
            var env = hostingContext.HostingEnvironment;
            var appSettingsEnvironmentFilename = Path.ChangeExtension(
                    AppSettingsFilename,
                    $".{env.EnvironmentName}{AppSettingsFilenameExtension}");

            configHost
                .AddJsonFile(AppSettingsFilename, optional: true, reloadOnChange: true)
                .AddJsonFile(appSettingsEnvironmentFilename, true, true);

        }
    }
}
