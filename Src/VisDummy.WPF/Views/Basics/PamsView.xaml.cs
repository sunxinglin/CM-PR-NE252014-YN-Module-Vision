using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.WPF.ViewModels;

namespace VisDummy.WPF.Views.Basics
{
    /// <summary>
    /// Interaction logic for PamsView.xaml
    /// </summary>
    public partial class PamsView : UserControl, IViewFor<PamsViewModel>
    {
        public PamsView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.Bind(this.ViewModel, vm => vm.MesEditVM, v => v.mesEditView.ViewModel).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.MesTestVM, v => v.mesTestView.ViewModel).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.GlobalParamsVM, v => v.vmGlobalParams.ViewModel).DisposeWith(d);
            });
        }

        #region


        public PamsViewModel ViewModel
        {
            get { return (PamsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => ViewModel; set => this.ViewModel = (PamsViewModel?)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PamsViewModel), typeof(PamsView), new PropertyMetadata(null));
        #endregion
    }
}
