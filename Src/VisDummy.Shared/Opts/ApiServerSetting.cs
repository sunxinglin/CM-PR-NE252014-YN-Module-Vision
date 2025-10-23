using System;

namespace VisDummy.Shared.Opts;


public class ApiServerSetting
{
    /// <summary>
    /// cloud BaseURL
    /// </summary>
    public string BaseUrl { get; set; } = "http://localhost:5000";

    public string ApiKeyPrefix { get; set; } = "itminus.Key";

    public string ApiKeyIdentifier { get; set; } = "";

    /// <summary>
    /// Api Key to Access Cloud Resource
    /// </summary>
    public string ApiKey { get; set; } = "";


    public string DefaultOperatorAccount { get; set; } = "Operator";
    public string DefaultOperatorPassword { get; set; } = "123456";
}
