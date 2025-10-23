using Itminus.Middlewares;
using Itminus.Protocols.Loading.Middlewares;
using VisDummy.Protocols.Loading.Middlewares;


namespace Itminus.Protocols.Loading
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
                .Use<HandleStation2DMiddleware>()
                .Use<HandleStation2DSpotMiddleware>()
                .Use<HandleStation3DMiddleware>()
                .Use<HandleStation3DSpotMiddleware>()
            #endregion

                .Use<FlushPendingMiddleware>()
                .Build();

            return container;
        }

        public async Task HandleAsync(ScanContext ctx)
        {
            var workcontainer = this.BuildContainer();
            await workcontainer.Invoke(ctx);
        }

    }
}
