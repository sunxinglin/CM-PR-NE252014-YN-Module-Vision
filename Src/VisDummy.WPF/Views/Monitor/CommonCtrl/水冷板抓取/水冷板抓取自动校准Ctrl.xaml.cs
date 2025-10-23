using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.水冷板抓取.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// 模组贴标Station2DCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class 水冷板抓取自动校准Ctrl : UserControl, INotifyPropertyChanged
    {
        public 水冷板抓取自动校准Ctrl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DevMsg_自动校准 DevMsg
        {
            get { return (DevMsg_自动校准)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(DevMsg_自动校准), typeof(水冷板抓取自动校准Ctrl), new PropertyMetadata(null, DevMsgCallBack));
        protected virtual void NotifyDevPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Trigger)));
        }
        private static void DevMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is 水冷板抓取自动校准Ctrl ctrl)
            {
                ctrl.NotifyDevPropertyChanged();
            }
        }
        public bool? Trigger => DevMsg?.Trigger;

        public MstMsg_自动校准 MstMsg
        {
            get { return (MstMsg_自动校准)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(MstMsg_自动校准), typeof(水冷板抓取自动校准Ctrl), new PropertyMetadata(null, MstMsgCallBack));

        protected virtual void NotifyMstPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Reply)));
        }
        private static void MstMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is 水冷板抓取自动校准Ctrl ctrl)
            {
                ctrl.NotifyMstPropertyChanged();
            }
        }
        public bool? Reply => MstMsg?.Ack;
    }
}
