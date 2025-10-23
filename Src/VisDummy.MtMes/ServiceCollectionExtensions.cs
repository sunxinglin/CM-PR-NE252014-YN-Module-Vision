using Catl.MesInvocation.CatlMesInvoker;
using VisDummy.Shared.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using VisDummy.MtMes.CatlMes;
using VisDummy.MtMes.CatlMes.ViewModel;
using VisDummy.MtMes.MtMes;
using VisDummy.MtMesp.CatlMes.ViewModel;

namespace VisDummy.MtMes
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterMtMesViews(this IServiceProvider sp)
        {
            Locator.CurrentMutable.RegisterLazySingletonEx<DataCollectForResourceFAIVM>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<DataCollectForResourceFAIVM>, DataCollectForResourceFAICtrl>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<CatlMesSettingEditVM>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<CatlMesSettingEditVM>, CatlMesEditor>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<MtMesCtrlViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<MtMesCtrlViewModel>, MtMesCtrl>(sp);

        }

        public static IServiceCollection AddMtMesServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCatlWSInvokerServices().Bind(config.GetSection("CatlMesOpt"));
            services.AddCatlLoggingServices();
            return services;
        }
    }
}
