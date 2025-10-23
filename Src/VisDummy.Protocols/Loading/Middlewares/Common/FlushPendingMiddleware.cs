using Itminus.Middlewares;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Itminus.Protocols.Loading.Middlewares
{
    public class FlushPendingMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly ILogger<FlushPendingMiddleware> _logger;
        private readonly LoadingFlusher _flusher;

        public FlushPendingMiddleware(ILogger<FlushPendingMiddleware> logger, LoadingFlusher flusher)
        {
            this._logger = logger;
            this._flusher = flusher;
        }

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                await this._flusher.FlushAsync(context.MstMsg);
            }
            finally
            {
                await next(context);
            }
        }
    }

}
