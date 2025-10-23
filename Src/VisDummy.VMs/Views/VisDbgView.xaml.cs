using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
using VM.Core;

namespace VisDummy.VMs.Views
{
    /// <summary>
    /// Interaction logic for VisDbgView.xaml
    /// </summary>
    public partial class VisDbgView : UserControl
    {
        public VisDbgView()
        {
            InitializeComponent();
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "sol files (*.sol)|*.sol|All files (*.*)|*.*";
            var r = dialog.ShowDialog();
            if (r != true)
            {
                return;
            }

            var filename = dialog.FileName;
            try
            {
                VmSolution.Load(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".sol";
            dialog.Filter = "sol files (*.sol)|*.sol|All files (*.*)|*.*";
            var r = dialog.ShowDialog();
            if (r != true)
            {
                return;
            }
            var filename = dialog.FileName;
            try
            {
                VmSolution.SaveAs(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
