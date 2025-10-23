using Microsoft.FSharp.Core;
using VisDummy.Abstractions.VmSolutionParams;

namespace VisDummy.Abstractions.Infra
{
    public interface IVisParams
    {
        /// <summary>
        /// 设定VM全局变量
        /// </summary>
        /// <returns></returns>
        Task<FSharpResult<string[], string>> SetTriggerGlobalParams();
        /// <summary>
        /// 设定VM全局变量
        /// </summary>
        /// <returns></returns>
        Task<FSharpResult<string[], string>> SetSingleGlobalParams();
        /// <summary>
        /// 设定VM全局变量
        /// </summary>
        /// <returns></returns>
        Task<FSharpResult<string[], string>> SetTimingGlobalParams();
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        Task<FSharpResult<GlobalParams, string>> GetGlobalParams();
        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="globalParams"></param>
        /// <returns></returns>
        Task<FSharpResult<GlobalParams, string>> SaveGlobalParams(GlobalParams globalParams);
    }
}
