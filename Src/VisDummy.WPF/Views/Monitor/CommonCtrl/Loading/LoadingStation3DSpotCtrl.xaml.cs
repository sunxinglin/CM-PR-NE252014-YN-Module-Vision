using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.Loading.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// LoadingStation3DSpotCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingStation3DSpotCtrl : UserControl
    {
        public LoadingStation3DSpotCtrl()
        {
            InitializeComponent();
        }

        public DevMsg_3DSpotStation DevMsg
        {
            get { return (DevMsg_3DSpotStation)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(DevMsg_3DSpotStation), typeof(LoadingStation3DSpotCtrl), new PropertyMetadata(null));

        public MstMsg_3DSpotStation MstMsg
        {
            get { return (MstMsg_3DSpotStation)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(MstMsg_3DSpotStation), typeof(LoadingStation3DSpotCtrl), new PropertyMetadata(null));
    }
}
