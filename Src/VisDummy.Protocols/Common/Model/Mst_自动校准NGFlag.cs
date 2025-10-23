using Itminus.Protocols.Common;

namespace VisDummy.Protocols.Common.Model
{
    public enum Mst_自动校准NGFlag : byte
    {
        None = 0,
        找特征点NG = 1 << 0,
        像素精度NG = 1 << 1,
        测距NG = 1 << 2,
        视觉流程NG = 1 << 3,
    }
    public class Mst_自动校准NGFlagsBuilder : FlagsBuilder<Mst_自动校准NGFlag>
    {
        public Mst_自动校准NGFlagsBuilder(Mst_自动校准NGFlag wCmd) : base(wCmd)
        {
        }

        public Mst_自动校准NGFlagsBuilder SetOn(bool 找特征点NG, bool 像素精度NG, bool 测距NG, bool 视觉流程NG)
        {
            SetOnOff(Mst_自动校准NGFlag.找特征点NG, 找特征点NG);
            SetOnOff(Mst_自动校准NGFlag.像素精度NG, 像素精度NG);
            SetOnOff(Mst_自动校准NGFlag.测距NG, 测距NG);
            SetOnOff(Mst_自动校准NGFlag.视觉流程NG, 视觉流程NG);
            return this;
        }
        public Mst_自动校准NGFlagsBuilder SetOff()
        {
            SetOnOff(Mst_自动校准NGFlag.找特征点NG, false);
            SetOnOff(Mst_自动校准NGFlag.像素精度NG, false);
            SetOnOff(Mst_自动校准NGFlag.测距NG, false);
            SetOnOff(Mst_自动校准NGFlag.视觉流程NG, false);
            return this;
        }
    }
}
