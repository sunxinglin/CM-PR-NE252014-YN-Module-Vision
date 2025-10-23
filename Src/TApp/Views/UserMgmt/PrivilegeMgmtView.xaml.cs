using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using TApp.ViewModels.UserMgmt;

namespace TApp.Views.UserMgmt
{
    /// <summary>
    /// Interaction logic for PrivilegeMgmtView.xaml
    /// </summary>
    public partial class PrivilegeMgmtView : UserControl, IViewFor<PrivilegeMgmtViewModel>
    {
        public PrivilegeMgmtView()
        {
            InitializeComponent();

            this.WhenActivated(d => {
                this.OneWayBind(this.ViewModel, vm => vm.Resources, v => v.lstResources.ItemsSource).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdLoadResouces, v => v.btnReload).DisposeWith(d);

                this.ViewModel.CmdLoadResouces.ThrownExceptions
                    .Subscribe(e => MessageBox.Show(e.Message));

                this.ViewModel.CmdLoadResouces.Execute(Unit.Default);
            });
        }

        #region ViewModel
        public PrivilegeMgmtViewModel ViewModel
        {
            get { return (PrivilegeMgmtViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (PrivilegeMgmtViewModel) value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PrivilegeMgmtViewModel), typeof(PrivilegeMgmtView), new PropertyMetadata(null));
        #endregion
    }
}
