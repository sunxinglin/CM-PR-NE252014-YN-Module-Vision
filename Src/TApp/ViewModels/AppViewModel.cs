using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using TApp.Auth;
using VisDummy.Lang;
using VisDummy.Protocols.Vision.Models;
using VisDummy.Shared.Opts;


namespace TApp.ViewModels
{
    /// <summary>
    /// App ViewModel： 保存了当前用户信息和一些最基本的状态
    /// </summary>
    public class AppViewModel : ReactiveObject, IScreen
    {
        private readonly IUserMgmtApi _userApi;
        private readonly ILogger<AppViewModel> _logger;

        public AppViewModel(IUserMgmtApi userApi, IOptions<AppClientSetting> clientOpt, ILogger<AppViewModel> logger)
        {

            this._userApi = userApi;
            this._logger = logger;

            this.CmdRefreshResources = ReactiveCommand.CreateFromTask(this.RefreshResourcesImpl);
            this.CmdRefreshResources.ThrownExceptions.Subscribe(msg => MessageBox.Show(msg.Message));

            this.CmdSwitchLanguage = ReactiveCommand.Create(this.SwitchLanguage_Impl);
            this.CmdSwitchLanguage.ThrownExceptions.Subscribe(msg => MessageBox.Show(msg.Message));

            var appsetting = clientOpt.Value;
            this.AppTitle = appsetting.AppTitle;
            this.EquipId = appsetting.EquipId;

            CpuUsageSub.Select(s => $"{s}%").ToPropertyEx(this, t => t.CpuUsage, scheduler: RxApp.MainThreadScheduler);
            DiskUsageSub.Select(s => $"{s}").ToPropertyEx(this, t => t.DiskUsage, scheduler: RxApp.MainThreadScheduler);
            MemoryUsageSub.Select(s => $"{s}%").ToPropertyEx(this, t => t.MemoryUsage, scheduler: RxApp.MainThreadScheduler);

            VisionStatusSubject.Select(s => s.CameraStatus).ToPropertyEx(this, t => t.VisionCameraStatusDic, scheduler: RxApp.MainThreadScheduler);
        }

        public RoutingState Router { get; } = new RoutingState();
        public Unit NavigateTo(string url)
        {
            var vm = UrlDefines.GetRoutableViewModel(url);
            this.Router.Navigate.Execute(vm);
            return Unit.Default;
        }

     

        /// <summary>
        /// 软件版本
        /// </summary>
        [Reactive]
        public string AppVersion { get; set; }

        /// <summary>
        /// 应用标题—— 和 AppName不同，这个可以动态变更
        /// </summary>
        [Reactive]
        public string AppTitle { get; set; } = "";

        /// <summary>
        /// 设备ID
        /// </summary>
        [Reactive]
        public string EquipId { get; set; } = "";

        /// <summary>
        /// 剩余登录时间
        /// </summary>
        [Reactive]
        public int RemainingTime { get; set; } = 0;


        #region Account & Claim

        [Reactive]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 当前用户名
        /// </summary>
        [Reactive]
        public string UserName { get; set; } = string.Empty;

        [Reactive]
        public bool CanAccessUserMgmt { get; set; }
        [Reactive]
        public bool CanAccessUserMgmt_MaintainUser { get; set; }
        [Reactive]
        public bool CanAccessUserMgmt_Privilege { get; set; }
        [Reactive]
        public bool CanAccessParamsSetting { get; set; }


        private async Task RefreshResourcesImpl()
        {
            try
            {
                var res = await this._userApi.CurrentUserAsync();
                if (!res.Success)
                {
                    throw new Exception(res.ErrorMessage);
                }
                var reslist = res.Data.FuncResources;
                this.CanAccessUserMgmt = reslist.FirstOrDefault(r => r.UniqueName == "用户管理") != null;
                this.CanAccessUserMgmt_MaintainUser = reslist.FirstOrDefault(r => r.UniqueName == "用户维护") != null;
                this.CanAccessUserMgmt_Privilege = reslist.FirstOrDefault(r => r.UniqueName == "权限管理") != null;
                this.CanAccessParamsSetting = reslist.FirstOrDefault(r => r.UniqueName == "参数设置") != null;
            }
            catch (Exception ex)
            {
                _logger.LogError("刷新资源错误：{exMessage}\r\n{exStackTrace}", ex.Message, ex.StackTrace);
            }
        }

        public ReactiveCommand<Unit, Unit> CmdRefreshResources { get; }

        #endregion

        #region 服务器连接状态
        [Reactive]
        public bool HubConected { get; set; }


        #endregion

        #region 多语言

        public ReactiveCommand<Unit, Unit> CmdSwitchLanguage { get; }

        /// <summary>
        /// 当前语言
        /// </summary>
        [Reactive]
        public LanguageEnum Language { get; set; } = LanguageEnum.中文;
        private void SwitchLanguage_Impl()
        {
            LanguageManager.Instance.ChangeLanguge(Language);
        }
        #endregion

        #region PC状态

        public Subject<double> CpuUsageSub { get; set; } = new Subject<double>();
        [ObservableAsProperty]
        public string CpuUsage { get; set; }
        public Subject<string> DiskUsageSub { get; set; } = new Subject<string>();
        [ObservableAsProperty]
        public string DiskUsage { get; set; }
        public Subject<uint> MemoryUsageSub { get; set; } = new Subject<uint>();
        [ObservableAsProperty]
        public string MemoryUsage { get; set; }

        #endregion

        #region 视觉状态

        public Subject<VisionStatusNotification> VisionStatusSubject { get; set; } = new();
        [ObservableAsProperty]
        public Dictionary<string, bool> VisionCameraStatusDic { get; set; }

        #endregion
    }
}
