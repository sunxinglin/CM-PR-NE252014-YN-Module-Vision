using GlobalCameraModuleCs;
using Itminus.FSharpExtensions;
using Microsoft.FSharp.Core;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Calibrations;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.Warp;
using VisDummy.Abstractions.Warp.人工位;
using VisDummy.Abstractions.Warp.侧板自动拧紧;
using VisDummy.Abstractions.Warp.水冷板抓取;
using VisDummy.Lang.Resources;
using VisDummy.Shared.Utils;
using VisDummy.VMs.Common;
using VisDummy.VMs.ViewModels;
using VM.Core;
using VM.PlatformSDKCS;

namespace VisDummy.VMs.HIKServices
{

	public class VisProcImpl : IVisProc
	{
		#region 标定
		public async Task<FSharpResult<CalibarateNthPointOkWrap, IErr_CalibrateNthPoint>> CalibrateAsync(CalibrateNthPointArgs args)
		{
			VmProcedure proc = await LoadProcAndRenderInRtViewAsync();
			if (proc == null)
			{
				var err = new Err_ProcedureNG() { Msg = $"当前尚未选定标定流程！" };
				return err.ToErrResult<CalibarateNthPointOkWrap, IErr_CalibrateNthPoint>();
			}

			proc.ModuParams.SetInputInt("nth", new int[] { args.Nth });
			proc.ModuParams.SetInputFloat("物理坐标X", new float[] { args.WldX });
			proc.ModuParams.SetInputFloat("物理坐标Y", new float[] { args.WldY });
			proc.ModuParams.SetInputFloat("物理角度A", new float[] { args.WldA });

			proc.Run();

			var mres = proc.ModuResult;
			var outputs =
				from x in mres.GetFloat("图像坐标X")
				from y in mres.GetFloat("图像坐标Y")
				from a in mres.GetFloat("图像坐标A")
				from status in mres.GetInt("标定状态")
				select new CalibarateNthPointOkWrap { ImgA = a, ImgX = x, ImgY = y, 标定状态 = status != 0 };

			var res = outputs
				.SelectError(e => new Err_ProcedureNG() { Msg = e } as IErr_CalibrateNthPoint);
			return res;
		}

		protected virtual Task<VmProcedure> LoadProcAndRenderInRtViewAsync()
		{
			var vm = Locator.Current.GetService<ManualVisCalibrationViewModel>();
			var proc = vm.CurrentProc;
			return Task.FromResult(proc);
		}
		#endregion

		protected virtual async Task<VmProcedure> LoadProcAsync(string procName)
		{
			var rts = Locator.Current.GetServices<IVisionMarker>();

			var rt = rts.FirstOrDefault(i => i.ProcName == procName);
			if (rt is VisRtViewModel vm)
			{
				var proc = vm.CurrentProc;

				// 如果尚未加载，则等待加载完成
				if (proc == null)
				{
					var obs = Observable.Start(async () =>
					{
						var x = await vm.CmdSelectProcedure.Execute();
						proc = vm.CurrentProc;
					}, scheduler: RxApp.MainThreadScheduler);
					await obs;
				}
				return proc;
			}
			throw new System.Exception($"{Language.Msg_流程未注册或未注册成}{typeof(VisRtViewModel)}:{procName}");
		}

		public async Task<FSharpResult<StationOkWrap_Loading, StationErrWrap_Loading>> LoadingProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(ProcedureLoading2D_Defines.流程名);
			if (proc == null)
			{
				var micerr = new StationErrWrap_Loading() { ErrMsg = $"{Language.Msg_加载流程失败}:{ProcedureLoading2D_Defines.流程名}" };
				return micerr.ToErrResult<StationOkWrap_Loading, StationErrWrap_Loading>();
			}
			proc.ModuParams.SetInputInt(ProcedureLoading2D_Defines.Function, [args.Function]);
			proc.ModuParams.SetInputInt(ProcedureLoading2D_Defines.Position, [args.Position]);
			proc.ModuParams.SetInputString(ProcedureLoading2D_Defines.Batch, [new InputStringData { strValue = args.Batch.ToString() }]);

			proc.Run();

			var mres = proc.ModuResult;

			var s =
				 from ImagePath in mres.GetString(ProcedureLoading2D_Defines.ImagePath)
				 from Direction in mres.GetInt(ProcedureLoading2D_Defines.IncomingDirection)
				 from BarCode in mres.GetString(ProcedureLoading2D_Defines.BarCode)
				 from ErrorCode in mres.GetInt(ProcedureLoading2D_Defines.ErrorCode)
				 select new { ImagePath, Direction, BarCode, ErrorCode };

			if (s.IsError)
			{
				var micerr = new StationErrWrap_Loading() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<StationOkWrap_Loading, StationErrWrap_Loading>();
			}
			var outputs = s.ResultValue;
			if (outputs.ErrorCode == 1)
			{
				var ok = new StationOkWrap_Loading
				{
					ImagePath = outputs.ImagePath,
					Direction = (ushort)outputs.Direction,
					BarCode = outputs.BarCode,
				};
				return ok.ToOkResult<StationOkWrap_Loading, StationErrWrap_Loading>();
			}
			var err = new StationErrWrap_Loading()
			{
				ImagePath = outputs.ImagePath,
				ErrorCode = (uint)outputs.ErrorCode,
			};
			return err.ToErrResult<StationOkWrap_Loading, StationErrWrap_Loading>();
		}

		public async Task<FSharpResult<SpotStationOkWarp, SpotStationErrWarp>> SpotProcAsync(SpotStationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure2DSpot_Defines.流程名);
			if (proc == null)
			{
				var micerr = new SpotStationErrWarp() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure2DSpot_Defines.流程名}" };
				return micerr.ToErrResult<SpotStationOkWarp, SpotStationErrWarp>();
			}
			proc.ModuParams.SetInputInt(Procedure2DSpot_Defines.Camera, [args.Camera]);
			proc.ModuParams.SetInputInt(Procedure2DSpot_Defines.Position, [args.Position]);

			proc.Run();

			var mres = proc.ModuResult;

			var s =
				 from ImagePath in mres.GetString(Procedure2DSpot_Defines.ImagePath)
				 from Feature in mres.GetFloat(Procedure2DSpot_Defines.Feature)
				 from Pixel in mres.GetFloat(Procedure2DSpot_Defines.Pixel)
				 from ErrorCode in mres.GetInt(Procedure2DSpot_Defines.ErrorCode)
				 select new { ImagePath, Feature, Pixel, ErrorCode };

			if (s.IsError)
			{
				var micerr = new SpotStationErrWarp() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<SpotStationOkWarp, SpotStationErrWarp>();
			}
			var outputs = s.ResultValue;
			if (outputs.ErrorCode == 1)
			{
				var ok = new SpotStationOkWarp
				{
					ImagePath = outputs.ImagePath,
					Features = outputs.Feature,
					Pixels = outputs.Pixel,
				};
				return ok.ToOkResult<SpotStationOkWarp, SpotStationErrWarp>();
			}
			var err = new SpotStationErrWarp()
			{
				ImagePath = outputs.ImagePath,
				ErrorCode = (uint)outputs.ErrorCode,
			};
			return err.ToErrResult<SpotStationOkWarp, SpotStationErrWarp>();
		}

		public async Task<FSharpResult<StationOkWrap_模组检测, StationErrWrap_模组检测>> 模组检测ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure模组检测2D_Defines.流程名);
			if (proc == null)
			{
				var micerr = new StationErrWrap_模组检测() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure模组检测2D_Defines.流程名}" };
				return micerr.ToErrResult<StationOkWrap_模组检测, StationErrWrap_模组检测>();
			}
			proc.ModuParams.SetInputInt(Procedure模组检测2D_Defines.Function, [args.Function]);
			proc.ModuParams.SetInputInt(Procedure模组检测2D_Defines.Position, [args.Position]);
			proc.ModuParams.SetInputString(Procedure模组检测2D_Defines.Batch, [new InputStringData { strValue = args.Batch.ToString() }]);

			proc.Run();

			var mres = proc.ModuResult;

			var s =
				 from ImagePath in mres.GetString(Procedure模组检测2D_Defines.ImagePath)
				 from Component in mres.GetInt(Procedure模组检测2D_Defines.Component)
				 from P1 in mres.GetInt(Procedure模组检测2D_Defines.Polarity1)
				 from P2 in mres.GetInt(Procedure模组检测2D_Defines.Polarity2)
				 from P3 in mres.GetInt(Procedure模组检测2D_Defines.Polarity3)
				 from P4 in mres.GetInt(Procedure模组检测2D_Defines.Polarity4)
				 from P5 in mres.GetInt(Procedure模组检测2D_Defines.Polarity5)
				 from P6 in mres.GetInt(Procedure模组检测2D_Defines.Polarity6)
				 from P7 in mres.GetInt(Procedure模组检测2D_Defines.Polarity7)
				 from P8 in mres.GetInt(Procedure模组检测2D_Defines.Polarity8)
				 from P9 in mres.GetInt(Procedure模组检测2D_Defines.Polarity9)
				 from P10 in mres.GetInt(Procedure模组检测2D_Defines.Polarity10)
				 from P11 in mres.GetInt(Procedure模组检测2D_Defines.Polarity11)
				 from P12 in mres.GetInt(Procedure模组检测2D_Defines.Polarity12)
				 from P13 in mres.GetInt(Procedure模组检测2D_Defines.Polarity13)
				 from P14 in mres.GetInt(Procedure模组检测2D_Defines.Polarity14)
				 from P15 in mres.GetInt(Procedure模组检测2D_Defines.Polarity15)
				 from P16 in mres.GetInt(Procedure模组检测2D_Defines.Polarity16)
				 from P17 in mres.GetInt(Procedure模组检测2D_Defines.Polarity17)
				 from P18 in mres.GetInt(Procedure模组检测2D_Defines.Polarity18)
				 from P19 in mres.GetInt(Procedure模组检测2D_Defines.Polarity19)
				 from P20 in mres.GetInt(Procedure模组检测2D_Defines.Polarity20)
				 let Polaritys = new int[20] { P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20 }
				 from ErrorCode in mres.GetInt(Procedure模组检测2D_Defines.ErrorCode)
				 select new { ImagePath, Component, Polaritys, ErrorCode };

			if (s.IsError)
			{
				var micerr = new StationErrWrap_模组检测() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<StationOkWrap_模组检测, StationErrWrap_模组检测>();
			}
			var outputs = s.ResultValue;
			if (outputs.ErrorCode == 1)
			{
				var ok = new StationOkWrap_模组检测
				{
					ImagePath = outputs.ImagePath,
					Component = (ushort)outputs.Component,
					Polaritys = outputs.Polaritys.Select(d => (ushort)d).ToArray(),
				};
				return ok.ToOkResult<StationOkWrap_模组检测, StationErrWrap_模组检测>();
			}
			var err = new StationErrWrap_模组检测()
			{
				ImagePath = outputs.ImagePath,
				ErrorCode = (uint)outputs.ErrorCode,
			};
			return err.ToErrResult<StationOkWrap_模组检测, StationErrWrap_模组检测>();
		}

		public async Task<FSharpResult<StationOkWrap_模组贴标, StationErrWrap_模组贴标>> 模组贴标ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure模组贴标2D_Defines.流程名);
			if (proc == null)
			{
				var micerr = new StationErrWrap_模组贴标() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure模组贴标2D_Defines.流程名}" };
				return micerr.ToErrResult<StationOkWrap_模组贴标, StationErrWrap_模组贴标>();
			}
			proc.ModuParams.SetInputInt(Procedure模组贴标2D_Defines.Function, [args.Function]);
			proc.ModuParams.SetInputInt(Procedure模组贴标2D_Defines.Position, [args.Position]);
			proc.ModuParams.SetInputString(Procedure模组贴标2D_Defines.Batch, [new InputStringData { strValue = args.Batch.ToString() }]);

			proc.Run();

			var mres = proc.ModuResult;

			var s =
				 from ImagePath in mres.GetString(Procedure模组贴标2D_Defines.ImagePath)
				 from BarCode in mres.GetString(Procedure模组贴标2D_Defines.BarCode)
				 from X in mres.GetFloat(Procedure模组贴标2D_Defines.X)
				 from Y in mres.GetFloat(Procedure模组贴标2D_Defines.Y)
				 from R in mres.GetFloat(Procedure模组贴标2D_Defines.R)
				 from ErrorCode in mres.GetInt(Procedure模组贴标2D_Defines.ErrorCode)
				 select new { ImagePath, BarCode, X, Y, R, ErrorCode };

			if (s.IsError)
			{
				var micerr = new StationErrWrap_模组贴标() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<StationOkWrap_模组贴标, StationErrWrap_模组贴标>();
			}
			var outputs = s.ResultValue;
			if (outputs.ErrorCode == 1)
			{
				var ok = new StationOkWrap_模组贴标
				{
					ImagePath = outputs.ImagePath,
					BarCode1 = outputs.BarCode,
					X = outputs.X,
					Y = outputs.Y,
					R = outputs.R,
				};
				return ok.ToOkResult<StationOkWrap_模组贴标, StationErrWrap_模组贴标>();
			}
			var err = new StationErrWrap_模组贴标()
			{
				ImagePath = outputs.ImagePath,
				ErrorCode = (uint)outputs.ErrorCode,
			};
			return err.ToErrResult<StationOkWrap_模组贴标, StationErrWrap_模组贴标>();
		}

		public async Task<FSharpResult<StationOkWrap_垫片检测, StationErrWrap_垫片检测>> 垫片检测1ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure垫片检测12D_Defines.流程名);
			if (proc == null)
			{
				var micerr = new StationErrWrap_垫片检测() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure垫片检测12D_Defines.流程名}" };
				return micerr.ToErrResult<StationOkWrap_垫片检测, StationErrWrap_垫片检测>();
			}
			proc.ModuParams.SetInputInt(Procedure垫片检测12D_Defines.Function, [args.Function]);
			proc.ModuParams.SetInputInt(Procedure垫片检测12D_Defines.Position, [args.Position]);
			proc.ModuParams.SetInputString(Procedure垫片检测12D_Defines.Batch, [new InputStringData { strValue = args.Batch.ToString() }]);

			proc.Run();

			var mres = proc.ModuResult;

			var s =
				 from ImagePath in mres.GetString(Procedure垫片检测12D_Defines.ImagePath)
				 from ErrorCode in mres.GetInt(Procedure垫片检测12D_Defines.ErrorCode)
				 select new { ImagePath, ErrorCode };

			if (s.IsError)
			{
				var micerr = new StationErrWrap_垫片检测() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<StationOkWrap_垫片检测, StationErrWrap_垫片检测>();
			}
			var outputs = s.ResultValue;
			if (outputs.ErrorCode == 1)
			{
				var ok = new StationOkWrap_垫片检测
				{
					ImagePath = outputs.ImagePath,
				};
				return ok.ToOkResult<StationOkWrap_垫片检测, StationErrWrap_垫片检测>();
			}
			var err = new StationErrWrap_垫片检测()
			{
				ImagePath = outputs.ImagePath,
				ErrorCode = (uint)outputs.ErrorCode,
			};
			return err.ToErrResult<StationOkWrap_垫片检测, StationErrWrap_垫片检测>();
		}

		public async Task<FSharpResult<StationOkWrap_模组入箱, StationErrWrap_模组入箱>> 模组入箱1ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure模组入箱12D_Defines.流程名);
			if (proc == null)
			{
				var micerr = new StationErrWrap_模组入箱() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure模组入箱12D_Defines.流程名}" };
				return micerr.ToErrResult<StationOkWrap_模组入箱, StationErrWrap_模组入箱>();
			}
			proc.ModuParams.SetInputInt(Procedure模组入箱12D_Defines.Function, [args.Function]);
			proc.ModuParams.SetInputInt(Procedure模组入箱12D_Defines.Position, [args.Position]);
			proc.ModuParams.SetInputString(Procedure模组入箱12D_Defines.Batch, [new InputStringData { strValue = args.Batch.ToString() }]);

			proc.Run();

			var mres = proc.ModuResult;

			var s =
				 from ImagePath in mres.GetString(Procedure模组入箱12D_Defines.ImagePath)
				 from ErrorCode in mres.GetInt(Procedure模组入箱12D_Defines.ErrorCode)
				 from OffsetX in mres.GetFloat(Procedure模组入箱12D_Defines.OffsetX)
				 from OffsetY in mres.GetFloat(Procedure模组入箱12D_Defines.OffsetY)
				 from OffsetA in mres.GetFloat(Procedure模组入箱12D_Defines.OffsetA)
				 select new { ImagePath, OffsetX, OffsetY, OffsetA, ErrorCode };

			if (s.IsError)
			{
				var micerr = new StationErrWrap_模组入箱() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<StationOkWrap_模组入箱, StationErrWrap_模组入箱>();
			}
			var outputs = s.ResultValue;
			if (outputs.ErrorCode == 1)
			{
				var ok = new StationOkWrap_模组入箱
				{
					ImagePath = outputs.ImagePath,
					X = outputs.OffsetX,
					Y = outputs.OffsetY,
					A = outputs.OffsetA,
				};
				return ok.ToOkResult<StationOkWrap_模组入箱, StationErrWrap_模组入箱>();
			}
			var err = new StationErrWrap_模组入箱()
			{
				ImagePath = outputs.ImagePath,
				ErrorCode = (uint)outputs.ErrorCode,
			};
			return err.ToErrResult<StationOkWrap_模组入箱, StationErrWrap_模组入箱>();
		}

		public async Task<FSharpResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>> 水冷板抓取定位引导1ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure水冷板抓取定位引导1_Defines.流程名);
			if (proc == null)
			{
				var micerr = new 定位引导ErrWrap_水冷板抓取() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure水冷板抓取定位引导1_Defines.流程名}" };
				return micerr.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			proc.ModuParams.SetInputInt(Procedure水冷板抓取定位引导1_Defines.Function, [args.Function]);
			proc.ModuParams.SetInputInt(Procedure水冷板抓取定位引导1_Defines.Position, [args.Position]);
			proc.ModuParams.SetInputString(Procedure水冷板抓取定位引导1_Defines.Batch, [new InputStringData { strValue = args.Batch.ToString() }]);
			proc.Run();

			var mres = proc.ModuResult;
			var s =
				 from PLC参数NG in mres.GetInt(Procedure水冷板抓取定位引导1_Defines.PLC参数NG)
				 from 找特征点NG in mres.GetInt(Procedure水冷板抓取定位引导1_Defines.找特征点NG)
				 from 偏移量NG in mres.GetInt(Procedure水冷板抓取定位引导1_Defines.偏移量NG)
				 from 视觉流程NG in mres.GetInt(Procedure水冷板抓取定位引导1_Defines.视觉流程NG)
				 from ImagePath in mres.GetString(Procedure水冷板抓取定位引导1_Defines.ImagePath)
				 from X in mres.GetFloat(Procedure水冷板抓取定位引导1_Defines.X偏移量)
				 from Y in mres.GetFloat(Procedure水冷板抓取定位引导1_Defines.Y偏移量)
				 from R in mres.GetFloat(Procedure水冷板抓取定位引导1_Defines.R偏移量)
				 select new { PLC参数NG, 找特征点NG, 偏移量NG, 视觉流程NG, ImagePath, X, Y, R };

			if (s.IsError)
			{
				var micerr = new 定位引导ErrWrap_水冷板抓取() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			var outputs = s.ResultValue;

			if (outputs.PLC参数NG != 1)
			{
				var err = new 定位引导ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, PLC参数NG = true, };
				return err.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			if (outputs.找特征点NG != 1)
			{
				var err = new 定位引导ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 找特征点NG = true, };
				return err.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			if (outputs.偏移量NG != 1)
			{
				var err = new 定位引导ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 偏移量NG = true, };
				return err.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			if (outputs.视觉流程NG != 1)
			{
				var err = new 定位引导ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 视觉流程NG = true, };
				return err.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}

			var ok = new 定位引导OkWrap_水冷板抓取 { ImagePath = outputs.ImagePath, X = outputs.X, Y = outputs.Y, R = outputs.R, };
			return ok.ToOkResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
		}

		public async Task<FSharpResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>> 水冷板抓取定位引导2ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure水冷板抓取定位引导2_Defines.流程名);
			if (proc == null)
			{
				var micerr = new 定位引导ErrWrap_水冷板抓取() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure水冷板抓取定位引导2_Defines.流程名}" };
				return micerr.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			proc.ModuParams.SetInputInt(Procedure水冷板抓取定位引导2_Defines.Function, [args.Function]);
			proc.ModuParams.SetInputInt(Procedure水冷板抓取定位引导2_Defines.Position, [args.Position]);
			proc.ModuParams.SetInputString(Procedure水冷板抓取定位引导2_Defines.Batch, [new InputStringData { strValue = args.Batch.ToString() }]);
			proc.Run();

			var mres = proc.ModuResult;
			var s =
				 from PLC参数NG in mres.GetInt(Procedure水冷板抓取定位引导2_Defines.PLC参数NG)
				 from 找特征点NG in mres.GetInt(Procedure水冷板抓取定位引导2_Defines.找特征点NG)
				 from 偏移量NG in mres.GetInt(Procedure水冷板抓取定位引导2_Defines.偏移量NG)
				 from 视觉流程NG in mres.GetInt(Procedure水冷板抓取定位引导2_Defines.视觉流程NG)
				 from ImagePath in mres.GetString(Procedure水冷板抓取定位引导2_Defines.ImagePath)
				 from X in mres.GetFloat(Procedure水冷板抓取定位引导2_Defines.X偏移量)
				 from Y in mres.GetFloat(Procedure水冷板抓取定位引导2_Defines.Y偏移量)
				 from R in mres.GetFloat(Procedure水冷板抓取定位引导2_Defines.R偏移量)
				 select new { PLC参数NG, 找特征点NG, 偏移量NG, 视觉流程NG, ImagePath, X, Y, R };

			if (s.IsError)
			{
				var micerr = new 定位引导ErrWrap_水冷板抓取() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			var outputs = s.ResultValue;

			if (outputs.PLC参数NG != 1)
			{
				var err = new 定位引导ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, PLC参数NG = true, };
				return err.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			if (outputs.找特征点NG != 1)
			{
				var err = new 定位引导ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 找特征点NG = true, };
				return err.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			if (outputs.偏移量NG != 1)
			{
				var err = new 定位引导ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 偏移量NG = true, };
				return err.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}
			if (outputs.视觉流程NG != 1)
			{
				var err = new 定位引导ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 视觉流程NG = true, };
				return err.ToErrResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
			}

			var ok = new 定位引导OkWrap_水冷板抓取 { ImagePath = outputs.ImagePath, X = outputs.X, Y = outputs.Y, R = outputs.R, };
			return ok.ToOkResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>();
		}

		public async Task<FSharpResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>> 水冷板抓取校验1ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure水冷板抓取校验1_Defines.流程名);
			if (proc == null)
			{
				var micerr = new 校验ErrWrap_水冷板抓取() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure水冷板抓取校验1_Defines.流程名}" };
				return micerr.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			proc.ModuParams.SetInputInt(Procedure水冷板抓取校验1_Defines.Function, [args.Function]);
			proc.Run();

			var mres = proc.ModuResult;
			var s =
				 from PLC参数NG in mres.GetInt(Procedure水冷板抓取校验1_Defines.PLC参数NG)
				 from 找特征点NG in mres.GetInt(Procedure水冷板抓取校验1_Defines.找特征点NG)
				 from 偏移量NG in mres.GetInt(Procedure水冷板抓取校验1_Defines.偏移量NG)
				 from 视觉流程NG in mres.GetInt(Procedure水冷板抓取校验1_Defines.视觉流程NG)
				 from ImagePath in mres.GetString(Procedure水冷板抓取校验1_Defines.ImagePath)
				 from X in mres.GetFloat(Procedure水冷板抓取校验1_Defines.X偏移量)
				 from Y in mres.GetFloat(Procedure水冷板抓取校验1_Defines.Y偏移量)
				 from R in mres.GetFloat(Procedure水冷板抓取校验1_Defines.R偏移量)
				 select new { PLC参数NG, 找特征点NG, 偏移量NG, 视觉流程NG, ImagePath, X, Y, R };

			if (s.IsError)
			{
				var micerr = new 校验ErrWrap_水冷板抓取() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			var outputs = s.ResultValue;

			if (outputs.PLC参数NG != 1)
			{
				var err = new 校验ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, PLC参数NG = true, };
				return err.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			if (outputs.找特征点NG != 1)
			{
				var err = new 校验ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 找特征点NG = true, };
				return err.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			if (outputs.偏移量NG != 1)
			{
				var err = new 校验ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 偏移量NG = true, };
				return err.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			if (outputs.视觉流程NG != 1)
			{
				var err = new 校验ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 视觉流程NG = true, };
				return err.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}

			var ok = new 校验OkWrap_水冷板抓取 { ImagePath = outputs.ImagePath, X = outputs.X, Y = outputs.Y, R = outputs.R, };
			return ok.ToOkResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
		}

		public async Task<FSharpResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>> 水冷板抓取校验2ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure水冷板抓取校验2_Defines.流程名);
			if (proc == null)
			{
				var micerr = new 校验ErrWrap_水冷板抓取() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure水冷板抓取校验2_Defines.流程名}" };
				return micerr.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			proc.ModuParams.SetInputInt(Procedure水冷板抓取校验2_Defines.Function, [args.Function]);
			proc.Run();

			var mres = proc.ModuResult;
			var s =
				 from PLC参数NG in mres.GetInt(Procedure水冷板抓取校验2_Defines.PLC参数NG)
				 from 找特征点NG in mres.GetInt(Procedure水冷板抓取校验2_Defines.找特征点NG)
				 from 偏移量NG in mres.GetInt(Procedure水冷板抓取校验2_Defines.偏移量NG)
				 from 视觉流程NG in mres.GetInt(Procedure水冷板抓取校验2_Defines.视觉流程NG)
				 from ImagePath in mres.GetString(Procedure水冷板抓取校验2_Defines.ImagePath)
				 from X in mres.GetFloat(Procedure水冷板抓取校验2_Defines.X偏移量)
				 from Y in mres.GetFloat(Procedure水冷板抓取校验2_Defines.Y偏移量)
				 from R in mres.GetFloat(Procedure水冷板抓取校验2_Defines.R偏移量)
				 select new { PLC参数NG, 找特征点NG, 偏移量NG, 视觉流程NG, ImagePath, X, Y, R };

			if (s.IsError)
			{
				var micerr = new 校验ErrWrap_水冷板抓取() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			var outputs = s.ResultValue;

			if (outputs.PLC参数NG != 1)
			{
				var err = new 校验ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, PLC参数NG = true, };
				return err.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			if (outputs.找特征点NG != 1)
			{
				var err = new 校验ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 找特征点NG = true, };
				return err.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			if (outputs.偏移量NG != 1)
			{
				var err = new 校验ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 偏移量NG = true, };
				return err.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}
			if (outputs.视觉流程NG != 1)
			{
				var err = new 校验ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 视觉流程NG = true, };
				return err.ToErrResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
			}

			var ok = new 校验OkWrap_水冷板抓取 { ImagePath = outputs.ImagePath, X = outputs.X, Y = outputs.Y, R = outputs.R, };
			return ok.ToOkResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>();
		}

		public async Task<FSharpResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>> 水冷板抓取自动校准1ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure水冷板抓取自动校准1_Defines.流程名);
			if (proc == null)
			{
				var micerr = new 自动校准ErrWrap_水冷板抓取() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure水冷板抓取自动校准1_Defines.流程名}" };
				return micerr.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			proc.ModuParams.SetInputInt(Procedure水冷板抓取自动校准1_Defines.Function, [args.Function]);
			proc.Run();

			var mres = proc.ModuResult;
			var s =
				 from 找特征点NG in mres.GetInt(Procedure水冷板抓取自动校准1_Defines.找特征点NG)
				 from 像素精度NG in mres.GetInt(Procedure水冷板抓取自动校准1_Defines.像素精度NG)
				 from 测距NG in mres.GetInt(Procedure水冷板抓取自动校准1_Defines.测距NG)
				 from 视觉流程NG in mres.GetInt(Procedure水冷板抓取自动校准1_Defines.视觉流程NG)
				 from ImagePath in mres.GetString(Procedure水冷板抓取自动校准1_Defines.ImagePath)
				 select new { 找特征点NG, 像素精度NG, 测距NG, 视觉流程NG, ImagePath };

			if (s.IsError)
			{
				var micerr = new 自动校准ErrWrap_水冷板抓取() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			var outputs = s.ResultValue;

			if (outputs.找特征点NG != 1)
			{
				var err = new 自动校准ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 找特征点NG = true, };
				return err.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			if (outputs.像素精度NG != 1)
			{
				var err = new 自动校准ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 像素精度NG = true, };
				return err.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			if (outputs.测距NG != 1)
			{
				var err = new 自动校准ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 测距NG = true, };
				return err.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			if (outputs.视觉流程NG != 1)
			{
				var err = new 自动校准ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 视觉流程NG = true, };
				return err.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}

			var ok = new 自动校准OkWrap_水冷板抓取 { ImagePath = outputs.ImagePath, };
			return ok.ToOkResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
		}

		public async Task<FSharpResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>> 水冷板抓取自动校准2ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure水冷板抓取自动校准2_Defines.流程名);
			if (proc == null)
			{
				var micerr = new 自动校准ErrWrap_水冷板抓取() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure水冷板抓取自动校准2_Defines.流程名}" };
				return micerr.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			proc.ModuParams.SetInputInt(Procedure水冷板抓取自动校准2_Defines.Function, [args.Function]);
			proc.Run();

			var mres = proc.ModuResult;
			var s =
				 from 找特征点NG in mres.GetInt(Procedure水冷板抓取自动校准2_Defines.找特征点NG)
				 from 像素精度NG in mres.GetInt(Procedure水冷板抓取自动校准2_Defines.像素精度NG)
				 from 测距NG in mres.GetInt(Procedure水冷板抓取自动校准2_Defines.测距NG)
				 from 视觉流程NG in mres.GetInt(Procedure水冷板抓取自动校准2_Defines.视觉流程NG)
				 from ImagePath in mres.GetString(Procedure水冷板抓取自动校准2_Defines.ImagePath)
				 select new { 找特征点NG, 像素精度NG, 测距NG, 视觉流程NG, ImagePath };

			if (s.IsError)
			{
				var micerr = new 自动校准ErrWrap_水冷板抓取() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			var outputs = s.ResultValue;

			if (outputs.找特征点NG != 1)
			{
				var err = new 自动校准ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 找特征点NG = true, };
				return err.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			if (outputs.像素精度NG != 1)
			{
				var err = new 自动校准ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 像素精度NG = true, };
				return err.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			if (outputs.测距NG != 1)
			{
				var err = new 自动校准ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 测距NG = true, };
				return err.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}
			if (outputs.视觉流程NG != 1)
			{
				var err = new 自动校准ErrWrap_水冷板抓取() { ImagePath = outputs.ImagePath, 视觉流程NG = true, };
				return err.ToErrResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
			}

			var ok = new 自动校准OkWrap_水冷板抓取 { ImagePath = outputs.ImagePath, };
			return ok.ToOkResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>();
		}

		public async Task<FSharpResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>> 人工位水冷板检测ProcAsync(StationArgs args)
		{
			VmProcedure proc = await LoadProcAsync(Procedure人工位水冷板检测_Defines.流程名);
			if (proc == null)
			{
				var micerr = new 水冷板检测ErrWrap_人工位() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure人工位水冷板检测_Defines.流程名}" };
				return micerr.ToErrResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>();
			}
			proc.ModuParams.SetInputString(Procedure人工位水冷板检测_Defines.Batch, [new InputStringData { strValue = args.Batch.ToString() }]);
			proc.Run();

			var mres = proc.ModuResult;
			var s =
				 from PLC参数NG in mres.GetInt(Procedure人工位水冷板检测_Defines.PLC参数NG)
				 from 找特征点NG in mres.GetInt(Procedure人工位水冷板检测_Defines.找特征点NG)
				 from 偏移量NG in mres.GetInt(Procedure人工位水冷板检测_Defines.偏移量NG)
				 from 视觉流程NG in mres.GetInt(Procedure人工位水冷板检测_Defines.视觉流程NG)
				 from ImagePath in mres.GetString(Procedure人工位水冷板检测_Defines.ImagePath)
				 from 左短位置度 in mres.GetFloat(Procedure人工位水冷板检测_Defines.左短位置度)
				 from 左上位置度 in mres.GetFloat(Procedure人工位水冷板检测_Defines.左上位置度)
				 from 左下位置度 in mres.GetFloat(Procedure人工位水冷板检测_Defines.左下位置度)
				 from 右短位置度 in mres.GetFloat(Procedure人工位水冷板检测_Defines.右短位置度)
				 from 右上位置度 in mres.GetFloat(Procedure人工位水冷板检测_Defines.右上位置度)
				 from 右下位置度 in mres.GetFloat(Procedure人工位水冷板检测_Defines.右下位置度)
				 select new
				 {
					 PLC参数NG,
					 找特征点NG,
					 偏移量NG,
					 视觉流程NG,
					 ImagePath,
					 左短位置度,
					 左上位置度,
					 左下位置度,
					 右短位置度,
					 右上位置度,
					 右下位置度,
				 };

			if (s.IsError)
			{
				var micerr = new 水冷板检测ErrWrap_人工位() { ErrMsg = s.ErrorValue };
				return micerr.ToErrResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>();
			}
			var outputs = s.ResultValue;

			if (outputs.PLC参数NG != 1)
			{
				var err = new 水冷板检测ErrWrap_人工位() { ImagePath = outputs.ImagePath, PLC参数NG = true, };
				return err.ToErrResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>();
			}
			if (outputs.找特征点NG != 1)
			{
				var err = new 水冷板检测ErrWrap_人工位() { ImagePath = outputs.ImagePath, 找特征点NG = true, };
				return err.ToErrResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>();
			}
			if (outputs.偏移量NG != 1)
			{
				var err = new 水冷板检测ErrWrap_人工位() { ImagePath = outputs.ImagePath, 偏移量NG = true, };
				return err.ToErrResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>();
			}
			if (outputs.视觉流程NG != 1)
			{
				var err = new 水冷板检测ErrWrap_人工位() { ImagePath = outputs.ImagePath, 视觉流程NG = true, };
				return err.ToErrResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>();
			}

			var ok = new 水冷板检测OkWrap_人工位
			{
				ImagePath = outputs.ImagePath,
				左短位置度 = outputs.左短位置度,
				左上位置度 = outputs.左上位置度,
				左下位置度 = outputs.左下位置度,
				右短位置度 = outputs.右短位置度,
				右上位置度 = outputs.右上位置度,
				右下位置度 = outputs.右下位置度,
			};
			return ok.ToOkResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>();
		}

		public async Task<FSharpResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>> 机器人1相机ProcAsync(StationArgs args, CamModelInput model)
		{
			VmProcedure proc = await LoadProcAsync(Procedure侧板相机1工位_Defines.流程名);
			if (proc == null)
			{
				var micerr = new StationErrWrap_侧板自动拧紧() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure侧板相机1工位_Defines.流程名}" };
				return micerr.ToErrResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
			}

			proc.ModuParams.SetInputFloat(Procedure侧板相机1工位_Defines.InputX, [model.PositionX]);
			proc.ModuParams.SetInputFloat(Procedure侧板相机1工位_Defines.InputY, [model.PositionY]);
			proc.ModuParams.SetInputFloat(Procedure侧板相机1工位_Defines.InputZ, [model.PositionZ]);
			proc.ModuParams.SetInputFloat(Procedure侧板相机1工位_Defines.InputA, [model.PositionA]);
			proc.ModuParams.SetInputInt(Procedure侧板相机1工位_Defines.Batch, [(int)args.Batch]);
			proc.ModuParams.SetInputString(Procedure侧板相机1工位_Defines.ModuleCode, [new InputStringData { strValue = model.ModuleCode }]);
			proc.ModuParams.SetInputInt(Procedure侧板相机1工位_Defines.Function, [(int)args.Function]);
			proc.ModuParams.SetInputInt(Procedure侧板相机1工位_Defines.Position, [(int)args.Position]);

			proc.Run();

			var mres = proc.ModuResult;

			var s =
				 from PositionX in mres.GetFloat(Procedure侧板相机1工位_Defines.PositionX)
				 from PositionY in mres.GetFloat(Procedure侧板相机1工位_Defines.PositionY)
				 from PositionZ in mres.GetFloat(Procedure侧板相机1工位_Defines.PositionZ)
				 from PositionA in mres.GetFloat(Procedure侧板相机1工位_Defines.PositionA)

				 from PLCNG in mres.GetInt(Procedure侧板相机1工位_Defines.PLCNG)
				 from TZDNG in mres.GetInt(Procedure侧板相机1工位_Defines.TZDNG)
				 from LCNG in mres.GetInt(Procedure侧板相机1工位_Defines.LCNG)
				 from QTNG in mres.GetInt(Procedure侧板相机1工位_Defines.QTNG)


				 from ImagePath in mres.GetString(Procedure侧板相机1工位_Defines.ImagePath)
				 from Status in mres.GetInt(Procedure侧板相机1工位_Defines.Status)
				 select new { ImagePath, PositionX, PositionY, PositionZ, PositionA, PLCNG, TZDNG, LCNG, QTNG, Status };

			if (s.IsError)
			{
				var micerr = new StationErrWrap_侧板自动拧紧() { ErrMsg = s.ErrorValue, ErrorCode = 1, Flag = false };
				return micerr.ToErrResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
			}
			var outputs = s.ResultValue;
			if (outputs.Status == 1)
			{
				var ok = new StationOkWrap_侧板自动拧紧
				{
					ImagePath = s.ResultValue.ImagePath,
					PositionA = s.ResultValue.PositionA,
					PositionX = s.ResultValue.PositionX,
					PositionY = s.ResultValue.PositionY,
					PositionZ = s.ResultValue.PositionZ,
					Status = s.ResultValue.Status
				};
				return ok.ToOkResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
			}
			var err = new StationErrWrap_侧板自动拧紧()
			{
				ImagePath = outputs.ImagePath,
				ErrorCode = (uint)outputs.Status,
				特征点NG = s.ResultValue.LCNG == 1,
				PLC参数NG = s.ResultValue.PLCNG == 1,
				其它NG = s.ResultValue.QTNG == 1,
				视觉检测流程NG = s.ResultValue.LCNG == 1
			};
			return err.ToErrResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
		}

        public async Task<FSharpResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>> 机器人2相机ProcAsync(StationArgs args, CamModelInput model)
        {
            VmProcedure proc = await LoadProcAsync(Procedure侧板相机2工位_Defines.流程名);
            if (proc == null)
            {
                var micerr = new StationErrWrap_侧板自动拧紧() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure侧板相机2工位_Defines.流程名}" };
                return micerr.ToErrResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
            }

			proc.ModuParams.SetInputFloat(Procedure侧板相机2工位_Defines.InputX, [model.PositionX]);
			proc.ModuParams.SetInputFloat(Procedure侧板相机2工位_Defines.InputY, [model.PositionY]);
			proc.ModuParams.SetInputFloat(Procedure侧板相机2工位_Defines.InputZ, [model.PositionZ]);
			proc.ModuParams.SetInputFloat(Procedure侧板相机2工位_Defines.InputA, [model.PositionA]);
			proc.ModuParams.SetInputInt(Procedure侧板相机2工位_Defines.Batch, [(int)args.Batch]);
            proc.ModuParams.SetInputString(Procedure侧板相机2工位_Defines.ModuleCode, [new InputStringData { strValue = model.ModuleCode }]);
            proc.ModuParams.SetInputInt(Procedure侧板相机2工位_Defines.Function, [(int)args.Function]);
            proc.ModuParams.SetInputInt(Procedure侧板相机2工位_Defines.Position, [(int)args.Position]);

            proc.Run();

            var mres = proc.ModuResult;

            var s =
                 from PositionX in mres.GetFloat(Procedure侧板相机2工位_Defines.PositionX)
                 from PositionY in mres.GetFloat(Procedure侧板相机2工位_Defines.PositionY)
                 from PositionZ in mres.GetFloat(Procedure侧板相机2工位_Defines.PositionZ)
                 from PositionA in mres.GetFloat(Procedure侧板相机2工位_Defines.PositionA)

                 from PLCNG in mres.GetInt(Procedure侧板相机2工位_Defines.PLCNG)
                 from TZDNG in mres.GetInt(Procedure侧板相机2工位_Defines.TZDNG)
                 from LCNG in mres.GetInt(Procedure侧板相机2工位_Defines.LCNG)
                 from QTNG in mres.GetInt(Procedure侧板相机2工位_Defines.QTNG)


                 from ImagePath in mres.GetString(Procedure侧板相机2工位_Defines.ImagePath)
                 from Status in mres.GetInt(Procedure侧板相机2工位_Defines.Status)
                 select new { ImagePath, PositionX, PositionY, PositionZ, PositionA, PLCNG, TZDNG, LCNG, QTNG, Status };

            if (s.IsError)
            {
                var micerr = new StationErrWrap_侧板自动拧紧() { ErrMsg = s.ErrorValue, ErrorCode = 1, Flag = false };
                return micerr.ToErrResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
            }
            var outputs = s.ResultValue;
            if (outputs.Status == 1)
            {
                var ok = new StationOkWrap_侧板自动拧紧
                {
                    ImagePath = s.ResultValue.ImagePath,
                    PositionA = s.ResultValue.PositionA,
                    PositionX = s.ResultValue.PositionX,
                    PositionY = s.ResultValue.PositionY,
                    PositionZ = s.ResultValue.PositionZ,
                    Status = s.ResultValue.Status
                };
                return ok.ToOkResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
            }
            var err = new StationErrWrap_侧板自动拧紧()
            {
                ImagePath = outputs.ImagePath,
                ErrorCode = (uint)outputs.Status,
                特征点NG = s.ResultValue.LCNG == 1,
                PLC参数NG = s.ResultValue.PLCNG == 1,
                其它NG = s.ResultValue.QTNG == 1,
                视觉检测流程NG = s.ResultValue.LCNG == 1
            };
            return err.ToErrResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
        }

        public async Task<FSharpResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>> 机器人3相机ProcAsync(StationArgs args, CamModelInput model)
        {
            VmProcedure proc = await LoadProcAsync(Procedure侧板相机3工位_Defines.流程名);
            if (proc == null)
            {
                var micerr = new StationErrWrap_侧板自动拧紧() { ErrMsg = $"{Language.Msg_加载流程失败}:{Procedure侧板相机3工位_Defines.流程名}" };
                return micerr.ToErrResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
            }

            proc.ModuParams.SetInputFloat(Procedure侧板相机3工位_Defines.InputX, [model.PositionX]);
            proc.ModuParams.SetInputFloat(Procedure侧板相机3工位_Defines.InputY, [model.PositionY]);
            proc.ModuParams.SetInputFloat(Procedure侧板相机3工位_Defines.InputZ, [model.PositionZ]);
            proc.ModuParams.SetInputFloat(Procedure侧板相机3工位_Defines.InputA, [model.PositionA]);
            proc.ModuParams.SetInputInt(Procedure侧板相机3工位_Defines.Batch, [(int)args.Batch]);
            proc.ModuParams.SetInputString(Procedure侧板相机3工位_Defines.ModuleCode, [new InputStringData { strValue = model.ModuleCode }]);
            proc.ModuParams.SetInputInt(Procedure侧板相机3工位_Defines.Function, [(int)args.Function]);
            proc.ModuParams.SetInputInt(Procedure侧板相机3工位_Defines.Position, [(int)args.Position]);

            proc.Run();

            var mres = proc.ModuResult;

            var s =
                 from PositionX in mres.GetFloat(Procedure侧板相机3工位_Defines.PositionX)
                 from PositionY in mres.GetFloat(Procedure侧板相机3工位_Defines.PositionY)
                 from PositionZ in mres.GetFloat(Procedure侧板相机3工位_Defines.PositionZ)
                 from PositionA in mres.GetFloat(Procedure侧板相机3工位_Defines.PositionA)

                 from PLCNG in mres.GetInt(Procedure侧板相机3工位_Defines.PLCNG)
                 from TZDNG in mres.GetInt(Procedure侧板相机3工位_Defines.TZDNG)
                 from LCNG in mres.GetInt(Procedure侧板相机3工位_Defines.LCNG)
                 from QTNG in mres.GetInt(Procedure侧板相机3工位_Defines.QTNG)

                 from ImagePath in mres.GetString(Procedure侧板相机3工位_Defines.ImagePath)
                 from Status in mres.GetInt(Procedure侧板相机3工位_Defines.Status)
                 select new { ImagePath, PositionX, PositionY, PositionZ, PositionA, PLCNG, TZDNG, LCNG, QTNG, Status };

            if (s.IsError)
            {
                var micerr = new StationErrWrap_侧板自动拧紧() { ErrMsg = s.ErrorValue, ErrorCode = 1, Flag = false };
                return micerr.ToErrResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
            }
            var outputs = s.ResultValue;
            if (outputs.Status == 1)
            {
                var ok = new StationOkWrap_侧板自动拧紧
                {
                    ImagePath = s.ResultValue.ImagePath,
                    PositionA = s.ResultValue.PositionA,
                    PositionX = s.ResultValue.PositionX,
                    PositionY = s.ResultValue.PositionY,
                    PositionZ = s.ResultValue.PositionZ,
                    Status = s.ResultValue.Status
                };
                return ok.ToOkResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
            }
            var err = new StationErrWrap_侧板自动拧紧()
            {
                ImagePath = outputs.ImagePath,
                ErrorCode = (uint)outputs.Status,
                特征点NG = s.ResultValue.LCNG == 1,
                PLC参数NG = s.ResultValue.PLCNG == 1,
                其它NG = s.ResultValue.QTNG == 1,
                视觉检测流程NG = s.ResultValue.LCNG == 1
            };
            return err.ToErrResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>();
        }


        public async Task<FSharpResult<Dictionary<string, bool>, string>> GetCameraStatus()
		{
			try
			{
				var globalCameraList = new List<GlobalCameraModuleTool>();
				var vmModules = new List<VmModule>();
				VmSolution.Instance.GetAllModule(vmModules);
				var s = VmSolution.Instance;
				//GlobalCameraModuleTool globalCamera = (GlobalCameraModuleTool)VmSolution.Instance["全局相机1"];

				foreach (var module in vmModules)
				{
					if (module.GetType() == typeof(GlobalCameraModuleTool))
					{
						globalCameraList.Add((GlobalCameraModuleTool)module);
					}
				}

				//获取流程中所有已配置的连接状态
				var dic = new Dictionary<string, bool>();
				foreach (var cameraTool in globalCameraList)
				{
					dic.Add(cameraTool.Name, cameraTool.bIsCameraConnect());
				}
				return dic.ToOkResult<Dictionary<string, bool>, string>();
			}
			catch (Exception ex)
			{
				return ex.Message.ToErrResult<Dictionary<string, bool>, string>();
			}
		}


	}
}
