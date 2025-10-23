using Itminus.Middlewares;
using VisDummy.Protocols.垫片检测.Middlewares;
using VisDummy.Protocols.垫片检测.Middlewares.Common;
using VisDummy.Protocols.垫片检测.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.垫片检测
{
    /// <summary>
    /// 处理器
    /// </summary>
    public class ScanProcessor
    {
        private WorkDelegate<ScanContext> BuildContainer()
        {
            var container = new WorkBuilder<ScanContext>()
                .Use<PublishNotificationMiddleware>()     // 发布
                .Use<HeartBeatMiddleware>()               // 心跳
                .Use<MaintainMiddleware>()                // 维护

            #region 具体业务中间件
                .Use<HandleStation12DMiddleware>()
                .Use<HandleStation2DSpotMiddleware>()
            #endregion

                .Use<FlushPendingMiddleware>()
                .Build();

            return container;
        }

        public async Task HandleAsync(ScanContext ctx)
        {
            var workcontainer = BuildContainer();
            await workcontainer.Invoke(ctx);
        }

    }
}
