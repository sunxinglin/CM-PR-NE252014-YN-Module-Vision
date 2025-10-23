using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Views.Monitor
{
    /// <summary>
    /// Interaction logic for LoadingMonitorView.xaml
    /// </summary>
    public partial class LoadingMonitorView : UserControl, IViewFor<LoadingMonitorViewModel>
    {
        public LoadingMonitorView()
        {
            InitializeComponent();
            this.ViewModel = Locator.Current.GetRequiredService<LoadingMonitorViewModel>();
            this.WhenActivated(d =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.Dev_CmdHeart, v => v.heart.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.Mst_CmdHeart, v => v.heart.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_3DStation, v => v.station3d.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_3DStation, v => v.station3d.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_3DSpotStation, v => v.station3dspot.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_3DSpotStation, v => v.station3dspot.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_2DStation, v => v.station2d.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_2DStation, v => v.station2d.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_2DSpotStation, v => v.station2dspot.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_2DSpotStation, v => v.station2dspot.MstMsg).DisposeWith(d);

            });
        }

        #region ViewModel
        public LoadingMonitorViewModel ViewModel
        {
            get { return (LoadingMonitorViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (LoadingMonitorViewModel)value; }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(LoadingMonitorViewModel), typeof(LoadingMonitorView), new PropertyMetadata(null));
        #endregion
    }
}
