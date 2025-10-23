using Microsoft.Extensions.Hosting;

namespace VisDummy.Protocols.人工位
{
    public class PlcHostedService : IHostedService
    {
        private readonly PlcScanner _plcScanner;

        public PlcHostedService(PlcScanner PlcScanner)
        {
            _plcScanner = PlcScanner;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _plcScanner.ExecuteAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
