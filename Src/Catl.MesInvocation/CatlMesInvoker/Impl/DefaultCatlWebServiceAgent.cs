using Catl.MesInvocation.Agent;
using Catl.WebServices.MiCustomDCForCellServiceService;
using Itminus.FSharpExtensions;
using Microsoft.FSharp.Core;

namespace Catl.MesInvocation.CatlMesInvoker
{
    public class DefaultCatlWebServiceAgent : ICatlWebServiceAgent
    {
        private readonly ICatlMesInvoker _mesInvoker;

        public DefaultCatlWebServiceAgent(ICatlMesInvoker mesInvoker)
        {
            this._mesInvoker = mesInvoker;
        }


        public async Task<FSharpResult<ValueTuple, (int, string)>> CheckInBatchAsync(string[] barcodes)
        {

            var s = await this._mesInvoker.CheckInventoryAttributesAsync(barcodes);
            if (s.code == 0)
            {
                return new ValueTuple().ToOkResult<ValueTuple, (int, string)>();
            }
            var res = await CheckOneByOneAsync(barcodes);
            return (s.code, s.message).ToErrResult<ValueTuple, (int, string)>();
        }
        /// <summary>
        /// 电芯逐个审查
        /// </summary>
        /// <param name="barcodes">电芯码</param>
        /// <returns></returns>
        private async Task<IList<(int, string)>> CheckOneByOneAsync(string[] barcodes)
        {
            var result = new List<(int, string)>();
            foreach (var barcode in barcodes)
            {
                var res = await this._mesInvoker.CheckInventoryAttributesAsync([barcode]);
                var bc = (res.code, res.message);
                result.Add(bc);
            }
            return result;
        }

        public async Task<FSharpResult<string, (int, string)>> ReleaseSfcAsync(string shoporder)
        {
            var s = await this._mesInvoker.ReleaseSfcByShoporderAsync(shoporder);
            if (s.code == 0)
            {
                var sfc = s.sfcArray.FirstOrDefault()?.sfc ?? "";
                return FSharpResult<string, (int, string)>.NewOk(sfc);
            }
            return FSharpResult<string, (int, string)>.NewError((s.code, s.message));
        }

        public async Task<FSharpResult<ValueTuple, (int, string)>> DataCollectForSfcAsync(string sfc, Action<IList<Catl.WebServices.MachineIntegrationServiceService.machineIntegrationParametricData>> updateParams)
        {
            var s = await this._mesInvoker.DataCollectForSfcExAsync(sfc, updateParams);
            if (s.code == 0)
            {
                return new ValueTuple().ToOkResult<ValueTuple, (int, string)>();
            }
            else
            {
                return (s.code, s.message).ToErrResult<ValueTuple, (int, string)>();
            }
        }

        public async Task<FSharpResult<ValueTuple, (int, string)>> AssembleComponentToSfcAsync(string sfc, string[] inventoryArray)
        {
            var data = inventoryArray.Select(i => new Catl.WebServices.MiAssembleComponentsToSfcsServiceService.inventoryData
            {
                inventory = i,
                qty = "1",
            }).ToArray();
            var r = await this._mesInvoker.AssembleComponentsToSfcsAsync(sfc, data);
            if (r.code == 0)
            {
                return new ValueTuple().ToOkResult<ValueTuple, (int, string)>();
            }
            else
            {
                return (r.code, r.message).ToErrResult<ValueTuple, (int, string)>();
            }
        }

        public async Task<FSharpResult<string, (int, string)>> MiFindCustomSfcAsync(string sfc)
        {
            var s = await this._mesInvoker.FindCustomAndSfcDataAsync(sfc);
            if (s.code == 0)
            {
                return FSharpResult<string, (int, string)>.NewOk(sfc);
            }
            return FSharpResult<string, (int, string)>.NewError((s.code, s.message));
        }

        public async Task<FSharpResult<ValueTuple, (int, string)>> OCVcheckAsync(Action<IList<miCustomDCForCellInventory>> updateParams)
        {
            var s = await this._mesInvoker.MiCustomDCForCellAsync(updateParams);
            if (s.code == 0)
            {
                return FSharpResult<ValueTuple, (int, string)>.NewOk(new ValueTuple());
            }
            return (s.code, s.message).ToErrResult<ValueTuple, (int, string)>();
        }

        public async Task<FSharpResult<ValueTuple, (int, string)>> DataCollectForResourceFAIAsync(string sfc, Action<IList<WebServices.DataCollectForResourceFAI.machineIntegrationParametricData>> updateParams)
        {
            var s = await this._mesInvoker.DataCollectForResourceFAIAsync(sfc, updateParams);
            if (s.code == 0)
            {
                return FSharpResult<ValueTuple, (int, string)>.NewOk(new ValueTuple());
            }
            return (s.code, s.message).ToErrResult<ValueTuple, (int, string)>();
        }
    }
}
