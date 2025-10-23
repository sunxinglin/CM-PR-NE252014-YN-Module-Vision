using Itminus.Middlewares;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.人工位.Middlewares.Common
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
