using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.侧板自动拧紧.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
	/// <summary>
	/// 侧板自动拧紧CAM2.xaml 的交互逻辑
	/// </summary>
	public partial class 侧板自动拧紧CAM2 : UserControl, INotifyPropertyChanged
	{
		public 侧板自动拧紧CAM2()
		{
			InitializeComponent();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public DevMsg_CAM2 DevMsg
		{
			get { return (DevMsg_CAM2)GetValue(DevMsgProperty); }
			set { SetValue(DevMsgProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DevMsgProperty =
			DependencyProperty.Register("DevMsg", typeof(DevMsg_CAM2), typeof(侧板自动拧紧CAM2), new PropertyMetadata(null, DevMsgCallBack));
		protected virtual void NotifyDevPropertyChanged()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dev_PhotoREQ)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dev_FunctionNumber)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dev_PhotoNumben)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dev_PositionX)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dev_PositionY)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dev_PositionZ)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dev_PositionA)));
		}
		private static void DevMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is 侧板自动拧紧CAM2 ctrl)
			{
				ctrl.NotifyDevPropertyChanged();
			}
		}
		public bool? Dev_PhotoREQ => DevMsg?.Flag.Req;
		public ushort? Dev_FunctionNumber => DevMsg?.Flag.FunctionNumber;
		public ushort? Dev_PhotoNumben => DevMsg?.Flag.PhotoNumben;
		public float? Dev_PositionX => DevMsg?.Flag.PositionX;
		public float? Dev_PositionY => DevMsg?.Flag.PositionY;
		public float? Dev_PositionZ => DevMsg?.Flag.PositionZ;
		public float? Dev_PositionA => DevMsg?.Flag.PositionA;

		public MstMsg_CAM2 MstMsg
		{
			get { return (MstMsg_CAM2)GetValue(MstMsgProperty); }
			set { SetValue(MstMsgProperty, value); }
		}

		// Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MstMsgProperty =
			DependencyProperty.Register("MstMsg", typeof(MstMsg_CAM2), typeof(侧板自动拧紧CAM2), new PropertyMetadata(null, MstMsgCallBack));

		protected virtual void NotifyMstPropertyChanged()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Reply)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Z)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(A)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(特征点NG)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(视觉检测流程NG)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(其它NG)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PLC参数NG)));

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Response_Ok)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Response_Ng)));
		}
		private static void MstMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is 侧板自动拧紧CAM2 ctrl)
			{
				ctrl.NotifyMstPropertyChanged();
			}
		}
		public bool? Reply => MstMsg?.Flag.Ready;

		public bool? Status => MstMsg?.Flag.Status == 1;
		public float? X => MstMsg?.Flag.X;
		public float? Y => MstMsg?.Flag.Y;
		public float? Z => MstMsg?.Flag.Z;
		public float? A => MstMsg?.Flag.A;

		public bool? 特征点NG => MstMsg?.Flag.特征点NG;
		public bool? 视觉检测流程NG => MstMsg?.Flag.视觉检测流程NG;
		public bool? 其它NG => MstMsg?.Flag.其它NG;
		public bool? PLC参数NG => MstMsg?.Flag.PLC参数NG;

		public bool? Response_Ok => MstMsg?.Flag?.Response_Ok;
		public bool? Response_Ng => MstMsg?.Flag?.Response_Ng;
	}
}
