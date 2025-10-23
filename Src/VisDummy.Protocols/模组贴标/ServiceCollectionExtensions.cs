using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VisDummy.Protocols.模组贴标.Middlewares;
using VisDummy.Protocols.模组贴标.Middlewares.Common;
using VisDummy.Protocols.模组贴标.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.模组贴标
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddPlcServicesFor模组贴标(this IServiceCollection services)
        {
            // background services & plc processor
            services.AddSingleton<IHostedService, PlcHostedService>();
            services.AddSingleton<模组贴标Flusher>();
            services.AddSingleton<模组贴标Scanner>();
            services.AddSingleton<ScanProcessor>();

            #region 中间件
            services.TryAddScoped<HeartBeatMiddleware>();
            services.AddScoped<PublishNotificationMiddleware>();
            services.AddScoped<MaintainMiddleware>();
            services.AddScoped<FlushPendingMiddleware>();

            services.AddScoped<HandleStation2DMiddleware>();
            services.AddScoped<HandleStation2DSpotMiddleware>();
            #endregion

            return services;
        }


    }
}
