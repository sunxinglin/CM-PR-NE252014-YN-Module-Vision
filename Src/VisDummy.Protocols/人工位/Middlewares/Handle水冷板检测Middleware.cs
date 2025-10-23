using Itminus.FSharpExtensions;
using Itminus.Protocols;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using System.Text.Json;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.Warp.人工位;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.人工位.Middlewares.Common;
using VisDummy.Protocols.人工位.Model;

namespace VisDummy.Protocols.人工位.Middlewares
{
    public class Handle水冷板检测Middleware(
        PlcCtrlFlusher flusher,
        IVisProc visproc,
        ILogger<Handle水冷板检测Middleware> logger,
        IMediator mediator
        ) : HandlePlcRequestMiddlewareBase<DevMsg_水冷板检测, MstMsg_水冷板检测, StationArgs, 水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>(flusher, logger, mediator)
    {
        public override string PlcName => PlcNames.PLCNAME_人工位;

        public override string ProcName => "人工位水冷板检测";

        public override bool HasAck(MstMsg_水冷板检测 p) => p.Ack;

        public override bool HasReq(DevMsg_水冷板检测 i) => i.Trigger;

        public override DevMsg_水冷板检测 RefIncoming(ScanContext ctx) => ctx.DevMsg.水冷板检测;

        public override MstMsg_水冷板检测 RefPending(ScanContext ctx) => ctx.MstMsg.水冷板检测;

        protected override async Task<FSharpResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>> HandleArgsAsync(StationArgs args)
        {
            await RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_视觉输入参数}：{args.ToMsg()}");
            var res =
                //from r1 in visParams.SetTriggerGlobalParams().SelectError(s => new 水冷板检测ErrWrap_人工位 { ErrMsg = s })
                from r2 in visproc.人工位水冷板检测ProcAsync(args)
                select r2;
            return await res;
        }

        protected override async Task HandleErrAsync(MstMsg_水冷板检测 pending, 水冷板检测ErrWrap_人工位 err)
        {
            pending.ackFlag = new Mst_AckFlagsBuilder(pending.ackFlag).SetOn(false).Build();
            pending.ngFlag = new Mst_NGFlagsBuilder(pending.ngFlag).SetOn(err.PLC参数NG, err.找特征点NG, err.偏移量NG, err.视觉流程NG).Build();
            await RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{JsonSerializer.Serialize(err)}");
        }

        protected override async Task HandleOkAsync(MstMsg_水冷板检测 pending, 水冷板检测OkWrap_人工位 descriptions)
        {
            pending.ackFlag = new Mst_AckFlagsBuilder(pending.ackFlag).SetOn(true).Build();
            pending.leftShortPosition = descriptions.左短位置度;
            pending.upperLeftPosition = descriptions.左上位置度;
            pending.lowerLeftPosition = descriptions.左下位置度;
            pending.rightShortPosition = descriptions.右短位置度;
            pending.upperRightPosition = descriptions.右上位置度;
            pending.lowerRightPosition = descriptions.右下位置度;
            await RecordLogAsync(LogLevel.Information, $"{Language.Msg_拍照成功}：{JsonSerializer.Serialize(descriptions)}");
        }

        protected override async Task ResetAckAsync(MstMsg_水冷板检测 pending)
        {
            pending.ackFlag = new Mst_AckFlagsBuilder(pending.ackFlag).SetOff().Build();
            pending.ngFlag = new Mst_NGFlagsBuilder(pending.ngFlag).SetOff().Build();
            pending.leftShortPosition = default;
            pending.upperLeftPosition = default;
            pending.lowerLeftPosition = default;
            pending.rightShortPosition = default;
            pending.upperRightPosition = default;
            pending.lowerRightPosition = default;
            await RecordLogAsync(LogLevel.Information, Language.Msg_初始化);
        }

        protected override FSharpResult<StationArgs, string> TryParseArgs(DevMsg_水冷板检测 incoming)
        {
            var args = new StationArgs()
            {
                Batch = incoming.Batch,
            };
            return args.ToOkResult<StationArgs, string>();
        }
    }
}
