using Microsoft.Extensions.Hosting;

namespace VisDummy.Protocols.侧板自动拧紧
{
    public class PlcHostedService : IHostedService
    {
        private readonly 侧板自动拧紧Scanner _plcScanner;

        public PlcHostedService(侧板自动拧紧Scanner PlcScanner)
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
