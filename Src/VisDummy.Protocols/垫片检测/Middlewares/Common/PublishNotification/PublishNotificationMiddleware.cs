﻿using Itminus.Middlewares;
using MediatR;
using VisDummy.Protocols.垫片检测;

namespace VisDummy.Protocols.垫片检测.Middlewares.Common.PublishNotification
{
    public class PublishNotificationMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly IMediator _mediator;

        public PublishNotificationMiddleware(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                await _mediator.Publish(new ScanContextNotification(context));
            }
            finally
            {
                await next(context);
            }
        }
    }
}
