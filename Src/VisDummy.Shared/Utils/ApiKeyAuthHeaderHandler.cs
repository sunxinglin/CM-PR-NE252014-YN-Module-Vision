using System.Text;
using Microsoft.Extensions.Options;
using VisDummy.Shared.Opts;

namespace VisDummy.Shared.Utils;


/// <summary>
/// 使用ApiKey方式进行认证
/// </summary>
public class ApiKeyAuthHeaderHandler : DelegatingHandler
{
    private readonly IOptionsMonitor<ApiServerSetting> _apiServerSetting;

    public ApiKeyAuthHeaderHandler(IOptionsMonitor<ApiServerSetting> optsMonitor)
    {
        this._apiServerSetting = optsMonitor;
    }

    protected virtual (string scheme, string token) CreateApiKeyCredential()
    {
        var settings = this._apiServerSetting.CurrentValue;
        var credentials = $"{settings.ApiKeyIdentifier}:{settings.ApiKey}";
        var bytes = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
        return (settings.ApiKeyPrefix, bytes);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var (scheme, token) = this.CreateApiKeyCredential();
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, token);
        return base.SendAsync(request, cancellationToken);
    }


}