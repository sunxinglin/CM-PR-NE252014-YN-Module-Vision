namespace VisDummy.Shared.Opts
{
    public class Vision3DOpt
    {
        /// <summary>
        /// 可见性
        /// </summary>
        public bool Visibility { get; set; }

        /// <summary>
        /// 流程名
        /// </summary>
        public string ProcName { get; set; } = string.Empty;
        public string IpAddr { get; set; }
        public int Port { get; set; }
    }
}
