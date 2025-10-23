using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using StdUnit.One.Shared;
using TApp.ViewModels.Realtime;

namespace TApp.Views.Realtime
{
    /// <summary>
    /// Interaction logic for UILogsView.xaml
    /// </summary>
    public partial class UILogsView : UserControl, IViewFor<UILogsViewModel>
    {
        public UILogsView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.Logs, v => v.logs.ItemsSource).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.EventGroup, v => v.logsEventGroup.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.ScrollEnabled, v => v.tgBtnEnableScroll.IsChecked).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdClearFilter, v => v.clearFilter).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdClear, v => v.clear, nameof(this.clear.Click)).DisposeWith(d);
                this.ViewModel?.ChangeObs.ObserveOn(RxApp.MainThreadScheduler).Subscribe(lm => { if (this.ViewModel.ScrollEnabled) this.scroll.ScrollToEnd(); }).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.Logs2, v => v.logs2.ItemsSource).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.EventGroup2, v => v.logsEventGroup2.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.ScrollEnabled2, v => v.tgBtnEnableScroll2.IsChecked).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdClearFilter2, v => v.clearFilter2).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdClear2, v => v.clear2, nameof(this.clear2.Click)).DisposeWith(d);
                this.ViewModel?.ChangeObs2.ObserveOn(RxApp.MainThreadScheduler).Subscribe(lm => { if (this.ViewModel.ScrollEnabled2) this.scroll2.ScrollToEnd(); }).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.Logs3, v => v.logs3.ItemsSource).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.EventGroup3, v => v.logsEventGroup3.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.ScrollEnabled3, v => v.tgBtnEnableScroll3.IsChecked).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdClearFilter3, v => v.clearFilter3).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdClear3, v => v.clear3, nameof(this.clear3.Click)).DisposeWith(d);
                this.ViewModel?.ChangeObs3.ObserveOn(RxApp.MainThreadScheduler).Subscribe(lm => { if (this.ViewModel.ScrollEnabled3) this.scroll3.ScrollToEnd(); }).DisposeWith(d);

            });
        }

        #region ViewModel
        public UILogsViewModel ViewModel
        {
            get { return (UILogsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (UILogsViewModel)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(UILogsViewModel), typeof(UILogsView), new PropertyMetadata(null));
        #endregion

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuitem = sender as MenuItem;
            var logmsg = menuitem.DataContext as LogMessage;
            this.logsEventGroup.Text = logmsg.EventGroup;
        }
    }
}
