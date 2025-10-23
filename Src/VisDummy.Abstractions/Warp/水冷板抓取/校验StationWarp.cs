namespace VisDummy.Abstractions.Warp.水冷板抓取
{
    public class 校验OkWrap_水冷板抓取
    {
        public string ImagePath { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public float R { get; set; }
        public float 像素精度 { get; set; }
        public string ToMsg()
        {
            return $"ImagePath:{ImagePath};X:{X};Y:{Y};R:{R};像素精度:{像素精度}";
        }
    }

    public class 校验ErrWrap_水冷板抓取
    {
        public string ImagePath { get; set; } = string.Empty;
        public string ErrMsg { get; set; } = string.Empty;
        public bool PLC参数NG { get; set; }
        public bool 找特征点NG { get; set; }
        public bool 偏移量NG { get; set; }
        public bool 视觉流程NG { get; set; }

        public string ToMsg()
        {
            return $"ImagePath:{ImagePath};ErrMsg:{ErrMsg};";
        }
    }
}
