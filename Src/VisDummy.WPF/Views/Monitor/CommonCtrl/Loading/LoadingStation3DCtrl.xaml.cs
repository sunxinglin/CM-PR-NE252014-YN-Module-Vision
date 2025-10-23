using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.Loading.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// LoadingStation3DCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingStation3DCtrl : UserControl
    {
        public LoadingStation3DCtrl()
        {
            InitializeComponent();
        }

        public DevMsg_3DStation DevMsg
        {
            get { return (DevMsg_3DStation)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(DevMsg_3DStation), typeof(LoadingStation3DCtrl), new PropertyMetadata(null));

        public MstMsg_3DStation MstMsg
        {
            get { return (MstMsg_3DStation)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(MstMsg_3DStation), typeof(LoadingStation3DCtrl), new PropertyMetadata(null));
    }
}
