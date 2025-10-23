using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.垫片检测.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// 垫片检测Station2DSpotCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class 垫片检测Station2DSpotCtrl : UserControl, INotifyPropertyChanged
    {
        public 垫片检测Station2DSpotCtrl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DevMsg_2DSpotStation DevMsg
        {
            get { return (DevMsg_2DSpotStation)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(DevMsg_2DSpotStation), typeof(垫片检测Station2DSpotCtrl), new PropertyMetadata(null, DevMsgCallBack));
        protected virtual void NotifyDevPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Trigger)));
        }
        private static void DevMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is 垫片检测Station2DSpotCtrl ctrl)
            {
                ctrl.NotifyDevPropertyChanged();
            }
        }
        public Dev_CmdSpot Trigger => DevMsg?.CmdSpot;

        public MstMsg_2DSpotStation MstMsg
        {
            get { return (MstMsg_2DSpotStation)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(MstMsg_2DSpotStation), typeof(垫片检测Station2DSpotCtrl), new PropertyMetadata(null, MstMsgCallBack));

        protected virtual void NotifyMstPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Reply)));
        }
        private static void MstMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is 垫片检测Station2DSpotCtrl ctrl)
            {
                ctrl.NotifyMstPropertyChanged();
            }
        }
        public Mst_CmdSpot Reply => MstMsg?.CmdSpot;
    }
}
