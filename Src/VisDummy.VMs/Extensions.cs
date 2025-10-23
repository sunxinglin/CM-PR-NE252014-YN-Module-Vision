using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ReactiveUI;
using Splat;
using System;
using VisDummy.Abstractions.Infra;
using VisDummy.Shared.Opts;
using VisDummy.Shared.Utils;
using VisDummy.VMs.HIKServices;
using VisDummy.VMs.HostedService;
using VisDummy.VMs.Services.VmSolutionParams;
using VisDummy.VMs.ViewModels;
using VisDummy.VMs.Views;

namespace VisDummy.VMs
{
    public static class Extensions
    {
        public static void RegisteVisionViews(this IServiceProvider sp, IConfiguration config)
        {
            // Vision Master
            Locator.CurrentMutable.RegisterLazySingletonEx<VMSlnViewModel>(sp);
            foreach (IConfigurationSection rtVision in config.GetSection("RtVisSettings").GetChildren())
            {
                RegisterVisRtViewModel(sp, viewName: rtVision.Key);
            }
            RegisterCalibrationViewModel(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<GlobalParamsViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<GlobalParamsViewModel>, GlobalParamsView>(sp);
        }

        public static IServiceCollection AddVisionServices(this IServiceCollection services, IConfiguration config, string configPath)
        {
            foreach (IConfigurationSection rtVision in config.GetSection("RtVisSettings").GetChildren())
            {
                services.AddOptions<RtVisSetting>(rtVision.Key).Bind(rtVision);
            }
            services.AddSingleton<IHostedService, VmSlnHostedService>();
            services.AddSingleton<IVisProc, VisProcImpl>();
            services.AddSingleton<IVisParams, VisParams>(sp => ActivatorUtilities.CreateInstance<VisParams>(sp, configPath));
            return services;
        }

        #region static

        static void RegisterVisRtViewModel(IServiceProvider sp, string viewName)
        {
            var slnvm = Locator.Current.GetService<VMSlnViewModel>();
            var monitor = sp.GetRequiredService<IOptionsMonitor<RtVisSetting>>();
            var opts = monitor!.Get(viewName);
            Locator.CurrentMutable.RegisterLazySingletonEx<IVisionMarker, VisRtViewModel>(sp, viewName, opts.ProcName, opts.Visibility, slnvm);
        }
        static void RegisterCalibrationViewModel(IServiceProvider sp)
        {
            var slnvm = Locator.Current.GetService<VMSlnViewModel>();
            var visproc = sp.GetRequiredService<IVisProc>();
            Locator.CurrentMutable.RegisterLazySingletonEx<ManualVisCalibrationViewModel>(sp, slnvm, visproc);
        }
        #endregion
    }
}
