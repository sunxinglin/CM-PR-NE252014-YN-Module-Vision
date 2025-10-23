using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Views.Monitor
{
    /// <summary>
    /// Interaction logic for 模组检测MonitorView.xaml
    /// </summary>
    public partial class 模组检测MonitorView : UserControl, IViewFor<模组检测MonitorViewModel>
    {
        public 模组检测MonitorView()
        {
            InitializeComponent();
            this.ViewModel = Locator.Current.GetRequiredService<模组检测MonitorViewModel>();
            this.WhenActivated(d =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.Dev_CmdHeart, v => v.heart.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.Mst_CmdHeart, v => v.heart.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_2DStation, v => v.station2d.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_2DStation, v => v.station2d.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_2DSpotStation, v => v.station2dspot.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_2DSpotStation, v => v.station2dspot.MstMsg).DisposeWith(d);
            });
        }

        #region ViewModel
        public 模组检测MonitorViewModel ViewModel
        {
            get { return (模组检测MonitorViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (模组检测MonitorViewModel)value; }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(模组检测MonitorViewModel), typeof(模组检测MonitorView), new PropertyMetadata(null));
        #endregion
    }
}
