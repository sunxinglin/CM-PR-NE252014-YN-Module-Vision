using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VisDummy.Protocols.模组入箱.Middlewares;
using VisDummy.Protocols.模组入箱.Middlewares.Common;
using VisDummy.Protocols.模组入箱.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.模组入箱
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddPlcServicesFor模组入箱(this IServiceCollection services)
        {
            // background services & plc processor
            services.AddSingleton<IHostedService, PlcHostedService>();
            services.AddSingleton<模组入箱Flusher>();
            services.AddSingleton<模组入箱Scanner>();
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
