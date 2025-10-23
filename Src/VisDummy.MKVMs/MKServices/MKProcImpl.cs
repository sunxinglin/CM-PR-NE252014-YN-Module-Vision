using Itminus.FSharpExtensions;
using Microsoft.FSharp.Core;
using Splat;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.Warp;
using VisDummy.Lang.Resources;
using VisDummy.MKVMs.Common;
using VisDummy.MKVMs.ViewModels;
using VisDummy.Shared.Utils;

namespace VisDummy.MKVMs.MKServices
{
    public class MKProcImpl : IMKProc
    {
        protected virtual Vision3DCtrl Load3DProcAsync(string procName)
        {
            var rts = Locator.Current.GetServices<IVisionMarker>();
            var rt = rts.FirstOrDefault(i => i.ProcName == procName);
            if (rt is Vis3DRtViewModel vm)
            {
                return vm.Vision3DCtrl;
            }
            throw new System.Exception($"{Language.Msg_流程未注册或未注册成}{typeof(Vis3DRtViewModel)}:{procName}");
        }
        public async Task<FSharpResult<StationOkWrap_MK, StationErrWrap_MK>> ProcAsync(StationArgs_MK args)
        {
            Vision3DCtrl proc = Load3DProcAsync("大包装上料");
            if (proc == null)
            {
                var err = new StationErrWrap_MK() { ErrMsg = Language.Msg_无法加载3D视觉控制器, };
                return err.ToErrResult<StationOkWrap_MK, StationErrWrap_MK>();
            }

            var fn = args.Function_Number;
            var r = from r1 in proc.ReadCmdAsync(proc.Cmd_103(fn, 1))
                    from r2 in r1.Analyze103().ToTask()
                    from r3 in proc.ReadCmdAsync(proc.Cmd_101(fn))
                    from r4 in r3.Analyze101().ToTask()
                    from r5 in proc.ReadCmdAsync(proc.Cmd_110(fn))
                    from r6 in r5.Analyze110().ToTask()
                    select r6;
            var s = await r;

            if (s.IsError)
            {
                var err = new StationErrWrap_MK() { ErrMsg = s.ErrorValue };
                return err.ToErrResult<StationOkWrap_MK, StationErrWrap_MK>();
            }

            var res = s.ResultValue;

            //正常拍照状态
            if (res.ResultStatus == 2)
            {
                var ok = new StationOkWrap_MK()
                {
                    ResultStatus = res.ResultStatus,
                    Foam = 2,
                    Floor = res.Floor,
                    Column = res.Column,
                    Direction = res.Postion,
                    PreciseX = res.X,
                    PreciseY = res.Y,
                    PreciseZ = res.Z,
                    PreciseA = res.A,
                    PreciseB = res.B,
                    PreciseC = res.C,
                };
                return ok.ToOkResult<StationOkWrap_MK, StationErrWrap_MK>();
            }
            //空泡棉状态
            if (res.ResultStatus == 6)
            {
                var ok = new StationOkWrap_MK()
                {
                    ResultStatus = res.ResultStatus,
                    Foam = 1,
                    Floor = res.Floor,
                    Column = res.Column,
                    Direction = res.Postion,
                    PreciseX = res.X,
                    PreciseY = res.Y,
                    PreciseZ = res.Z,
                    PreciseA = res.A,
                    PreciseB = res.B,
                    PreciseC = res.C,
                };
                return ok.ToOkResult<StationOkWrap_MK, StationErrWrap_MK>();
            }
            var micerr = new StationErrWrap_MK()
            {
                ErrMsg = Language.Msg_3D视觉拍照状态码异常,
                ResultStatus = res.ResultStatus,
            };
            return micerr.ToErrResult<StationOkWrap_MK, StationErrWrap_MK>();
        }

        public async Task<FSharpResult<StationOkWrap_MK, StationErrWrap_MK>> Proc3DSpotCheckAsync(StationArgs_MK args)
        {
            Vision3DCtrl proc = Load3DProcAsync("大包装上料");
            if (proc == null)
            {
                var err = new StationErrWrap_MK() { ErrMsg = Language.Msg_无法加载3D视觉控制器, };
                return err.ToErrResult<StationOkWrap_MK, StationErrWrap_MK>();
            }
            var fn = args.Function_Number;
            var pn = args.Position_Number;
            var r = from r1 in proc.ReadCmdAsync(proc.Cmd_103(fn, pn))
                    from r2 in r1.Analyze103().ToTask()
                    from r3 in proc.ReadCmdAsync(proc.Cmd_101(fn))
                    from r4 in r3.Analyze101().ToTask()
                    from r5 in proc.ReadCmdAsync(proc.Cmd_102(fn))
                    from r6 in r5.Analyze102().ToTask()
                    select r6;
            var s = await r;

            if (s.IsError)
            {
                var err = new StationErrWrap_MK() { ErrMsg = s.ErrorValue };
                return err.ToErrResult<StationOkWrap_MK, StationErrWrap_MK>();
            }

            var res = s.ResultValue;

            if (res.ResultStatus == "20")
            {
                var ok = new StationOkWrap_MK();
                return ok.ToOkResult<StationOkWrap_MK, StationErrWrap_MK>();
            }
            var micerr = new StationErrWrap_MK()
            {
                ErrMsg = Language.Msg_3D视觉拍照状态码异常,
            };
            return micerr.ToErrResult<StationOkWrap_MK, StationErrWrap_MK>();
        }

        public async Task<FSharpResult<StationOkWrap_MK, StationErrWrap_MK>> Proc3DSpotCheckResultAsync(StationArgs_MK args)
        {
            Vision3DCtrl proc = Load3DProcAsync("大包装上料");
            if (proc == null)
            {
                var err = new StationErrWrap_MK() { ErrMsg = Language.Msg_无法加载3D视觉控制器, };
                return err.ToErrResult<StationOkWrap_MK, StationErrWrap_MK>();
            }
            var fn = args.Function_Number;
            var r = from r1 in proc.ReadCmdAsync(proc.Cmd_101(fn))
                    from r2 in r1.Analyze101().ToTask()
                    from r3 in proc.ReadCmdAsync(proc.Cmd_102(fn))
                    from r4 in r3.Analyze102().ToTask()
                    select r4;
            var s = await r;

            if (s.IsError)
            {
                var err = new StationErrWrap_MK() { ErrMsg = s.ErrorValue };
                return err.ToErrResult<StationOkWrap_MK, StationErrWrap_MK>();
            }

            var res = s.ResultValue;

            if (res.ResultStatus == "21")
            {
                var ok = new StationOkWrap_MK();
                return ok.ToOkResult<StationOkWrap_MK, StationErrWrap_MK>();
            }
            var micerr = new StationErrWrap_MK()
            {
                ErrMsg = Language.Msg_3D视觉拍照状态码异常,
            };
            return micerr.ToErrResult<StationOkWrap_MK, StationErrWrap_MK>();
        }
    }
}
