using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VisDummy.Protocols.水冷板抓取.Middlewares;
using VisDummy.Protocols.水冷板抓取.Middlewares.Common;
using VisDummy.Protocols.水冷板抓取.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.水冷板抓取
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddPlcServicesFor水冷板抓取(this IServiceCollection services)
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

            services.AddScoped<Handle工位1定位引导Middleware>();
            services.AddScoped<Handle工位2定位引导Middleware>();
            services.AddScoped<Handle工位1校验Middleware>();
            services.AddScoped<Handle工位2校验Middleware>();
            services.AddScoped<Handle工位1自动校准Middleware>();
            services.AddScoped<Handle工位2自动校准Middleware>();
            #endregion

            return services;
        }


    }
}
