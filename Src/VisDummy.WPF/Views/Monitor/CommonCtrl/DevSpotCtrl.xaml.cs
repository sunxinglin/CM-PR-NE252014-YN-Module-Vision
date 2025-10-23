using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// DevSpotCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class DevSpotCtrl : UserControl
    {
        public DevSpotCtrl()
        {
            InitializeComponent();
        }

        public Dev_CmdSpot DevMsg
        {
            get { return (Dev_CmdSpot)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(Dev_CmdSpot), typeof(DevSpotCtrl), new PropertyMetadata(null));
    }
}
