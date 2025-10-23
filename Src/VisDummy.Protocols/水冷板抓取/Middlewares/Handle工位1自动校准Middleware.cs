using Itminus.FSharpExtensions;
using Itminus.Protocols;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.Warp.水冷板抓取;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.水冷板抓取.Middlewares.Common;
using VisDummy.Protocols.水冷板抓取.Model;

namespace VisDummy.Protocols.水冷板抓取.Middlewares
{
    public class Handle工位1自动校准Middleware(
        PlcCtrlFlusher flusher,
        IVisProc visproc,
        ILogger<Handle工位1自动校准Middleware> logger,
        IMediator mediator
        ) : HandlePlcRequestMiddlewareBase<DevMsg_自动校准, MstMsg_自动校准, StationArgs, 自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>(flusher, logger, mediator)
    {
        public override string PlcName => PlcNames.PLCNAME_水冷板抓取;

        public override string ProcName => "机器人1自动校准";

        public override bool HasAck(MstMsg_自动校准 p) => p.Ack;

        public override bool HasReq(DevMsg_自动校准 i) => i.Trigger;

        public override DevMsg_自动校准 RefIncoming(ScanContext ctx) => ctx.DevMsg.机器人1自动校准;

        public override MstMsg_自动校准 RefPending(ScanContext ctx) => ctx.MstMsg.机器人1自动校准;

        protected override async Task<FSharpResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>> HandleArgsAsync(StationArgs args)
        {
            await RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_视觉输入参数}：{args.ToMsg()}");
            var res =
                //from r1 in visParams.SetTriggerGlobalParams().SelectError(s => new 自动校准ErrWrap_水冷板抓取 { ErrMsg = s })
                from r2 in visproc.水冷板抓取自动校准1ProcAsync(args)
                select r2;
            return await res;
        }

        protected override async Task HandleErrAsync(MstMsg_自动校准 pending, 自动校准ErrWrap_水冷板抓取 err)
        {
            pending.ackFlag = new Mst_AckFlagsBuilder(pending.ackFlag).SetOn(false).Build();
            pending.ngFlag = new Mst_自动校准NGFlagsBuilder(pending.ngFlag).SetOn(err.找特征点NG, err.像素精度NG, err.测距NG, err.视觉流程NG).Build();
            await RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{err.ToMsg()}");
        }

        protected override async Task HandleOkAsync(MstMsg_自动校准 pending, 自动校准OkWrap_水冷板抓取 descriptions)
        {
            pending.ackFlag = new Mst_AckFlagsBuilder(pending.ackFlag).SetOn(true).Build();
            await RecordLogAsync(LogLevel.Information, $"{Language.Msg_拍照成功}：{descriptions.ToMsg()}");
        }

        protected override async Task ResetAckAsync(MstMsg_自动校准 pending)
        {
            pending.ackFlag = new Mst_AckFlagsBuilder(pending.ackFlag).SetOff().Build();
            pending.ngFlag = new Mst_自动校准NGFlagsBuilder(pending.ngFlag).SetOff().Build();
            await RecordLogAsync(LogLevel.Information, Language.Msg_初始化);
        }

        protected override FSharpResult<StationArgs, string> TryParseArgs(DevMsg_自动校准 incoming)
        {
            var args = new StationArgs() { Function = incoming.Function, };
            return args.ToOkResult<StationArgs, string>();
        }
    }
}
