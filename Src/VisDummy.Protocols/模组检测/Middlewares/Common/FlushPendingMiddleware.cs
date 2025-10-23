using Itminus.Middlewares;
using Microsoft.Extensions.Logging;

namespace VisDummy.Protocols.模组检测.Middlewares.Common
{
    public class FlushPendingMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly ILogger<FlushPendingMiddleware> _logger;
        private readonly 模组检测Flusher _flusher;

        public FlushPendingMiddleware(ILogger<FlushPendingMiddleware> logger, 模组检测Flusher flusher)
        {
            _logger = logger;
            _flusher = flusher;
        }

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                await _flusher.FlushAsync(context.MstMsg);
            }
            finally
            {
                await next(context);
            }
        }
    }

}
