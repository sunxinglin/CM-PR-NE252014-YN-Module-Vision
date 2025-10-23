using Microsoft.Extensions.Hosting;

namespace VisDummy.Protocols.垫片检测
{
    public class PlcHostedService : IHostedService
    {
        private readonly 垫片检测Scanner _plcScanner;

        public PlcHostedService(垫片检测Scanner PlcScanner)
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
