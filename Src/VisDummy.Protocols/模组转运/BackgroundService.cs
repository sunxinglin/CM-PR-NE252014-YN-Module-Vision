using Microsoft.Extensions.Hosting;

namespace VisDummy.Protocols.模组转运
{
    public class PlcHostedService : IHostedService
    {
        private readonly 模组转运Scanner _plcScanner;

        public PlcHostedService(模组转运Scanner PlcScanner)
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
