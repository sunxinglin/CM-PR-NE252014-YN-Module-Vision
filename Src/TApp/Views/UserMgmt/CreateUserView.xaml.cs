using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using StdUnit.One.Users;
using TApp.ViewModels.UserMgmt;

namespace TApp.Views.UserMgmt
{
    /// <summary>
    /// Interaction logic for CreateUserView.xaml
    /// </summary>
    public partial class CreateUserView : UserControl, IViewFor<CreateUserViewModel>
    {
        public CreateUserView()
        {
            InitializeComponent();

            this.WhenActivated(d => {
                this.Bind(this.ViewModel, vm => vm.Account, v => v.txtAccount.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Name, v => v.txtName.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.CardNo, v => v.txtCardNo.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.CreateUser_Role, v => v.cboxRole.SelectedValue).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.CreateUserRolesSource, v => v.cboxRole.ItemsSource).DisposeWith(d);

                this.BindCommand(this.ViewModel, vm => vm.CmdResetCreateUserForm, v => v.btnResetForm).DisposeWith(d);

                this.ViewModel.CmdCreateUser
                    .ThrownExceptions
                    .Subscribe(e => MessageBox.Show(e.Message));
   
            });
        }

        #region
        public CreateUserViewModel ViewModel
        {
            get { return (CreateUserViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel =(CreateUserViewModel)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CreateUserViewModel), typeof(CreateUserView), new PropertyMetadata(null));

        #endregion

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            if (this.CreatePassword.Password != this.CreatePassword2.Password)
            {
                MessageBox.Show($"两次输入的密码不同！");
                return;
            }
            var pass = this.CreatePassword2.Password;
            this.ViewModel.CmdCreateUser.Execute(pass)
                    .Catch((Exception ex) => {
                        var s = ex.Message.ToErrResult<User,string>();
                        return Observable.Return(s);
                    })
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(s => {
                        if (s.IsError)
                        {
                            MessageBox.Show(s.ErrorValue);
                        }
                        else { 
                            MessageBox.Show($"用户创建成功：账号名={s.ResultValue?.Account}");
                            this.ViewModel.CmdResetCreateUserForm.Execute(Unit.Default);
                        }
                    }); 
        }
    }
}
