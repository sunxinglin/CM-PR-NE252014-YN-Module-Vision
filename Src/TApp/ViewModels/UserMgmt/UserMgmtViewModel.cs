
using System.Reactive.Linq;
using System.Windows;
using Microsoft.Extensions.Options;
using ReactiveUI;
using Splat;
using TApp.ViewModels.UserMgmt;
using VisDummy.Lang.Resources;
using VisDummy.Shared.Opts;
using VisDummy.Shared.Utils;

namespace TApp.ViewModels;

public class UserMgmtViewModel : ReactiveObject, IRoutableViewModel
{
    public UserMgmtViewModel(IOptionsMonitor<ApiServerSetting> optsMonitor)
    {
        this._optsMonitor = optsMonitor;

        // 各ViewModel
        this.HostScreen = Locator.Current.GetRequiredService<IScreen>();
        this._loginViewModel = Locator.Current.GetRequiredService<LoginViewModel>();
        this.AppVM = Locator.Current.GetRequiredService<AppViewModel>();
        this.ChangeUserPasswordViewModel = Locator.Current.GetRequiredService<ChangeUserPasswordViewModel>();
        this.CreateUserViewModel = Locator.Current.GetRequiredService<CreateUserViewModel>();
        this.PrivilegeMgmtViewModel = Locator.Current.GetRequiredService<PrivilegeMgmtViewModel>();
        this.ListUsersViewModel = Locator.Current.GetRequiredService<ListUsersViewModel>();

        this.CmdSwitchToOperator = ReactiveCommand.CreateFromTask(CmdSwitchToOperator_Impl);

    }

    private readonly LoginViewModel _loginViewModel;

    private readonly IOptionsMonitor<ApiServerSetting> _optsMonitor;

    public AppViewModel AppVM { get; }

    #region Routing
    public string UrlPathSegment => UrlDefines.URL_UserMgmt;

    public IScreen HostScreen { get; }
    #endregion


    #region
    public ReactiveCommand<Unit, Unit> CmdSwitchToOperator { get; }

    private async Task CmdSwitchToOperator_Impl()
    {
        var opts = this._optsMonitor.CurrentValue;
        var account = opts.DefaultOperatorAccount;
        var password = opts.DefaultOperatorPassword;
        var response = await this._loginViewModel.TrySwithUserByPaswordAsync(account, password);

        if (!response.Status)
        {
            MessageBox.Show(
                response.Tip,
                $"{Language.Msg_切换到用户失败请检查服务器是否启动或操作员凭据};{account}"
            );
        }

        this.AppVM.NavigateTo(UrlDefines.URL_Realtime);
    }
    #endregion


    #region 子视图模型
    public ChangeUserPasswordViewModel ChangeUserPasswordViewModel { get; }
    public CreateUserViewModel CreateUserViewModel { get; }
    public PrivilegeMgmtViewModel PrivilegeMgmtViewModel { get; }
    public ListUsersViewModel ListUsersViewModel { get; }
    #endregion
}
