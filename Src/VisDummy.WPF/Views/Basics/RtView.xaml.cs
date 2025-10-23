using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.WPF.ViewModels;

namespace VisDummy.WPF.Views.Basics
{
    /// <summary>
    /// Interaction logic for RtView.xaml
    /// </summary>
    public partial class RtView : UserControl, IViewFor<RtViewModel>
    {
        public RtView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.Bind(this.ViewModel, vm => vm.DynamicViewModel, v => v.viewDynamicVisionCtrl.ViewModels).DisposeWith(d);
            });
        }

        #region ViewModel

        public RtViewModel ViewModel
        {
            get { return (RtViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (RtViewModel)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(RtViewModel), typeof(RtView), new PropertyMetadata(null));
        #endregion
    }
}
