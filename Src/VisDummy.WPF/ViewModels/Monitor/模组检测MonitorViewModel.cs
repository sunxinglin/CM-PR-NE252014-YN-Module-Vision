using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.模组检测;
using VisDummy.Protocols.模组检测.Model;

namespace VisDummy.WPF.ViewModels.Monitor
{
    public class 模组检测MonitorViewModel : ReactiveObject
    {
        public 模组检测MonitorViewModel()
        {
            ScanContextSubject.Select(c => c.DevMsg.Heart).ToPropertyEx(this, x => x.Dev_CmdHeart, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.Heart).ToPropertyEx(this, x => x.Mst_CmdHeart, scheduler: RxApp.MainThreadScheduler);

            ScanContextSubject.Select(c => c.DevMsg.Station2D).ToPropertyEx(this, x => x.DevMsg_2DStation, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.Station2D).ToPropertyEx(this, x => x.MstMsg_2DStation, scheduler: RxApp.MainThreadScheduler);

            ScanContextSubject.Select(c => c.DevMsg.Station2DSpot).ToPropertyEx(this, x => x.DevMsg_2DSpotStation, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.Station2DSpot).ToPropertyEx(this, x => x.MstMsg_2DSpotStation, scheduler: RxApp.MainThreadScheduler);
        }

        public Subject<ScanContext> ScanContextSubject { get; } = new Subject<ScanContext>();

        [ObservableAsProperty]
        public Dev_CmdHeart Dev_CmdHeart { get; }

        [ObservableAsProperty]
        public Mst_CmdHeart Mst_CmdHeart { get; }

        [ObservableAsProperty]
        public DevMsg_2DStation DevMsg_2DStation { get; }

        [ObservableAsProperty]
        public MstMsg_2DStation MstMsg_2DStation { get; }

        [ObservableAsProperty]
        public DevMsg_2DSpotStation DevMsg_2DSpotStation { get; }

        [ObservableAsProperty]
        public MstMsg_2DSpotStation MstMsg_2DSpotStation { get; }
    }
}
