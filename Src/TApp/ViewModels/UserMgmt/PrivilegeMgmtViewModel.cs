using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Security.Claims;
using System.Windows;
using DynamicData;
using MediatR;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using StdUnit.One.Claims;
using StdUnit.One.Shared;
using TApp.Auth;
using VisDummy.Lang.Resources;
using VisDummy.Shared.LogGroup;


namespace TApp.ViewModels.UserMgmt;


public class PrivilegeMgmtViewModel : ReactiveObject
{
    private readonly IUserMgmtApi _api;
    private readonly IMediator _mediator;

    public AppViewModel AppVM { get; }

    public PrivilegeMgmtViewModel(IUserMgmtApi api, IMediator mediator)
    {
        this.AppVM = Locator.Current.GetService<AppViewModel>() ?? throw new Exception($"Locator未注册{nameof(AppViewModel)}");
        this._api = api;
        this._mediator = mediator;
        this.CmdLoadResouces = ReactiveCommand.CreateFromTask(LoadResourcesImpl);
        this.CmdLoadResouces.Select(x => x.Count).ToPropertyEx(this, x => x.Size);
    }


    #region Resources
    public ObservableCollection<ResourceItem> Resources { get; } = new ObservableCollection<ResourceItem>();

    [ObservableAsProperty]
    public int Size { get; }

    public ReactiveCommand<System.Reactive.Unit, IList<ResourceItem>> CmdLoadResouces { get; }
    private async Task<IList<ResourceItem>> LoadResourcesImpl()
    {
        var claims = await this._api.LoadAllClaimsAsync();
        var pageRes = await this._api.PagesAsync(1, int.MaxValue, uniqueName: null, parentId: null);
        if (!pageRes.Success)
        {
            // todo
        }
        var list = pageRes.Data;
        var items = list
            .Where(i => i.Configurable)         // 剔除不可配置的资源
            .Select(res => new ResourceItem(this._api, _mediator)
            {
                Id = res.Id,
                ClaimsAllowed = new ObservableCollection<ClaimSelection>(
                    claims.Select(c =>
                        new ClaimSelection
                        {
                            Claim = c,
                            Checked = res.AllowedClaims?.Any(a => a.Type == c.Type && a.Value == c.Value) == true
                        }
                    )
                ),
                ShortName = res.UniqueName
            })
            .ToList();
        this.Resources.Clear();
        this.Resources.AddRange(items);
        return items;
    }
    #endregion

}
public class ClaimSelection : ReactiveObject
{
    [Reactive]
    public Claim Claim { get; set; }

    [Reactive]
    public bool Checked { get; set; }
}

public class ResourceItem : ReactiveObject
{
    private readonly IUserMgmtApi _api;
    private readonly IMediator _mediator;

    public ResourceItem(IUserMgmtApi api, IMediator mediator)
    {
        this._api = api;
        this._mediator = mediator;
        this.CmdUpdateResource = ReactiveCommand.CreateFromTask(UpdateResourceImpl);
    }

    private async Task UpdateResourceImpl()
    {
        try
        {
            var existedRes = await this._api.LoadResouceAsync(this.Id);
            if (!existedRes.Success)
            {
                throw new Exception($"加载资源id={this.Id}失败！");
            }
            var existed = existedRes.Data;
            var allowed = this.ClaimsAllowed
                .Where(i => i != null && i.Checked && i.Claim != null)
                .Select(i => new ClaimEntity { ClaimType = i.Claim!.Type, ClaimValue = i.Claim.Value })
                .ToList();
            var res = await this._api.UpdatePermissionClaimsAsync(this.Id, allowed);
            if (res.Success)
                await RecordLogAsync($"{Language.Msg_已修改权限分配}；Id:{this.Id},Allowed:{string.Join(",", allowed.Select(s => s.ClaimValue))}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    [Reactive]
    public int Id { get; set; }

    [Reactive]
    public string ShortName { get; set; } = "";


    /// <summary>
    /// 允许的Claim
    /// </summary>
    public ObservableCollection<ClaimSelection> ClaimsAllowed { get; set; } = new ObservableCollection<ClaimSelection>();
    public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> CmdUpdateResource { get; }

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