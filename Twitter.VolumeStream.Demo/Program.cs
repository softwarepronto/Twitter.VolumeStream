// Licensed to the softwarepronto.com blog under the GNU General Public License.

const int StatusSuccess = 1;
const int StatusError = 1;
var existStatus = StatusSuccess;

try
{
    using var host = Host.CreateDefaultBuilder(args).ConfigureServices(
        services =>
        {
            services.AddTwitterApiServices();
        })
        .ConfigureHostConfiguration(configurationHost =>
        {
            configurationHost.AddTwitterApiHostConfiguration();
        })
        .ConfigureAppConfiguration((hostingContext, configurationApp) =>
        {
            hostingContext.AddTwitterApiAppConfiguration(configurationApp);
        }).ConfigureLogging((hostingContext, logging) =>
        {
            logging.AddTwitterApiLogging(hostingContext);
        })
        .Build();

    await host.RunAsync();
}

catch (Exception ex)
{
    Console.WriteLine($"{ex}");
    existStatus = StatusError;
}

return existStatus;
