using System.Security.Claims;

namespace TApp.Auth;

internal class ClaimsPrincipalAccessor : IPrincipalAccessor
{
    private ClaimsPrincipal _principal;


    public ClaimsPrincipal GetCurrentPrincipal()
    {
        return _principal;
    }


    public void SetCurrentPrincipal(ClaimsPrincipal principal)
    {
        this._principal = principal;
    }
}
