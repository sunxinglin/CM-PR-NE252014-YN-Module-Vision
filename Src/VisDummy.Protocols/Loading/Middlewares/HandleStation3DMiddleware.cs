using Itminus.FSharpExtensions;
using Itminus.Protocols;
using Itminus.Protocols.Loading;
using Itminus.Protocols.Loading.Middlewares;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.Warp;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.Loading.Model;

namespace VisDummy.Protocols.Loading.Middlewares
{
    public class HandleStation3DMiddleware(LoadingFlusher flusher, IMKProc mkproc, ILogger<HandlePlcRequestMiddlewareBase<DevMsg_3DStation, MstMsg_3DStation, StationArgs_MK, StationOkWrap_MK, StationErrWrap_MK>> logger, IMediator mediator) : HandlePlcRequestMiddlewareBase<
        DevMsg_3DStation, MstMsg_3DStation, StationArgs_MK,
        StationOkWrap_MK, StationErrWrap_MK
        >(flusher, logger, mediator)
    {
        public override string PlcName => PlcNames.PLCNAME_Loading;

        public override string ProcName => Language.Msg_正常工作拍照;

        public override bool HasAck(MstMsg_3DStation p) => p.Ack;

        public override bool HasReq(DevMsg_3DStation i) => i.Trigger;

        public override DevMsg_3DStation RefIncoming(ScanContext ctx) => ctx.DevMsg.Station3D;

        public override MstMsg_3DStation RefPending(ScanContext ctx) => ctx.MstMsg.Station3D;

        protected override async Task<FSharpResult<StationOkWrap_MK, StationErrWrap_MK>> HandleArgsAsync(StationArgs_MK args)
        {
            await this.RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_视觉输入参数}：{args.ToMsg()}");
            var res = await mkproc.ProcAsync(args);
            return res;
        }

        protected override async Task HandleErrAsync(MstMsg_3DStation pending, StationErrWrap_MK err)
        {
            pending.SetOn(false, new CmdArg_Reply3D(err.ResultStatus));
            await this.RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{err.ToMsg()}");
        }

        protected override async Task HandleOkAsync(MstMsg_3DStation pending, StationOkWrap_MK descriptions)
        {
            pending.SetOn(true, new CmdArg_Reply3D(descriptions.ResultStatus, descriptions.Foam, descriptions.Floor, descriptions.Column, descriptions.Direction, descriptions.PreciseX, descriptions.PreciseY, descriptions.PreciseZ, descriptions.PreciseA, descriptions.PreciseB, descriptions.PreciseC));
            await this.RecordLogAsync(LogLevel.Information, $"{Language.Msg_拍照成功}：{descriptions.ToMsg()}");
        }

        protected override async Task ResetAckAsync(MstMsg_3DStation pending)
        {
            pending.SetOff();
            await RecordLogAsync(LogLevel.Information, Language.Msg_初始化);
        }

        protected override FSharpResult<StationArgs_MK, string> TryParseArgs(DevMsg_3DStation incoming)
        {
            var args = new StationArgs_MK()
            {
                Function_Number = incoming.Function,
            };
            return args.ToOkResult<StationArgs_MK, string>();
        }
    }
}
