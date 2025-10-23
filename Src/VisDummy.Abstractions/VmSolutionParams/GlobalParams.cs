namespace VisDummy.Abstractions.VmSolutionParams
{
    public enum SolutionParamEnum
    {
        /// <summary>
        /// 单次
        /// </summary>
        Single = 0,
        /// <summary>
        /// 触发写入
        /// </summary>
        Trigger = 1,
        /// <summary>
        /// 定时
        /// </summary>
        Timing = 2
    }
    public class GlobalParams
    {
        public IList<SolutionParam> SolutionParams = [];
    }

    public class SolutionParam
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";
        public SolutionParamEnum Type { get; set; } = SolutionParamEnum.Single;
    }
}
