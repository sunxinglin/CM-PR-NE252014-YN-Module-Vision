using Itminus.Protocols.Loading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StdUnit.Sharp7.Options;
using VisDummy.Protocols.Vision;
using VisDummy.Protocols.人工位;
using VisDummy.Protocols.侧板自动拧紧;
using VisDummy.Protocols.水冷板抓取;

namespace Itminus.Protocols
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Plc相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddPlcServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddS7PlcOptions(config.GetSection("PlcConnections"), config.GetSection("PlcScanOpts"));
            services.AddPlcServicesForLoading();
            services.AddPlcServicesFor水冷板抓取();
            services.AddPlcServicesFor人工位();
            services.AddPlcServicesFor侧板自动拧紧();
            //services.AddPlcServicesFor模组检测();
            //services.AddPlcServicesFor模组贴标();
            //services.AddPlcServicesFor垫片检测();
            //services.AddPlcServicesFor模组入箱();


            services.AddHostedService<CameraStatusBackgroundService>();

            return services;
        }

    }
}
