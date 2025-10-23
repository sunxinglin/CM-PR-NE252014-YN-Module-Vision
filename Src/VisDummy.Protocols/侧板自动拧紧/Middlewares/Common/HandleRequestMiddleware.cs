using Itminus.Middlewares;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using StdUnit.One.Shared;
using StdUnit.Sharp7.Options;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.侧板自动拧紧;
using VisDummy.Protocols.侧板自动拧紧.Model;

namespace VisDummy.Protocols.侧板自动拧紧.Middlewares.Common
{
    public abstract class HandlePlcRequestMiddlewareBase<TIncoming, TPending, TArgs, TOk, TErr> :
        HandleIncomingRequestMiddlewareBase<DevMsg, MstMsg, ScanContext, 侧板自动拧紧Scanner, TIncoming, TPending, TArgs, TOk, TErr>
    {
        protected readonly IMediator _mediator;

        protected HandlePlcRequestMiddlewareBase(侧板自动拧紧Flusher flusher, ILogger<HandlePlcRequestMiddlewareBase<TIncoming, TPending, TArgs, TOk, TErr>> logger, IMediator mediator) : base(flusher, logger)
        {
            _mediator = mediator;
        }

        public override async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            TIncoming incoming = RefIncoming(context);
            TPending pending = RefPending(context);
            bool skipNext = false;
            try
            {
                if (ShouldTakeAction(incoming, pending, context))
                {
                    await RecordLogAsync(LogLevel.Information, $"{ProcName}:{Language.Msg_收到PLC拍照请求}");
                    FSharpResult<TArgs, string> fSharpResult = TryParseArgs(incoming);
                    if (fSharpResult.IsOk)
                    {
                        TArgs resultValue = fSharpResult.ResultValue;
                        FSharpResult<TOk, TErr> fSharpResult2 = await HandleArgsAsync(resultValue);
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

        public override async Task RecordLogAsync(LogLevel level, string logTemplate, params object[] args)
        {
            var logmsg = string.Format(logTemplate, args);
            logmsg = $"{ProcName}：{logmsg}";
            var notification = new UILogNotification(new LogMessage
            {
                EventSource = PlcName,
                EventGroup = ProcName,
                Level = level,
                Content = logmsg,
                Timestamp = DateTime.Now,
            });
            await _mediator.Publish(notification);
        }
    }
}
