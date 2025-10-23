namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// 装配接口2配置参数
    /// </summary>
    public class AssembleComponentsToSfcConfig
    {
        public CatlMesConnectionParams ConnectionParams { get; set; } = new CatlMesConnectionParams();
        public AssembleComponentForSfcParams InterfaceParams { get; set; } = new AssembleComponentForSfcParams();
    }
    public class AssembleComponentForSfcParams : CatlMesConfigurationBase
    {
        public string Resource { get; set; } = "";
        public string Activity { get; set; } = "";
    }
}
