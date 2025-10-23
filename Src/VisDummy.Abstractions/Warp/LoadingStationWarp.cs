namespace VisDummy.Abstractions.Warp
{
    public class StationOkWrap_Loading
    {
        public string ImagePath { get; set; } = string.Empty;
        public ushort Direction { get; set; }
        public string BarCode { get; set; } = string.Empty;
        public string ToMsg()
        {
            return $"ImagePath:{ImagePath};Direction:{Direction},BarCode:{BarCode}";
        }
    }

    public class StationErrWrap_Loading
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
