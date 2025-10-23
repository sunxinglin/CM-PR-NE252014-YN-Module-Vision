namespace VisDummy.Abstractions.Args
{
    public class StationArgs_MK
    {
        /// <summary>
        /// 功能号
        /// </summary>
        public ushort Function_Number { get; set; }
        /// <summary>
        ///  拍照位置号
        /// </summary>
        public ushort Position_Number { get; set; }

        public string ToMsg()
        {
            return $"Function:{Function_Number};Position:{Position_Number}";
        }
    }
}
