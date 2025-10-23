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
    public class HandleStation2DSpotMiddleware(模组入箱Flusher flusher, IVisProc visproc, IVisParams visParams, ILogger<HandlePlcRequestMiddlewareBase<DevMsg_2DSpotStation, MstMsg_2DSpotStation, SpotStationArgs, SpotStationOkWarp, SpotStationErrWarp>> logger, IMediator mediator) : HandlePlcRequestMiddlewareBase<
        DevMsg_2DSpotStation, MstMsg_2DSpotStation, SpotStationArgs,
        SpotStationOkWarp, SpotStationErrWarp
        >(flusher, logger, mediator)
    {
        public override string PlcName => PlcNames.PLCNAME_模组入箱;

        public override string ProcName => Language.Msg_精度校验;

        public override bool HasAck(MstMsg_2DSpotStation p) => p.CmdSpot.Ack;


        public override bool HasReq(DevMsg_2DSpotStation i) => i.CmdSpot.Trigger;

        public override DevMsg_2DSpotStation RefIncoming(ScanContext ctx) => ctx.DevMsg.Station2DSpot;


        public override MstMsg_2DSpotStation RefPending(ScanContext ctx) => ctx.MstMsg.Station2DSpot;

        protected override async Task<FSharpResult<SpotStationOkWarp, SpotStationErrWarp>> HandleArgsAsync(SpotStationArgs args)
        {
            await RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_视觉输入参数}：{args.ToMsg()}");
            var res = from r1 in visParams.SetTriggerGlobalParams()
                      .SelectError(s => new SpotStationErrWarp { ErrMsg = s })
                      from r2 in visproc.SpotProcAsync(args)
                      select r2;
            return await res;
        }

        protected override async Task HandleErrAsync(MstMsg_2DSpotStation pending, SpotStationErrWarp err)
        {
            pending.CmdSpot.SetOn(false, err.ErrorCode, 0, 0);
            await RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{err.ToMsg()}");
        }

        protected override async Task HandleOkAsync(MstMsg_2DSpotStation pending, SpotStationOkWarp descriptions)
        {
            pending.CmdSpot.SetOn(true, 0, descriptions.Features, descriptions.Pixels);
            await RecordLogAsync(LogLevel.Information, $"{Language.Msg_拍照成功}：{descriptions.ToMsg()}");
        }

        protected override async Task ResetAckAsync(MstMsg_2DSpotStation pending)
        {
            pending.CmdSpot.SetOff();
            await RecordLogAsync(LogLevel.Information, Language.Msg_初始化);
        }

        protected override FSharpResult<SpotStationArgs, string> TryParseArgs(DevMsg_2DSpotStation incoming)
        {
            var args = new SpotStationArgs()
            {
                Camera = incoming.CmdSpot.Camera,
                Position = incoming.CmdSpot.Position,
            };
            return args.ToOkResult<SpotStationArgs, string>();
        }
    }
}
