namespace VisDummy.Abstractions.Args
{
    public class StationArgs
    {
        /// <summary>
        /// 功能号
        /// </summary>
        public ushort Function { get; set; }
        /// <summary>
        ///  拍照位置号
        /// </summary>
        public ushort Position { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public uint Batch { get; set; }
        public string ToMsg()
        {
            return $"Function:{Function};Position:{Position};Batch:{Batch};";
        }
    }
}
