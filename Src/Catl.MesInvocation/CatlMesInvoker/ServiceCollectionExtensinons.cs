using AsZero.Core.Services;
using Catl.HostComputer.CommonServices.Actions.Loggings;
using Catl.HostComputer.CommonServices.Alarm.Loggings;
using Catl.HostComputer.CommonServices.Mes;
using Catl.HostComputer.CommonServices.Mes.Loggings;
using Catl.MesInvocation.Agent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Catl.MesInvocation.CatlMesInvoker
{
    public static class ServiceCollectionExtensinons
    {
        public static OptionsBuilder<CatlMesOpt> AddCatlWSInvokerServices(this IServiceCollection services)
        {
            services.AddSingleton<ICatlWebServiceAgent, DefaultCatlWebServiceAgent>();
            services.AddSingleton<CatlMesIniConfigHelper>();
            services.AddSingleton<ICatlMesInvoker, CatlMesInvoker>();
            return services.AddOptions<CatlMesOpt>();
        }

        public static void AddCatlLoggingServices(this IServiceCollection services)
        {
            #region CATL Logging
            services.AddSingleton(typeof(ISettingsManager<>), typeof(JsonFileSettingsManager<>));
            services.AddSfcLoggings();
            services.AddAlarmLoggings();
            services.AddActionLoggings();
            services.AddSingleton<ISfcInvocationLogger, Impl.ExcelSfcInvocationLogger>();
            #endregion
        }
    }
}
