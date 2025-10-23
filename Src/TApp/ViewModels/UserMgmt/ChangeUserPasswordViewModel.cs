using MediatR;
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

namespace TApp.ViewModels.UserMgmt;


public class ChangeUserPasswordViewModel : ReactiveObject, IValidatableViewModel
{
    private readonly IUserMgmtApi _api;
    private readonly IMediator _mediator;
    public AppViewModel AppVM { get; }
    public ChangeUserPasswordViewModel(IUserMgmtApi api, IMediator mediator)
    {
        this.AppVM = Locator.Current.GetService<AppViewModel>() ?? throw new Exception($"Locator未注册{nameof(AppViewModel)}");
        this._api = api;
        this._mediator = mediator;
        this.WhenAnyValue(x => x.AppVM.Account)
            .ToPropertyEx(this, x => x.Account);

        this.CmdChangeUserPassword = ReactiveCommand.CreateFromTask(CmdChangeUserPassword_Impl, this.ValidationContext.Valid);
        this.CmdResetForm = ReactiveCommand.Create(CmdResetForm_Impl);


        this.ValidationRule(
            vm => vm.OldPassword,
            pswd => !string.IsNullOrEmpty(pswd),
            Language.Msg_请输入密码);

        this.ValidationRule(
            vm => vm.NewPassword,
            pswd => !string.IsNullOrEmpty(pswd),
            Language.Msg_新密码不得为空);
        this.ValidationRule(
            vm => vm.NewPasswordConfirm,
            this.WhenAnyValue(x => x.NewPassword, x => x.NewPasswordConfirm, (pw, confirm) => pw == confirm),
            Language.Msg_两次输入的密码必须一致
        );
    }

    public IValidationContext ValidationContext { get; } = new ValidationContext();
    [ObservableAsProperty]
    public string Account { get; }
    [Reactive]
    public string OldPassword { get; set; }
    [Reactive]
    public string NewPassword { get; set; }
    [Reactive]
    public string NewPasswordConfirm { get; set; }

    public ReactiveCommand<Unit, FSharpResult<User, string>> CmdChangeUserPassword { get; }
    private async Task<FSharpResult<User, string>> CmdChangeUserPassword_Impl()
    {
        try
        {
            var account = this.Account;
            var password = this.NewPassword;
            var oldpass = this.OldPassword;

            if (string.IsNullOrEmpty(account))
            {
                var msg = Language.Msg_账号名不可为空;
                return msg.ToErrResult<User, string>();
            }

            if (string.IsNullOrEmpty(oldpass))
            {
                var msg = Language.Msg_当前密码不可为空;
                return msg.ToErrResult<User, string>();
            }

            if (string.IsNullOrEmpty(password))
            {
                var msg = Language.Msg_新密码不可为空;
                return msg.ToErrResult<User, string>();
            }

            var res = await _api.UpdatePasswordAsync(new ChangePasswordParams
            {
                NewPassword = password,
                OldPassword = oldpass,
                UserName = account,
            });
            if (res.Success)
                await RecordLogAsync($"{Language.Msg_已修改密码}，Account：{account}");
            return res.Success ?
                res.Data.ToOkResult<User, string>() :
                res.ErrorMessage.ToErrResult<User, string>();
        }
        catch (Exception ex)
        {
            return ex.Message.ToErrResult<User, string>();
        }
    }
    public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> CmdResetForm { get; }

    private void CmdResetForm_Impl()
    {
        this.OldPassword = "";
        this.NewPassword = "";
        this.NewPasswordConfirm = "";
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