using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using TApp.ViewModels.LogSearch;

namespace TApp.Views.LogSearch
{
    /// <summary>
    /// LogSearchView.xaml 的交互逻辑
    /// </summary>
    public partial class LogSearchView : UserControl, IViewFor<LogSearchViewModel>
    {
        public LogSearchView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {

                this.BindCommand(this.ViewModel, vm => vm.CmdLoadResouces, v => v.btnReload, nameof(btnReload.Click)).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdPrevPage, v => v.btnPrevPage, nameof(btnPrevPage.Click)).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdNextPage, v => v.btnNextPage, nameof(btnNextPage.Click)).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.Resources, v => v.lstResources.ItemsSource).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.Current, v => v.txtPageIndex.Text).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.Total, v => v.txtTotal.Text).DisposeWith(d);

                this.BindCommand(this.ViewModel, vm => vm.CmdSearch, v => v.btnSearch, nameof(btnSearch.Click)).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Source, v => v.txtSearchSource.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Group, v => v.txtSearchGroup.Text).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.EnumValues, v => v.ddSearchLevel.ItemsSource).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Level, v => v.ddSearchLevel.SelectedValue).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Content, v => v.txtSearchContent.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.StartTime, v => v.dpStartTime.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.EndTime, v => v.dpEndTime.Text).DisposeWith(d);

            });
        }

        #region ViewModel
        public LogSearchViewModel ViewModel
        {
            get { return (LogSearchViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (LogSearchViewModel)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(LogSearchViewModel), typeof(LogSearchView), new PropertyMetadata(null));
        #endregion
    }
}
