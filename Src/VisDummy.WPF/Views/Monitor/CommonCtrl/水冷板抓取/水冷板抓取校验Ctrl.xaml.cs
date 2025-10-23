using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.水冷板抓取.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// 模组贴标Station2DCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class 水冷板抓取校验Ctrl : UserControl, INotifyPropertyChanged
    {
        public 水冷板抓取校验Ctrl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DevMsg_校验 DevMsg
        {
            get { return (DevMsg_校验)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(DevMsg_校验), typeof(水冷板抓取校验Ctrl), new PropertyMetadata(null, DevMsgCallBack));
        protected virtual void NotifyDevPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Trigger)));
        }
        private static void DevMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is 水冷板抓取校验Ctrl ctrl)
            {
                ctrl.NotifyDevPropertyChanged();
            }
        }
        public bool? Trigger => DevMsg?.Trigger;

        public MstMsg_校验 MstMsg
        {
            get { return (MstMsg_校验)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(MstMsg_校验), typeof(水冷板抓取校验Ctrl), new PropertyMetadata(null, MstMsgCallBack));

        protected virtual void NotifyMstPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Reply)));
        }
        private static void MstMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is 水冷板抓取校验Ctrl ctrl)
            {
                ctrl.NotifyMstPropertyChanged();
            }
        }
        public bool? Reply => MstMsg?.Ack;
    }
}
