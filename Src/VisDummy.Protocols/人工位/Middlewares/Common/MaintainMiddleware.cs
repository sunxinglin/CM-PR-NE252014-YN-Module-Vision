using Itminus.Middlewares;
using Itminus.Protocols;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StdUnit.One.Shared;
using StdUnit.Sharp7.Options;
using VisDummy.Lang.Resources;

namespace VisDummy.Protocols.人工位.Middlewares.Common
{
    public class MaintainMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly ILogger<MaintainMiddleware> _logger;
        private readonly IOptionsMonitor<S7ScanOpt> _scanOptsMonitor;
        private readonly IMediator _mediator;
        private readonly PlcScanner _scanner;

        public MaintainMiddleware(ILogger<MaintainMiddleware> logger, IOptionsMonitor<S7ScanOpt> scanOptsMonitor, IMediator mediator, PlcScanner scanner)
        {
            _logger = logger;
            _scanOptsMonitor = scanOptsMonitor;
            _mediator = mediator;
            _scanner = scanner;
        }



        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            var maintain = context.DevMsg.Heart.IsMaintaining;
            // 如果是维护模式，直接发送数据，并且不再执行后续中间件
            if (maintain)
            {

                var s7ScanOpt = _scanOptsMonitor.Get(_scanner.ScanName);
                var res = await _scanner.PlcCtrl.WriteDBAsync(s7ScanOpt.MstMsg_DB_INDEX, s7ScanOpt.MstMsg_DB_OFFSET, context.MstMsg);
                if (res.IsError)
                {
                    throw new Exception($"【{PlcNames.PLCNAME_Loading}】{Language.Msg_向PLC写数据错误}：{res.ErrorValue}");
                }
            }
            else
            {
                await next(context);
            }
        }

        public async Task RecordLogAsync(LogLevel level, string logTemplate, params object[] args)
        {
            var logmsg = string.Format(logTemplate, args);
            var notification = level < LogLevel.Warning ?
                new UILogNotification(new LogMessage
                {
                    Level = level,
                    Content = logmsg,
                    Timestamp = DateTime.Now,
                }) :
                new UILogNotification(new AlarmMessage
                {
                    Level = level,
                    EventSource = PlcNames.PLCNAME_Loading,
                    Content = logmsg,
                    Timestamp = DateTime.Now,
                });
            await _mediator.Publish(notification);
        }
    }

}
