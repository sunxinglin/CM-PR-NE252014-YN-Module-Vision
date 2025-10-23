using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.Loading.Model;

namespace VisDummy.WPF.Views.Monitor.CommonCtrl
{
    /// <summary>
    /// LoadingStation2DCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingStation2DCtrl : UserControl, INotifyPropertyChanged
    {
        public LoadingStation2DCtrl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DevMsg_2DStation DevMsg
        {
            get { return (DevMsg_2DStation)GetValue(DevMsgProperty); }
            set { SetValue(DevMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DevMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DevMsgProperty =
            DependencyProperty.Register("DevMsg", typeof(DevMsg_2DStation), typeof(LoadingStation2DCtrl), new PropertyMetadata(null, DevMsgCallBack));
        protected virtual void NotifyDevPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Trigger)));
        }
        private static void DevMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LoadingStation2DCtrl ctrl)
            {
                ctrl.NotifyDevPropertyChanged();
            }
        }
        public Dev_CmdTrigger Trigger => DevMsg?.CmdTrigger;

        public MstMsg_2DStation MstMsg
        {
            get { return (MstMsg_2DStation)GetValue(MstMsgProperty); }
            set { SetValue(MstMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MstMsg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MstMsgProperty =
            DependencyProperty.Register("MstMsg", typeof(MstMsg_2DStation), typeof(LoadingStation2DCtrl), new PropertyMetadata(null, MstMsgCallBack));

        protected virtual void NotifyMstPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Reply)));
        }
        private static void MstMsgCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LoadingStation2DCtrl ctrl)
            {
                ctrl.NotifyMstPropertyChanged();
            }
        }
        public Mst_CmdReply Reply => MstMsg?.CmdReply;
        public string BarCode => MstMsg?.BarCode?.EffectiveContent ?? string.Empty;
    }
}
