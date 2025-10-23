namespace VisDummy.Abstractions.Warp
{
    public class StationOkWrap_模组贴标
    {
        public string ImagePath { get; set; } = string.Empty;
        public string BarCode1 { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public float R { get; set; }
        public string ToMsg()
        {
            return $"ImagePath:{ImagePath};BarCode:{BarCode1};X:{X};Y:{Y};R:{R}";
        }
    }

    public class StationErrWrap_模组贴标
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
