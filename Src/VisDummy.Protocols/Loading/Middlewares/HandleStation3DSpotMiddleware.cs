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
    public class HandleStation3DSpotMiddleware(LoadingFlusher flusher, IMKProc mkproc, ILogger<HandlePlcRequestMiddlewareBase<DevMsg_3DSpotStation, MstMsg_3DSpotStation, StationArgs_MK, StationOkWrap_MK, StationErrWrap_MK>> logger, IMediator mediator) : HandlePlcRequestMiddlewareBase<
        DevMsg_3DSpotStation, MstMsg_3DSpotStation, StationArgs_MK,
        StationOkWrap_MK, StationErrWrap_MK
        >(flusher, logger, mediator)
    {
        private bool IsFinish = false;
        public override string PlcName => PlcNames.PLCNAME_Loading;

        public override string ProcName => Language.Msg_标定球拍照请求;

        public override bool HasAck(MstMsg_3DSpotStation p) => p.Ack;


        public override bool HasReq(DevMsg_3DSpotStation i) => i.Trigger || i.TriggerFinish;

        public override DevMsg_3DSpotStation RefIncoming(ScanContext ctx) => ctx.DevMsg.Station3DSpot;


        public override MstMsg_3DSpotStation RefPending(ScanContext ctx) => ctx.MstMsg.Station3DSpot;

        protected override async Task<FSharpResult<StationOkWrap_MK, StationErrWrap_MK>> HandleArgsAsync(StationArgs_MK args)
        {
            await this.RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_视觉输入参数}：{args.ToMsg()}");
            return IsFinish ? await mkproc.Proc3DSpotCheckResultAsync(args) : await mkproc.Proc3DSpotCheckAsync(args);
        }

        protected override async Task HandleErrAsync(MstMsg_3DSpotStation pending, StationErrWrap_MK err)
        {
            pending.SetOn(false);
            await this.RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{err.ToMsg()}");
        }

        protected override async Task HandleOkAsync(MstMsg_3DSpotStation pending, StationOkWrap_MK descriptions)
        {
            pending.SetOn(true);
            await this.RecordLogAsync(LogLevel.Information, $"{Language.Msg_拍照成功}：{descriptions.ToMsg()}");
        }

        protected override async Task ResetAckAsync(MstMsg_3DSpotStation pending)
        {
            pending.SetOff();
            await RecordLogAsync(LogLevel.Information, Language.Msg_初始化);
        }

        protected override FSharpResult<StationArgs_MK, string> TryParseArgs(DevMsg_3DSpotStation incoming)
        {
            if (incoming.Trigger)
            {
                if (incoming.Position == 0)
                {
                    return $"标定球拍照次数不可为0:{incoming.Position}".ToErrResult<StationArgs_MK, string>();
                }
                IsFinish = false;
                return new StationArgs_MK { Function_Number = 2, Position_Number = incoming.Position }.ToOkResult<StationArgs_MK, string>();
            }

            if (incoming.TriggerFinish)
            {
                IsFinish = true;
                return new StationArgs_MK { Function_Number = 3 }.ToOkResult<StationArgs_MK, string>();
            }
            throw new Exception(Language.Msg_系统错误);
        }
    }
}
