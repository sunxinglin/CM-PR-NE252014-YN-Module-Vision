using Itminus.Middlewares;
using Microsoft.Extensions.Logging;
using VisDummy.Protocols.垫片检测;

namespace VisDummy.Protocols.垫片检测.Middlewares.Common
{
    public class FlushPendingMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly ILogger<FlushPendingMiddleware> _logger;
        private readonly 垫片检测Flusher _flusher;

        public FlushPendingMiddleware(ILogger<FlushPendingMiddleware> logger, 垫片检测Flusher flusher)
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
