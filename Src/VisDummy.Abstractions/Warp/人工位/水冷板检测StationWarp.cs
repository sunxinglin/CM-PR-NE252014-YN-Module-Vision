namespace VisDummy.Abstractions.Warp.人工位
{
    public class 水冷板检测OkWrap_人工位
    {
        public string ImagePath { get; set; } = string.Empty;
        public float 左短位置度 { get; set; }
        public float 左上位置度 { get; set; }
        public float 左下位置度 { get; set; }
        public float 右短位置度 { get; set; }
        public float 右上位置度 { get; set; }
        public float 右下位置度 { get; set; }
      
    }

    public class 水冷板检测ErrWrap_人工位
    {
        public string ImagePath { get; set; } = string.Empty;
        public string ErrMsg { get; set; } = string.Empty;
        public bool PLC参数NG { get; set; }
        public bool 找特征点NG { get; set; }
        public bool 偏移量NG { get; set; }
        public bool 视觉流程NG { get; set; }
    }
}
