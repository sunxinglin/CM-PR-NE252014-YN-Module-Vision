namespace VisDummy.Abstractions.Warp.水冷板抓取
{
    public class 自动校准OkWrap_水冷板抓取
    {
        public string ImagePath { get; set; } = string.Empty;
        public string ToMsg()
        {
            return $"ImagePath:{ImagePath}";
        }
    }

    public class 自动校准ErrWrap_水冷板抓取
    {
        public string ImagePath { get; set; } = string.Empty;
        public string ErrMsg { get; set; } = string.Empty;
        public bool 找特征点NG { get; set; }
        public bool 像素精度NG { get; set; }
        public bool 测距NG { get; set; }
        public bool 视觉流程NG { get; set; }

        public string ToMsg()
        {
            return $"ImagePath:{ImagePath};ErrMsg:{ErrMsg};";
        }
    }
}
