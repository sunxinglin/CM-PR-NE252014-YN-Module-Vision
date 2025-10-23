using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisDummy.VMs.ViewModels;
using VM.Core;

namespace VisDummy.VMs.Views
{
    /// <summary>
    /// Interaction logic for VisCalibrationView.xaml
    /// </summary>
    public partial class ManulVisCalibrationView : UserControl, IViewFor<ManualVisCalibrationViewModel>
    {
        public ManulVisCalibrationView()
        {
            InitializeComponent();

            this.WhenActivated(d => {
                this.OneWayBind(this.ViewModel, vm => vm.SlnVM.Procedures, v => v.cboxProcedures.ItemsSource).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.CurrentProc, v => v.cboxProcedures.SelectedItem).DisposeWith(d);

                this.Bind(this.ViewModel, vm => vm.StepA, v => v.stepAngle.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.StepX, v => v.stepX.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.StepY, v => v.stepY.Text).DisposeWith(d);

                this.Bind(this.ViewModel, vm => vm.WldA, v => v.wldAngle.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.WldX, v => v.wldX.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.WldY, v => v.wldY.Text).DisposeWith(d);

                #region
                this.OneWayBind(this.ViewModel, vm => vm.CalibrationRecords, v => v.records.ItemsSource).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.AllowEditRecords, v => v.records.IsReadOnly).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.AllowEditRecords, v => v.btnToggleEdit.IsChecked).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdClearCalibrationPoints, v => v.btnClearRecords).DisposeWith(d);
                #endregion

                this.BindCommand(this.ViewModel, vm => vm.AddStepA, v => v.addStepA).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.AddStepX, v => v.addStepX).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.AddStepY, v => v.addStepY).DisposeWith(d);

                this.BindCommand(this.ViewModel, vm => vm.MinusStepA, v => v.minusStepA).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.MinusStepX, v => v.minusStepX).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.MinusStepY, v => v.minusStepY).DisposeWith(d);


                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibT1).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibT2).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibT3).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibT4).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibT5).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibT6).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibT7).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibT8).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibT9).DisposeWith(d);

                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibR10).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibR11).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibR12).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibR13).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CalibrateNthPoint, v => v.calibR14).DisposeWith(d);

                this.ViewModel.CalibrateNthPoint.ThrownExceptions.Subscribe(ex => MessageBox.Show(ex.Message));

                if (this.ViewModel != null)
                {
                    this.ViewModel.OnProcedureSelected = selection => {
                        this.vmRenderCtrl.ModuleSource = selection;
                    };
                }
            });
        }

        #region



        public ManualVisCalibrationViewModel ViewModel
        {
            get { return (ManualVisCalibrationViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel =(ManualVisCalibrationViewModel) value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ManualVisCalibrationViewModel), typeof(ManulVisCalibrationView), new PropertyMetadata(null));

        #endregion

        private void wldCoord_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var txtBox = sender as TextBox;
            var index = txtBox.CaretIndex;
            var txt = txtBox.Text.Insert(index, e.Text);
            var isvalid = float.TryParse(txt, out var _);
            e.Handled = !isvalid; 
        }
    }
}
