using Itminus.FSharpExtensions;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using VisDummy.VMs.ViewModels;

namespace VisDummy.VMs.Views
{
    /// <summary>
    /// Interaction logic for VisRtView.xaml
    /// </summary>
    public partial class VisRtView : UserControl, IViewFor<VisRtViewModel>
    {
        public VisRtView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(this.ViewModel, vm => vm.CmdExecCurrProc, v => v.btnExecProc).DisposeWith(d);

                this.Bind(this.ViewModel, vm => vm.Visibility, v => v.ele.Visibility).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.CurrentProcedureName, v => v.lblProc.Content).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Function_Number, v => v.Function_Number.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Position_Number, v => v.Position_Number.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Batch, v => v.Batch.Text).DisposeWith(d);

                if (this.ViewModel != null)
                {
                    this.ViewModel.OnProcedureSelected = selection =>
                    {
                        this.vmRenderCtrl.ModuleSource = selection;
                    };
                }

                if (this.ViewModel.CurrentProc == null)
                {
                    this.ViewModel.CmdSelectProcedure
                        .Execute()
                        .Subscribe(
                            e =>
                            {
                            },
                            err =>
                            {
                                //MessageBox.Show(err.Message);
                            }
                        );
                }
            });
        }

        #region View Model
        public VisRtViewModel ViewModel
        {
            get { return (VisRtViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (VisRtViewModel)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(VisRtViewModel), typeof(VisRtView), new PropertyMetadata(null));
        #endregion

    }
}
