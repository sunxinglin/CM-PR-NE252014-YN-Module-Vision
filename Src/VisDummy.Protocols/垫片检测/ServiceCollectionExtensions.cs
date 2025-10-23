using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VisDummy.Protocols.垫片检测.Middlewares;
using VisDummy.Protocols.垫片检测.Middlewares.Common;
using VisDummy.Protocols.垫片检测.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.垫片检测
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddPlcServicesFor垫片检测(this IServiceCollection services)
        {
            // background services & plc processor
            services.AddSingleton<IHostedService, PlcHostedService>();
            services.AddSingleton<垫片检测Flusher>();
            services.AddSingleton<垫片检测Scanner>();
            services.AddSingleton<ScanProcessor>();

            #region 中间件
            services.TryAddScoped<HeartBeatMiddleware>();
            services.AddScoped<PublishNotificationMiddleware>();
            services.AddScoped<MaintainMiddleware>();
            services.AddScoped<FlushPendingMiddleware>();

            services.AddScoped<HandleStation12DMiddleware>();
            services.AddScoped<HandleStation2DSpotMiddleware>();
            #endregion

            return services;
        }


    }
}
