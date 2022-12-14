// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Interfaces
{
    public interface ITweetStatistician
    {
        Task GenerateAsync(CancellationToken stoppingToken);
    }
}
