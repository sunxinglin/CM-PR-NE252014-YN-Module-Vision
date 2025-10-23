using System.ServiceModel;

namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// CATL MES 连接参数
    /// </summary>
    public class CatlMesConnectionParams
    {
        public string Url { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public int Timeout { get; set; } = default;
        public BasicHttpSecurityMode BasicHttpSecurityMode { get; set; }
    }
}
