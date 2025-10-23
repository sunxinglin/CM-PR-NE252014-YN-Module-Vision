using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
	/// <summary>
	/// HeartSideCtrl.xaml 的交互逻辑
	/// </summary>
	public partial class HeartSideCtrl : UserControl
	{
		public HeartSideCtrl()
		{
			InitializeComponent();
		}

		public Dev_SideHeart DevMsg
		{
			get { return (Dev_SideHeart)GetValue(DevMsgProperty); }
			set { SetValue(DevMsgProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DevMsgProperty =
			DependencyProperty.Register("DevMsg", typeof(Dev_SideHeart), typeof(HeartSideCtrl), new PropertyMetadata(null));

		public Mst_SideHeart MstMsg
		{
			get { return (Mst_SideHeart)GetValue(MstMsgProperty); }
			set { SetValue(MstMsgProperty, value); }
		}

		// Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MstMsgProperty =
			DependencyProperty.Register("MstMsg", typeof(Mst_SideHeart), typeof(HeartSideCtrl), new PropertyMetadata(null));
	}
}
