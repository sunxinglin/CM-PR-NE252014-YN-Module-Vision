using Itminus.Middlewares;
using Microsoft.Extensions.Logging;
using VisDummy.Protocols.模组转运;

namespace VisDummy.Protocols.模组转运.Middlewares.Common
{
    public class FlushPendingMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly ILogger<FlushPendingMiddleware> _logger;
        private readonly 模组转运Flusher _flusher;

        public FlushPendingMiddleware(ILogger<FlushPendingMiddleware> logger, 模组转运Flusher flusher)
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
