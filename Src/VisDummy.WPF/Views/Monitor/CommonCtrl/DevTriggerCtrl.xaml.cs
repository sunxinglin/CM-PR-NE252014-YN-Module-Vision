using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// DevTriggerCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class DevTriggerCtrl : UserControl
    {
        public DevTriggerCtrl()
        {
            InitializeComponent();
        }

        public Dev_CmdTrigger DevMsg
        {
            get { return (Dev_CmdTrigger)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(Dev_CmdTrigger), typeof(DevTriggerCtrl), new PropertyMetadata(null));
    }
}
