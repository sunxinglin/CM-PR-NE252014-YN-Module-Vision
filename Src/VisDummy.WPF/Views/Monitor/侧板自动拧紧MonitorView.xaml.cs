using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Shared.Utils;
using VisDummy.WPF.ViewModels.Monitor;

namespace VisDummy.WPF.Views.Monitor
{
	/// <summary>
	/// 侧板自动拧紧MonitorView.xaml 的交互逻辑
	/// </summary>
	public partial class 侧板自动拧紧MonitorView : UserControl, IViewFor<侧板自动拧紧MonitorViewModel>
	{
		public 侧板自动拧紧MonitorView()
		{
			InitializeComponent();
			this.ViewModel = Locator.Current.GetRequiredService<侧板自动拧紧MonitorViewModel>();
			this.WhenActivated(d =>
			{
				this.OneWayBind(this.ViewModel, vm => vm.Dev_CmdHeart, v => v.heart.DevMsg).DisposeWith(d);
				this.OneWayBind(this.ViewModel, vm => vm.Mst_CmdHeart, v => v.heart.MstMsg).DisposeWith(d);

				this.OneWayBind(this.ViewModel, vm => vm.DevMsg_CAM1, v => v.侧板自动拧紧CAM1.DevMsg).DisposeWith(d);
				this.OneWayBind(this.ViewModel, vm => vm.DevMsg_CAM2, v => v.侧板自动拧紧CAM2.DevMsg).DisposeWith(d);
				this.OneWayBind(this.ViewModel, vm => vm.DevMsg_CAM3, v => v.侧板自动拧紧CAM3.DevMsg).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_CAM1, v => v.侧板自动拧紧CAM1.MstMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_CAM2, v => v.侧板自动拧紧CAM2.MstMsg).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.MstMsg_CAM3, v => v.侧板自动拧紧CAM3.MstMsg).DisposeWith(d);
            });
		}

		#region ViewModel
		public 侧板自动拧紧MonitorViewModel ViewModel
		{
			get { return (侧板自动拧紧MonitorViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (侧板自动拧紧MonitorViewModel)value; }

		public static readonly DependencyProperty ViewModelProperty =
			DependencyProperty.Register(nameof(ViewModel), typeof(侧板自动拧紧MonitorViewModel), typeof(侧板自动拧紧MonitorView), new PropertyMetadata(null));
		#endregion
	}
}
