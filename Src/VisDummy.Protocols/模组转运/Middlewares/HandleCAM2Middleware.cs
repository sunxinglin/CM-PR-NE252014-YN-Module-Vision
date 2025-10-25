using Itminus.FSharpExtensions;
using Itminus.Middlewares;
using Itminus.Protocols;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using System.Runtime.Remoting.Contexts;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.Warp.模组转运;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.Common;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.模组转运.Middlewares.Common;
using VisDummy.Protocols.模组转运.Model;

namespace VisDummy.Protocols.模组转运.Middlewares
{
	public class HandleCAM2Middleware(
		模组转运Flusher flusher,
		IVisProc visproc,
		IVisParams visParams,
		ILogger<HandlePlcRequestMiddlewareBase<DevMsg_CAM2, MstMsg_CAM2, StationArgs, StationOkWrap_模组转运, StationErrWrap_模组转运>> logger,
		IMediator mediator) :
		HandlePlcRequestMiddlewareBase<
			DevMsg_CAM2,
			MstMsg_CAM2,
			StationArgs,
			StationOkWrap_模组转运,
			StationErrWrap_模组转运
			>(flusher, logger, mediator)
	{
		public override string PlcName => PlcNames.PLCNAME_模组转运;


		public override string ProcName => Language.Msg_正常工作拍照 + "1";

		public override bool HasAck(MstMsg_CAM2 p) => p.Ready;


		public override bool HasReq(DevMsg_CAM2 i) => i.Flag.Req;

		public override DevMsg_CAM2 RefIncoming(ScanContext ctx) => ctx.DevMsg.CAM2;


		public override MstMsg_CAM2 RefPending(ScanContext ctx) => ctx.MstMsg.CAM2;


		public string ModuleCode { get; set; }
		public float PositionX { get; set; }
		public float PositionY { get; set; }
		public float PositionZ { get; set; }
		public float PositionA { get; set; }

		protected override async Task<FSharpResult<StationOkWrap_模组转运, StationErrWrap_模组转运>> HandleArgsAsync(StationArgs args)
		{
			CamModelInput camModelInput = new CamModelInput()
			{
				Batch = args.Batch.ToString(),
				Function = args.Function,
				Position = args.Position,
				ModuleCode = ModuleCode,
				PositionA = PositionA,
				PositionX = PositionX,
				PositionY = PositionY,
				PositionZ = PositionZ,
			};

			await RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_视觉输入参数}：{args.ToMsg()}");
			var res = from r1 in visParams.SetTriggerGlobalParams()
					  .SelectError(s => new StationErrWrap_模组转运 { ErrMsg = s })
					  from r2 in visproc.模组转运机器人2相机ProcAsync(args, camModelInput)
					  select r2;
			return await res;
		}

		protected override async Task HandleErrAsync(MstMsg_CAM2 pending, StationErrWrap_模组转运 err)
		{
			pending.ErrorCode = new Mst_SideNGFlagsBuilder(pending.errorCode).SetOn(err.特征点NG, err.视觉检测流程NG, err.其它NG, err.PLC参数NG).Build();
			pending.Flag = new MstMsg_SideReplyFlagsBuilder(pending.Flag).SetResponseNg().Build();
            await RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{err.ToMsg()}");
		}

        protected override async Task HandleOkAsync(MstMsg_CAM2 pending, StationOkWrap_模组转运 descriptions)
        {
            pending.X = descriptions.PositionX;
            pending.Y = descriptions.PositionY;
            pending.A = descriptions.PositionA;
            pending.Z = descriptions.PositionZ;
            pending.Status = (ushort)descriptions.Status;
            pending.Flag = new MstMsg_SideReplyFlagsBuilder(pending.Flag).SetResponseOk(true).Build();
            await RecordLogAsync(LogLevel.Information, $"{Language.Msg_拍照成功}：{descriptions.ToMsg()}");
        }

        protected override async Task ResetAckAsync(MstMsg_CAM2 pending)
        {
            pending.Flag = new MstMsg_SideReplyFlagsBuilder(pending.Flag).SetOff().Build();
            pending.ErrorCode = new Mst_SideNGFlagsBuilder(pending.ErrorCode).SetOff().Build();
            pending.X = 0;
            pending.Y = 0;
            pending.A = 0;
            pending.Z = 0;
            pending.Status = 0;
            await RecordLogAsync(LogLevel.Information, Language.Msg_初始化);
        }

        protected override FSharpResult<StationArgs, string> TryParseArgs(DevMsg_CAM2 incoming)
		{
			var args = new StationArgs()
			{
				Function = incoming.Flag.FunctionNumber,
				Position = incoming.Flag.PhotoNumben,
			};
			return args.ToOkResult<StationArgs, string>();
		}

		public override async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
		{
			DevMsg_CAM2 incoming = RefIncoming(context);
			MstMsg_CAM2 pending = RefPending(context);
			bool skipNext = false;
			try
			{
				if (ShouldTakeAction(incoming, pending, context))
				{
					await RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_收到PLC拍照请求}");
					FSharpResult<StationArgs, string> fSharpResult = TryParseArgs(incoming);
					if (fSharpResult.IsOk)
					{
						StationArgs resultValue = fSharpResult.ResultValue;
						this.ModuleCode = context.DevMsg.Heart.ModelCode2.Content;
						this.PositionX = context.DevMsg.CAM2.Flag.PositionX;
						this.PositionY = context.DevMsg.CAM2.Flag.PositionY;
						this.PositionZ = context.DevMsg.CAM2.Flag.PositionZ;
						this.PositionA = context.DevMsg.CAM2.Flag.PositionA;
						
						FSharpResult<StationOkWrap_模组转运, StationErrWrap_模组转运> fSharpResult2 = await HandleArgsAsync(resultValue);
						if (!fSharpResult2.IsError)
						{
							await HandleOkAsync(pending, fSharpResult2.ResultValue);
						}
						else
						{
							await HandleErrAsync(pending, fSharpResult2.ErrorValue);
							skipNext = ShouldSkipIfArgsHandlingIsNg();
						}
					}
					else
					{
						string errorValue = fSharpResult.ErrorValue;
						await RecordLogAsync(LogLevel.Warning, $"{ProcName}: {Language.Msg_PLC参数错误}：{errorValue}");
					}
				}

				if (ShouldResetAction(incoming, pending, context))
				{
					await ResetAckAsync(pending);
				}
			}
			finally
			{
				if (skipNext)
				{
					await _flusher.FlushAsync(context.MstMsg);
				}
				else
				{
					await next(context);
				}
			}
		}
	}
}
