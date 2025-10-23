using Itminus.FSharpExtensions;
using Itminus.Middlewares;
using Itminus.Protocols;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.Warp.侧板自动拧紧;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.侧板自动拧紧.Middlewares.Common;
using VisDummy.Protocols.侧板自动拧紧.Model;

namespace VisDummy.Protocols.侧板自动拧紧.Middlewares
{
	public class HandleCAM1Middleware(
		侧板自动拧紧Flusher flusher,
		IVisProc visproc,
		IVisParams visParams,
		ILogger<HandlePlcRequestMiddlewareBase<DevMsg_CAM1, MstMsg_CAM1, StationArgs, StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>> logger,
		IMediator mediator) :
		HandlePlcRequestMiddlewareBase<
			DevMsg_CAM1,
			MstMsg_CAM1,
			StationArgs,
			StationOkWrap_侧板自动拧紧,
			StationErrWrap_侧板自动拧紧
			>(flusher, logger, mediator)
	{
		public override string PlcName => PlcNames.PLCNAME_侧板自动拧紧;


		public override string ProcName => Language.Msg_正常工作拍照 + "1";

		public override bool HasAck(MstMsg_CAM1 p) => p.Flag.Ready;


		public override bool HasReq(DevMsg_CAM1 i) => i.Flag.Req;

		public override DevMsg_CAM1 RefIncoming(ScanContext ctx) => ctx.DevMsg.CAM1;


		public override MstMsg_CAM1 RefPending(ScanContext ctx) => ctx.MstMsg.CAM1;


		public string ModuleCode { get; set; }
		public float PositionX { get; set; }
		public float PositionY { get; set; }
		public float PositionZ { get; set; }
		public float PositionA { get; set; }

		protected override async Task<FSharpResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>> HandleArgsAsync(StationArgs args)
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
					  .SelectError(s => new StationErrWrap_侧板自动拧紧 { ErrMsg = s })
					  from r2 in visproc.机器人1相机ProcAsync(args, camModelInput)
					  select r2;
			return await res;
		}

        protected override async Task HandleErrAsync(MstMsg_CAM1 pending, StationErrWrap_侧板自动拧紧 err)
        {
            pending.Flag.ErrorCode = new Mst_SideNGFlagsBuilder(pending.Flag.errorCode).SetOn(err.特征点NG, err.视觉检测流程NG, err.其它NG, err.PLC参数NG).Build();
            pending.Flag.Flag = new MstMsg_SideReplyFlagsBuilder(pending.Flag.Flag).SetResponseNg().Build();
            await RecordLogAsync(LogLevel.Error, $"{Language.Msg_拍照失败}：{err.ToMsg()}");
        }

        protected override async Task HandleOkAsync(MstMsg_CAM1 pending, StationOkWrap_侧板自动拧紧 descriptions)
        {
            pending.Flag.X = descriptions.PositionX;
            pending.Flag.Y = descriptions.PositionY;
            pending.Flag.A = descriptions.PositionA;
            pending.Flag.Z = descriptions.PositionZ;
            pending.Flag.Status = (ushort)descriptions.Status;
            pending.Flag.Flag = new MstMsg_SideReplyFlagsBuilder(pending.Flag.Flag).SetResponseOk(true).Build();
            await RecordLogAsync(LogLevel.Information, $"{Language.Msg_拍照成功}：{descriptions.ToMsg()}");
        }

        protected override async Task ResetAckAsync(MstMsg_CAM1 pending)
		{
			pending.Flag.Flag = new MstMsg_SideReplyFlagsBuilder(pending.Flag.Flag).SetOff().Build();
			pending.Flag.ErrorCode = new Mst_SideNGFlagsBuilder(pending.Flag.ErrorCode).SetOff().Build();
            pending.Flag.X = 0;
            pending.Flag.Y = 0;
            pending.Flag.A = 0;
            pending.Flag.Z = 0;
			pending.Flag.Status = 0;
			await RecordLogAsync(LogLevel.Information, Language.Msg_初始化);
		}

		protected override FSharpResult<StationArgs, string> TryParseArgs(DevMsg_CAM1 incoming)
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
			DevMsg_CAM1 incoming = RefIncoming(context);
			MstMsg_CAM1 pending = RefPending(context);
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
						this.ModuleCode = context.DevMsg.Heart.ModelCode1.Content;
						this.PositionX = context.DevMsg.CAM1.Flag.PositionX;
						this.PositionY = context.DevMsg.CAM1.Flag.PositionY;
						this.PositionZ = context.DevMsg.CAM1.Flag.PositionZ;
						this.PositionA = context.DevMsg.CAM1.Flag.PositionA;

						FSharpResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧> fSharpResult2 = await HandleArgsAsync(resultValue);
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
