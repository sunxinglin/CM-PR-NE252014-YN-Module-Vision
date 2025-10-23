using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using StdUnit.One.Shared;

namespace TApp.ViewModels.Realtime
{
    public class UILogsViewModel : ReactiveObject, IDisposable
    {
        private IDisposable _cleanup;
        private IDisposable _cleanup2;
        private IDisposable _cleanup3;
        public UILogsViewModel()
        {
            this.CmdClearFilter = ReactiveCommand.Create(() => { this.EventGroup = ""; });
            var disposeCmdClearFilterException = this.CmdClearFilter.ThrownExceptions.Subscribe(x => { });
            this.CmdClear = ReactiveCommand.Create(() => { this._source.Clear(); });
            var disposeCmdClear = this.CmdClear.ThrownExceptions.Subscribe(x => { });
            var eventgroupFilter = this.WhenAnyValue(x => x.EventGroup).Throttle(TimeSpan.FromMilliseconds(400)).DistinctUntilChanged().Select(x =>
            {
                Func<LogMessage, bool> res = lm =>
                {
                    if (string.IsNullOrEmpty(x)) return true;
                    return lm.EventGroup == x;
                };
                return res;
            });
            this.ChangeObs = this._source.Connect().Filter(eventgroupFilter);
            var d = this.ChangeObs.ObserveOn(RxApp.MainThreadScheduler).Bind(out _logs).DisposeMany().Subscribe();
            this._cleanup = new CompositeDisposable(d, disposeCmdClearFilterException, disposeCmdClear);

            this.CmdClearFilter2 = ReactiveCommand.Create(() => { this.EventGroup2 = ""; });
            var disposeCmdClearFilterException2 = this.CmdClearFilter2.ThrownExceptions.Subscribe(x => { });
            this.CmdClear2 = ReactiveCommand.Create(() => { this._source2.Clear(); });
            var disposeCmdClear2 = this.CmdClear2.ThrownExceptions.Subscribe(x => { });
            var eventgroupFilter2 = this.WhenAnyValue(x => x.EventGroup2).Throttle(TimeSpan.FromMilliseconds(400)).DistinctUntilChanged().Select(x =>
            {
                Func<LogMessage, bool> res = lm =>
                {
                    if (string.IsNullOrEmpty(x)) return true;
                    return lm.EventGroup == x;
                };
                return res;
            });
            this.ChangeObs2 = this._source2.Connect().Filter(eventgroupFilter2);
            var d2 = this.ChangeObs2.ObserveOn(RxApp.MainThreadScheduler).Bind(out _logs2).DisposeMany().Subscribe();
            this._cleanup2 = new CompositeDisposable(d2, disposeCmdClearFilterException2, disposeCmdClear2);

            this.CmdClearFilter3 = ReactiveCommand.Create(() => { this.EventGroup3 = ""; });
            var disposeCmdClearFilterException3 = this.CmdClearFilter3.ThrownExceptions.Subscribe(x => { });
            this.CmdClear3 = ReactiveCommand.Create(() => { this._source3.Clear(); });
            var disposeCmdClear3 = this.CmdClear3.ThrownExceptions.Subscribe(x => { });
            var eventgroupFilter3 = this.WhenAnyValue(x => x.EventGroup3).Throttle(TimeSpan.FromMilliseconds(400)).DistinctUntilChanged().Select(x =>
            {
                Func<LogMessage, bool> res = lm =>
                {
                    if (string.IsNullOrEmpty(x)) return true;
                    return lm.EventGroup == x;
                };
                return res;
            });
            this.ChangeObs3 = this._source3.Connect().Filter(eventgroupFilter3);
            var d3 = this.ChangeObs3.ObserveOn(RxApp.MainThreadScheduler).Bind(out _logs3).DisposeMany().Subscribe();
            this._cleanup3 = new CompositeDisposable(d3, disposeCmdClearFilterException3, disposeCmdClear3);


        }

        private SourceList<LogMessage> _source = new();
        private SourceList<LogMessage> _source2 = new();
        private SourceList<LogMessage> _source3 = new();

        [Reactive]
        public bool ScrollEnabled { get; set; } = true;

        [Reactive]
        public bool ScrollEnabled2 { get; set; } = true;

        [Reactive]
        public bool ScrollEnabled3 { get; set; } = true;


        #region
        private readonly ReadOnlyObservableCollection<LogMessage> _logs;
        public ReadOnlyObservableCollection<LogMessage> Logs => _logs;
        public IObservable<IChangeSet<LogMessage>> ChangeObs { get; }

        private readonly ReadOnlyObservableCollection<LogMessage> _logs2;
        public ReadOnlyObservableCollection<LogMessage> Logs2 => _logs2;
        public IObservable<IChangeSet<LogMessage>> ChangeObs2 { get; }

        private readonly ReadOnlyObservableCollection<LogMessage> _logs3;
        public ReadOnlyObservableCollection<LogMessage> Logs3 => _logs3;
        public IObservable<IChangeSet<LogMessage>> ChangeObs3 { get; }
        #endregion

        #region
        [Reactive]
        public string EventGroup { get; set; }
        [Reactive]
        public string EventGroup2 { get; set; }
        [Reactive]
        public string EventGroup3 { get; set; }
        #endregion

        public void OnNext(LogMessage msg)
        {
            while (this._source.Count > 1000)
            {
                this._source.RemoveAt(0);
            }
            while (this._source2.Count > 1000)
            {
                this._source.RemoveAt(0);
            }
            while (this._source3.Count > 1000)
            {
                this._source.RemoveAt(0);
            }

            switch (msg.Level)
            {
                case LogLevel.Information:
                    this._source.Add(msg);
                    break;
                case LogLevel.Warning:
                    this._source2.Add(msg);
                    break;
                case LogLevel.Error:
                    this._source3.Add(msg);
                    break;
                default:
                    break;
            }
        }

        public ReactiveCommand<Unit, Unit> CmdClear { get; }
        public ReactiveCommand<Unit, Unit> CmdClearFilter { get; }

        public ReactiveCommand<Unit, Unit> CmdClear2 { get; }
        public ReactiveCommand<Unit, Unit> CmdClearFilter2 { get; }

        public ReactiveCommand<Unit, Unit> CmdClear3 { get; }
        public ReactiveCommand<Unit, Unit> CmdClearFilter3 { get; }

        public void Dispose()
        {
            this._cleanup.Dispose();
        }
        public void Dispose2()
        {
            this._cleanup2.Dispose();
        }

        public void Dispose3()
        {
            this._cleanup3.Dispose();
        }

    }
}
