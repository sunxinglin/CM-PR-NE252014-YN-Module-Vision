using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using TApp.ViewModels.UserMgmt;

namespace TApp.Views.UserMgmt
{
    /// <summary>
    /// Interaction logic for ChangeUserPasswordView.xaml
    /// </summary>
    public partial class ChangeUserPasswordView : UserControl, IViewFor<ChangeUserPasswordViewModel>
    {
        public ChangeUserPasswordView()
        {
            InitializeComponent();

            this.WhenActivated(d => {
                this.Bind(this.ViewModel, vm => vm.Account, v => v.txtAccount.Text).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdResetForm, v => v.btnResetForm).DisposeWith(d);
            });
        }

        #region
        public ChangeUserPasswordViewModel ViewModel
        {
            get { return (ChangeUserPasswordViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (ChangeUserPasswordViewModel) value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ChangeUserPasswordViewModel), typeof(ChangeUserPasswordView), new PropertyMetadata(null));
        #endregion

        private void BtnClick_ChangeUserPassword(object sender, RoutedEventArgs e)
        {
            if (this.Confirm.Password != this.NewPassword.Password)
            {
                MessageBox.Show("两次输入的新密码不一致！");
                return;
            }
            try
            {
                this.ViewModel.OldPassword = this.OldPassword.Password;
                this.ViewModel.NewPassword = this.NewPassword.Password;
                this.ViewModel.CmdChangeUserPassword.Execute()
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(res =>
                        {
                            if (res.IsOk)
                            {
                                MessageBox.Show($"修改用户密码成功！（账号名={res.ResultValue.Account}）");
                                this.ViewModel.CmdResetForm?.Execute(Unit.Default);
                            }
                            else
                            {
                                MessageBox.Show($"修改用户密码失败！原因是={res.ErrorValue}");
                            }

                            // 重置表单
                            this.OldPassword.Clear();
                            this.NewPassword.Clear();
                            this.Confirm.Clear();
                            this.ViewModel.CmdResetForm.Execute();
                        });
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
