using Catl.WebServices.MiCheckInventoryAttributesServiceService;

namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// 电芯检查配置参数
    /// </summary>
    public class CheckInventoryParams : CatlMesConfigurationBase
    {
        public string Sfc { get; set; } = "";

        public modeCheckInventory Mode { get; set; }

        public string Resource { get; set; } = "";

        public string ActivityId { get; set; } = "";
    }

    public class CheckInventoryAttributeConfig
    {
        public CatlMesConnectionParams ConnectionParams { get; set; } = new CatlMesConnectionParams();
        public CheckInventoryParams InterfaceParams { get; set; } = new CheckInventoryParams();
    }

}
