using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VisDummy.Protocols.人工位.Middlewares;
using VisDummy.Protocols.人工位.Middlewares.Common;
using VisDummy.Protocols.人工位.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.人工位
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddPlcServicesFor人工位(this IServiceCollection services)
        {
            // background services & plc processor
            services.AddSingleton<IHostedService, PlcHostedService>();
            services.AddSingleton<PlcCtrlFlusher>();
            services.AddSingleton<PlcScanner>();
            services.AddSingleton<ScanProcessor>();

            #region 中间件
            services.TryAddScoped<HeartBeatMiddleware>();
            services.AddScoped<PublishNotificationMiddleware>();
            services.AddScoped<MaintainMiddleware>();
            services.AddScoped<FlushPendingMiddleware>();

            services.AddScoped<Handle水冷板检测Middleware>();
            #endregion

            return services;
        }


    }
}
