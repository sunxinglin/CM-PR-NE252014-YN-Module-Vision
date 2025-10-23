using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using Splat;
using TApp.ViewModels.Realtime;

namespace TApp.Views.Realtime
{
    /// <summary>
    /// Interaction logic for RealtimeView.xaml
    /// </summary>
    public partial class RealtimeView : UserControl, IViewFor<RealtimeViewModel>
    {
        public RealtimeView()
        {
            InitializeComponent();

            this.ViewModel = Locator.Current.GetService<RealtimeViewModel>()!;
            this.WhenActivated(d => {

                this.OneWayBind(this.ViewModel, vm => vm.UIlogsViewModel, v => v.uilogView.ViewModel).DisposeWith(d);

                #region Vision Rt Views
                this.Bind(this.ViewModel, vm => vm.RtMarker, v => v.viewRtHost.ViewModel).DisposeWith(d);
                #endregion
            });
        }

        #region ViewModel
        public RealtimeViewModel ViewModel
        {
            get { return (RealtimeViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (RealtimeViewModel) value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(RealtimeViewModel), typeof(RealtimeView), new PropertyMetadata(null));
        #endregion
    }
}
