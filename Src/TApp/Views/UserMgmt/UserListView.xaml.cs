using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using TApp.ViewModels.UserMgmt;

namespace TApp.Views.UserMgmt
{
    /// <summary>
    /// Interaction logic for UserListView.xaml
    /// </summary>
    public partial class UserListView : UserControl, IViewFor<ListUsersViewModel>
    {
        public UserListView()
        {
            InitializeComponent();

            this.WhenActivated(d => {
                this.OneWayBind(this.ViewModel, vm => vm.Users, v => v.lstUsers.ItemsSource).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.PageIndex, v => v.txtPageIndex.Text).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.PageCount, v => v.txtTotalPages.Text).DisposeWith(d);

                this.BindCommand(this.ViewModel, vm => vm.CmdPrevPage, v => v.btnPrevPage).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdNextPage, v => v.btnNextPage).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdLoadTable, v => v.btnRefresh).DisposeWith(d);

 

                this.ViewModel.CmdDisableAccount.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message)).DisposeWith(d);
                this.ViewModel.CmdLoadTable.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message)).DisposeWith(d);
                this.ViewModel.CmdNextPage.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message)).DisposeWith(d);
                this.ViewModel.CmdPrevPage.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message)).DisposeWith(d);
            });
        }

        #region
        public ListUsersViewModel ViewModel
        {
            get { return (ListUsersViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel =(ListUsersViewModel) value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ListUsersViewModel), typeof(UserListView), new PropertyMetadata(null));
        #endregion
    }
}
