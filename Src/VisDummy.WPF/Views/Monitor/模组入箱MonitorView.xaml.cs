using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Views.Monitor
{
    /// <summary>
    /// Interaction logic for 模组入箱MonitorView.xaml
    /// </summary>
    public partial class 模组入箱MonitorView : UserControl, IViewFor<模组入箱MonitorViewModel>
    {
        public 模组入箱MonitorView()
        {
            InitializeComponent();
            this.ViewModel = Locator.Current.GetRequiredService<模组入箱MonitorViewModel>();
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
        public 模组入箱MonitorViewModel ViewModel
        {
            get { return (模组入箱MonitorViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (模组入箱MonitorViewModel)value; }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(模组入箱MonitorViewModel), typeof(模组入箱MonitorView), new PropertyMetadata(null));
        #endregion
    }
}
