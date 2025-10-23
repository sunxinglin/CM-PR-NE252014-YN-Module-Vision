using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Views.Monitor
{
    /// <summary>
    /// Interaction logic for 模组贴标MonitorView.xaml
    /// </summary>
    public partial class 水冷板抓取MonitorView : UserControl, IViewFor<水冷板抓取MonitorViewModel>
    {
        public 水冷板抓取MonitorView()
        {
            InitializeComponent();
            this.ViewModel = Locator.Current.GetRequiredService<水冷板抓取MonitorViewModel>();
            this.WhenActivated(d =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.Dev_CmdHeart, v => v.heart.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.Mst_CmdHeart, v => v.heart.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_机器人1定位引导, v => v.station1定位引导.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_机器人1定位引导, v => v.station1定位引导.MstMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_机器人2定位引导, v => v.station2定位引导.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_机器人2定位引导, v => v.station2定位引导.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_机器人1校验, v => v.station1校验.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_机器人1校验, v => v.station1校验.MstMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_机器人2校验, v => v.station2校验.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_机器人2校验, v => v.station2校验.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_机器人1自动校准, v => v.station1自动校准.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_机器人1自动校准, v => v.station1自动校准.MstMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_机器人2自动校准, v => v.station2自动校准.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_机器人2自动校准, v => v.station2自动校准.MstMsg).DisposeWith(d);
            });
        }

        #region ViewModel
        public 水冷板抓取MonitorViewModel ViewModel
        {
            get { return (水冷板抓取MonitorViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (水冷板抓取MonitorViewModel)value; }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(水冷板抓取MonitorViewModel), typeof(水冷板抓取MonitorView), new PropertyMetadata(null));
        #endregion
    }
}
