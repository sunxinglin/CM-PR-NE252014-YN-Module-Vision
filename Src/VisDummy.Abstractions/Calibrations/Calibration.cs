namespace VisDummy.Abstractions.Calibrations
{
    public class CalibrateNthPointArgs
    {
        /// <summary>
        /// 物料号
        /// </summary>
        public ushort MatNum { get; set; }

        /// <summary>
        /// 第Nth点
        /// </summary>
        public int Nth { get; set; }

        /// <summary>
        /// 物理坐标X
        /// </summary>
        public float WldX { get; set; }

        /// <summary>
        /// 物理坐标Y
        /// </summary>
        public float WldY { get; set; }

        /// <summary>
        /// 物理坐标A
        /// </summary>
        public float WldA { get; set; }
    }


    public class CalibarateNthPointOkWrap
    {
        public bool 标定状态 { get; set; }
        public float ImgX { get; set; }
        public float ImgY { get; set; }
        public float ImgA { get; set; }
    }

    #region Errors
    public interface IErr_CalibrateNthPoint
    {

    }
    public class Err_ProcedureNG : IErr_CalibrateNthPoint
    {
        public string Msg { get; set; }
    }
    #endregion
}
