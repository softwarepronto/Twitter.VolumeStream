// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class TweetStatisticsReporterWorker : IHostedService, IDisposable
    {
        private const int CycleSeconds = 10;

        private readonly ILogger<TweetStatisticsReporterWorker> _logger;

        private readonly ITweetStatisticsReporter _tweetStatisticsReporter;

        private Timer? _timer = null;

        private bool _disposedValue = false;

        public TweetStatisticsReporterWorker(
                ILogger<TweetStatisticsReporterWorker> logger,
                ITweetStatisticsReporter tweetStatisticsReporter)
        {
            _logger = logger;
            _tweetStatisticsReporter = tweetStatisticsReporter;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Tweet Statistics Reporter Service running.");
            _timer = new Timer(PerformWork,
                               null,
                               TimeSpan.Zero,
                               TimeSpan.FromSeconds(CycleSeconds));

            return Task.CompletedTask;
        }

        private void PerformWork(object? state)
        {
            _logger.LogInformation("Tweet Statistics Reporter Service performing work.");
            _tweetStatisticsReporter.Report();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Tweet Statistics Reporter Service stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                        _timer = null;
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
