using Itminus.Protocols.Loading.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VisDummy.Protocols.Loading.Middlewares;

namespace Itminus.Protocols.Loading
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddPlcServicesForLoading(this IServiceCollection services)
        {
            // background services & plc processor
            services.AddSingleton<IHostedService, PlcHostedService>();
            services.AddSingleton<LoadingFlusher>();
            services.AddSingleton<LoadingScanner>();
            services.AddSingleton<ScanProcessor>();

            #region 中间件
            services.TryAddScoped<HeartBeatMiddleware>();
            services.AddScoped<PublishNotificationMiddleware>();
            services.AddScoped<MaintainMiddleware>();
            services.AddScoped<FlushPendingMiddleware>();

            services.AddScoped<HandleStation2DMiddleware>();
            services.AddScoped<HandleStation2DSpotMiddleware>();
            services.AddScoped<HandleStation3DMiddleware>();
            services.AddScoped<HandleStation3DSpotMiddleware>();
            #endregion

            return services;
        }


    }
}
