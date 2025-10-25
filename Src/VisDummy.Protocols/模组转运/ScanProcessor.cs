using Itminus.Middlewares;
using VisDummy.Protocols.模组转运.Middlewares;
using VisDummy.Protocols.模组转运.Middlewares.Common;
using VisDummy.Protocols.模组转运.Middlewares.Common.PublishNotification;

namespace VisDummy.Protocols.模组转运
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
                .Use<HandleCAM1Middleware>()
				.Use<HandleCAM4Middleware>()
				.Use<HandleCAM2Middleware>()
				.Use<HandleCAM3Middleware>()
				//.Use<HandleStation2DSpotMiddleware>()
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
