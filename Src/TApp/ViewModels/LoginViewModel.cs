using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Security.Claims;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using StdUnit.One.Shared;
using StdUnit.Zero.Shared;
using TApp.Auth;
using TApp.Views;
using VisDummy.Lang.Resources;
using VisDummy.Shared.LogGroup;
using VisDummy.Shared.Utils;

namespace TApp.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        private readonly IPrincipalAccessor _principalAccessor;
        private readonly IServiceScopeFactory _scf;
        private readonly IUserMgmtApi _api;
        private readonly IMediator _mediator;
        private readonly AppViewModel _appVM;

        public LoginViewModel(IPrincipalAccessor principalAccessor, IServiceScopeFactory scf, IUserMgmtApi api, IMediator mediator)
        {
            this._principalAccessor = principalAccessor;
            this._scf = scf;
            this._api = api;
            this._mediator = mediator;
            this._appVM = Locator.Current.GetService<AppViewModel>() ?? throw new Exception($"未注册{nameof(AppViewModel)}");
            this.Account = "operator";

            this.CmdLoginByPassword = ReactiveCommand.CreateFromTask<PasswordBox, Unit>(CmdLoginByPassword_Impl);
            this.CmdLoginByCard = ReactiveCommand.CreateFromTask<PasswordBox, Unit>(this.CmdLoginByCard_Impl);

            this.WhenAnyValue(x => x._appVM.AppTitle)
                .ToPropertyEx(this, x => x.AppName);

            this._idleTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
        }

        #region 三分钟自动退出
        private readonly DispatcherTimer _idleTimer; // 定时器
        private DateTime _lastInputTime; // 上次输入时间
        // 捕获输入活动
        private void OnInputActivity(object sender, PreProcessInputEventArgs e)
        {
            if (e.StagingItem.Input is MouseEventArgs || e.StagingItem.Input is KeyEventArgs)
            {
                this._lastInputTime = DateTime.Now; // 更新最后输入时间
            }
        }

        // 检查空闲时间
        private async void CheckIdleTime(object sender, EventArgs e)
        {
            var idleTime = DateTime.Now - _lastInputTime;
            if (idleTime.TotalMinutes >= 3)
            {
                var user = Locator.Current.GetRequiredService<UserMgmtViewModel>();
                await user.CmdSwitchToOperator.Execute();
            }
            else
            {
                _appVM.RemainingTime = idleTime.Seconds;
            }
        }
        private void TimerStart()
        {
            this._idleTimer.Start();
            this._idleTimer.Tick += CheckIdleTime;
            InputManager.Current.PreProcessInput += OnInputActivity;
        }
        private void TimerStop()
        {
            _appVM.RemainingTime = 0;
            this._idleTimer.Stop();
            this._idleTimer.Tick -= CheckIdleTime;
            InputManager.Current.PreProcessInput -= OnInputActivity;
        }
        #endregion

        [ObservableAsProperty]
        public string AppName { get; }

        [Reactive]
        public string Account { get; set; }


        [Reactive]
        public string Tips { get; set; }


        public ReactiveCommand<PasswordBox, Unit> CmdLoginByPassword { get; }
        private async Task<Unit> CmdLoginByPassword_Impl(PasswordBox passBox)
        {
            var loginResp = await this.TrySwithUserByPaswordAsync(this.Account, passBox.Password);
            passBox.Clear();
            using (var scope = this._scf.CreateScope())
            {
                var sp = scope.ServiceProvider;
                if (loginResp.Status)
                {
                    TimerStop();
                    var loginWnd = Locator.Current.GetRequiredService<IViewFor<LoginViewModel>>() as LoginWindow;
                    loginWnd.Hide();

                    var mainWnd = Locator.Current.GetRequiredService<IViewFor<MainViewModel>>() as MainWindow;
                    mainWnd.Show();
                }
                else
                {
                    MessageBox.Show(
                        loginResp.Tip,
                        "登录失败"
                    );
                }
                return Unit.Default;
            }
        }

        public ReactiveCommand<PasswordBox, Unit> CmdLoginByCard { get; }

        private async Task<Unit> CmdLoginByCard_Impl(PasswordBox card)
        {
            var loginResp = await this.TrySwithUserByCardAsync(card.Password);
            card.Clear();
            using (var scope = this._scf.CreateScope())
            {
                var sp = scope.ServiceProvider;
                if (loginResp.Status)
                {
                    TimerStart();

                    var loginWnd = Locator.Current.GetRequiredService<IViewFor<LoginViewModel>>() as LoginWindow;
                    loginWnd.Hide();

                    var mainWnd = Locator.Current.GetRequiredService<IViewFor<MainViewModel>>() as MainWindow;
                    mainWnd.Show();

                    await RecordLogAsync(Language.Msg_已登录);
                }
                else
                {
                    MessageBox.Show(
                        loginResp.Tip,
                        "登录失败"
                    );
                }
                return Unit.Default;
            }
        }

        /// <summary>
        /// 切换用户
        /// </summary>
        /// <returns></returns>
        public async Task<LoginResponse> TrySwithUserByPaswordAsync(string account, string password)
        {
            try
            {
                var q =
                    from loginResult in this._api.LoginAsync(new LoginParams { Equipment = "", UserName = account, Password = password })
                        .ToResult()
                        .SelectError(e => e.Message)
                    from claimDtos in this._api.LoadMyClaimsAsync()!
                        .MapNullableToResult(() => "加载Claims失败")
                    let claims = claimDtos.Select(r => new Claim(r.ClaimType, r.ClaimValue)).ToList()
                    select (loginResult, claims);

                var res = await q;
                if (res.IsError)
                {
                    return new LoginResponse
                    {
                        Status = false,
                        Principal = null,
                        Tip = res.ErrorValue,
                    };
                }
                else
                {
                    var (user, claims) = res.ResultValue;
                    var iden = new ClaimsIdentity(claims, "X-Auth");
                    var principal = new ClaimsPrincipal(iden);
                    await this.BindPrincipalAsync(principal);
                    return new LoginResponse
                    {
                        Status = true,
                        Principal = principal,
                        Tip = "登录成功！"
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Status = false,
                    Principal = null,
                    Tip = $"{ex.Message}\r\n{ex.StackTrace}"
                };
            }
        }

        /// <summary>
        /// 切换用户
        /// </summary>
        /// <returns></returns>
        public async Task<LoginResponse> TrySwithUserByCardAsync(string card)
        {
            try
            {
                var q =
                    from loginResult in this._api.LoginAsync(new LoginByCard { CardNo = card, Equipment = "" })
                        .ToResult()
                        .SelectError(e => e.Message)
                    from claimDtos in this._api.LoadMyClaimsAsync()!
                        .MapNullableToResult(() => "加载Claims失败")
                    let claims = claimDtos.Select(r => new Claim(r.ClaimType, r.ClaimValue)).ToList()
                    select (loginResult, claims);

                var res = await q;
                if (res.IsError)
                {
                    return new LoginResponse
                    {
                        Status = false,
                        Principal = null,
                        Tip = "登录失败！"
                    };
                }
                else
                {
                    var (user, claims) = res.ResultValue;
                    var iden = new ClaimsIdentity(claims, "X-Auth");
                    var principal = new ClaimsPrincipal(iden);
                    await this.BindPrincipalAsync(principal);

                    return new LoginResponse
                    {
                        Status = true,
                        Principal = principal,
                        Tip = "登录成功！"
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Status = false,
                    Principal = null,
                    Tip = $"{ex.Message}\r\n{ex.StackTrace}"
                };
            }
        }

        private async Task BindPrincipalAsync(ClaimsPrincipal principal)
        {
            this._principalAccessor.SetCurrentPrincipal(principal);
            this._appVM.Account = principal?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            this._appVM.UserName = principal?.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
            if (this._appVM.UserName != null)
            {
                await this._appVM.CmdRefreshResources.Execute();
            }
        }

        public async Task RecordLogAsync(string logmsg)
        {
            logmsg = $"{LogGroupName.OperationLog}：{logmsg}";
            var notification = new UILogNotification(new LogMessage
            {
                EventSource = LogGroupName.OperationLog,
                EventGroup = LogGroupName.OperationLog,
                Level = LogLevel.Critical,
                Content = logmsg,
                Timestamp = DateTime.Now,
            });
            await _mediator.Publish(notification);
        }
    }
}
