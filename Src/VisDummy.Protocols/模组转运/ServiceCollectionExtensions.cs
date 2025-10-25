using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VisDummy.Protocols.模组转运.Middlewares;
using VisDummy.Protocols.模组转运.Middlewares.Common;
using VisDummy.Protocols.模组转运.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.模组转运
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddPlcServicesFor模组转运(this IServiceCollection services)
        {
            // background services & plc processor
            services.AddSingleton<IHostedService, PlcHostedService>();
            services.AddSingleton<模组转运Flusher>();
            services.AddSingleton<模组转运Scanner>();
            services.AddSingleton<ScanProcessor>();

            #region 中间件
            services.TryAddScoped<HeartBeatMiddleware>();
            services.AddScoped<PublishNotificationMiddleware>();
            services.AddScoped<MaintainMiddleware>();
            services.AddScoped<FlushPendingMiddleware>();

            services.AddScoped<HandleCAM1Middleware>();
			services.AddScoped<HandleCAM4Middleware>();
			services.AddScoped<HandleCAM2Middleware>();
			services.AddScoped<HandleCAM4Middleware>();
			services.AddScoped<HandleCAM5Middleware>();
			services.AddScoped<HandleCAM3Middleware>();
			//services.AddScoped<HandleStation2DSpotMiddleware>();
			#endregion

			return services;
        }


    }
}
