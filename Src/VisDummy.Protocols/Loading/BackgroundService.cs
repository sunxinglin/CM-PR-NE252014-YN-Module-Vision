using Microsoft.Extensions.Hosting;

namespace Itminus.Protocols.Loading
{
    public class PlcHostedService : IHostedService
    {
        private readonly LoadingScanner _plcScanner;

        public PlcHostedService(LoadingScanner PlcScanner)
        {
            _plcScanner = PlcScanner;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this._plcScanner.ExecuteAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
