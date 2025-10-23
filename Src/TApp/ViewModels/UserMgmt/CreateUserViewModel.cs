using System.Security.Claims;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.FSharp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using Splat;
using StdUnit.One.Shared;
using StdUnit.One.Users;
using TApp.Auth;
using VisDummy.Lang.Resources;
using VisDummy.Shared.LogGroup;
using VisDummy.Shared.Opts;


namespace TApp.ViewModels.UserMgmt;

public class CreateUserViewModel : ReactiveObject, IValidatableViewModel
{
    private readonly IPrincipalAccessor _principalAccessor;
    private readonly IUserMgmtApi _api;
    private readonly IMediator _mediator;

    public AppViewModel AppVM { get; }
    public CreateUserViewModel(
        IPrincipalAccessor principalAccessor,
        IUserMgmtApi api,
        IOptionsMonitor<ApiServerSetting> apiOptsMonitor,
        IMediator mediator)
    {
        this._principalAccessor = principalAccessor;
        this.AppVM = Locator.Current.GetService<AppViewModel>() ?? throw new Exception($"Locator未注册{nameof(AppViewModel)}");
        this._api = api;
        this._mediator = mediator;
        var principal = this._principalAccessor.GetCurrentPrincipal();
        var account = principal?.FindFirst(i => i.Type == ClaimTypes.NameIdentifier)?.Value ??
            apiOptsMonitor.CurrentValue.DefaultOperatorAccount;

        this.CmdCreateUser = ReactiveCommand.CreateFromTask<string, FSharpResult<User, string>>(CmdCreateUser_Impl, this.ValidationContext.Valid);
        this.CmdResetCreateUserForm = ReactiveCommand.Create(CmdResetForm_Impl);


        this.ValidationRule(vm => vm.Account, account => !string.IsNullOrEmpty(account), Language.Msg_账号名不可为空);
        this.ValidationRule(vm => vm.Name, account => !string.IsNullOrEmpty(account), Language.Msg_名称不可为空);
        this.ValidationRule(
            vm => vm.CardNo,
            this.WhenAnyValue(x => x.Passowrd1, x => x.CardNo, (pass, code) => !string.IsNullOrEmpty(pass) || !string.IsNullOrEmpty(code)),
            Language.Msg_密码与工卡不可均为空
        );
        this.ValidationRule(
            vm => vm.Passowrd2,
            this.WhenAnyValue(x => x.Passowrd1, x => x.Passowrd2,
                (pass1, pass2) =>
                    pass1 == pass2 || (string.IsNullOrEmpty(pass1) && string.IsNullOrEmpty(pass2))
            ),
            Language.Msg_两次输入的密码必须一致
        );
    }


    public IValidationContext ValidationContext { get; } = new ValidationContext();

    [Reactive]
    public string Account { get; set; }


    [Reactive]
    public string Name { get; set; }

    [Reactive]
    public string Passowrd1 { get; set; }
    [Reactive]
    public string Passowrd2 { get; set; }

    [Reactive]
    public string CardNo { get; set; }

    [Reactive]
    public CreateUserRoles CreateUser_Role { get; set; }

    public IList<CreateUserRoles> CreateUserRolesSource { get; } = new List<CreateUserRoles> {
            CreateUserRoles.超级管理员,
            CreateUserRoles.PE,
            CreateUserRoles.ME,
            CreateUserRoles.Tec,
            CreateUserRoles.Ope,
        };


    #region
    public ReactiveCommand<string, FSharpResult<User, string>> CmdCreateUser { get; }

    private async Task<FSharpResult<User, string>> CmdCreateUser_Impl(string password)
    {
        var account = this.Account;
        var name = this.Name;

        try
        {
            var res = await this._api.CreateUserAsync(new CreateUserParams
            {
                UserName = account,
                Sername = name,
                Password = password,
                UserCode = this.CardNo,
                Role = this.CreateUser_Role,
            });
            if (res.Success)
                await RecordLogAsync($"{Language.Msg_已创建用户}，Account：{account}，Name:{name}");
            return res.Success ?
                res.Data.ToOkResult<User, string>() :
                res.ErrorMessage.ToErrResult<User, string>();
        }
        catch (Exception ex)
        {
            var msg = ex.Message;
            return msg.ToErrResult<User, string>();
        }
    }
    #endregion

    #region

    public ReactiveCommand<Unit, Unit> CmdResetCreateUserForm { get; }

    private void CmdResetForm_Impl()
    {
        this.Account = "";
        this.Name = "";
        this.CardNo = "";
        this.CreateUser_Role = CreateUserRoles.Ope;
    }
    #endregion

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
