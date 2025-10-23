using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StdUnit.One.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisDummy.Shared.Opts;
using VisDummy.Shared.Utils;
using VisDummy.VMs.Common;

namespace VisDummy.VMs.HostedService
{
    public class VmSlnHostedService(IServiceProvider sp, IMediator mediator, ILogger<VmSlnHostedService> logger) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var sopce = sp.CreateScope();
                var s = sopce.ServiceProvider;
                var setting = s.GetRequiredService<IOptions<AppClientSetting>>();
                var loaded = VmHelper.PreLoadSln(setting.Value.VisionSlnPath);
                if (!loaded)
                {
                    await RecordLogAsync(LogLevel.Warning, "配置的方案路径不存在!请稍后手工加载");
                }
            }
            catch (Exception ex)
            {
                await RecordLogAsync(LogLevel.Warning, ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                VmHelper.DisposeSln();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Task.CompletedTask;
        }

        public async Task RecordLogAsync(LogLevel level, string logTemplate, params object[] args)
        {
            var logmsg = string.Format(logTemplate, args);
            var notification = new UILogNotification(new LogMessage
            {
                EventSource = "流程预加载",
                EventGroup = "流程预加载",
                Level = level,
                Content = logmsg,
                Timestamp = DateTime.Now,
            });
            await mediator.Publish(notification);
        }
    }
}
