using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TApp.Auth;

class ClaimConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Claim));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        string type = (string)jo["type"]!;
        string value = (string)jo["value"]!;
        string valueType = (string)jo["valueType"];
        string issuer = (string)jo["issuer"];
        string originalIssuer = (string)jo["originalIssuer"];
        return new Claim(type, value, valueType, issuer, originalIssuer);
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
