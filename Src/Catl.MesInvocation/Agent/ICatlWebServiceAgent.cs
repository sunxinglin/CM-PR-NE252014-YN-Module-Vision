using Catl.WebServices.MiCustomDCForCellServiceService;
using Microsoft.FSharp.Core;



namespace Catl.MesInvocation.Agent
{
    /// <summary>
    /// 调用CATL WS的接口。
    /// 1. 只需要传递关键参数，其他参数从配置文件获取
    /// 2. 自动记录日志到特定的Excel
    /// </summary>
    public interface ICatlWebServiceAgent
    {
        /// <summary>
        /// 检查电芯码
        /// </summary>
        /// <param name="barcodes"></param>
        /// <returns></returns>
        Task<FSharpResult<ValueTuple, (int, string)>> CheckInBatchAsync(string[] barcodes);

        /// <summary>
        /// 释放模组号
        /// </summary>
        /// <param name="shoporder"></param>
        /// <returns></returns>
        Task<FSharpResult<string, (int, string)>> ReleaseSfcAsync(string shoporder);

        /// <summary>
        /// 装配模组
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="inventoryArray"></param>
        /// <returns></returns>
        Task<FSharpResult<ValueTuple, (int, string)>> AssembleComponentToSfcAsync(string sfc, string[] inventoryArray);

        /// <summary>
        /// 收数
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="updateParams"></param>
        /// <returns></returns>
        Task<FSharpResult<ValueTuple, (int, string)>> DataCollectForSfcAsync(string sfc, Action<IList<Catl.WebServices.MachineIntegrationServiceService.machineIntegrationParametricData>> updateParams);

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<FSharpResult<string, (int, string)>> MiFindCustomSfcAsync(string sfc);
        /// <summary>
        /// OCV检测
        /// </summary>
        /// <param name="updateParams"></param>
        /// <returns></returns>
        Task<FSharpResult<ValueTuple, (int, string)>> OCVcheckAsync(Action<IList<miCustomDCForCellInventory>> updateParams);
        /// <summary>
        /// 首件测试
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="updateParams"></param>
        /// <returns></returns>
        Task<FSharpResult<ValueTuple, (int, string)>> DataCollectForResourceFAIAsync(string sfc, Action<IList<WebServices.DataCollectForResourceFAI.machineIntegrationParametricData>> updateParams);

    }
}
