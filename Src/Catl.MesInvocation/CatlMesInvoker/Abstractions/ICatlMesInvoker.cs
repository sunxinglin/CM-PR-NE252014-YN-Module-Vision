using Catl.WebServices.MachineIntegrationServiceService;
using Catl.WebServices.MiAssembleComponentsToSfcsServiceService;
using Catl.WebServices.MiCheckInventoryAttributesServiceService;
using Catl.WebServices.MiCustomDCForCellServiceService;
using Catl.WebServices.MiFindCustomAndSfcDataServiceService;
using Catl.WebServices.MiReleaseSfcWithActivityServiceService;
using MiSFCAttriDataEntryServiceService;

namespace Catl.MesInvocation.CatlMesInvoker
{
    /// <summary>
    /// 调用CATL WS，接收的参数和返回的类型和WSDL相关。并自动按C的要求记录Excel日志
    /// </summary>
    public interface ICatlMesInvoker
    {
        /// <summary>
        /// 电芯检查
        /// </summary>
        /// <param name="inventoryArray">电芯码</param>
        /// <returns></returns>
        Task<checkInventoryAttributesResponse> CheckInventoryAttributesAsync(string[] inventoryArray);
        /// <summary>
        /// 释放模组码
        /// </summary>
        /// <returns></returns>
        Task<releaseSfcWithActivityResponse> ReleaseSfcByShoporderAsync(string shoporder);
        /// <summary>
        /// 收数出站
        /// </summary>
        /// <param name="sfc">模组码</param>
        /// <param name="updateParams">数据收集组</param>
        /// <returns></returns>
        Task<sfcDcExResponse> DataCollectForSfcExAsync(string sfc, Action<IList<machineIntegrationParametricData>> updateParams);
        /// <summary>
        /// 组装扣料
        /// </summary>
        /// <param name="sfc">模组码</param>
        /// <param name="inventoryArray">电芯码</param>
        /// <returns></returns>
        Task<assembleComponentsToSfcsResponse> AssembleComponentsToSfcsAsync(string sfc, inventoryData[] inventoryArray);
        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="sfc">模组码</param>
        /// <returns></returns>
        Task<findCustomAndSfcDataResponse> FindCustomAndSfcDataAsync(string sfc);
        /// <summary>
        /// Ocv检查
        /// </summary>
        /// <param name="updateParams"></param>
        /// <returns></returns>
        Task<miCustomDCForCellResponse> MiCustomDCForCellAsync(Action<IList<miCustomDCForCellInventory>> updateParams);

        /// <summary>
        /// 首件测试
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="updateParams"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<Catl.WebServices.DataCollectForResourceFAI.machineIntegrationResourceDcResponse> DataCollectForResourceFAIAsync(string sfc, Action<IList<Catl.WebServices.DataCollectForResourceFAI.machineIntegrationParametricData>> updateParams);

        /// <summary>
        /// 装配检查
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="barCodes"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<miSFCAttriDataEntryResponse> MiSFCAttriDataEntryAsync(string sfc, string[] barCodes);
    }
}
