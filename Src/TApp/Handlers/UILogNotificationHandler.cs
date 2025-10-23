using System.Threading;
using MediatR;
using Microsoft.Extensions.Logging;
using Splat;
using StdUnit.One.Shared;
using TApp.Apis;
using TApp.ViewModels;
using TApp.ViewModels.Realtime;
using VisDummy.Shared.LogGroup;
using VisDummy.Shared.Utils;

namespace TApp.Handlers
{
    internal class UILogNotificationHandler : INotificationHandler<UILogNotification>
    {
        private readonly UILogsViewModel _vm;
        private readonly ILogger<UILogNotificationHandler> _logger;
        private readonly ICloudApi _api;

        public UILogNotificationHandler(ILogger<UILogNotificationHandler> logger, ICloudApi api)
        {
            this._vm = Locator.Current.GetRequiredService<UILogsViewModel>();
            this._logger = logger;
            this._api = api;
        }

        public async Task Handle(UILogNotification notification, CancellationToken cancellationToken)
        {
            var msg = notification.LogMessage;
            var appvm = Locator.Current.GetRequiredService<AppViewModel>();

            #region 操作日志
            //if (msg.EventGroup == LogGroupName.OperationLog)
            //{
            //    var appvm = Locator.Current.GetRequiredService<AppViewModel>();
            //    try
            //    {
            //        await _api.AddOperationLog(appvm.UserName, msg.Content);
            //    }
            //    catch
            //    {
            //    }
            //    msg.Content = $"{msg.Content};【Account:{appvm.Account};Name:{appvm.UserName}】";
            //}
            #endregion

            try
            {
                await _api.AddLog(new Apis.Models.LogAddRequest
                {
                    EventSource = msg.EventSource,
                    EventGroup = msg.EventGroup,
                    Level = msg.Level,
                    Content = msg.Content,
                    Operator = appvm.UserName,
                });
            }
            catch
            {
            }


            if (msg != null && msg.Level >= LogLevel.Information)
            {
                this._vm.OnNext(msg);
            }
            _logger.Log(msg.Level, msg.Content);
        }
    }
}
