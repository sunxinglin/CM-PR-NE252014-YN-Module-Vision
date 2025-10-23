using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using TApp.ViewModels;

namespace TApp.Views.UserMgmt
{
    /// <summary>
    /// Interaction logic for UseMgmtPage.xaml
    /// </summary>
    public partial class UserMgmtView : UserControl, IViewFor<UserMgmtViewModel>
    {
        public UserMgmtView()
        {
            InitializeComponent();

            this.ViewModel = Locator.Current.GetService<UserMgmtViewModel>();

            this.WhenActivated(d => {
                this.BindCommand(this.ViewModel, vm => vm.CmdSwitchToOperator, v => v.btnSwitchToOperator, nameof(btnSwitchToOperator.Click)).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.AppVM.CanAccessUserMgmt_MaintainUser, v => v.tabCreateUser.IsEnabled).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppVM.CanAccessUserMgmt_Privilege, v => v.tabClaimsMgmt.IsEnabled).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppVM.CanAccessUserMgmt_Privilege, v => v.tabInspectUsers.IsEnabled).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.ChangeUserPasswordViewModel, v => v.viewChangeUserPass.ViewModel).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.CreateUserViewModel, v => v.viewCreateUser.ViewModel).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.PrivilegeMgmtViewModel, v => v.viewPrivilegeMgmt.ViewModel).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.ListUsersViewModel, v => v.viewInspectUsers.ViewModel).DisposeWith(d);
            });
        }

        #region ViewModel
        public UserMgmtViewModel ViewModel
        {
            get { return (UserMgmtViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (UserMgmtViewModel) value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(UserMgmtViewModel), typeof(UserMgmtView), new PropertyMetadata(null));
        #endregion

        private void BtnClick_SwitchUser(object sender, RoutedEventArgs e)
        {
            var logWin = App.CreateLoginWindows();
            logWin.Show();
            logWin.Focus();
        }

    }
}
