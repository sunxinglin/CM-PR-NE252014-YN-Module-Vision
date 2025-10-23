using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TApp.ViewModels;
using VisDummy.Shared.Utils;

namespace TApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ViewModel = Locator.Current.GetRequiredService<MainViewModel>();

            this.WhenActivated(d =>
            {

                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.AppTitle, v => v.Title).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.AppTitle, v => v.txtTitle.Text).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.EquipId, v => v.txtEquipId.Text).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.UserName, v => v.txtUserName.Text).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.CpuUsage, v => v.CPU.Text).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.DiskUsage, v => v.Disk.Text).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.MemoryUsage, v => v.Memory.Text).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.VisionCameraStatusDic, v => v.cameraItems.ItemsSource).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.CanAccessParamsSetting, v => v.menuSettings.IsEnabled).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.CanAccessParamsSetting, v => v.menuDbgTool.IsEnabled).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.RemainingTime, v => v.txtRemainingTime.Text).DisposeWith(d);

                this.Bind(this.ViewModel, vm => vm.AppViewModel.Language, v => v.langSelect.SelectedValue).DisposeWith(d);

                this.OneWayBind(this.ViewModel, vm => vm.AppViewModel.Router, x => x.routedHost.Router).DisposeWith(d);

                this.ViewModel.AppViewModel.Router.Navigate.ThrownExceptions
                    .Subscribe(e => MessageBox.Show(e.Message))
                    .DisposeWith(d);
            });

        }

        #region
        public MainViewModel ViewModel
        {
            get { return (MainViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MainViewModel), typeof(MainWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (MainViewModel)value; }
        #endregion


        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var res = MessageBox.Show("是否真的要退出？", "退出程序确认", MessageBoxButton.YesNo);
            if (!res.Equals(MessageBoxResult.Yes))
            {
                e.Cancel = true;
                return;
            }
        }

        private void ToggleLeftNavBarShowHiden(object sender, MouseButtonEventArgs e)
        {
            if (left.Visibility != Visibility.Visible)
            {
                left.Visibility = Visibility.Visible;
            }
            else
            {
                left.Visibility = Visibility.Collapsed;
            }
            e.Handled = true;
        }


        private void MenuClick_Realtime(object sender, RoutedEventArgs e)
        {
            this.ViewModel.AppViewModel.NavigateTo(UrlDefines.URL_Realtime);
        }

        private void MenuClick_UserMgmt(object sender, RoutedEventArgs e)
        {
            this.ViewModel.AppViewModel.NavigateTo(UrlDefines.URL_UserMgmt);
        }

        private void MenuClick_ParamsMgmt(object sender, RoutedEventArgs e)
        {
            this.ViewModel.AppViewModel.NavigateTo(UrlDefines.URL_ParamsMgmt);
        }

        private void MenuClick_DebugTools(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            this.ViewModel.AppViewModel.NavigateTo(UrlDefines.URL_DebugTools);
        }

        private void LangSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.AppViewModel.CmdSwitchLanguage.Execute().Subscribe();
        }

        private void MenuClick_LogSearch(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            this.ViewModel.AppViewModel.NavigateTo(UrlDefines.URL_LogSearch);
        }
    }
}
