using Itminus.Middlewares;
using Microsoft.Extensions.Logging;
using VisDummy.Protocols.侧板自动拧紧;

namespace VisDummy.Protocols.侧板自动拧紧.Middlewares.Common
{
    public class FlushPendingMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly ILogger<FlushPendingMiddleware> _logger;
        private readonly 侧板自动拧紧Flusher _flusher;

        public FlushPendingMiddleware(ILogger<FlushPendingMiddleware> logger, 侧板自动拧紧Flusher flusher)
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
