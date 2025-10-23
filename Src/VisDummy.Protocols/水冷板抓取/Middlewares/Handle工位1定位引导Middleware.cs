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
    public class Handle工位1定位引导Middleware(
        PlcCtrlFlusher flusher,
        IVisProc visproc,
        ILogger<Handle工位1定位引导Middleware> logger,
        IMediator mediator
        ) : HandlePlcRequestMiddlewareBase<DevMsg_定位引导, MstMsg_定位引导, StationArgs, 定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>(flusher, logger, mediator)
    {
        public override string PlcName => PlcNames.PLCNAME_水冷板抓取;

        public override string ProcName => "机器人1定位引导";

        public override bool HasAck(MstMsg_定位引导 p) => p.Ack;

        public override bool HasReq(DevMsg_定位引导 i) => i.Trigger;

        public override DevMsg_定位引导 RefIncoming(ScanContext ctx) => ctx.DevMsg.机器人1定位引导;

        public override MstMsg_定位引导 RefPending(ScanContext ctx) => ctx.MstMsg.机器人1定位引导;

        protected override async Task<FSharpResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>> HandleArgsAsync(StationArgs args)
        {
            await RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_视觉输入参数}：{args.ToMsg()}");
            var res =
                //from r1 in visParams.SetTriggerGlobalParams().SelectError(s => new 定位引导ErrWrap_水冷板抓取 { ErrMsg = s })
                from r2 in visproc.水冷板抓取定位引导1ProcAsync(args)
                select r2;
            return await res;
        }

        protected override async Task HandleErrAsync(MstMsg_定位引导 pending, 定位引导ErrWrap_水冷板抓取 err)
        {
            pending.ackFlag = new Mst_AckFlagsBuilder(pending.ackFlag).SetOn(false).Build();
            pending.ngFlag = new Mst_NGFlagsBuilder(pending.ngFlag).SetOn(err.PLC参数NG, err.找特征点NG, err.偏移量NG, err.视觉流程NG).Build();
            await RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{err.ToMsg()}");
        }

        protected override async Task HandleOkAsync(MstMsg_定位引导 pending, 定位引导OkWrap_水冷板抓取 descriptions)
        {
            pending.ackFlag = new Mst_AckFlagsBuilder(pending.ackFlag).SetOn(true).Build();
            pending.x = descriptions.X;
            pending.y = descriptions.Y;
            pending.a = descriptions.R;
            await RecordLogAsync(LogLevel.Information, $"{Language.Msg_拍照成功}：{descriptions.ToMsg()}");
        }

        protected override async Task ResetAckAsync(MstMsg_定位引导 pending)
        {
            pending.ackFlag = new Mst_AckFlagsBuilder(pending.ackFlag).SetOff().Build();
            pending.ngFlag = new Mst_NGFlagsBuilder(pending.ngFlag).SetOff().Build();
            pending.x = default;
            pending.y = default;
            pending.a = default;
            await RecordLogAsync(LogLevel.Information, Language.Msg_初始化);
        }

        protected override FSharpResult<StationArgs, string> TryParseArgs(DevMsg_定位引导 incoming)
        {
            var args = new StationArgs()
            {
                Function = incoming.Function,
                Position = incoming.Position,
                Batch = incoming.Batch,
            };
            return args.ToOkResult<StationArgs, string>();
        }
    }
}
