using Itminus.FSharpExtensions;
using Itminus.Protocols;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.Warp;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.模组入箱.Middlewares.Common;
using VisDummy.Protocols.模组入箱.Model;

namespace VisDummy.Protocols.模组入箱.Middlewares
{
    public class HandleStation12DMiddleware(模组入箱Flusher flusher, IVisProc visproc, IVisParams visParams, ILogger<HandlePlcRequestMiddlewareBase<DevMsg_2DStation, MstMsg_2DStation, StationArgs, StationOkWrap_模组入箱, StationErrWrap_模组入箱>> logger, IMediator mediator) : HandlePlcRequestMiddlewareBase<
        DevMsg_2DStation, MstMsg_2DStation, StationArgs,
        StationOkWrap_模组入箱, StationErrWrap_模组入箱
        >(flusher, logger, mediator)
    {
        public override string PlcName => PlcNames.PLCNAME_模组入箱;

        public override string ProcName => Language.Msg_正常工作拍照 + "1";

        public override bool HasAck(MstMsg_2DStation p) => p.CmdReply.Ack;


        public override bool HasReq(DevMsg_2DStation i) => i.CmdTrigger.Trigger;

        public override DevMsg_2DStation RefIncoming(ScanContext ctx) => ctx.DevMsg.Station2D1;


        public override MstMsg_2DStation RefPending(ScanContext ctx) => ctx.MstMsg.Station2D1;

        protected override async Task<FSharpResult<StationOkWrap_模组入箱, StationErrWrap_模组入箱>> HandleArgsAsync(StationArgs args)
        {
            await RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_视觉输入参数}：{args.ToMsg()}");
            var res = from r1 in visParams.SetTriggerGlobalParams()
                      .SelectError(s => new StationErrWrap_模组入箱 { ErrMsg = s })
                      from r2 in visproc.模组入箱1ProcAsync(args)
                      select r2;
            return await res;
        }

        protected override async Task HandleErrAsync(MstMsg_2DStation pending, StationErrWrap_模组入箱 err)
        {
            pending.CmdReply.SetOn(false, err.ErrorCode);
            await RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{err.ToMsg()}");
        }

        protected override async Task HandleOkAsync(MstMsg_2DStation pending, StationOkWrap_模组入箱 descriptions)
        {
            pending.CmdReply.SetOn(true, 0);
            pending.SetOffset(descriptions.X, descriptions.Y, descriptions.A);
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
