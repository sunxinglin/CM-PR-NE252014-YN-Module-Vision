using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.人工位;
using VisDummy.Protocols.人工位.Model;

namespace VisDummy.WPF.ViewModels.Monitor
{
    public class 人工位MonitorViewModel : ReactiveObject
    {
        public 人工位MonitorViewModel()
        {
            ScanContextSubject.Select(c => c.DevMsg.Heart).ToPropertyEx(this, x => x.Dev_CmdHeart, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.Heart).ToPropertyEx(this, x => x.Mst_CmdHeart, scheduler: RxApp.MainThreadScheduler);

            ScanContextSubject.Select(c => c.DevMsg.水冷板检测).ToPropertyEx(this, x => x.DevMsg_水冷板检测, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.水冷板检测).ToPropertyEx(this, x => x.MstMsg_水冷板检测, scheduler: RxApp.MainThreadScheduler);
        }

        public Subject<ScanContext> ScanContextSubject { get; } = new Subject<ScanContext>();

        [ObservableAsProperty]
        public Dev_CmdHeart Dev_CmdHeart { get; }
        [ObservableAsProperty]
        public Mst_CmdHeart Mst_CmdHeart { get; }

        [ObservableAsProperty]
        public DevMsg_水冷板检测 DevMsg_水冷板检测 { get; }
        [ObservableAsProperty]
        public MstMsg_水冷板检测 MstMsg_水冷板检测 { get; }

    }
}
