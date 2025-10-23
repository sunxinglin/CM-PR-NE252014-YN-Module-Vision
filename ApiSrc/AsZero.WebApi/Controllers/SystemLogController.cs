using AsZero.WebApi.Models.DBModels;
using AsZero.WebApi.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StdUnit.Zero.DAL;
using StdUnit.Zero.Shared;

namespace AsZero.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemLogController : ControllerBase
    {
        private readonly AsZeroDbContext _dbcontext;

        public SystemLogController(AsZeroDbContext context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        public async Task<object> Pagination(string? source, string? group, LogLevel? level, string? content, DateTime? startTime, DateTime? endTime, int current = 1, int pageSize = 20)
        {
            var query = _dbcontext.Set<SystemLog>().AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(source))
            {
                query = query.Where(e => e.EventSource.Contains(source));
            }
            if (!string.IsNullOrWhiteSpace(group))
            {
                query = query.Where(e => e.EventGroup.Contains(group));
            }
            if (level.HasValue)
            {
                query = query.Where(e => e.Level == level);
            }
            if (!string.IsNullOrWhiteSpace(content))
            {
                query = query.Where(e => e.Content.Contains(content));
            }
            if (startTime.HasValue)
            {
                query = query.Where(e => e.CreateTime >= startTime);
            }
            if (endTime.HasValue)
            {
                query = query.Where(e => e.CreateTime <= endTime);
            }

            var total = await query.CountAsync();
            var list = await query.OrderByDescending(e => e.Id)
                .Skip((current - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new { Total = total, Current = current, PageSize = pageSize, List = list };
        }

        [HttpPost]
        public async Task<Resp<bool>> Add(LogAddRequest request)
        {
            await _dbcontext.AddAsync(new SystemLog
            {
                EventSource = request.EventSource,
                EventGroup = request.EventGroup,
                Level = request.Level,
                Content = request.Content,
                CreateUser = request.Operator,
                CreateTime = DateTime.Now,
            });
            await _dbcontext.SaveChangesAsync();

            return new Resp<bool> { Data = true, Success = true };
        }
    }
}
