using MediatR;
using Microsoft.Extensions.Hosting;
using VisDummy.Abstractions.Infra;
using VisDummy.Protocols.Vision.Models;

namespace VisDummy.Protocols.Vision
{
    public class CameraStatusBackgroundService : BackgroundService
    {
        private readonly IVisProc _visProc;
        private readonly IMediator _mediator;

        public CameraStatusBackgroundService(IVisProc visProc, IMediator mediator)
        {
            _visProc = visProc;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunAsync(stoppingToken);
        }


        public Task RunAsync(CancellationToken stoppingToken)
        {
            Thread thread = new((ThreadStart)async delegate
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var s = await _visProc.GetCameraStatus();
                        if (s.IsError)
                        {
                            return;
                        }

                        await _mediator.Publish(new VisionStatusNotification { CameraStatus = s.ResultValue });
                    }
                    catch (Exception ex)
                    {
                        _ = ex;
                    }
                    finally
                    {
                        await Task.Delay(5000, stoppingToken);
                        //await _mediator.Publish(new VisionStatusNotification { CameraStatus = new Dictionary<string, bool> { { "全局相机1", true } } });
                    }
                }
            });
            thread.Start();
            return Task.CompletedTask;
        }



    }
}
