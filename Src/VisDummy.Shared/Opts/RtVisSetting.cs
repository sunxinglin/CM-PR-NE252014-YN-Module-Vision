namespace VisDummy.Shared.Opts
{

    /// <summary>
    /// Vision 视图设置
    /// </summary>
    public class RtVisSetting
    {
        /// <summary>
        /// 可见性
        /// </summary>
        public bool Visibility { get; set; }

        /// <summary>
        /// 手动触发使能
        /// </summary>
        public string ProcName { get; set; } = string.Empty;
    }
}
