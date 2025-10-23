using ReactiveUI;
using Splat;
using TApp.ViewModels;
using TApp.ViewModels.Dbg;
using TApp.ViewModels.LogSearch;
using TApp.ViewModels.ParamsMgmt;
using TApp.ViewModels.Realtime;
using TApp.ViewModels.UserMgmt;
using TApp.Views;
using TApp.Views.Dbg;
using TApp.Views.LogSearch;
using TApp.Views.ParamsMgmt;
using TApp.Views.Realtime;
using TApp.Views.UserMgmt;
using VisDummy.Shared.Utils;

namespace TApp
{
    public static class ShellViewRegistrar
    {

        public static void RegisterViews(IServiceProvider sp)
        {


            Locator.CurrentMutable.RegisterLazySingletonEx<IScreen, AppViewModel>(sp);

            // App标题、当前用户信息等
            Locator.CurrentMutable.RegisterLazySingletonEx<AppViewModel>(sp);

            // 各主窗体
            Locator.CurrentMutable.RegisterLazySingletonEx<LoginViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<MainViewModel>(sp);

            // 各页面
            Locator.CurrentMutable.RegisterLazySingletonEx<RealtimeViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<UILogsViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<UserMgmtViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<ParamsMgmtViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<DbgToolsViewModel>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<LogSearchViewModel>(sp);

            // 用户管理
            Locator.CurrentMutable.RegisterTransientEx<ChangeUserPasswordViewModel>(sp);
            Locator.CurrentMutable.RegisterTransientEx<CreateUserViewModel>(sp);
            Locator.CurrentMutable.RegisterTransientEx<PrivilegeMgmtViewModel>(sp);
            Locator.CurrentMutable.RegisterTransientEx<ListUsersViewModel>(sp);

            // view location
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<LoginViewModel>, LoginWindow>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<MainViewModel>, MainWindow>(sp);

            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<UserMgmtViewModel>, UserMgmtView>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<DbgToolsViewModel>, DbgToolsView>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<ParamsMgmtViewModel>, ParamsMgmtView>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<RealtimeViewModel>, RealtimeView>(sp);
            Locator.CurrentMutable.RegisterLazySingletonEx<IViewFor<LogSearchViewModel>, LogSearchView>(sp);
        }
    }
}