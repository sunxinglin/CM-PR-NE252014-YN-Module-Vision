using Microsoft.FSharp.Core;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Warp;

namespace VisDummy.Abstractions.Infra
{
    public interface IMKProc
    {
        /// <summary>
        /// 正常拍照
        /// <para/>
        /// 103=>101=>110
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<FSharpResult<StationOkWrap_MK, StationErrWrap_MK>> ProcAsync(StationArgs_MK args);

        /// <summary>
        /// 标定球拍照
        /// <para/>
        /// 103=>101=>102
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<FSharpResult<StationOkWrap_MK, StationErrWrap_MK>> Proc3DSpotCheckAsync(StationArgs_MK args);

        /// <summary>
        /// 标定球生成
        /// <para/>
        /// 101=>102
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<FSharpResult<StationOkWrap_MK, StationErrWrap_MK>> Proc3DSpotCheckResultAsync(StationArgs_MK args);
    }
}
