namespace VisDummy.Abstractions.Args
{
    public class SpotStationArgs
    {
        /// <summary>
        /// 相机号
        /// </summary>
        public ushort Camera { get; set; }
        /// <summary>
        ///  拍照位置号
        /// </summary>
        public ushort Position { get; set; }

        public string ToMsg()
        {
            return $"Camera:{Camera};Position:{Position}";
        }
    }
}
