using Itminus.FSharpExtensions;
using Itminus.Protocols;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.Warp;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.模组检测.Middlewares.Common;
using VisDummy.Protocols.模组检测.Model;

namespace VisDummy.Protocols.模组检测.Middlewares
{
    public class HandleStation2DMiddleware(模组检测Flusher flusher, IVisProc visproc, IVisParams visParams, ILogger<HandlePlcRequestMiddlewareBase<DevMsg_2DStation, MstMsg_2DStation, StationArgs, StationOkWrap_模组检测, StationErrWrap_模组检测>> logger, IMediator mediator) : HandlePlcRequestMiddlewareBase<
        DevMsg_2DStation, MstMsg_2DStation, StationArgs,
        StationOkWrap_模组检测, StationErrWrap_模组检测
        >(flusher, logger, mediator)
    {
        public override string PlcName => PlcNames.PLCNAME_模组检测;

        public override string ProcName => Language.Msg_正常工作拍照;

        public override bool HasAck(MstMsg_2DStation p) => p.CmdReply.Ack;


        public override bool HasReq(DevMsg_2DStation i) => i.CmdTrigger.Trigger;

        public override DevMsg_2DStation RefIncoming(ScanContext ctx) => ctx.DevMsg.Station2D;


        public override MstMsg_2DStation RefPending(ScanContext ctx) => ctx.MstMsg.Station2D;

        protected override async Task<FSharpResult<StationOkWrap_模组检测, StationErrWrap_模组检测>> HandleArgsAsync(StationArgs args)
        {
            await RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_视觉输入参数}：{args.ToMsg()}");
            var res = from r1 in visParams.SetTriggerGlobalParams()
                      .SelectError(s => new StationErrWrap_模组检测 { ErrMsg = s })
                      from r2 in visproc.模组检测ProcAsync(args)
                      select r2;
            return await res;
        }

        protected override async Task HandleErrAsync(MstMsg_2DStation pending, StationErrWrap_模组检测 err)
        {
            pending.CmdReply.SetOn(false, err.ErrorCode);
            await RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{err.ToMsg()}");
        }

        protected override async Task HandleOkAsync(MstMsg_2DStation pending, StationOkWrap_模组检测 descriptions)
        {
            pending.CmdReply.SetOn(true, 0);
            pending.component = descriptions.Component;
            pending.SetPolaritys(descriptions.Polaritys);
            await RecordLogAsync(LogLevel.Information, $"{Language.Msg_拍照成功}：{descriptions.ToMsg()}");
        }

        protected override async Task ResetAckAsync(MstMsg_2DStation pending)
        {
            pending.SetOff();
            await RecordLogAsync(LogLevel.Information, Language.Msg_初始化);
        }

        protected override FSharpResult<StationArgs, string> TryParseArgs(DevMsg_2DStation incoming)
        {
            var args = new StationArgs()
            {
                Function = incoming.CmdTrigger.Function,
                Position = incoming.CmdTrigger.Position,
                Batch = incoming.CmdTrigger.Batch,
            };
            return args.ToOkResult<StationArgs, string>();
        }
    }
}
