using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using TApp.ViewModels.Dbg;

namespace TApp.Views.Dbg
{
    /// <summary>
    /// Interaction logic for DbgToolsView.xaml
    /// </summary>
    public partial class DbgToolsView : UserControl, IViewFor<DbgToolsViewModel>
    {
        public DbgToolsView()
        {
            InitializeComponent();


            this.WhenActivated(d => {
                this.OneWayBind(this.ViewModel, vm => vm.DbgMarker, v => v.viewhost.ViewModel).DisposeWith(d);
            });
        }

        #region
        public DbgToolsViewModel ViewModel
        {
            get { return (DbgToolsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel =(DbgToolsViewModel) value; }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DbgToolsViewModel), typeof(DbgToolsView), new PropertyMetadata(null));
        #endregion
    }
}
