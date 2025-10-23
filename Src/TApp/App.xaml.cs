using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Windows;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReactiveUI;
using Splat;
using StdUnit.One.Shared;
using StdUnit.Zero.Core;
using TApp.ViewModels;
using TApp.ViewModels.Realtime;
using TApp.Views;
using VisDummy.Shared.Opts;
using VisDummy.Shared.Utils;


namespace TApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Mutex _singletonMutex;

        public App()
        {
            var appname = typeof(App).AssemblyQualifiedName;
            this._singletonMutex = new Mutex(true, appname, out var createdNew);
            if (!createdNew)
            {
                MessageBox.Show($"软件已经启动！不可重复启动！");
                Environment.Exit(0);
                return;
            }

            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            InitHost();
        }


        private void InitHost()
        {
            var rootPath = System.Configuration.ConfigurationManager.AppSettings["RootPath"];// ?? throw new Exception($"App.config 未配置 RootPath");
            if (string.IsNullOrEmpty(rootPath))
            {
                throw new Exception($"App.config's RootPath not configured");
            }

            this.HostStartup = TApp.Startup.NewStartup(rootPath);

        }


        public Startup HostStartup { get; private set; } = null!;
        public IServiceProvider RootServiceProvider => this.HostStartup.RootServiceProvider;

        public void HosPreRun(IServiceProvider sp)
        {
            var hostpreruns = sp.GetServices<IHostPreRun>();
            foreach (var hostprerun in hostpreruns.OrderBy(p => p.Priority))
            {
                hostprerun?.DoAsync(sp);
            }
        }


        private UnloadProgressWnd _unloadWnd;
        public UnloadProgressWnd UnloadProgress
        {
            get
            {
                if (this._unloadWnd == null)
                {
                    this._unloadWnd = new UnloadProgressWnd();
                }
                return this._unloadWnd;
            }
            set
            {
                this._unloadWnd = value;
            }
        }



        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            HosPreRun(this.RootServiceProvider);
            BeginBackgroundService(this.RootServiceProvider);

            try
            {
                var appvm = Locator.Current.GetRequiredService<AppViewModel>();
                var rtvm = Locator.Current.GetRequiredService<RealtimeViewModel>();
                _ = appvm.Router.Navigate.Execute(rtvm);
            }
            catch (Exception ex)
            {
                var logger = this.HostStartup.RootServiceProvider.GetRequiredService<ILogger<App>>();
                logger.LogError("系统启动异常：{eMsg}。 {eStackTrace}", ex.Message, ex.StackTrace);
            }


            try
            {
                this.UnloadProgress.Hide();
                // 按照CATL WM-S8-008：直接以默认Opeator启动（无需启动登录窗口）
                //var loginWindow = Locator.Current.GetService<IViewFor<LoginViewModel>>() as Window;
                //loginWindow.Show();
                await SigninOperatorAsync(this.RootServiceProvider);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                using (var scope = this.RootServiceProvider.CreateScope())
                {
                    var sp = scope.ServiceProvider;
                    var logger = sp.GetRequiredService<ILogger<App>>();
                    logger.LogError("{message}\r\n{stackTrace}", ex.Message, ex.StackTrace);
                }
                this.Shutdown();
            }

        }

        public static LoginWindow CreateLoginWindows()
        {
            var wnd = Locator.Current.GetService<IViewFor<LoginViewModel>>() as LoginWindow;
            if (wnd == null)
            {
                throw new System.Exception($"未能找到登录界面，是否忘记注册 IViewFor<LoginViewModel> ？");
            }
            return wnd;
        }


        private static async Task SigninOperatorAsync(IServiceProvider sp)
        {
            var uservm = Locator.Current.GetRequiredService<UserMgmtViewModel>();
            await uservm.CmdSwitchToOperator.Execute();
            var mainWin = Locator.Current.GetRequiredService<IViewFor<MainViewModel>>() as MainWindow;
            mainWin.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            this._singletonMutex.ReleaseMutex();

            using (var mre = new ManualResetEventSlim(false))
            {
                this.StopBackgroundService(this.RootServiceProvider, mre);
                mre.Wait();
            }
            Environment.Exit(0);
        }


        private CancellationTokenSource _ctsStartBackgroundService = new CancellationTokenSource();


        void StopBackgroundService(IServiceProvider sp, ManualResetEventSlim mre)
        {
            var logger = sp.GetRequiredService<ILogger<App>>();
            var thread = new Thread(async () =>
            {
                try
                {
                    this._ctsStartBackgroundService.Cancel();
                    var bgs = sp.GetRequiredService<IEnumerable<IHostedService>>();
                    var tasks = bgs.Select(bg =>
                    {
                        var svcName = bg.GetType().Name;
                        return Observable.FromAsync(async () =>
                        {

                            logger.LogInformation("正在停止后台服务: {svcName}", svcName);
                            await bg.StopAsync(CancellationToken.None);
                            logger.LogInformation("后台服务已停止: {svcName}", svcName);
                        })
                        .Timeout(TimeSpan.FromSeconds(10))
                        .Catch((TimeoutException ex) =>
                        {
                            logger.LogError("停止后台服务超时: {svcName}", svcName);
                            return Observable.Return(Unit.Default);
                        })
                        .ToTask();
                    });
                    await Task.WhenAll(tasks);
                }
                finally
                {
                    mre.Set();
                }
            });
            thread.Start();
        }

        void BeginBackgroundService(IServiceProvider sp)
        {
            var th = new Thread(async () =>
            {
                var logger = sp.GetRequiredService<ILogger<App>>();
                var mediator = sp.GetRequiredService<IMediator>();
                try
                {
                    var bgs = sp.GetRequiredService<IEnumerable<IHostedService>>();
                    var tasks = bgs.Select(bg => bg.StartAsync(this._ctsStartBackgroundService.Token));
                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    logger.LogError("后台任务异常：{exMsg}\r\n{exStackTrace}", ex.Message, ex.StackTrace);
                    await mediator.Publish(new UILogNotification(new LogMessage
                    {
                        EventSource = nameof(App),
                        EventGroup = nameof(App),
                        Content = ex.Message,
                        Level = LogLevel.Error,
                        Timestamp = DateTime.Now,
                    }));
                }
            });
            th.Start();
        }


    }
}
