﻿namespace Catl.MesInvocation.CatlMesParams
{
    /// <summary>
    /// CATL MES 配置基类
    /// </summary>
    public class CatlMesConfigurationBase
    {
        public virtual string Site { get; set; } = "";
        public virtual string User { get; set; } = "";

        public virtual string Operation { get; set; } = "";
        public virtual string OperationRevision { get; set; } = "";
    }
}
