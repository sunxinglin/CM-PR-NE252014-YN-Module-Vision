using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// MstReplyCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class MstReplyCtrl : UserControl
    {
        public MstReplyCtrl()
        {
            InitializeComponent();
        }

        public Mst_CmdReply MstMsg
        {
            get { return (Mst_CmdReply)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(Mst_CmdReply), typeof(MstReplyCtrl), new PropertyMetadata(null));
    }
}
