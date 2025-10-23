using System.Security.Claims;

namespace TApp.Auth;


public interface IPrincipalAccessor
{
    ClaimsPrincipal GetCurrentPrincipal();
    void SetCurrentPrincipal(ClaimsPrincipal principal);
}
