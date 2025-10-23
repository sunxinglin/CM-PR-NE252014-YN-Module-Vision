using System.Windows;
using System.Windows.Controls;
using VisDummy.MKVMs.ViewModels;
using VisDummy.MKVMs.Views;
using VisDummy.Shared.Utils;
using VisDummy.VMs.ViewModels;
using VisDummy.VMs.Views;

namespace VisDummy.WPF.Common
{
    /// <summary>
    /// DynamicVisionCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class DynamicVisionCtrl : UserControl
    {
        public DynamicVisionCtrl()
        {
            InitializeComponent();
        }
        public IEnumerable<IVisionMarker> ViewModels
        {
            get { return (IEnumerable<IVisionMarker>)GetValue(ViewModelsProperty); }
            set { SetValue(ViewModelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelsProperty =
            DependencyProperty.Register("ViewModels", typeof(IEnumerable<IVisionMarker>), typeof(DynamicVisionCtrl), new PropertyMetadata(null, OnValueChangedCallback));

        private static void OnValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IEnumerable<IVisionMarker> vms)
            {
                if (d is DynamicVisionCtrl ctrl)
                {
                    foreach (var vm in vms)
                    {
                        if (vm is VisRtViewModel vis2d)
                        {
                            var view = new VisRtView { ViewModel = vis2d };
                            ctrl.itemsControl.Items.Add(view);
                        }
                        if (vm is Vis3DRtViewModel vis3d)
                        {
                            var view = new Vis3DRtView { ViewModel = vis3d };
                            ctrl.itemsControl.Items.Add(view);
                        }
                    }
                }
            }
        }
    }
}
