using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows;
using DynamicData;
using MediatR;
using Microsoft.FSharp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using StdUnit.One.Shared;
using StdUnit.One.Users;
using StdUnit.Zero.Shared;
using TApp.Auth;
using VisDummy.Lang.Resources;
using VisDummy.Shared.LogGroup;

namespace TApp.ViewModels.UserMgmt;


public class ListUsersViewModel : ReactiveObject
{
    private readonly IUserMgmtApi _api;
    private readonly IMediator _mediator;

    public ListUsersViewModel(IUserMgmtApi api, IMediator mediator)
    {
        this._api = api;
        this._mediator = mediator;
        this.CmdLoadTable = ReactiveCommand.CreateFromTask<Unit, PagedResp<User>>(CmdLoadTable_ImplAsync);
        this.CmdDisableAccount = ReactiveCommand.CreateFromTask<User, FSharpResult<User, string>>(CmdDisableAccount_Impl);


        this.CmdLoadTable
            .Subscribe(resp =>
            {
                this.Users.Clear();
                this.Users.AddRange(resp.Data);
                this.TotalCounts = resp.Total;
                this.PageCount = (int)Math.Ceiling((resp.Total + 3) * 1.0 / this.PageSize);
            });

        var canPageNext = this.WhenAnyValue(x => x.PageIndex, x => x.PageCount)
            .Select(pair => pair.Item1 <= pair.Item2);
        this.CmdNextPage = ReactiveCommand.Create(
            () =>
            {
                this.PageIndex++;
            },
            canPageNext
        );

        var canPagePrev = this.WhenAnyValue(x => x.PageIndex)
            .Select(idx => idx > 1);
        this.CmdPrevPage = ReactiveCommand.Create(
            () =>
            {
                this.PageIndex--;
            },
            canPagePrev
        );
    }


    public ReactiveCommand<User, FSharpResult<User, string>> CmdDisableAccount { get; }
    private async Task<FSharpResult<User, string>> CmdDisableAccount_Impl(User user)
    {
        try
        {
            var res = await this._api.RemoveUserAsync(user.Account);

            if (res.Success)
            {
                MessageBox.Show(
                    Language.Msg_删除账户成功,
                   Language.Msg_删除账户完成
                );
                await this.CmdLoadTable.Execute();
                await RecordLogAsync($"{Language.Msg_已删除用户}，Account：{user.Account}，Name:{user.Name}");
            }
            else
            {
                MessageBox.Show(
                    res.ErrorMessage,
                    Language.Msg_删除账户错误
                );
            }
            return res.ToResult().SelectError(e => e.Message);
        }
        catch (Exception ex)
        {
            return ex.Message.ToErrResult<User, string>();
        }

    }

    public ReactiveCommand<Unit, PagedResp<User>> CmdLoadTable { get; }


    private async Task<PagedResp<User>> CmdLoadTable_ImplAsync(Unit _)
    {
        var s = await this._api.LoadUsersAsync(this.PageIndex, this.PageSize);
        s.Data = s.Data.Where(i => i.Id > 3).ToList();//剔除内置用户
        return s;
    }

    public ReactiveCommand<Unit, Unit> CmdNextPage { get; }
    public ReactiveCommand<Unit, Unit> CmdPrevPage { get; }

    [Reactive]
    public int PageIndex { get; set; } = 1;

    [Reactive]
    public int PageSize { get; set; } = 10;

    [Reactive]
    public int TotalCounts { get; set; } = 0;

    [Reactive]
    public int PageCount { get; set; }

    public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();


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
