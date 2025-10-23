using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// HeartCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class HeartCtrl : UserControl
    {
        public HeartCtrl()
        {
            InitializeComponent();
        }

        public Dev_CmdHeart DevMsg
        {
            get { return (Dev_CmdHeart)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(Dev_CmdHeart), typeof(HeartCtrl), new PropertyMetadata(null));

        public Mst_CmdHeart MstMsg
        {
            get { return (Mst_CmdHeart)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(Mst_CmdHeart), typeof(HeartCtrl), new PropertyMetadata(null));
    }
}
