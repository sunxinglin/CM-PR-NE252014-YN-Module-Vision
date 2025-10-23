namespace Catl.MesInvocation
{
    public class CatlMesOpt
    {
        /// <summary>
        /// Ini配置文件路径
        /// </summary>
        public string IniFileDir { get; set; } = string.Empty;
        /// <summary>
        /// Ini配置文件路径
        /// </summary>
        public string IniFileName { get; set; } = "MESCFG.ini";
        /// <summary>
        /// 检查电芯
        /// </summary>
        public string MiCheckInventoryAttributesInterfaceName { get; set; } = "MiCheckInventoryAttributesInterface";
        /// <summary>
        /// 释放模组号
        /// </summary>
        public string MiReleaseSfcInterfaceName { get; set; } = "MiReleaseSfcInterface";
        /// <summary>
        /// 收数
        /// </summary>
        public string DataCollectForSfcExInterfaceName { get; set; } = "DataCollectForSfcExInterface";
        /// <summary>
        /// 组装
        /// </summary>
        public string MiAssembleComponentsForSfcsInterfaceName { get; set; } = "MiAssembleComponentsForSfcsInterface";
        /// <summary>
        /// 进站
        /// </summary>
        public string MiFindCustomAndSfcDataInterfaceName { get; set; } = "MiFindCustomAndSfcDataInterface";
        /// <summary>
        /// OCV
        /// </summary>
        public string MiCustomDCForCellConfigInterfaceName { get; set; } = "MiCustomDCForCellConfigInterface";
        /// <summary>
        /// 首件
        /// </summary>
        public string DataCollectForResourceFAIInterfaceName { get; set; } = "DataCollectForResourceFAIInterface";
        /// <summary>
        /// 电芯装配检查
        /// </summary>
        public string MiSFCAttriDataEntryInterfaceName { get; set; } = "MiSFCAttriDataEntryInterface";
    }
}
