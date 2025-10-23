using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.WPF.ViewModels;

namespace VisDummy.WPF.Views.Basics
{
    /// <summary>
    /// Interaction logic for DbgView.xaml
    /// </summary>
    public partial class DbgView : UserControl, IViewFor<DbgViewModel>
    {
        public DbgView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.Bind(this.ViewModel, vm => vm.CalibVM, v => v.caliView.ViewModel).DisposeWith(d);
            });
        }

        #region


        public DbgViewModel ViewModel
        {
            get { return (DbgViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => ViewModel; set => this.ViewModel = (DbgViewModel?)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DbgViewModel), typeof(DbgView), new PropertyMetadata(null));
        #endregion
    }
}
