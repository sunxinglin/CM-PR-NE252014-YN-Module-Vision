using MediatR;
using Microsoft.Extensions.Hosting;
using Splat;
using StdUnit.One.Shared;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using TApp.ViewModels;
using static TApp.BackgroundServices.PCInfoAPI;

namespace TApp.BackgroundServices
{
    internal class PCInfoBackgroundService(IMediator mediator) : BackgroundService
    {
        private readonly IMediator _mediator = mediator;
        private readonly AppViewModel _appViewModel = Locator.Current.GetService<AppViewModel>();

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => GetCpuUsage(stoppingToken));
            Task.Run(() => GetDisksUsedRate(stoppingToken));
            Task.Run(() => GetMemoryStatus(stoppingToken));

            return Task.CompletedTask;
        }

        public void GetCpuUsage(CancellationToken stoppingToken)
        {
            using var counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            while (!stoppingToken.IsCancellationRequested)
            {
                _appViewModel.CpuUsageSub.OnNext(Math.Round(counter.NextValue(), 2));
                _ = counter.NextValue();
                Thread.Sleep(1000); // 等待1秒
            }
        }

        public void GetDisksUsedRate(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var str = "";
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.DriveType == DriveType.Fixed)
                    {
                        double total = drive.TotalSize / 1024 / 1024 / 1024;
                        double totalfreespace = drive.TotalFreeSpace / 1024 / 1024 / 1024;
                        var a = (totalfreespace / total) * 100.0;
                        str += $"{drive.Name.Replace("\\", "")}{Math.Round((totalfreespace / total) * 100, 2)}% ";
                        if (Math.Round((totalfreespace / total) * 100, 2) < 10)
                        {
                            _mediator.Publish(new UILogNotification(new AlarmMessage
                            {
                                Level = LogLevel.Error,
                                EventSource = "",
                                EventGroup = "",
                                Content = $"{drive.Name}空间不足10%",
                                Timestamp = DateTime.Now,
                            }));
                        }
                    }
                }
                _appViewModel.DiskUsageSub.OnNext(str);

                Thread.Sleep(1000); // 等待1秒
            }
        }

        public void GetMemoryStatus(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                MEMORY_INFO mi = new();
                mi.dwLength = (uint)Marshal.SizeOf(mi);
                GlobalMemoryStatusEx(ref mi);
                _appViewModel.MemoryUsageSub.OnNext(mi.dwMemoryLoad);
                Thread.Sleep(1000); // 等待1秒
            }
        }


    }
}
