using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StdUnit.One.Shared;
using StdUnit.Sharp7.Options;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.Loading.Model;

namespace Itminus.Protocols.Loading
{
    public class LoadingScanner : S7PlcScanner<DevMsg, MstMsg>
    {
        private readonly IMediator _mediator;

        public LoadingScanner(IMediator mediator, IServiceScopeFactory ssf, ILogger<S7PlcScanner<DevMsg, MstMsg>> logger, IOptionsMonitor<S7ScanOpt> scanOpts, S7PlcMgr plcMgr)
            : base(ssf, logger, scanOpts, plcMgr)
        {
            this._mediator = mediator;
        }

        public override string PlcName => PlcNames.PLCNAME_Loading;

        public override string ScanName => PlcNames.PLCNAME_Loading;

        public override async Task<DevMsg> GetDevMsgAsync(S7ScanOpt scanOpt)
        {
            var read = await PlcCtrl.ReadDBAsync<DevMsg>(scanOpt.DevMsg_DB_INDEX, scanOpt.DevMsg_DB_OFFSET);
            if (read.IsError)
            {
                throw new Exception(read.ErrorValue.ToString());
            }

            return read.ResultValue;
        }

        public override async Task<MstMsg> GetMstMsgAsync(S7ScanOpt scanOpt)
        {
            object obj = PlcCtrl.Pending;
            if (obj == null)
            {
                _logger.LogWarning("[!] PLC=[" + PlcName + "]{ " + Language.Msg_MST信号为空将从PLC重新读取 + "}");
                var read = await PlcCtrl.ReadDBAsync<MstMsg>(scanOpt.MstMsg_DB_INDEX, scanOpt.MstMsg_DB_OFFSET);
                if (read.IsError)
                {
                    throw new Exception(read.ErrorValue.ToString());
                }

                obj = read.ResultValue;
                PlcCtrl.Pending = obj;
                _logger.LogWarning("[-] PLC=[" + PlcName + "] " + Language.Msg_MST信号已从PLC恢复 + "");
            }

            return (MstMsg)obj;
        }

        public override async Task HandleErrAsync(LogLevel level, string message)
        {
            var uilogmsg = new UILogNotification()
            {
                LogMessage = new AlarmMessage
                {
                    EventSource = this.PlcName,
                    EventGroup = this.PlcName,
                    Content = message,
                    EventNumber = this.PlcName,
                    Level = LogLevel.Error,
                    Timestamp = DateTime.UtcNow,
                },
            };
            await this._mediator.Publish(uilogmsg);
        }

        protected override async Task HandleMsgAsync(IServiceProvider sp, DevMsg devMsg, MstMsg mstMsg, DateTimeOffset scanedAt)
        {
            var context = new ScanContext(sp, devMsg, mstMsg, scanedAt);
            var processor = sp.GetRequiredService<ScanProcessor>();

            await processor.HandleAsync(context);
        }
    }

}
