using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using VisDummy.MtMes.CatlMes.ViewModel;

namespace VisDummy.MtMes.CatlMes
{
    /// <summary>
    /// Interaction logic for CatlMesEditor.xaml
    /// </summary>
    public partial class CatlMesEditor : UserControl, IViewFor<CatlMesSettingEditVM>
    {
        public CatlMesEditor()
        {
            InitializeComponent();
            this.WhenActivated(d => {
                this.Bind(this.ViewModel, vm => vm.DataCollectForResourceFAIVM, v => v.viewDataCollectForResourceFAICtrl.ViewModel).DisposeWith(d);
            });
        }

        #region

        public CatlMesSettingEditVM ViewModel
        {
            get { return (CatlMesSettingEditVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object? IViewFor.ViewModel { get => this.ViewModel; set => this.ViewModel = (CatlMesSettingEditVM) value; }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CatlMesSettingEditVM), typeof(CatlMesEditor), new PropertyMetadata(null));


        #endregion
    }
}
