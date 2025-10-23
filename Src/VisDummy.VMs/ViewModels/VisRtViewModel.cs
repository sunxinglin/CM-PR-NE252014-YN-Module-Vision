using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Itminus.FSharpExtensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VisDummy.Shared.Utils;
using VM.Core;
using VM.PlatformSDKCS;

namespace VisDummy.VMs.ViewModels
{

    public delegate void OnProcedureSelected(VmProcedure proc);

    public class VisRtViewModel : ReactiveObject, IVisionMarker
    {
        public VisRtViewModel(string viewName, string procName, bool visibility, VMSlnViewModel slnVM)
        {
            this.ViewName = viewName;
            this.ProcName = procName;
            this.Visibility = visibility;
            this.SlnVM = slnVM;
            this.CmdExecCurrProc = ReactiveCommand.Create(ExecuteCurrentProcImpl);
            this.CmdExecCurrProc.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message));

            this.CmdSelectProcedure = ReactiveCommand.CreateFromTask(SelectProcedureImplAsync);
            this.CmdSelectProcedure.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message));

            this.WhenAnyValue(x => x.CurrentProc)
                .Select(c => c?.Name)
                .ToPropertyEx(this, x => x.CurrentProcedureName, scheduler: RxApp.MainThreadScheduler);
        }
        public VMSlnViewModel SlnVM { get; }

        public string ViewName { get; }
        public string ProcName { get; }

        [Reactive]
        public bool Visibility { get; set; }

        public OnProcedureSelected OnProcedureSelected { get; set; }

        [Reactive]
        public ushort Function_Number { get; set; } = 0;
        [Reactive]
        public ushort Position_Number { get; set; } = 0;
        [Reactive]
        public ushort Batch { get; set; } = 0;

        #region 选择流程
        public async Task<VmProcedure> SelectProcedureImplAsync()
        {
            var procedures = this.SlnVM.Procedures;
            if (procedures == null || procedures.Count == 0)
            {
                await this.SlnVM.CmdRefreshAllProcedures.Execute();
            }
            procedures = this.SlnVM.Procedures;
            var selection = SelectProcedure(procedures, this.ProcName);
            this.CurrentProc = selection;
            this.OnProcedureSelected?.Invoke(selection);
            return selection;

            VmProcedure SelectProcedure(IList<VmProcedure> procs, string rtName)
            {
                return procs.FirstOrDefault(proc => proc.Name == rtName);
            }
        }
        public ReactiveCommand<Unit, VmProcedure> CmdSelectProcedure { get; }
        #endregion

        #region 用户选择的当前流程

        [Reactive]
        public VmProcedure CurrentProc { get; set; }

        [ObservableAsProperty]
        public string CurrentProcedureName { get; }

        /// <summary>
        /// 运行当前流程
        /// </summary>
        public ReactiveCommand<Unit, Unit> CmdExecCurrProc { get; }

        private Unit ExecuteCurrentProcImpl()
        {
            this.CurrentProc?.ModuParams.SetInputInt("功能号", [Function_Number]);
            this.CurrentProc?.ModuParams.SetInputInt("拍照位置", [Position_Number]);
            this.CurrentProc?.ModuParams.SetInputString("批次号", [new InputStringData { strValue = Batch.ToString() }]);
            this.CurrentProc?.Run();
            return Unit.Default;
        }
        #endregion
    }
}
