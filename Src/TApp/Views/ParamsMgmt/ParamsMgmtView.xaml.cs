using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using TApp.ViewModels.ParamsMgmt;

namespace TApp.Views.ParamsMgmt
{
    /// <summary>
    /// Interaction logic for ParamsMgmtView.xaml
    /// </summary>
    public partial class ParamsMgmtView : UserControl, IViewFor<ParamsMgmtViewModel>
    {
        public ParamsMgmtView()
        {
            InitializeComponent();

            this.WhenActivated(d => {
                this.OneWayBind(this.ViewModel, vm => vm.PamsMarker, v => v.viewhost.ViewModel).DisposeWith(d);
            });
        }

        #region ViewModel
        public ParamsMgmtViewModel ViewModel
        {
            get { return (ParamsMgmtViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (ParamsMgmtViewModel) value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ParamsMgmtViewModel), typeof(ParamsMgmtView), new PropertyMetadata(null));
        #endregion
    }
}
