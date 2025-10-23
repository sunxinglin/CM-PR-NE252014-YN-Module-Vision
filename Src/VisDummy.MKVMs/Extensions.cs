using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReactiveUI;
using Splat;
using System;
using VisDummy.Abstractions.Infra;
using VisDummy.MKVMs.MessageHandles;
using VisDummy.MKVMs.MKServices;
using VisDummy.MKVMs.ViewModels;
using VisDummy.Shared.Opts;
using VisDummy.Shared.Utils;

namespace VisDummy.MKVMs
{
    public static class Extensions
    {
        public static void RegisteMKVisionViews(this IServiceProvider sp, IConfiguration config)
        {
            // Vision
            foreach (IConfigurationSection rtVision in config.GetSection("Vision3DOpt").GetChildren())
            {
                Register3DVisRtViewModel(sp, viewName: rtVision.Key);
            }
        }

        public static IServiceCollection AddMKVisionServices(this IServiceCollection services, IConfiguration config, string configPath)
        {
            foreach (IConfigurationSection ctrl3d in config.GetSection("Vision3DOpt").GetChildren())
            {
                services.AddOptions<Vision3DOpt>(ctrl3d.Key).Bind(ctrl3d);
            }
            services.AddSingleton<IMKProc, MKProcImpl>();
            services.AddMediatR(typeof(Vision3DNotificationHandle).Assembly);

            return services;
        }

        #region static
        static void Register3DVisRtViewModel(IServiceProvider sp, string viewName)
        {
            var mediator = sp.GetRequiredService<IMediator>();
            var monitor = sp.GetRequiredService<IOptionsMonitor<Vision3DOpt>>();
            var opts = monitor!.Get(viewName);
            var ctrl = new Vision3DCtrl(opts, mediator);
            Locator.CurrentMutable.RegisterLazySingletonEx<IVisionMarker, Vis3DRtViewModel>(sp, viewName, opts.ProcName, ctrl, opts.Visibility);
        }
        #endregion
    }
}
