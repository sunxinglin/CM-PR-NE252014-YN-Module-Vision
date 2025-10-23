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
    public partial class 人工位MonitorView : UserControl, IViewFor<人工位MonitorViewModel>
    {
        public 人工位MonitorView()
        {
            InitializeComponent();
            this.ViewModel = Locator.Current.GetRequiredService<人工位MonitorViewModel>();
            this.WhenActivated(d =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.Dev_CmdHeart, v => v.heart.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.Mst_CmdHeart, v => v.heart.MstMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.DevMsg_水冷板检测, v => v.水冷板检测.DevMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_水冷板检测, v => v.水冷板检测.MstMsg).DisposeWith(d);

            });
        }

        #region ViewModel
        public 人工位MonitorViewModel ViewModel
        {
            get { return (人工位MonitorViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (人工位MonitorViewModel)value; }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(人工位MonitorViewModel), typeof(人工位MonitorView), new PropertyMetadata(null));
        #endregion
    }
}
