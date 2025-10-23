namespace VisDummy.Abstractions.Warp
{
    public class StationOkWrap_模组入箱
    {
        public string ImagePath { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public float A { get; set; }
        public string ToMsg()
        {
            return $"ImagePath:{ImagePath};X:{X};Y:{Y};A:{A}";
        }
    }

    public class StationErrWrap_模组入箱
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
