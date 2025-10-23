using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.水冷板抓取;
using VisDummy.Protocols.水冷板抓取.Model;

namespace VisDummy.WPF.ViewModels.Monitor
{
    public class 水冷板抓取MonitorViewModel : ReactiveObject
    {
        public 水冷板抓取MonitorViewModel()
        {
            ScanContextSubject.Select(c => c.DevMsg.Heart).ToPropertyEx(this, x => x.Dev_CmdHeart, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.Heart).ToPropertyEx(this, x => x.Mst_CmdHeart, scheduler: RxApp.MainThreadScheduler);

            ScanContextSubject.Select(c => c.DevMsg.机器人1定位引导).ToPropertyEx(this, x => x.DevMsg_机器人1定位引导, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.机器人1定位引导).ToPropertyEx(this, x => x.MstMsg_机器人1定位引导, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.DevMsg.机器人2定位引导).ToPropertyEx(this, x => x.DevMsg_机器人2定位引导, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.机器人2定位引导).ToPropertyEx(this, x => x.MstMsg_机器人2定位引导, scheduler: RxApp.MainThreadScheduler);

            ScanContextSubject.Select(c => c.DevMsg.机器人1校验).ToPropertyEx(this, x => x.DevMsg_机器人1校验, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.机器人1校验).ToPropertyEx(this, x => x.MstMsg_机器人1校验, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.DevMsg.机器人2校验).ToPropertyEx(this, x => x.DevMsg_机器人2校验, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.机器人2校验).ToPropertyEx(this, x => x.MstMsg_机器人2校验, scheduler: RxApp.MainThreadScheduler);

            ScanContextSubject.Select(c => c.DevMsg.机器人1自动校准).ToPropertyEx(this, x => x.DevMsg_机器人1自动校准, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.机器人1自动校准).ToPropertyEx(this, x => x.MstMsg_机器人1自动校准, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.DevMsg.机器人2自动校准).ToPropertyEx(this, x => x.DevMsg_机器人2自动校准, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.机器人2自动校准).ToPropertyEx(this, x => x.MstMsg_机器人2自动校准, scheduler: RxApp.MainThreadScheduler);
        }

        public Subject<ScanContext> ScanContextSubject { get; } = new Subject<ScanContext>();

        [ObservableAsProperty]
        public Dev_CmdHeart Dev_CmdHeart { get; }
        [ObservableAsProperty]
        public Mst_CmdHeart Mst_CmdHeart { get; }

        [ObservableAsProperty]
        public DevMsg_定位引导 DevMsg_机器人1定位引导 { get; }
        [ObservableAsProperty]
        public MstMsg_定位引导 MstMsg_机器人1定位引导 { get; }
        [ObservableAsProperty]
        public DevMsg_定位引导 DevMsg_机器人2定位引导 { get; }
        [ObservableAsProperty]
        public MstMsg_定位引导 MstMsg_机器人2定位引导 { get; }

        [ObservableAsProperty]
        public DevMsg_校验 DevMsg_机器人1校验 { get; }
        [ObservableAsProperty]
        public MstMsg_校验 MstMsg_机器人1校验 { get; }
        [ObservableAsProperty]
        public DevMsg_校验 DevMsg_机器人2校验 { get; }
        [ObservableAsProperty]
        public MstMsg_校验 MstMsg_机器人2校验 { get; }

        [ObservableAsProperty]
        public DevMsg_自动校准 DevMsg_机器人1自动校准 { get; }
        [ObservableAsProperty]
        public MstMsg_自动校准 MstMsg_机器人1自动校准 { get; }
        [ObservableAsProperty]
        public DevMsg_自动校准 DevMsg_机器人2自动校准 { get; }
        [ObservableAsProperty]
        public MstMsg_自动校准 MstMsg_机器人2自动校准 { get; }
    }
}
