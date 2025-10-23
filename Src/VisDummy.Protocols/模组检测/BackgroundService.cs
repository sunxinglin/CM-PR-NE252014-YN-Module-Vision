using Microsoft.Extensions.Hosting;
using VisDummy.Protocols.模组检测;

namespace Itminus.Protocols.模组检测
{
    public class PlcHostedService : IHostedService
    {
        private readonly 模组检测Scanner _plcScanner;

        public PlcHostedService(模组检测Scanner PlcScanner)
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
