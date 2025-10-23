using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.侧板自动拧紧;
using VisDummy.Protocols.侧板自动拧紧.Model;

namespace VisDummy.WPF.ViewModels.Monitor
{
	public class 侧板自动拧紧MonitorViewModel: ReactiveObject
	{
		public 侧板自动拧紧MonitorViewModel() {
			ScanContextSubject.Select(c => c.DevMsg.Heart).ToPropertyEx(this, x => x.Dev_CmdHeart, scheduler: RxApp.MainThreadScheduler);
			ScanContextSubject.Select(c => c.MstMsg.Heart).ToPropertyEx(this, x => x.Mst_CmdHeart, scheduler: RxApp.MainThreadScheduler);

			ScanContextSubject.Select(c => c.DevMsg.CAM1).ToPropertyEx(this, x => x.DevMsg_CAM1, scheduler: RxApp.MainThreadScheduler);
			ScanContextSubject.Select(c => c.DevMsg.CAM2).ToPropertyEx(this, x => x.DevMsg_CAM2, scheduler: RxApp.MainThreadScheduler);
			ScanContextSubject.Select(c => c.DevMsg.CAM3).ToPropertyEx(this, x => x.DevMsg_CAM3, scheduler: RxApp.MainThreadScheduler);


			ScanContextSubject.Select(c => c.MstMsg.CAM1).ToPropertyEx(this, x => x.MstMsg_CAM1, scheduler: RxApp.MainThreadScheduler);
			ScanContextSubject.Select(c => c.MstMsg.CAM2).ToPropertyEx(this, x => x.MstMsg_CAM2, scheduler: RxApp.MainThreadScheduler);
			ScanContextSubject.Select(c => c.MstMsg.CAM3).ToPropertyEx(this, x => x.MstMsg_CAM3, scheduler: RxApp.MainThreadScheduler);
		}

		public Subject<ScanContext> ScanContextSubject { get; } = new Subject<ScanContext>();

		[ObservableAsProperty]
		public Dev_SideHeart Dev_CmdHeart { get; }
		[ObservableAsProperty]
		public Mst_SideHeart Mst_CmdHeart { get; }

		[ObservableAsProperty]
		public DevMsg_CAM1 DevMsg_CAM1 { get; }

		[ObservableAsProperty]
		public DevMsg_CAM2 DevMsg_CAM2 { get; }

		[ObservableAsProperty]
		public DevMsg_CAM3 DevMsg_CAM3 { get; }

		[ObservableAsProperty]
		public MstMsg_CAM1 MstMsg_CAM1 { get; }

		[ObservableAsProperty]
		public MstMsg_CAM2 MstMsg_CAM2 { get; }

		[ObservableAsProperty]
		public MstMsg_CAM3 MstMsg_CAM3 { get; }

	}
}
