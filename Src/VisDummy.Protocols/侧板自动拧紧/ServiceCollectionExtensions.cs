using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using VisDummy.Protocols.侧板自动拧紧.Middlewares;
using VisDummy.Protocols.侧板自动拧紧.Middlewares.Common;
using VisDummy.Protocols.侧板自动拧紧.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.侧板自动拧紧
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddPlcServicesFor侧板自动拧紧(this IServiceCollection services)
        {
            // background services & plc processor
            services.AddSingleton<IHostedService, PlcHostedService>();
            services.AddSingleton<侧板自动拧紧Flusher>();
            services.AddSingleton<侧板自动拧紧Scanner>();
            services.AddSingleton<ScanProcessor>();

            #region 中间件
            services.TryAddScoped<HeartBeatMiddleware>();
            services.AddScoped<PublishNotificationMiddleware>();
            services.AddScoped<MaintainMiddleware>();
            services.AddScoped<FlushPendingMiddleware>();

            services.AddScoped<HandleCAM1Middleware>();
			services.AddScoped<HandleCAM2Middleware>();
			services.AddScoped<HandleCAM3Middleware>();
			//services.AddScoped<HandleStation2DSpotMiddleware>();
			#endregion

			return services;
        }


    }
}
