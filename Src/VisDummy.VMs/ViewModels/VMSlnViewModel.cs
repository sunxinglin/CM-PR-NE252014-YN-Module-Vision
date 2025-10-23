using ReactiveUI;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using VM.Core;
using System.Reactive;
using System.Windows;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Linq;
using VMControls.Interface;
using System.Threading.Tasks;

namespace VisDummy.VMs.ViewModels
{
    public class VMSlnViewModel : ReactiveObject
    {
        public VMSlnViewModel()
        {
            this.CmdRefreshAllProcedures = ReactiveCommand.CreateFromTask(RefreshAllProcedureesImplAsync);
            this.CmdRefreshAllProcedures.ThrownExceptions.Subscribe(t => MessageBox.Show(t.Message));

            this.WhenAnyValue(x => x.Procedures)
                .Select(list => list?.Where(p => p != null).Select(p => p.Name).ToList() ?? new List<string> { })
                .ToPropertyEx(this, x => x.ProcedureNames, scheduler: RxApp.MainThreadScheduler);
        }
        #region 各流程

        [ObservableAsProperty]
        public IList<string> ProcedureNames { get; }

        [Reactive]
        public IList<VmProcedure> Procedures { get; set; } 
        #endregion

        #region 刷新获取所有流程
        public ReactiveCommand<Unit, Unit> CmdRefreshAllProcedures { get; }

        private async Task RefreshAllProcedureesImplAsync()
        {
            List<VmProcedure> procedureList = new List<VmProcedure>();
            IVmSolution sln = VmSolution.Instance;
            while(sln.IsLoading)
            { 
                await Task.Delay(200);
            }
            VmSolution.Instance.GetAllProcedureObjects(ref procedureList);
            this.Procedures = procedureList;
        }
        #endregion

    }
}
