using System.Security.Claims;
using Refit;
using StdUnit.One.Claims;
using StdUnit.One.FuncResources;
using StdUnit.One.Shared;
using StdUnit.One.Users;
using StdUnit.Zero.Shared;

namespace TApp.Auth;


public class CurrentUserDto
{
    public string UserId { get; set; } = "";
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Email { get; set; }
    public string? Title { get; set; }
    public string? Group { get; set; }

    public static readonly CurrentUserDto Empty = new CurrentUserDto() { };

    public IList<string> RoleNames { get; set; } = new List<string>();
    public IList<ClaimItem> Claims { get; set; } = new List<ClaimItem>();
    public IList<FuncResource> FuncResources { get; set; } = new List<FuncResource>();
}

public interface IUserMgmtApi
{

    #region 登录
    [Post("/api/Login/Login")]
    Task<Resp<LoginResultDto>> LoginAsync(LoginParams args);

    [Post("/api/Login/LoginByCard")]
    Task<Resp<LoginResultDto>> LoginAsync(LoginByCard args);

    [Get("/api/Login/Claims")]
    Task<List<ClaimEntity>> LoadMyClaimsAsync();

    [Get("/api/Login/CurrentUser")]
    Task<Resp<CurrentUserDto>> CurrentUserAsync();
    #endregion

    #region 用户管理
    [Get("/api/sys-admin/Account/Users")]
    Task<PagedResp<User>> LoadUsersAsync([AliasAs("current")]int pageIndex, int pageSize);

    [Post("/api/sys-admin/Account/CreateUser")]
    Task<Resp<User>> CreateUserAsync(CreateUserParams args);

    [Post("/api/sys-admin/Account/ChangePassword")]
    Task<Resp<User>> UpdatePasswordAsync(ChangePasswordParams input);

    [Post("/api/sys-admin/Account/DisableAccount")]
    Task<Resp<User>>DisableAccountAsync(string account);

    [Post("/api/sys-admin/Account/RemoveUser")]
    Task<Resp<User>> RemoveUserAsync(string account);
    #endregion

    #region 资源

    [Get("/api/sys-admin/FuncResources/Pages")]
    Task<PagedResp<FuncResource>> PagesAsync(int current, int pageSize, string? uniqueName, int? parentId);

    [Get("/api/sys-admin/FuncResources/Details")]
    Task<Resp<FuncResource>> LoadResouceAsync(int id);

    [Post("/api/sys-admin/FuncResources/UpdatePermissionClaims")]
    Task<Resp<FuncResource>> UpdatePermissionClaimsAsync(int id, IList<ClaimEntity> claims);
    #endregion

    #region 权限
    [Get("/api/sys-admin/Claims/All")]
    Task<IList<Claim>> LoadAllClaimsAsync();
    #endregion
}
