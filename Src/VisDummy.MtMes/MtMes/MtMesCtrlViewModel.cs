using Catl.MesInvocation.Agent;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace VisDummy.MtMes.MtMes
{
    public class MtMesCtrlViewModel : ReactiveObject
    {
        private readonly IServiceScopeFactory _ssf;

        public MtMesCtrlViewModel(IServiceScopeFactory ssf)
        {
            this._ssf = ssf;
            this.CmdDataCollectForResourceFAI = ReactiveCommand.CreateFromTask(CmdDataCollectForResourceFAI_Impl);
        }
        #region CATL MES
        [Reactive]
        public string PackCode_CMes { get; set; }
        [Reactive]
        public string CatlMesResponse { get; set; }

        public ReactiveCommand<Unit, Unit> CmdDataCollectForResourceFAI { get; set; }
        public async Task CmdDataCollectForResourceFAI_Impl()
        {
            using IServiceScope scope = _ssf.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;
            var _catlMesInvoker = serviceProvider.GetRequiredService<ICatlWebServiceAgent>();
            var f = await _catlMesInvoker.DataCollectForResourceFAIAsync(this.PackCode_CMes, d =>
            {

            });
            this.CatlMesResponse = JsonConvert.SerializeObject(f);
        }
        #endregion

    }
}
