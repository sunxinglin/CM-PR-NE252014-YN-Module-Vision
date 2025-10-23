using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.人工位.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// 模组贴标Station2DCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class 人工位水冷板检测Ctrl : UserControl, INotifyPropertyChanged
    {
        public 人工位水冷板检测Ctrl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DevMsg_水冷板检测 DevMsg
        {
            get { return (DevMsg_水冷板检测)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(DevMsg_水冷板检测), typeof(人工位水冷板检测Ctrl), new PropertyMetadata(null, DevMsgCallBack));
        protected virtual void NotifyDevPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Trigger)));
        }
        private static void DevMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is 人工位水冷板检测Ctrl ctrl)
            {
                ctrl.NotifyDevPropertyChanged();
            }
        }
        public bool? Trigger => DevMsg?.Trigger;

        public MstMsg_水冷板检测 MstMsg
        {
            get { return (MstMsg_水冷板检测)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(MstMsg_水冷板检测), typeof(人工位水冷板检测Ctrl), new PropertyMetadata(null, MstMsgCallBack));

        protected virtual void NotifyMstPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Reply)));
        }
        private static void MstMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is 人工位水冷板检测Ctrl ctrl)
            {
                ctrl.NotifyMstPropertyChanged();
            }
        }
        public bool? Reply => MstMsg?.Ack;
    }
}
