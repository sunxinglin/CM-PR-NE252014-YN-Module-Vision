using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// MstSpotCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class MstSpotCtrl : UserControl
    {
        public MstSpotCtrl()
        {
            InitializeComponent();
        }

        public Mst_CmdSpot MstMsg
        {
            get { return (Mst_CmdSpot)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(Mst_CmdSpot), typeof(MstSpotCtrl), new PropertyMetadata(null));
    }
}
