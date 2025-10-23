using Itminus.Protocols;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisDummy.MKVMs;
using VisDummy.MtMes;
using VisDummy.Shared.Utils;
using VisDummy.VMs;
using VisDummy.WPF.Handlers;
using VisDummy.WPF.ViewModels;
using VisDummy.WPF.ViewModels.Monitor;
using VisDummy.WPF.Views.Basics;
using VisDummy.WPF.Views.Monitor;

namespace VisDummy.WPF
{
    public static class Extensions
    {
        public static void RegisterVisDummyViews(this IServiceProvider sp, IConfiguration config)
        {

            // RtView
            Locator.CurrentMutable.RegisterLazySingletonEx<IRtMarker, RtViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingleton(() => Locator.Current.GetRequiredService<IRtMarker>() as RtViewModel);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<RtViewModel>, RtView>(sp);

            // DbgView
            Locator.CurrentMutable.RegisterLazySingletonEx<IDbgMarker, DbgViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingleton(() => Locator.Current.GetRequiredService<IDbgMarker>() as DbgViewModel);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<DbgViewModel>, DbgView>(sp);

            // PamsView
            Locator.CurrentMutable.RegisterLazySingletonEx<IPamsMarker, PamsViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingleton(() => Locator.Current.GetRequiredService<IPamsMarker>() as PamsViewModel);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<PamsViewModel>, PamsView>(sp);

            //Monitor
            Locator.CurrentMutable.RegisterLazySingletonEx<LoadingMonitorViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<水冷板抓取MonitorViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<人工位MonitorViewModel>(sp);

			Locator.CurrentMutable.RegisterLazySingletonEx<侧板自动拧紧MonitorViewModel>(sp);


			Locator.CurrentMutable.RegisterLazySingletonEx<模组检测MonitorViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<模组贴标MonitorViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<垫片检测MonitorViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<模组入箱MonitorViewModel>(sp);

            //MT MES
            sp.RegisterMtMesViews();

            sp.RegisteVisionViews(config);
            sp.RegisteMKVisionViews(config);
        }

        public static IServiceCollection AddVisDummyServices(this IServiceCollection services, IConfiguration config, string configPath)
        {

            services.AddVisionServices(config, configPath);
            services.AddMKVisionServices(config, configPath);

            services.AddPlcServices(config);


            services.AddMtMesServices(config);

            services.AddMediatR(typeof(LoadingScanContextNotificationHandler).Assembly);
            return services;
        }
    }
}
