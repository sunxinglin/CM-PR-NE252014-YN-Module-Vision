using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using TApp.Apis;
using VisDummy.Shared.Opts;
using VisDummy.Shared.Utils;

namespace TApp.Auth
{
    public static class AuthApisServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthApis(this IServiceCollection services)
        {
            services.AddSingleton<ApiKeyAuthHeaderHandler>();
            services.AddSingleton<IPrincipalAccessor, ClaimsPrincipalAccessor>();


            var serializeSettings = new JsonSerializerSettings { };
            serializeSettings.Converters.Add(new ClaimConverter());

            services.AddTransient<ApiKeyAuthHeaderHandler>();

            services.AddRefitClient<IUserMgmtApi>(new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer(serializeSettings),
                })
                .ConfigureHttpClient((sp,c) => {
                    var opts = sp.GetRequiredService<IOptions<ApiServerSetting>>();
                    var settings = opts.Value;
                    c.BaseAddress = new Uri(settings.BaseUrl);
                })
                ;

            services
                .AddRefitClient<ICloudApi>(new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer(serializeSettings)
                })
                .ConfigureHttpClient((sp, c) =>
                {
                    var opts = sp.GetRequiredService<IOptions<ApiServerSetting>>();
                    var settings = opts.Value;
                    c.BaseAddress = new Uri(settings.BaseUrl);
                })
                .AddHttpMessageHandler<ApiKeyAuthHeaderHandler>();

            return services;
        }
    }
}
