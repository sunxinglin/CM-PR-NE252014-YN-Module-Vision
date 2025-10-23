using Itminus.Protocols.Loading;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.Loading.Model;

namespace VisDummy.WPF.ViewModels.Monitor
{
    public class LoadingMonitorViewModel : ReactiveObject
    {
        public LoadingMonitorViewModel()
        {
            ScanContextSubject.Select(c => c.DevMsg.Heart).ToPropertyEx(this, x => x.Dev_CmdHeart, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.Heart).ToPropertyEx(this, x => x.Mst_CmdHeart, scheduler: RxApp.MainThreadScheduler);

            ScanContextSubject.Select(c => c.DevMsg.Station3D).ToPropertyEx(this, x => x.DevMsg_3DStation, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.Station3D).ToPropertyEx(this, x => x.MstMsg_3DStation, scheduler: RxApp.MainThreadScheduler);

            ScanContextSubject.Select(c => c.DevMsg.Station3DSpot).ToPropertyEx(this, x => x.DevMsg_3DSpotStation, scheduler: RxApp.MainThreadScheduler);
            ScanContextSubject.Select(c => c.MstMsg.Station3DSpot).ToPropertyEx(this, x => x.MstMsg_3DSpotStation, scheduler: RxApp.MainThreadScheduler);

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
        public DevMsg_3DStation DevMsg_3DStation { get; }

        [ObservableAsProperty]
        public MstMsg_3DStation MstMsg_3DStation { get; }

        [ObservableAsProperty]
        public DevMsg_3DSpotStation DevMsg_3DSpotStation { get; }

        [ObservableAsProperty]
        public MstMsg_3DSpotStation MstMsg_3DSpotStation { get; }

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
