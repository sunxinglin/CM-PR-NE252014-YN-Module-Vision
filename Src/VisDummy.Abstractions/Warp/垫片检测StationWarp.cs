namespace VisDummy.Abstractions.Warp
{
    public class StationOkWrap_垫片检测
    {
        public string ImagePath { get; set; } = string.Empty;
        public string ToMsg()
        {
            return $"ImagePath:{ImagePath}";
        }
    }

    public class StationErrWrap_垫片检测
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
