using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.VMs.ViewModels;

namespace VisDummy.VMs.Views
{
    /// <summary>
    /// GlobalParamsView.xaml 的交互逻辑
    /// </summary>
    public partial class GlobalParamsView : UserControl, IViewFor<GlobalParamsViewModel>
    {
        public GlobalParamsView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.SolutionParams, v => v.list.ItemsSource).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdReload, v => v.reload).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdSave, v => v.save).DisposeWith(d);
            });
        }

        #region View Model
        public GlobalParamsViewModel ViewModel
        {
            get { return (GlobalParamsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (GlobalParamsViewModel)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(GlobalParamsViewModel), typeof(GlobalParamsView), new PropertyMetadata(null));
        #endregion
    }
}
