using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using StdUnit.One.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.VmSolutionParams;
using VisDummy.Lang.Resources;
using VisDummy.Shared.LogGroup;
using Unit = System.Reactive.Unit;

namespace VisDummy.VMs.ViewModels
{
    public class GlobalParamsViewModel : ReactiveObject
    {
        private readonly IServiceScopeFactory _ssf;
        private readonly IMediator _mediator;

        public GlobalParamsViewModel(IServiceScopeFactory ssf, IMediator mediator)
        {
            this._ssf = ssf;
            this._mediator = mediator;
            this.CmdReload = ReactiveCommand.CreateFromTask(CmdReload_Impl);
            this.CmdReload.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message));
            this.CmdSave = ReactiveCommand.CreateFromTask(CmdSave_Impl);
            this.CmdSave.ThrownExceptions.Subscribe(e => MessageBox.Show(e.Message));
        }

        [Reactive]
        public IList<SolutionParam> SolutionParams { get; set; } = [new()];

        public ReactiveCommand<Unit, Unit> CmdReload { get; }
        public ReactiveCommand<Unit, Unit> CmdSave { get; }
        private async Task<Unit> CmdReload_Impl()
        {
            using IServiceScope scope = _ssf.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;
            var _visparams = serviceProvider.GetRequiredService<IVisParams>();
            var f = await _visparams.GetGlobalParams();
            if (f.IsError)
                MessageBox.Show(f.ErrorValue);
            else
                this.SolutionParams = f.ResultValue.SolutionParams;

            return Unit.Default;
        }
        private async Task<Unit> CmdSave_Impl()
        {
            using IServiceScope scope = _ssf.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;
            var _visparams = serviceProvider.GetRequiredService<IVisParams>();
            var global = new GlobalParams { SolutionParams = this.SolutionParams };
            var f = await _visparams.SaveGlobalParams(global);
            if (f.IsError)
                MessageBox.Show(f.ErrorValue);
            else
            {
                MessageBox.Show(Language.Msg_保存成功);
                await RecordLogAsync($"{Language.Msg_全局参数变动}:{JsonConvert.SerializeObject(global)},{Language.Msg_修改前}:{JsonConvert.SerializeObject(f.ResultValue)}");
            }
            return Unit.Default;
        }
        public async Task RecordLogAsync(string logmsg)
        {
            logmsg = $"{LogGroupName.OperationLog}：{logmsg}";
            var notification = new UILogNotification(new LogMessage
            {
                EventSource = LogGroupName.OperationLog,
                EventGroup = LogGroupName.OperationLog,
                Level = LogLevel.Critical,
                Content = logmsg,
                Timestamp = DateTime.Now,
            });
            await _mediator.Publish(notification);
        }
    }
}
