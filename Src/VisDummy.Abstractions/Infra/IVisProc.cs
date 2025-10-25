using Microsoft.FSharp.Core;
using VisDummy.Abstractions.Args;
using VisDummy.Abstractions.Calibrations;
using VisDummy.Abstractions.Warp;
using VisDummy.Abstractions.Warp.人工位;
using VisDummy.Abstractions.Warp.侧板自动拧紧;
using VisDummy.Abstractions.Warp.模组转运;
using VisDummy.Abstractions.Warp.水冷板抓取;

namespace VisDummy.Abstractions.Infra
{
    public interface IVisProc
    {
        /// <summary>
        /// 标定
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<FSharpResult<CalibarateNthPointOkWrap, IErr_CalibrateNthPoint>> CalibrateAsync(CalibrateNthPointArgs args);

        /// <summary>
        /// 大包装2D
        /// </summary>
        /// <returns></returns>
        Task<FSharpResult<StationOkWrap_Loading, StationErrWrap_Loading>> LoadingProcAsync(StationArgs args);
        /// <summary>
        /// 大包装点检
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<FSharpResult<SpotStationOkWarp, SpotStationErrWarp>> SpotProcAsync(SpotStationArgs args);

        /// <summary>
        /// 模组检测2D
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<FSharpResult<StationOkWrap_模组检测, StationErrWrap_模组检测>> 模组检测ProcAsync(StationArgs args);

        /// <summary>
        /// 模组贴标2D
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<FSharpResult<StationOkWrap_模组贴标, StationErrWrap_模组贴标>> 模组贴标ProcAsync(StationArgs args);

        /// <summary>
        /// 垫片检测12D
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<FSharpResult<StationOkWrap_垫片检测, StationErrWrap_垫片检测>> 垫片检测1ProcAsync(StationArgs args);

        /// <summary>
        /// 模组入箱12D
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<FSharpResult<StationOkWrap_模组入箱, StationErrWrap_模组入箱>> 模组入箱1ProcAsync(StationArgs args);

        Task<FSharpResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>> 水冷板抓取定位引导1ProcAsync(StationArgs args);
        
        Task<FSharpResult<定位引导OkWrap_水冷板抓取, 定位引导ErrWrap_水冷板抓取>> 水冷板抓取定位引导2ProcAsync(StationArgs args);

        Task<FSharpResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>> 水冷板抓取校验1ProcAsync(StationArgs args);
        
        Task<FSharpResult<校验OkWrap_水冷板抓取, 校验ErrWrap_水冷板抓取>> 水冷板抓取校验2ProcAsync(StationArgs args);

        Task<FSharpResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>> 水冷板抓取自动校准1ProcAsync(StationArgs args);
        
        Task<FSharpResult<自动校准OkWrap_水冷板抓取, 自动校准ErrWrap_水冷板抓取>> 水冷板抓取自动校准2ProcAsync(StationArgs args);

        Task<FSharpResult<水冷板检测OkWrap_人工位, 水冷板检测ErrWrap_人工位>> 人工位水冷板检测ProcAsync(StationArgs args);

        Task<FSharpResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>> 机器人1相机ProcAsync(StationArgs args, CamModelInput model);

        Task<FSharpResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>> 机器人2相机ProcAsync(StationArgs args, CamModelInput model);

        Task<FSharpResult<StationOkWrap_侧板自动拧紧, StationErrWrap_侧板自动拧紧>> 机器人3相机ProcAsync(StationArgs args, CamModelInput model);

		Task<FSharpResult<StationOkWrap_模组转运, StationErrWrap_模组转运>> 模组转运机器人1相机ProcAsync(StationArgs args, CamModelInput model);

		Task<FSharpResult<StationOkWrap_模组转运, StationErrWrap_模组转运>> 模组转运机器人2相机ProcAsync(StationArgs args, CamModelInput model);

		Task<FSharpResult<StationOkWrap_模组转运, StationErrWrap_模组转运>> 模组转运机器人3相机ProcAsync(StationArgs args, CamModelInput model);

		Task<FSharpResult<Dictionary<string, bool>, string>> GetCameraStatus();
    }
}
