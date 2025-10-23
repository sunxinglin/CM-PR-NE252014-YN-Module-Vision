using DynamicData;
using Itminus.FSharpExtensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using VisDummy.MKVMs.Common;
using VisDummy.MKVMs.Messages;
using VisDummy.MKVMs.MKServices;
using VisDummy.Shared.Utils;

namespace VisDummy.MKVMs.ViewModels
{
    public class Vis3DRtViewModel : ReactiveObject, IDisposable, IVisionMarker
    {
        private IDisposable _cleanup;
        public Vis3DRtViewModel(string viewName, string procName, Vision3DCtrl vision3DCtrl, bool visibility)
        {
            ViewName = viewName;
            ProcName = procName;
            Vision3DCtrl = vision3DCtrl;
            Visibility = visibility;
            CmdTriggerProc = ReactiveCommand.CreateFromTask(ExecuteTrigger);
            CmdTriggerProc.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message));

            CmdSpot = ReactiveCommand.CreateFromTask(ExecuteSpot);
            CmdSpot.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message));

            ChangeObs = _source.Connect();
            var d = ChangeObs
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _records)
                .DisposeMany()
                .Subscribe();

            _cleanup = new CompositeDisposable(
                d
            );
        }

        public Vision3DCtrl Vision3DCtrl { get; }
        public string ViewName { get; }
        public string ProcName { get; }

        [Reactive]
        public bool Visibility { get; set; } = true;

        [Reactive]
        public ushort Function_Number { get; set; }
        [Reactive]
        public ushort SpotFunction_Number { get; set; }
        [Reactive]
        public ushort SpotPosition_Number { get; set; }

        public ReactiveCommand<Unit, Unit> CmdTriggerProc { get; }
        public ReactiveCommand<Unit, Unit> CmdSpot { get; }
        private async Task ExecuteTrigger()
        {
            try
            {
                var r = from r1 in Vision3DCtrl.ReadCmdAsync(Vision3DCtrl.Cmd_103(Function_Number, 1))
                        from r2 in r1.Analyze103().ToTask()
                        from r3 in Vision3DCtrl.ReadCmdAsync(Vision3DCtrl.Cmd_101(Function_Number))
                        from r4 in r3.Analyze101().ToTask()
                        from r5 in Vision3DCtrl.ReadCmdAsync(Vision3DCtrl.Cmd_110(Function_Number))
                        from r6 in r5.Analyze110().ToTask()
                        select r6;
                var s = await r;
                if (s.IsError)
                    OnNext(new Vision3DMessage { Way = Way.Error, Content = s.ErrorValue });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task ExecuteSpot()
        {
            try
            {
                if (SpotPosition_Number != 15)
                {
                    var r = from r1 in Vision3DCtrl.ReadCmdAsync(Vision3DCtrl.Cmd_103(SpotFunction_Number, SpotPosition_Number))
                            from r2 in r1.Analyze103().ToTask()
                            from r3 in Vision3DCtrl.ReadCmdAsync(Vision3DCtrl.Cmd_101(SpotFunction_Number))
                            from r4 in r3.Analyze101().ToTask()
                            from r5 in Vision3DCtrl.ReadCmdAsync(Vision3DCtrl.Cmd_102(SpotFunction_Number))
                            from r6 in r5.Analyze102().ToTask()
                            select r6;
                    var s = await r;
                    if (s.IsError)
                        OnNext(new Vision3DMessage { Way = Way.Error, Content = s.ErrorValue });
                }
                else
                {
                    var r = from r1 in Vision3DCtrl.ReadCmdAsync(Vision3DCtrl.Cmd_101(SpotFunction_Number))
                            from r2 in r1.Analyze101().ToTask()
                            from r3 in Vision3DCtrl.ReadCmdAsync(Vision3DCtrl.Cmd_102(SpotFunction_Number))
                            from r4 in r3.Analyze102().ToTask()
                            select r4;
                    var s = await r;
                    if (s.IsError)
                        OnNext(new Vision3DMessage { Way = Way.Error, Content = s.ErrorValue });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 交互框
        private SourceList<Vision3DMessage> _source = new SourceList<Vision3DMessage>();

        private readonly ReadOnlyObservableCollection<Vision3DMessage> _records;
        public ReadOnlyObservableCollection<Vision3DMessage> Records => _records;
        public IObservable<IChangeSet<Vision3DMessage>> ChangeObs { get; }
        public void OnNext(Vision3DMessage msg)
        {
            while (_source.Count > 100)
            {
                _source.RemoveAt(0);
            }
            _source.Add(msg);
        }
        public void Dispose()
        {
            _cleanup.Dispose();
        }
        #endregion
    }
}
