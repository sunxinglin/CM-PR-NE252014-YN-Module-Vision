using Itminus.FSharpExtensions;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using VisDummy.MKVMs.ViewModels;

namespace VisDummy.MKVMs.Views
{
    /// <summary>
    /// Interaction logic for Vis3DRtView.xaml
    /// </summary>
    public partial class Vis3DRtView : UserControl, IViewFor<Vis3DRtViewModel>
    {
        public Vis3DRtView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(this.ViewModel, vm => vm.CmdTriggerProc, v => v.btnTrigger).DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.CmdSpot, v => v.btnSpot).DisposeWith(d);

                this.Bind(this.ViewModel, vm => vm.Visibility, v => v.ele.Visibility).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.ProcName, v => v.lblProc.Content).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.Function_Number, v => v.Function_Number.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.SpotFunction_Number, v => v.SpotFunction_Number.Text).DisposeWith(d);
                this.Bind(this.ViewModel, vm => vm.SpotPosition_Number, v => v.SpotPosition_Number.Text).DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.Records, v => v.records.ItemsSource).DisposeWith(d);

                this.ViewModel?.ChangeObs
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(lm =>
                    {
                        this.scroll.ScrollToEnd();
                    })
                    .DisposeWith(d);
            });
        }

        #region View Model
        public Vis3DRtViewModel ViewModel
        {
            get { return (Vis3DRtViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (Vis3DRtViewModel)value; }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(Vis3DRtViewModel), typeof(Vis3DRtView), new PropertyMetadata(null));
        #endregion

    }
}
