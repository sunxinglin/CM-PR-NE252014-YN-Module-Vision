using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace VisDummy.MtMes.MtMes
{
    /// <summary>
    /// Interaction logic for MtMesCtrl.xaml
    /// </summary>
    public partial class MtMesCtrl : UserControl, IViewFor<MtMesCtrlViewModel>
    {
        public MtMesCtrl()
        {
            InitializeComponent();

            this.WhenActivated(d => {

                // C MES 调试
                this.Bind(this.ViewModel, vm => vm.PackCode_CMes, v => v.txtPackCode_CatlMes.Text).DisposeWith(d);

                this.Bind(this.ViewModel, vm => vm.CatlMesResponse, v => v.tboxCatlResponse.Text).DisposeWith(d);

                this.BindCommand(this.ViewModel, vm => vm.CmdDataCollectForResourceFAI, v => v.btnCatlMes_Invoke_CmdDataCollectForResourceFAI).DisposeWith(d);
                this.ViewModel?.CmdDataCollectForResourceFAI.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message)).DisposeWith(d);
            });
        }

        #region

        public MtMesCtrlViewModel ViewModel
        {
            get { return (MtMesCtrlViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object? IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (MtMesCtrlViewModel)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MtMesCtrlViewModel), typeof(MtMesCtrl), new PropertyMetadata(null));


        #endregion
    }
}
