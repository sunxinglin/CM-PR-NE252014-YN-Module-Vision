using Microsoft.Extensions.Hosting;

namespace VisDummy.Protocols.模组入箱
{
    public class PlcHostedService : IHostedService
    {
        private readonly 模组入箱Scanner _plcScanner;

        public PlcHostedService(模组入箱Scanner PlcScanner)
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
