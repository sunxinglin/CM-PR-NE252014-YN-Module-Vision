using Itminus.Middlewares;
using VisDummy.Protocols.水冷板抓取.Middlewares;
using VisDummy.Protocols.水冷板抓取.Middlewares.Common;
using VisDummy.Protocols.水冷板抓取.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.水冷板抓取
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
                .Use<Handle工位1定位引导Middleware>()
                .Use<Handle工位2定位引导Middleware>()
                .Use<Handle工位1校验Middleware>()
                .Use<Handle工位2校验Middleware>()
                .Use<Handle工位1自动校准Middleware>()
                .Use<Handle工位2自动校准Middleware>()
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
