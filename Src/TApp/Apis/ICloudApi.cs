using Refit;
using StdUnit.Zero.Shared;
using TApp.Apis.Models;

namespace TApp.Apis;


public interface ICloudApi
{
    //[Post("/api/Log/OperationLog/AddLog")]
    //Task<Resp<object>> AddOperationLog(string operatorName, string record);

    [Post("/api/SystemLog/Add")]
    Task<Resp<bool>> AddLog(LogAddRequest request);

    [Get("/api/SystemLog/Pagination")]
    Task<LogPaginationResponse> LogPagination(string source, string group, LogLevel? level, string content, DateTime? startTime, DateTime? endTime, int current = 1, int pageSize = 20);
}