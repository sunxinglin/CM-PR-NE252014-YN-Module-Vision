namespace VisDummy.Abstractions.Warp
{
    public class StationOkWrap_MK
    {
        public ushort ResultStatus { get; set; }
        public ushort Foam { get; set; }
        public ushort Floor { get; set; }
        public ushort Column { get; set; }
        public ushort Direction { get; set; }

        public float PreciseX { get; set; }
        public float PreciseY { get; set; }
        public float PreciseZ { get; set; }
        public float PreciseA { get; set; }
        public float PreciseB { get; set; }
        public float PreciseC { get; set; }

        public string ToMsg()
        {
            return $"ResultStatus:{WarpHelper.ResultConvert(ResultStatus)};Foam:{Foam},Floor:{Floor},Column:{Column},Direction:{Direction},X:{PreciseX},Y:{PreciseY},Z:{PreciseZ},A:{PreciseA},B:{PreciseB},C:{PreciseC}";
        }
    }

    public class StationErrWrap_MK
    {
        public string ErrMsg { get; set; } = string.Empty;
        public ushort ResultStatus { get; set; }
        public string ToMsg()
        {
            return $"ErrMsg:{ErrMsg},ResultStatus:{WarpHelper.ResultConvert(ResultStatus)}";
        }
    }
}
