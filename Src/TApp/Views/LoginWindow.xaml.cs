#define SWEETY_BOY_MODE

using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Input;
using ReactiveUI;
using VisDummy.Shared.Utils;
using Splat;
using TApp.ViewModels;

namespace TApp.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, IViewFor<LoginViewModel>
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.ViewModel = Locator.Current.GetRequiredService<LoginViewModel>();
            //this.Password.Password = "123456";
#if SWEETY_BOY_MODE
            this.CardNo.Visibility = Visibility.Hidden;
            this.CardNo.IsEnabled = false;
            this.LoginByCardLabel.Visibility = Visibility.Visible;
            this.loginByPasswordTab.Visibility = Visibility.Hidden;
            TextCompositionManager.AddPreviewTextInputStartHandler(this, new TextCompositionEventHandler(PrevewTextHandler));
            this.AddHandler(Window.KeyDownEvent, new KeyEventHandler(KeyDownHandler));
#endif



            this.WhenActivated(d =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.AppName, v => v.Title).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AppName, v => v.appName.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Account, v => v.Account.Text).DisposeWith(d);
            });
        }

        #region


        public LoginViewModel ViewModel
        {
            get { return (LoginViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(LoginViewModel), typeof(LoginWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (LoginViewModel)value; }
        #endregion


        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ViewModel.CmdLoginByCard.Execute(this.CardNo);
            }

            if (e.Key == Key.Escape)
            {
                this.CardNo.Clear();
                this.Hide();
            }
        }

        private void PrevewTextHandler(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "\r")
            {
                this.CardNo.Password += e.Text;
            }
        }

        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // 注意，这里是隐藏——CATL的上位机规范里是默认就是操作员登录。
            // 如果以后改成需要先登录，就不能简单隐藏！
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}

