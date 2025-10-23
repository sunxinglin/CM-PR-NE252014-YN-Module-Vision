using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Views.Monitor
{
    /// <summary>
    /// Interaction logic for 垫片检测MonitorView.xaml
    /// </summary>
    public partial class 垫片检测MonitorView : UserControl, IViewFor<垫片检测MonitorViewModel>
    {
        public 垫片检测MonitorView()
        {
            InitializeComponent();
            this.ViewModel = Locator.Current.GetRequiredService<垫片检测MonitorViewModel>();
            this.WhenActivated(d =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.Dev_CmdHeart, v => v.heart.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.Mst_CmdHeart, v => v.heart.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_2DStation1, v => v.station2d1.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_2DStation1, v => v.station2d1.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_2DSpotStation, v => v.station2dspot.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_2DSpotStation, v => v.station2dspot.MstMsg).DisposeWith(d);
            });
        }

        #region ViewModel
        public 垫片检测MonitorViewModel ViewModel
        {
            get { return (垫片检测MonitorViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (垫片检测MonitorViewModel)value; }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(垫片检测MonitorViewModel), typeof(垫片检测MonitorView), new PropertyMetadata(null));
        #endregion
    }
}
