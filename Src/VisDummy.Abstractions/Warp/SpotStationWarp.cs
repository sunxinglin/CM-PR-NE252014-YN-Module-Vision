namespace VisDummy.Abstractions.Warp
{
    public class SpotStationOkWarp
    {
        public string ImagePath { get; set; } = string.Empty;
        public float Features { get; set; }
        public float Pixels { get; set; } 
        public string ToMsg()
        {
            return $"ImagePath:{ImagePath};Features:{Features},Pixels:{Pixels}";
        }
    }

    public class SpotStationErrWarp
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
