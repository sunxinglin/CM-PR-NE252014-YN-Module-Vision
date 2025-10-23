namespace VisDummy.Abstractions.Warp
{
    public class StationOkWrap_模组检测
    {
        public string ImagePath { get; set; } = string.Empty;
        public ushort Component { get; set; }
        public ushort[] Polaritys { get; set; } = new ushort[20];
        public string ToMsg()
        {
            return $"ImagePath:{ImagePath};Component:{Component},Polaritys:{string.Join(",", Polaritys)}";
        }
    }

    public class StationErrWrap_模组检测
    {
        public string ImagePath { get; set; } = string.Empty;
        public string ErrMsg { get; set; } = string.Empty;
        public uint ErrorCode { get; set; }
        public string ToMsg()
        {
            return $"ImagePath:{ImagePath};ErrMsg:{ErrMsg};ErrorCode:{ErrorCode}";
        }
    }
}
