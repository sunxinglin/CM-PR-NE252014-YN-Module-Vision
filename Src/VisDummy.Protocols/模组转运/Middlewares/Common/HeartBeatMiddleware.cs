using Itminus.Middlewares;
using VisDummy.Protocols.Common.Model;
using VisDummy.Protocols.模组转运;

namespace VisDummy.Protocols.模组转运.Middlewares.Common
{

    public class HeartBeatMiddleware : IWorkMiddleware<ScanContext>
    {
        public HeartBeatMiddleware()
        {
        }

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            context.MstMsg.Heart.CmdFlags = new MstMsg_CmdFlagsBuilder(context.MstMsg.Heart.CmdFlags)
                .SetHeartBeatOnOff(context.DevMsg.Heart.HasHeartBeat)
                .Build();
            await next(context);
        }
    }
}
