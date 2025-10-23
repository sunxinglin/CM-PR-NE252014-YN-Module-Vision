using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.MtMesp.CatlMes.ViewModel;

namespace VisDummy.MtMes.CatlMes
{
    /// <summary>
    /// Interaction logic for DataCollectForResourceFAICtrl.xaml
    /// </summary>
    public partial class DataCollectForResourceFAICtrl : UserControl,IViewFor<DataCollectForResourceFAIVM>
    {
        public DataCollectForResourceFAICtrl()
        {
            InitializeComponent();
            this.WhenActivated(d => {
                this.Bind(this.ViewModel, vm => vm.Url, v => v.url.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.UserName, v => v.userName.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Timeout, v => v.timeout.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.BasicHttpSecurityMode, v => v.securitymode.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Site, v => v.site.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.User, v => v.user.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Operation, v => v.operation.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.OperationRevision, v => v.operationRevision.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.DcGroup, v => v.dcgroup.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.DcGroupRevision, v => v.dcgrouprevision.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Resource, v => v.resource.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.DcMode, v => v.dcmode.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Material, v => v.material.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.MaterialRevision, v => v.materialRevision.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.DcGroupSequence, v => v.dcGroupSequence.Text).DisposeWith(d);

                this.BindCommand(this.ViewModel, vm => vm.CmdSave, v => v.save).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdReload, v => v.reload).DisposeWith(d);
            });
        }

        #region ViewModel
        public DataCollectForResourceFAIVM ViewModel
        {
            get { return (DataCollectForResourceFAIVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object? IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (DataCollectForResourceFAIVM) value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DataCollectForResourceFAIVM), typeof(DataCollectForResourceFAICtrl), new PropertyMetadata(null));
        #endregion
    }
}
