using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Serilog;
using Splat;
using System.IO;
using TApp.Auth;
using TApp.BackgroundServices;
using TApp.Handlers;
using TApp.ViewModels;
using VisDummy.Shared.Opts;
using VisDummy.WPF;

namespace TApp;

public class Startup
{
    #region construct
    private Startup() { }

    public static Startup NewStartup(string rootpath)
    {
        var resolver = Locator.CurrentMutable;
        resolver.InitializeSplat();
        resolver.InitializeReactiveUI();

        var services = new ServiceCollection();
        var configpath = Startup.GetAppConfigRootPath(rootpath);
        var config = Startup.BuildConfiguration(configpath);


        Startup.RegisterServices(services, config);
        services.AddVisDummyServices(config, configpath);

        services.AddHostedService<PCInfoBackgroundService>();

        var sp = services.BuildServiceProvider();
        ShellViewRegistrar.RegisterViews(sp);
        sp.RegisterVisDummyViews(config);


        var startup = new Startup()
        {
            RootServiceProvider = sp,
            ConfigurationRoot = config,
            ConfigDirPath = configpath,
        };
        return startup;
    }
    #endregion

    #region 基本属性
    public IServiceProvider RootServiceProvider { get; private set; } = null!;
    public string ConfigDirPath { get; private set; } = null!;
    public IConfigurationRoot ConfigurationRoot { get; private set; } = null!;
    #endregion



    #region 构建配置
    protected static string GetAppConfigRootPath(string rootpath)
    {
        //var envroot = Environment.GetEnvironmentVariable("VisDummyRoot");
        //return string.IsNullOrEmpty(envroot) ? "D:/VisDummy" : envroot;
        if (!Directory.Exists(rootpath))
        {
            Directory.CreateDirectory(rootpath);
        }
        return rootpath;
    }

    protected static IConfigurationRoot BuildConfiguration(string configRootPath)
    {
        var appsettingsJsonPath = System.IO.Path.Combine(configRootPath, "appsettings.json");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddJsonFile(appsettingsJsonPath, optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        return configuration;
    }
    #endregion



    #region 注册服务
    protected static void RegisterServices(IServiceCollection services, IConfiguration config)
    {
        services.AddSerilog((sp, lc) => lc
                .ReadFrom.Configuration(config)
                .ReadFrom.Services(sp)
                .Enrich.FromLogContext()
            );

        services.AddOptions<ApiServerSetting>()
             .Bind(config.GetSection("ApiServerSettings"));
        services.AddOptions<AppClientSetting>()
             .Bind(config.GetSection("AppClientSettings"));

        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddAuthApis();

        services.AddMediatR(
            typeof(UILogNotificationHandler).Assembly
        //typeof(ScanCodeToLaunchMesFlowInstanceHandler).Assembly
        );
    }
    #endregion

}
